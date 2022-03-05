using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirVinyl;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args,
        AutofacServiceProviderFactory? autofacServiceProviderFactory = null)
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
