using Ardalis.Specification;

namespace OData.SharedKernel.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
  IQueryable<T> GetQueryBySpec<Spec>(Spec specification, CancellationToken cancellationToken = default) where Spec : ISpecification<T>;
}
