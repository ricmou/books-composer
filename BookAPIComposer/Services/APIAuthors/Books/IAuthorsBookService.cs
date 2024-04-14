using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIAuthors.Books;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APIAuthors.Books;

public interface IAuthorsBookService
{
    Task<List<AuthorsBooksDto>> GetAllAsync();
    Task<List<AuthorsBooksDto>> GetByAuthorIdAsync(AuthorId id);
    Task<AuthorsBooksDto> GetByIdAsync(BookId id);
    Task<AuthorsBooksDto> AddAsync(AuthorsCreatingBooksDto dto);
    Task<AuthorsBooksDto> UpdateAsync(AuthorsCreatingBooksDto dto);
    Task<AuthorsBooksDto> DeleteAsync(BookId id);
}