using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APICategories.Books;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APICategories.Books;

public interface ICategoryBookService
{
    Task<List<CategoryBooksDto>> GetAllAsync();
    Task<CategoryBooksDto> GetByIdAsync(BookId id);
    Task<CategoryBooksDto> AddAsync(CategoryCreatingBooksDto dto);
    Task<CategoryBooksDto> UpdateAsync(CategoryCreatingBooksDto dto);
    Task<CategoryBooksDto> DeleteAsync(BookId id);
}