using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIAuthors.Authors;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APIAuthors.Authors;

public interface IAuthorsService
{
    Task<List<AuthorDto>> GetAllAsync();
    
    Task<AuthorDto> GetByIdAsync(AuthorId id);
    Task<AuthorDto> AddAsync(CreatingAuthorsDto dto);
    Task<AuthorDto> UpdateAsync(AuthorDto dto);
    Task<AuthorDto> DeleteAsync(AuthorId id);
}