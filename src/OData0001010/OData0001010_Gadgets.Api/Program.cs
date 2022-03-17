using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OData0001010_Gadgets.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder<Startup>(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<OdataDbContext>();
                    context.Database.EnsureCreated();
                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args,
            AutofacServiceProviderFactory autofacServiceProviderFactory = null) where TStartup : class
        {
            if (autofacServiceProviderFactory == null)
                autofacServiceProviderFactory = new AutofacServiceProviderFactory();
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(autofacServiceProviderFactory)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>()
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.AddConsole();
                        // logging.AddAzureWebAppDiagnostics(); add this if deploying to Azure
                    });
                });
            return hostBuilder;
        }
    }
}
