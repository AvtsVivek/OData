using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using ODataCoreFaq.Data;

namespace ODataCoreFaq.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<OrderManagementContext>(options => options.UseSqlServer(
            //    Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddDbContext<OrderManagementContext>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("MyWorldDbConnection"));
                options.UseSqlite(@"Data Source=orderManagementDataBaseOdata.sqlite;");
            });

            //services.AddControllers()
            //    .AddOData(options => options.Count().Filter().Expand().Select().OrderBy().SetMaxTop(5)
            //    .AddModel("odata", GetEdmModel()));

            services.AddControllers()
                .AddOData(options => options.AddRouteComponents("odata", GetEdmModel())
                .Select().Filter().OrderBy().Expand().SetMaxTop(5).Count());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Odata Order Management", Version = "v1" });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Odata Order Management"));

                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Odata Order Management");
                    //c.RoutePrefix = "";
                });

            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            builder.EntityType<Customer>()
                .Collection
                .Function("OrderedBike")
                .ReturnsCollectionFromEntitySet<Customer>("Customer");

            builder.EntitySet<Product>("Products");
            builder.EntitySet<OrderHeader>("OrderHeaders");
            builder.EntitySet<OrderDetail>("OrderDetails");

            return builder.GetEdmModel();
        }
    }
}
