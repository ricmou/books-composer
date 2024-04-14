using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APICategories.Categories;
using BookAPIComposer.Domain.Shared;
using BookAPIComposer.Services.APICategories.Categories;
using Microsoft.AspNetCore.Mvc;

namespace BookAPIComposer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        return await _categoryService.GetAllAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetGetById(string id)
    {
        var category = await _categoryService.GetByIdAsync(new CategoryId(id));

        if (category == null)
        {
            return NotFound();
        }

        return category;
    }
    
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(CreatingCategoryDto dto)
    {
        var category = await _categoryService.AddAsync(dto);

        //return CreatedAtAction(nameof(GetGetById), new { id = category.CategoryId }, category);

        return category;
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> Update(string id, CategoryDto dto)
    {
        if (id != dto.CategoryId)
        {
            return BadRequest();
        }

        try
        {
            var publisher = await _categoryService.UpdateAsync(dto);

            if (publisher == null)
            {
                return NotFound();
            }

            return Ok(publisher);
        }
        catch (BusinessRuleValidationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<CategoryDto>> HardDelete(string id)
    {
        try
        {
            var category = await _categoryService.DeleteAsync(new CategoryId(id));

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        catch(BusinessRuleValidationException ex)
        {
            return BadRequest(new {Message = ex.Message});
        }
    }
}