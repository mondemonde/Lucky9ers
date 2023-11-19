using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lucky9.Application.Common.Interfaces;
using Lucky9.Application.Interfaces;
using Lucky9.Infrastructure.Persistence;

namespace Lucky9.Infrastructure.Data;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DataContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public RepositoryBase(IDataContext dbContext)
    {
        //var ef = dbContext as DataContext;
        // _dbContext = ef ?? throw new ArgumentNullException(nameof(dbContext));
         _dbSet = dbContext.Set<TEntity>();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.SingleOrDefaultAsync(predicate);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        var result = _dbSet.AddAsync(entity);
        return entity;
    }

    public TEntity AddFast(TEntity entity)
    {
       var result =  _dbSet.Add(entity);
        _dbContext.SaveChangesAsync();
       // var last = _dbSet.OrderBy(t=>t.id).LastOrDefault();
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public Task<int> SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }


}

