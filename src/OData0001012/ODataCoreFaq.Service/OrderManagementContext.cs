using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ODataCoreFaq.Data;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ODataCoreFaq.Service
{
    public class OrderManagementContext : DbContext
    {
        public OrderManagementContext(DbContextOptions<OrderManagementContext> options)
            : base(options)
        { }

        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<OrderHeader> Orders => Set<OrderHeader>();

        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Action<string> consoleLogger = logInfo => Console.WriteLine(logInfo);

            // To see the output from the following 'Debug logger',
            // open the Output window(View -> Output or Ctrl + Alt + O)
            // and then select Debug from the see output from dropdown
            Action<string> debugWindowLogger = logInfo => Debug.WriteLine(logInfo);

            optionsBuilder
            // 
            //.LogTo(consoleLogger, LogLevel.Information)

            .LogTo(debugWindowLogger, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();

            base.OnConfiguring(optionsBuilder);
        }
    }

    public class SeedData {
        internal static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var dbContext = new OrderManagementContext(
                serviceProvider.GetRequiredService<DbContextOptions<OrderManagementContext>>()))
            {
                //if (dbContext.Gadgets.Any())
                //{
                //    return;// DB has been seeded
                //}

                //try
                //{

                await PopulateTestData(dbContext);
                //}
                //catch (Exception excp)
                //{
                //    Debugger.Break();
                //    Debugger.Log(0, "Error", excp.Message);
                //    throw;
                //}
            }
        }

        public static async Task PopulateTestData(OrderManagementContext dbContext)
        {

            await dbContext.OrderDetails.ForEachAsync(od => dbContext.OrderDetails.Remove(od));
            await dbContext.Orders.ForEachAsync(o => dbContext.Orders.Remove(o));
            await dbContext.Customers.ForEachAsync(c => dbContext.Customers.Remove(c));
            await dbContext.Products.ForEachAsync(p => dbContext.Products.Remove(p));

            await dbContext.SaveChangesAsync();

            var demoCustomers = new[] {
                new Customer() { CompanyName = "Corina Air Conditioning", CountryIsoCode = "AT" },
                new Customer() { CompanyName = "Fernando Engineering", CountryIsoCode = "AT" },
                new Customer() { CompanyName = "Murakami Plumbing", CountryIsoCode = "CH" },
                new Customer() { CompanyName = "Naval Metal Construction", CountryIsoCode = "DE" }
            };
            dbContext.Customers.AddRange(demoCustomers);

            var demoProducts = new[] {
                new Product() { Description = "Mountain Bike", IsAvailable = true, CategoryCode = "BIKE", PricePerUom = 2500 },
                new Product() { Description = "Road Bike", IsAvailable = true, CategoryCode = "BIKE", PricePerUom = 2000 },
                new Product() { Description = "Skate Board", IsAvailable = true, CategoryCode = "BOARD", PricePerUom = 100 },
                new Product() { Description = "Long Board", IsAvailable = true, CategoryCode = "BOARD", PricePerUom = 250 },
                new Product() { Description = "Scooter", IsAvailable = false, CategoryCode = "OTHERS", PricePerUom = 150 }
            };
            dbContext.Products.AddRange(demoProducts);

            var rand = new Random();
            for (var i = 0; i < 100; i++)
            {
                var order = new OrderHeader()
                {
                    OrderDate = new DateTimeOffset(new DateTime(2021, rand.Next(1, 12), rand.Next(1, 28))),
                    Customer = demoCustomers[rand.Next(demoCustomers.Length - 1)]
                };
                dbContext.Orders.Add(order);

                for (var j = 0; j < 3; j++)
                {
                    dbContext.OrderDetails.Add(new OrderDetail()
                    {
                        Order = order,
                        Product = demoProducts[rand.Next(demoProducts.Length - 1)],
                        Amount = rand.Next(1, 5)
                    });
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
