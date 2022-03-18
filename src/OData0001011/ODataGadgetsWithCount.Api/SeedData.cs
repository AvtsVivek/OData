using System;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using OData0001010_Gadgets.Api.Models;

namespace OData0001010_Gadgets.Api
{
    public class SeedData
    {
        internal static void Initialize(IServiceProvider serviceProvider)
        {
            using (var dbContext = new OdataDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<OdataDbContext>>()))
            {
                if (dbContext.Gadgets.Any())
                {
                    return;// DB has been seeded
                }

                try
                {
                    PopulateTestData(dbContext);
                }
                catch (Exception excp)
                {
                    Debugger.Break();
                    Debugger.Log(0, "Error" , excp.Message);
                    throw;
                }
            }
        }

        public static void PopulateTestData(OdataDbContext dbContext)
        {
            GetRecordStoreList().ForEach(gadget => dbContext.Add(gadget));
            dbContext.SaveChanges();
        }

        public static List<Product> GetRecordStoreList()
        {
            return new List<Product>
            {
                new Product()
                {
                    Id = 1,
                    ProductName = "Pen Drive",
                    Brand = "Sony",
                    Cost = 600,
                    Type = "Accessories"
                },
                new Product()
                {
                    Id = 2,
                    ProductName = "Head Phone",
                    Brand = "Sony",
                    Cost = 3000,
                    Type = "Computer Accessories"
                },
                new Product()
                {
                    Id = 3,
                    ProductName = "Key board",
                    Brand = "Hp",
                    Cost = 1000,
                    Type = "Computer Accessories"
                }
            };
        }
    }
}