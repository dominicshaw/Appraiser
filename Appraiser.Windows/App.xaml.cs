using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using Appraiser.Windows.Views;
using DevExpress.Xpf.Core;
using Ninject;
using Serilog;
using Serilog.Events;

namespace Appraiser.Windows
{
    public partial class App
    {
        private static string LogFileLocation() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Appraiser-Front-End");

        private static readonly StandardKernel _kernel = new StandardKernel();

        protected override void OnStartup(StartupEventArgs e)
        {
            ApplicationThemeHelper.ApplicationThemeName = Theme.Office2019ColorfulName;

            ConfigureKernel();

            var log = _kernel.Get<ILogger>();

            try
            {
                Current.DispatcherUnhandledException +=
                    (s, ex) => log.Error("Dispatcher Unhandled Exception: {0}", ex.Exception);
                AppDomain.CurrentDomain.UnhandledException +=
                    (s, ex) => log.Error("Unhandled Exception: {0}", ex.ExceptionObject);

                base.OnStartup(e);

                MainWindow = _kernel.Get<MainWindow>();
                MainWindow.Show();
            }
            catch (Exception ex)
            {
                log.Error("{exception}", ex);
            }
        }

        private static void ConfigureKernel()
        {
            _kernel.Bind<ILogger>().ToMethod(ctx => new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", "Appraiser-Windows")
                    .Enrich.WithProperty("Username", Environment.UserName)
                    .Enrich.WithProperty("ComputerName", Environment.MachineName)
                    .WriteTo.RollingFile(Path.Combine(LogFileLocation(), @"Appraiser-{Date}.txt"))
                    .WriteTo.Email("foliometrics-uploader@ttint.com", "shawd@ttint.com", mailServer: "mailserver.ttint.com", restrictedToMinimumLevel: LogEventLevel.Error, outputTemplate: "[{Username}] [{ComputerName}] [{Timestamp:HH:mm:ss:ff}] [{Level:u3}] {Message:lj}")
                    .CreateLogger())
                .InSingletonScope();

            _kernel.Bind<HttpClient>().ToMethod(_ => new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true })).InSingletonScope();
        }
    }
}
