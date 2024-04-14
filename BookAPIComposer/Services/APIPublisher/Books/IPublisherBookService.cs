using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIPublisher.Books;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APIPublisher.Books;

public interface IPublisherBookService
{
    Task<List<PublisherBooksDto>> GetAllAsync();
    Task<List<PublisherBooksDto>> GetAllFromPublisherAsync(PublisherId id);
    Task<PublisherBooksDto> GetByIdAsync(BookId id);
    Task<PublisherBooksDto> AddAsync(PublisherCreatingBooksDto dto);
    Task<PublisherBooksDto> UpdateAsync(PublisherCreatingBooksDto dto);
    Task<PublisherBooksDto> DeleteAsync(BookId id);
}