using System.Collections.Generic;
using System.Threading.Tasks;
using ConsultEaseDAL.Context;
using ConsultEaseDAL.Entities;
using ConsultEaseDAL.Infrastructure.DependencyInjection.Abstractions;
using ConsultEaseDAL.Infrastructure.DependencyInjection.Implementations.Base;
using Microsoft.EntityFrameworkCore;

namespace ConsultEaseDAL.Infrastructure.DependencyInjection.Implementations;

public class CounsellingCategoryRepository(ConsultEaseDbContext dbContext)
    : RepositoryBase<int, CounsellingCategory>(dbContext), ICounsellingCategoryRepository
{
    public async Task<IEnumerable<CounsellingCategory>> GetAllCounsellingCategoriesAsync() =>
        await dbContext.CounsellingCategories.ToListAsync();

    public async Task<CounsellingCategory?> GetCounsellingCategoryByIdAsync(int id) =>
        await FindByKeyAsync(id);

    public async Task<CounsellingCategory?> CreateCounsellingCategoryAsync(CounsellingCategory counsellingCategory) =>
        await CreateAsync(counsellingCategory);

    public async Task<int> UpdateCounsellingCategoryAsync(CounsellingCategory counsellingCategory) =>
        await UpdateAsync(counsellingCategory);

    public async Task<int> DeleteCounsellingCategoryAsync(CounsellingCategory counsellingCategory) =>
        await DeleteAsync(counsellingCategory);
}