using Autofac.Extensions.DependencyInjection;

namespace SimpleAirVinyl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<SimpleAirVinylDbContext>();
                    // context.Database.Migrate();
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

        public static IHostBuilder CreateHostBuilder(string[] args,
            AutofacServiceProviderFactory autofacServiceProviderFactory = null!)
        {
            if (autofacServiceProviderFactory == null)
                autofacServiceProviderFactory = new AutofacServiceProviderFactory();
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(autofacServiceProviderFactory)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
            return hostBuilder;
        }
    }
}
