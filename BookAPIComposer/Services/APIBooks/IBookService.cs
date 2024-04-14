using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIBooks.Books;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APIBooks;

public interface IBookService
{
    Task<List<BooksBooksDto>> GetAllAsync();
    Task<List<BooksBooksDto>> GetAllOfLanguage(string language);
    Task<BooksBooksDto> GetByIdAsync(BookId id);
    Task<BooksBooksDto> AddAsync(BooksCreatingBooksDto dto);
    Task<BooksBooksDto> UpdateAsync(BooksBooksDto dto);
    Task<bool> DeleteAsync(BookId id);
}