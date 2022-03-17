using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OData6Demo.Api;

namespace OData6Demo.Api.Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration config, IWebHostEnvironment env)
          : base(config, env) { }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }
    }

}
