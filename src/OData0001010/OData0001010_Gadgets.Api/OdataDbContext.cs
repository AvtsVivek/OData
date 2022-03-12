using Microsoft.EntityFrameworkCore;
using OData0001010_Gadgets.Api.Models;
using System;

namespace OData0001010_Gadgets.Api
{

    public class OdataDbContext : DbContext
    {
        public OdataDbContext(DbContextOptions<OdataDbContext> options) : base(options)
        {

        }
        public DbSet<Gadgets> Gadgets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // below line to watch the ef core sql quiries generation
            // not at all recomonded for the production code
            optionsBuilder.LogTo(Console.WriteLine);
        }
    }
}
