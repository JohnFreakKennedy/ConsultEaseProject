using AutoMapper;
using ConsultEaseBLL.DTOs.CounsellingCategory;
using ConsultEaseBLL.Interfaces;
using ConsultEaseDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsultEaseBLL.Services;

public class CounsellingCategoryService: ICounsellingCategoryService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    
    public async Task<IEnumerable<CounsellingCategoryDto>> GetAllCounsellingCategories()
    {
        try
        {
            var counsellingCategories = await _repositoryManager.CounsellingCategoryRepository.GetAllCounsellingCategoriesAsync();
            return _mapper.Map<IEnumerable<CounsellingCategoryDto>>(counsellingCategories);
        }
        catch (Exception)
        {
            throw new Exception("Counselling categories list was not found!");
        }
    }

    public async Task<CounsellingCategoryDto> GetCounsellingCategoryById(int id)
    {
        try
        {
            var counsellingCategory = await _repositoryManager.CounsellingCategoryRepository.GetCounsellingCategoryByIdAsync(id);
            return _mapper.Map<CounsellingCategoryDto>(counsellingCategory);
        }
        catch (Exception)
        {
            throw new Exception($"Counselling category with id {id} was not found!");
        }
    }

    public async Task<CounsellingCategoryDto> CreateCounsellingCategory(CreateCounsellingCategoryDto newCounsellingCategory)
    {
        try
        {
            var counsellingCategory = _mapper.Map<CounsellingCategory>(newCounsellingCategory);
            return _mapper.Map<CounsellingCategoryDto>
                (await _repositoryManager.CounsellingCategoryRepository!.CreateCounsellingCategoryAsync(counsellingCategory));
        }
        catch (Exception)
        {
            throw new Exception("Counselling category could not be created!");
        }
    }

    public async Task<int> UpdateCounsellingCategory(int id, UpdateCounsellingCategoryDto updatedCounsellingCategory)
    {
        try
        {
            var counsellingCategory = _mapper.Map<CounsellingCategory>(updatedCounsellingCategory);
            return await _repositoryManager.CounsellingCategoryRepository.UpdateCounsellingCategoryAsync(counsellingCategory);
        }
        catch (DbUpdateException)
        {
            throw new Exception($"Counselling category with id {id} could not be updated!");
        }
    }

    public async Task<int> DeleteCounsellingCategory(int id)
    {
        try
        {
            var counsellingCategory = await _repositoryManager.CounsellingCategoryRepository.GetCounsellingCategoryByIdAsync(id);
            if (counsellingCategory == null) throw new Exception($"Counselling category with id {id} was not found!");
            return await _repositoryManager.CounsellingCategoryRepository.DeleteCounsellingCategoryAsync(counsellingCategory);
        }
        catch (Exception e)
        {
            throw new Exception($"Counselling category with id {id} could not be deleted!");
        }
    }
    
    
}