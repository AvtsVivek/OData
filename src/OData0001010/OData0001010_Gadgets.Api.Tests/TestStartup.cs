using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OData0001010_Gadgets.Api.Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration config, IWebHostEnvironment env)
          : base(config, env) { }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }

        public override void ConfigureContainer(ContainerBuilder builder)
        {
            //base.ConfigureContainer(builder);
            //builder.RegisterModule(new DefaultCoreModuleForMocks());
            //builder.RegisterModule(new DefaultInfrastructureModuleForMocks(true));
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }
    }

}
