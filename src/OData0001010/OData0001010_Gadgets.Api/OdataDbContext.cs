using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OData0001010_Gadgets.Api.Models;
using System;
using System.Diagnostics;

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
            // Below line to watch the ef core sql quiries generation
            // NOT FOR PRODUCTION 
            // NOT FOR PRODUCTION 
            // NOT FOR PRODUCTION 

            // To see the output from the following 'Debug logger',
            // open the Output window(View -> Output or Ctrl + Alt + O)
            // and then select Debug from the see output from dropdown.
            // Or elas you can configure console logger as below
            Action<string> consoleLogger = logInfo => Console.WriteLine(logInfo);
            Action<string> debugWindowLogger = logInfo => Debug.WriteLine(logInfo);

            optionsBuilder
            // 
            //.LogTo(consoleLogger, LogLevel.Information)

            .LogTo(debugWindowLogger, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
        }
    }
}
