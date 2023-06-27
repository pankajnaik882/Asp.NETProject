using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Path = System.IO.Path;

namespace FunDoNotesApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            NLog.GlobalDiagnosticsContext.Set("LogDirectory", logPath);
            var Logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            Logger.Debug("The Application Started");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(opt =>
                {
                    opt.ClearProviders();
                    opt.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();
    }
}
