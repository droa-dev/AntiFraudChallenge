using System.Linq.Expressions;

namespace Domain.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetById(Guid id);
    Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    Task Add(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
}
