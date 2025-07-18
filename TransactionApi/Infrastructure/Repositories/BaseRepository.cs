﻿using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TransactionApi.Infrastructure.Data;

namespace TransactionApi.Infrastructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    private readonly ApiContext _context;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(ApiContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T?> GetById(Guid id)
    {
        return await _dbSet.FindAsync(id) ?? default;
    }

    public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().Where(expression).ToListAsync(cancellationToken);
    }

    public async Task Add(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}
