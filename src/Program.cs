using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using MovieRating.Services;
using MovieRating.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using MovieRating.Domain;

namespace Template
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            IConfigurationRoot configurationRoot = null;
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("appsettings.json", optional: true);
                    configHost.AddEnvironmentVariables();
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    hostContext.HostingEnvironment.ApplicationName = "HttpClientMock";
                    configurationRoot = configApp.Build();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IConfiguration>(hostContext.Configuration);
                    services.AddSingleton<IConfigurationRoot>(configurationRoot);
                    services.AddSingleton<IHostedService, App>();
                    services.AddSingleton<IOmdbService<Movie>, OmdbService>();
                    services.AddHttpClient();
                })
                .Build();

            using (host)
            {
                await host.StartAsync();
                await host.StopAsync();
            }
        }
    }
}
