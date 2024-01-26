using AutoMapper;
using ConsultEaseBLL.Interfaces;
using ConsultEaseBLL.DTOs.CounsellingCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ConsultEaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CounsellingCategoryController: ControllerBase
{
   private readonly IMapper _mapper;
   private readonly ICounsellingCategoryService _counsellingCategoryService;

   public CounsellingCategoryController(ICounsellingCategoryService counsellingCategoryService, IMapper mapper)
   {
      _counsellingCategoryService = counsellingCategoryService;
      _mapper = mapper;
   }
   
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [HttpGet(Name = "GetCounsellingCategories")]
   public async Task<IActionResult> GetAllCounsellingCategories()
   {
      try
      {
         var counsellingCategoryDtos = await _counsellingCategoryService.GetAllCounsellingCategories();
         return Ok(counsellingCategoryDtos);
      }
      catch (Exception)
      {
         return NotFound("Counselling categories list was not found!");
      }
   }
   
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [HttpGet("{counsellingCategoryId}:int", Name = "GetCounsellingCategoryById")]
   public async Task<IActionResult> GetCounsellingCategoryById(int counsellingCategoryId)
   {
      try
      {
         var counsellingCategoryDto = await _counsellingCategoryService.GetCounsellingCategoryById(counsellingCategoryId);
         return Ok(counsellingCategoryDto);
      }
      catch (Exception)
      {
         return NotFound($"Counselling category with id {counsellingCategoryId} was not found!");
      }
   }
   
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
   [HttpPost(Name = "CreateCounsellingCategory")]
   public async Task<IActionResult> CreateCounsellingCategory([FromBody] CreateCounsellingCategoryDto newCounsellingCategory)
   {
      try
      {
         var counsellingCategory = await _counsellingCategoryService.CreateCounsellingCategory(newCounsellingCategory);
         return NoContent();
      }
      catch (Exception)
      {
         return BadRequest("Counselling category was not created!");
      }
   }
   
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
   [HttpPatch(Name = "UpdateCounsellingCategory")]
   public async Task<IActionResult> UpdateCounsellingCategory(int id, [FromBody] UpdateCounsellingCategoryDto updatedCounsellingCategory)
   {
      if (updatedCounsellingCategory.Id != id) return BadRequest("Id from body and id from route must be the same!");
      try
      {
         var counsellingCategoryId = await _counsellingCategoryService.UpdateCounsellingCategory(id, updatedCounsellingCategory);
         if(counsellingCategoryId == 0) return NotFound($"Counselling category with {counsellingCategoryId} was not found!");
         return NoContent();
      }
      catch (Exception)
      {
         return BadRequest("Counselling category was not updated!");
      }
   }
   
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
   [HttpDelete("{counsellingCategoryId:int}", Name = "DeleteCounsellingCategory")]
   public async Task<IActionResult> DeleteCounsellingCategory(int id)
   {
      try
      {
         var counsellingCategoryId = await _counsellingCategoryService.DeleteCounsellingCategory(id);
         if(counsellingCategoryId == 0) return NotFound($"Counselling category with {counsellingCategoryId} was not found!");
         return NoContent();
      }
      catch (Exception)
      {
         return BadRequest("Counselling category was not deleted!");
      }
   }
}