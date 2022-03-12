using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using OData0001010_Gadgets.Api.Services;
using Autofac;

namespace OData0001010_Gadgets.Api
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

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            //builder.RegisterModule(new DefaultCoreModule());
            //builder.RegisterModule(new DefaultInfrastructureModule(_env.EnvironmentName == "Development"));
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<OdataDbContext>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("MyWorldDbConnection"));
                options.UseSqlite(@"Data Source=databaseodata.sqlite;");
            });

            services.AddControllers().AddOData(options =>
                options.Select().Filter().OrderBy().Expand());

            services.AddControllers()
                .AddOData(
                options => options.Select().Filter().OrderBy().Expand()
                );


            services.AddTransient<IGadgetsService, GadgetsService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OData6Demo.Api", Version = "v1" });
            });
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OData6Demo.Api v1"));
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
}
