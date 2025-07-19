using Microsoft.EntityFrameworkCore;

namespace TransactionApi.Infrastructure.Data;

public class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
{
    public DbSet<Domain.Models.Account> Accounts { get; set; }
    public DbSet<Domain.Models.Transaction> Transactions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiContext).Assembly);
    }
}
