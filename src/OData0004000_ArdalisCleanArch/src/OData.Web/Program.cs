using System.Diagnostics;
using Autofac.Extensions.DependencyInjection;
using Serilog;

namespace OData.Web;

public class Program
{
  public static void Main(string[] args) 
  {
    var host = CreateHostBuilder<Startup>(args).Build();

    using (var scope = host.Services.CreateScope())
    {
      var services = scope.ServiceProvider;
      //SeedSampleData.SetAppConstant(services);
      try
      {
        SeedData.Initialize(services);
        //SeedSampleData.Initialize(services);
        //SeedDataExtensions.Initialize(services);
      }
      catch (Exception ex)
      {
        Debugger.Break();
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
      }
    }

    host.Run();
  }

  public static IHostBuilder CreateHostBuilder<TStartup>(string[] args,
  AutofacServiceProviderFactory? autofacServiceProviderFactory = null) where TStartup : class
  {
    var builder = WebApplication.CreateBuilder(args);
    if (autofacServiceProviderFactory == null)
      autofacServiceProviderFactory = new AutofacServiceProviderFactory();
    return Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(autofacServiceProviderFactory)
    // https://jkdev.me/asp-net-core-serilog/
    .UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration))
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
