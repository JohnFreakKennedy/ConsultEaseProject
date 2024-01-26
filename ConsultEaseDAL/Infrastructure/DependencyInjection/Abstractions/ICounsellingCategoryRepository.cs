using System.Collections.Generic;
using System.Threading.Tasks;
using ConsultEaseDAL.Entities;

namespace ConsultEaseDAL.Infrastructure.DependencyInjection.Abstractions;

public interface ICounsellingCategoryRepository
{
    Task<IEnumerable<CounsellingCategory>> GetAllCounsellingCategoriesAsync();
    Task<CounsellingCategory?> GetCounsellingCategoryByIdAsync(int id);
    Task<CounsellingCategory?> CreateCounsellingCategoryAsync(CounsellingCategory counsellingCategory);
    Task<int> UpdateCounsellingCategoryAsync(CounsellingCategory counsellingCategory);
    Task<int> DeleteCounsellingCategoryAsync(CounsellingCategory counsellingCategory);
}