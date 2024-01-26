using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConsultEaseDAL.Infrastructure.DependencyInjection.Abstractions.Base;


namespace ConsultEaseDAL.Infrastructure.DependencyInjection.Implementations.Base;

public abstract class RepositoryBase<TKey, TEntity>: IRepositoryBase<TKey, TEntity>
    where TEntity: class
    where TKey : IEquatable<TKey>
{
    private readonly bool _disposeContex;
    private bool _isDisposed;
    private DbContext DbContext { get; set; }
    protected DbSet<TEntity> Table { get; }
    
    protected RepositoryBase(DbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        Table = DbContext.Set<TEntity>();
        _disposeContex = false;
    }

    public async Task<TEntity?> FindByKeyAsync(TKey key)
    {
        return await Table.FindAsync(key);
    }

    public async Task<TEntity?> CreateAsync(TEntity entity)
    {
        var entry = await Table.AddAsync(entity);
        await DbContext.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<int> UpdateAsync(TEntity entity)
    {   
        DbContext.Entry(entity).State = EntityState.Modified;
        var result = await DbContext.SaveChangesAsync();
        return result;
    }

    public async Task<int> DeleteAsync(TEntity entity)
    {
        Table.Remove(entity);
        var result = await  DbContext.SaveChangesAsync();
        return result;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;
        if (disposing && _disposeContex) DbContext?.Dispose();

        _isDisposed = true;
    }
  
     ~RepositoryBase()
     {
         Dispose(true);
     }
     
     public void Dispose()
     {
         Dispose(true);
         GC.SuppressFinalize(this);
    }
}