using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using TransactionApi.Infrastructure.Data;

namespace TransactionApi.Infrastructure.Repositories
{
    public class UnitOfWork(ApiContext context, TimeProvider dateTime) : IUnitOfWork
    {
        public async Task<int> SaveChanges(CancellationToken cancellationToken = default)
        {
            foreach (var entry in context.ChangeTracker.Entries<IEntityBase>())
            {
                var utcNow = dateTime.GetUtcNow();
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = utcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = utcNow;
                        break;
                }
            }

            return await context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
