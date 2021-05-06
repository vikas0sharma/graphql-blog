using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Blog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string contentRootPath = Directory.GetCurrentDirectory();
            IHostBuilder host = new HostBuilder().ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                        .UseContentRoot(contentRootPath)
                        .ConfigureAppConfiguration(builder => AddConfiguration(builder, contentRootPath))
                        .ConfigureLogging((builder, logging) =>
                        {
                            logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
                            logging.AddDebug();
                            logging.AddConsole();
                        })
                        .ConfigureKestrel(serverOptions =>
                        {
                            serverOptions.Listen(IPAddress.Any, Convert.ToInt32(Environment.GetEnvironmentVariable("PORT")));
                        })
            .UseStartup<Startup>();
            });
            return host;
        }
        private static IConfigurationBuilder AddConfiguration(IConfigurationBuilder builder, string basePath)
        {
            string environmentName = GetEnvironmentName();

            builder.SetBasePath(basePath);
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            if (environmentName.Contains(".")) builder.AddJsonFile($"appsettings.{environmentName.Split('.')[0]}.json", optional: true, reloadOnChange: true);
            if (environmentName.Contains(".") && environmentName.Split('.').Length > 2) builder.AddJsonFile($"appsettings.{environmentName.Split('.')[0]}.{environmentName.Split('.')[1]}.json", optional: true, reloadOnChange: true);
            builder.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
            builder.AddEnvironmentVariables();
            return builder;
        }
        private static string GetEnvironmentName()
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(environmentName)) throw new Exception("ASPNETCORE_ENVIRONMENT environment variable is not set");
            return environmentName;
        }
    }
}
