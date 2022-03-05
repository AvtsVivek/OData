using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AirVinyl.Tests
{
    public class UnitTestExtensions
    {
        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args,
        AutofacServiceProviderFactory? autofacServiceProviderFactory = null) where TStartup : class
        {
            if (autofacServiceProviderFactory == null)
                autofacServiceProviderFactory = new AutofacServiceProviderFactory();
            return Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(autofacServiceProviderFactory)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                  .UseStartup<TStartup>()
                  .ConfigureLogging(logging =>
                  {
                      logging.ClearProviders();
                      logging.AddConsole();
                  // logging.AddAzureWebAppDiagnostics(); add this if deploying to Azure
              });
            });
        }
    }
}