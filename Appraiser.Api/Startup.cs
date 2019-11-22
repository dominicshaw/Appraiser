using Appraiser.Api.Services;
using Appraiser.Data;
using Appraiser.DTOs;
using Appraiser.Mapping;
using Appraiser.Reporting;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Appraiser.Api
{
    public class Startup
    {
        private static readonly string[] _origins =
        {
            "http://servername",
            "http://servername:7823",
            "http://servername:7825",
            "http://servername.ttint.com:7823",
            "http://servername.ttint.com:7825",
            "http://localhost",
            "http://localhost:7825",
            "http://localhost:4200"
        };

        [UsedImplicitly] private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", builder => builder.WithOrigins(_origins).AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

            services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                .AddNegotiate();

            services.AddHttpContextAccessor();

            services.AddDbContext<AppraiserContext>();

            services.AddTransient(provider => new ChangeChecker<AppraisalDTO>(provider.GetService<IHttpContextAccessor>(), "Appraisal", dto => dto.Id, provider.GetService<AppraiserContext>()));
            services.AddTransient(provider => new ChangeChecker<ReviewDTO>(provider.GetService<IHttpContextAccessor>(), "Review", dto => dto.Id, provider.GetService<AppraiserContext>()));
            services.AddTransient(provider => new ChangeChecker<TrainingDTO>(provider.GetService<IHttpContextAccessor>(), "Training", dto => dto.Id, provider.GetService<AppraiserContext>()));
            services.AddTransient(provider => new ChangeChecker<ObjectiveDTO>(provider.GetService<IHttpContextAccessor>(), "Objective", dto => dto.Id, provider.GetService<AppraiserContext>()));
            services.AddTransient(provider => new ChangeChecker<StaffDTO>(provider.GetService<IHttpContextAccessor>(), "Staff", dto => dto.Id, provider.GetService<AppraiserContext>()));
            services.AddTransient(provider => new ChangeChecker<ResponsibilityDTO>(provider.GetService<IHttpContextAccessor>(), "Responsibility", dto => dto.Id, provider.GetService<AppraiserContext>()));

            services.AddTransient<AppraisalMapper, AppraisalMapper>();
            services.AddTransient<ReviewMapper, ReviewMapper>();
            services.AddTransient<TrainingMapper, TrainingMapper>();
            services.AddTransient<ObjectiveMapper, ObjectiveMapper>();
            services.AddTransient<StaffMapper, StaffMapper>();
            services.AddTransient<ResponsibilityMapper, ResponsibilityMapper>();
            services.AddSingleton<ChangeMapper, ChangeMapper>();

            services.AddTransient<AppraisalService, AppraisalService>();
            services.AddTransient<StaffService, StaffService>();
            services.AddTransient<ResponsibilityService, ResponsibilityService>();
            services.AddTransient<ChangeService, ChangeService>();
            services.AddScoped<ReportManager, ReportManager>();

            services.AddControllers()
                .AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });

            services.AddHealthChecks()
                .AddDbContextCheck<AppraiserContext>();

            services.AddSwaggerDocument(settings =>
            {
                settings.DocumentName = "appraiser";
                settings.Title = "Appraiser-API";
                settings.Version = "v1";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors(builder => builder.WithOrigins(_origins).AllowAnyHeader().AllowAnyMethod().AllowCredentials());

            app.UseOpenApi();
            app.UseSwaggerUi3();

            if (GlobalConfiguration.UseHttps)
                app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
