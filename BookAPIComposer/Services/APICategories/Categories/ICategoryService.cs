using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APICategories.Categories;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APICategories.Categories;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto> GetByIdAsync(CategoryId id);
    Task<CategoryDto> AddAsync(CreatingCategoryDto dto);
    Task<CategoryDto> UpdateAsync(CategoryDto dto);
    Task<CategoryDto> DeleteAsync(CategoryId id);
}
