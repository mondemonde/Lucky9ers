using api.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public static class ConfigurationExtensions
{
    public static IHostBuilder ConfigureAppConfigurationFromCustomFile(
        this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            // Get the IWebHostEnvironment
            var env = hostingContext.HostingEnvironment;
            var currentDirectory = Directory.GetCurrentDirectory();

            config
                .SetBasePath(currentDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .AddUserSecrets<Program>(true)
                .AddEnvironmentVariables();

            config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
            AppGlobal.Configuration = config.Build();          

        });

        return hostBuilder;
    }
}
