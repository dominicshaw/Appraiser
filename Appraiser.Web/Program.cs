using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Serilog;
using Serilog.Events;
namespace Appraiser.Web
{
    public static class Program
    {
        private static string LogFileLocation() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Appraiser");

        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", "Appraiser-WebHost")
                .WriteTo.Console()
                .WriteTo.RollingFile(Path.Combine(LogFileLocation(), @"Appraiser-WebHost-{Date}.txt"))
                .WriteTo.Email("appraiser@ttint.com", "shawd@ttint.com", mailServer: "mailserver.ttint.com", restrictedToMinimumLevel: LogEventLevel.Error)
                .CreateLogger();

            try
            {
                Log.Information("Starting Appraiser-Web host");

                var isService = !(Debugger.IsAttached || args.Contains("--console"));

                if (isService)
                {
                    Log.Error(LogFileLocation());
                    using (var currentProcess = Process.GetCurrentProcess())
                    {
                        var pathToExe = currentProcess.MainModule?.FileName;
                        var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                        Directory.SetCurrentDirectory(pathToContentRoot);
                    }
                }

                var builder = CreateHostBuilder(args.Where(arg => arg != "--console").ToArray());

                using (var host = builder.Build())
                {
                    if (isService)
                        host.RunAsService();
                    else
                        host.Run();
                }

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(GlobalConfiguration.Urls[GlobalConfiguration.UseHttps])
                .UseSerilog();
    }
}
