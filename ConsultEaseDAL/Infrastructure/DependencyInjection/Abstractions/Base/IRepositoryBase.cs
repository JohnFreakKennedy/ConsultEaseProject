using System.Threading.Tasks;


namespace ConsultEaseDAL.Infrastructure.DependencyInjection.Abstractions.Base;

public interface IRepositoryBase<TKey, TEntity>
{
    Task<TEntity?> FindByKeyAsync(TKey key);
    Task<TEntity?> CreateAsync(TEntity entity);
    Task<int> UpdateAsync(TEntity entity);
    Task<int> DeleteAsync(TEntity entity);
}