using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIPublisher.Publishers;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APIPublisher.Publishers;

public interface IPublisherService
{
    Task<List<PublisherDto>> GetAllAsync();
    Task<PublisherDto> GetByIdAsync(PublisherId id);
    Task<PublisherDto> AddAsync(CreatingPublisherDto dto);
    Task<PublisherDto> UpdateAsync(PublisherDto dto);
    Task<PublisherDto> DeleteAsync(PublisherId id);
}