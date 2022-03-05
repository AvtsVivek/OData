using AirVinyl.API.DbContexts;
using AirVinyl.EntityDataModels;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AirVinyl;

public class Startup
{
    private readonly IWebHostEnvironment _env;

    public Startup(IConfiguration config, IWebHostEnvironment env)
    {
        Configuration = config;
        _env = env;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
                            .AddOData(opt =>
                    opt.AddRouteComponents("odata",
                        new AirVinylEntityDataModel().GetEntityDataModel()
                        //, batchHandler
                        )
                    //.Select()
                    //.Expand()
                    //.OrderBy()
                    //.SetMaxTop(10)
                    //.Count()
                    //.Filter()
                    );

        services.AddDbContext<AirVinylDbContext>(options =>
        {
            options.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=AirVinylDemoDB;Trusted_Connection=True;");
        });
    }
    public virtual void ConfigureContainer(ContainerBuilder builder)
    {
        //builder.RegisterModule(new DefaultCoreModule());
        //builder.RegisterModule(new DefaultInfrastructureModule(_env.EnvironmentName == "Development"));
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
