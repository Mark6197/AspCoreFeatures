using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreFeatures
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            //  .ConfigureServices((hostContext, services) =>
            // {
            //   if (hostContext.HostingEnvironment.EnvironmentName == "Test")
            //     FlightsManagmentSystemConfig.Instance.Init("FlightsManagmentSystemTests.Config.json");
            //   else
            //     FlightsManagmentSystemConfig.Instance.Init();
            //})
            .ConfigureLogging(builder =>//Configure log4net, need to add the log4net configuration file, 
                {
                    builder.SetMinimumLevel(LogLevel.Debug);
                    builder.AddLog4Net("Log4Net.config");
                });
    }
}
