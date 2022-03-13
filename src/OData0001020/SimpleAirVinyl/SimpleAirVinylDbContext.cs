using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SimpleAirVinyl.Entities;
using System.Diagnostics;
using System.Text.Json;

namespace SimpleAirVinyl
{
    public class SimpleAirVinylDbContext : DbContext
    {
        public DbSet<Person> People { get; set; } = default!;
        public DbSet<VinylRecord> VinylRecords { get; set; } = default!;
        public DbSet<RecordStore> RecordStores { get; set; } = default!;
        public DbSet<PressingDetail> PressingDetails { get; set; } = default!;

        public SimpleAirVinylDbContext(DbContextOptions<SimpleAirVinylDbContext> options)
           : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Below line to watch the ef core sql quiries
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PressingDetail>();

            modelBuilder.Entity<Person>().Property(p => p.AmountOfCashToSpend).HasColumnType("decimal(8,2)");

            modelBuilder.Entity<VinylRecord>();

            // comparer & converter for storing a list of strings
            var stringListValueComparer = new ValueComparer<List<string>>(
                  (v1, v2) => v1!.SequenceEqual(v2!),
                  v => v.Aggregate(0, (a, f) => HashCode.Combine(a, f.GetHashCode())),
                  v => v.ToList());

            modelBuilder.Entity<RecordStore>()
                .Property(e => e.Tags)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null!)!);

            modelBuilder.Entity<RecordStore>()
               .Property(r => r.Tags)
               .Metadata
               .SetValueComparer(stringListValueComparer);

            // address is an owned type (= type without id)
            modelBuilder.Entity<RecordStore>().OwnsOne(p => p.StoreAddress);

            modelBuilder.Entity<SpecializedRecordStore>(c =>
            {
                c.OwnsOne(r => r.StoreAddress);
            });

            modelBuilder.Entity<RecordStore>(c =>
            {
                c.OwnsOne(r => r.StoreAddress);
            });

            modelBuilder.Entity<Rating>();
        }
    }
}
