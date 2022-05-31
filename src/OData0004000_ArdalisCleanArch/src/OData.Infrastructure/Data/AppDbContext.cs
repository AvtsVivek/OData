using Ardalis.EFCore.Extensions;
using OData.Core.ProjectAggregate;
using OData.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace OData.Infrastructure.Data;

public class AppDbContext : DbContext
{
  private readonly IMediator? _mediator;

  //public AppDbContext(DbContextOptions options) : base(options)
  //{
  //}

  public AppDbContext(DbContextOptions<AppDbContext> options, IMediator? mediator)
      : base(options)
  {
    _mediator = mediator;
  }

  public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();
  public DbSet<Project> Projects => Set<Project>();

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
    base.OnModelCreating(modelBuilder);

    modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();

    // alternately this is built-in to EF Core 2.2
    //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    // ignore events if no dispatcher provided
    if (_mediator == null) return result;

    // dispatch events only if save was successful
    var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
        .Select(e => e.Entity)
        .Where(e => e.Events.Any())
        .ToArray();

    foreach (var entity in entitiesWithEvents)
    {
      var events = entity.Events.ToArray();
      entity.Events.Clear();
      foreach (var domainEvent in events)
      {
        await _mediator.Publish(domainEvent).ConfigureAwait(false);
      }
    }

    return result;
  }

  public override int SaveChanges()
  {
    return SaveChangesAsync().GetAwaiter().GetResult();
  }
}
