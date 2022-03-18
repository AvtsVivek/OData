using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SimpleAirVinyl.EntityDataModel;

namespace SimpleAirVinyl
{
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
                        .Select()
                        //.Expand()
                        //.OrderBy()
                        //.SetMaxTop(10)
                        //.Count()
                        //.Filter()
                        )
                                ;

            
            services.AddDbContext<AirVinylDbContext>(options =>
            {
                //options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=AirVinylDemoDbNew;Trusted_Connection=True;");
                options.UseSqlite(@"Data Source=database.sqlite;");
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AirVinyl API", Version = "v1" });
                c.EnableAnnotations();
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

            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AirVinyl API V1");
                //c.RoutePrefix = "";
            });

        }
    }

}
