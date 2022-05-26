using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using OData.SharedKernel.Interfaces;

namespace OData.Infrastructure.Data;

// inherit from Ardalis.Specification type
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
  public EfRepository(AppDbContext dbContext) : base(dbContext)
  {
  }

  public IQueryable<T> GetQueryBySpec<Spec>(Spec specification, CancellationToken cancellationToken = default) where Spec : ISpecification<T>
  {
    var query = ApplySpecification(specification);
    return query;
  }
}
