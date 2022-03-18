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

namespace OData0001010_Gadgets.Api.Tests
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
                .ConfigureServices(ConfigureServices);
        }
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            builder.UseContentRoot(currentDirectory);

            var host = base.CreateHost(builder);
            //host.StopAsync().Wait();
            SeedDatabase(host, true);
            host.Start();
            return host;
        }

        /// <summary>
        /// In most integration tests, we have to setup the data
        /// </summary>
        /// <param name="host">The host object from which the services will be retrieved.</param>
        /// <param name="bSeedDatabase">True if you want to seed the dabase with some seed data</param>
        protected virtual void SeedDatabase(IHost host, bool bSeedDatabase = false)
        {
            if (!bSeedDatabase)
                return; // Dont need to seed the database. Simply return.
                        // Get service provider.
            var serviceProvider = host.Services;

            // Create a scope to obtain a reference to the database
            // context (AppDbContext).
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<OdataDbContext>();

                var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TEntryPoint>>>();

                // Ensure the database is created.
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                try
                {
                    // Seed the database with test data.
                    SeedData.PopulateTestData(db);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                                        $"database with test messages. Error: {ex.Message}");
                }
            }
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMediator, NoOpMediator>();
            ConfigureInMemoryDatabase(services, false);
        }

        /// <summary>
        /// In most cases we need inmemory database.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="bConfigureDb"></param>
        protected virtual void ConfigureInMemoryDatabase(IServiceCollection services, bool bConfigureDb = true)
        {
            if (!bConfigureDb)
                return; // Simply return, we dont need to configure the in mem db.

            // Remove the app's ApplicationDbContext registration.
            var descriptor = services.SingleOrDefault(
              d => d.ServiceType == typeof(DbContextOptions<OdataDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // This should be set for each individual test run
            var inMemoryCollectionName = Guid.NewGuid().ToString();

            // Add ApplicationDbContext using an in-memory database for testing.
            services.AddDbContext<OdataDbContext>(options =>
            {
                options.UseInMemoryDatabase(inMemoryCollectionName);
                options.EnableSensitiveDataLogging();
            });
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
