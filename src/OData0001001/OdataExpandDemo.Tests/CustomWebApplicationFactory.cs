using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using OdataExpandDemo;

namespace OdataExpandDemo.Tests
{
    public abstract class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>
    where TEntryPoint : class
    {
        public ApplicationOptions ApplicationOptions { get; private set; } = default!;

        public List<Mock> MockList { get; protected set; } = new List<Mock>();

        public CustomWebApplicationFactory()
        {
            ClientOptions.AllowAutoRedirect = false;
        }

        protected override IHostBuilder? CreateHostBuilder()
        {
            var hostBuilder = base.CreateHostBuilder();

            if (hostBuilder == null)
            {
                var args = new string[0];
                var containerBuilderAction = AutofacContainerBuilder;
                var autofacServiceProviderFactory = new AutofacServiceProviderFactory(containerBuilderAction);
                hostBuilder = Program.CreateHostBuilder<TestStartup>(args, autofacServiceProviderFactory);
            }
            return hostBuilder;
        }

        protected virtual void AutofacContainerBuilder(ContainerBuilder containerBuilder)
        {

        }

        protected override IWebHostBuilder? CreateWebHostBuilder()
        {
            // WebHostBuilder will no longer be used. 
            // This is just there for backwrod compatiability.
            // So here the breakpoint will not be hit.
            // https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-6.0&tabs=visual-studio#hostbuilder-replaces-webhostbuilder
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-6.0
            return base.CreateWebHostBuilder();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment(RunningEnvironmentName.Test)
                ;
        }
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            builder.UseContentRoot(currentDirectory);

            var host = base.CreateHost(builder);
            host.Start();
            return host;
        }


        protected virtual void ConfigureServices(IServiceCollection services)
        {
            // services.AddScoped<IMediator, NoOpMediator>();
            // ConfigureInMemoryDatabase(services, false);
        }

        protected override IEnumerable<Assembly> GetTestAssemblies()
        {
            // This is not getting called. Not sure why.
            // What is the purpose of this method. Need to find out.
            return base.GetTestAssemblies();
        }
        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            // This is not getting called. Not sure why.
            // What is the purpose of this method. Need to find out.
            return base.CreateServer(builder);
        }

        protected override void ConfigureClient(HttpClient client)
        {
            using (var serviceScope = Services.CreateScope())
            {
                // Not very clear whats happening here. Need to find out.
                var serviceProvider = serviceScope.ServiceProvider;
                // this.ApplicationOptions = serviceProvider.GetRequiredService<ApplicationOptions>();
            }

            base.ConfigureClient(client);
        }

        protected override void Dispose(bool disposing)
        {
            // Looks like this dispose method is getting called twice. 
            // Not sure why.
            if (disposing)
            {
                //this.VerifyAllMocks();
                VerifyAllMocksAlternative();
            }

            base.Dispose(disposing);
        }

        private void VerifyAllMocksAlternative()
          => MockList.ForEach(mock => Mock.VerifyAll(mock));

        //private void VerifyAllMocks()
        //{
        //  Mock.VerifyAll(
        //      //this.CarRepositoryMock,
        //      //this.ClockServiceMock,
        //      this.GreeterServiceMock
        //  //this.BikeRepositoryMock
        //  );
        //  this.MockList.ForEach(mock => Mock.VerifyAll(mock));
        //}
    }
}
