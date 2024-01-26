using ConsultEaseBLL.DTOs.CounsellingCategory;

namespace ConsultEaseBLL.Interfaces;

public interface ICounsellingCategoryService
{
    Task<IEnumerable<CounsellingCategoryDto>> GetAllCounsellingCategories();
    Task<CounsellingCategoryDto> GetCounsellingCategoryById(int id);
    Task<CounsellingCategoryDto> CreateCounsellingCategory(CreateCounsellingCategoryDto newCounsellingCategory);
    Task<int> UpdateCounsellingCategory(int id, UpdateCounsellingCategoryDto updatedCounsellingCategory);
    Task<int> DeleteCounsellingCategory(int id);
}