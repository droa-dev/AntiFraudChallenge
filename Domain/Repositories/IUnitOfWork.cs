namespace Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChanges(CancellationToken cancellationToken = default);
}
