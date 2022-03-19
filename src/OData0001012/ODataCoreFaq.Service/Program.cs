using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ODataCoreFaq.Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // CreateHostBuilder(args).Build().Run();

            var host = CreateHostBuilder<Startup>(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<OrderManagementContext>();

                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    await SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();

        }

        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args
            //AutofacServiceProviderFactory autofacServiceProviderFactory = null
            ) where TStartup : class
        {
            //if (autofacServiceProviderFactory == null)
            //    autofacServiceProviderFactory = new AutofacServiceProviderFactory();
            var hostBuilder = Host.CreateDefaultBuilder(args)
                //.UseServiceProviderFactory(autofacServiceProviderFactory)
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
