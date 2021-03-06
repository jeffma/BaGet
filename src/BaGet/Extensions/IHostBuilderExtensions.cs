using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BaGet.Extensions
{
    // See https://github.com/aspnet/MetaPackages/blob/master/src/Microsoft.AspNetCore/WebHost.cs
    public static class IHostBuilderExtensions
    {
        public static IHostBuilder ConfigureBaGetConfiguration(this IHostBuilder builder, string[] args)
        {
            return builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddEnvironmentVariables();

                config.SetBasePath(Environment.CurrentDirectory);
                var env = context.HostingEnvironment.EnvironmentName;
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{env}.json", optional: true);

                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            });
        }

        public static IHostBuilder ConfigureBaGetLogging(this IHostBuilder builder)
        {
            return builder
                .ConfigureLogging((context, logging) =>
                {
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                });
        }

        public static IHostBuilder ConfigureBaGetServices(this IHostBuilder builder)
        {
            return builder
                .ConfigureServices((context, services) => services.ConfigureBaGet(context.Configuration));
        }
    }
}
