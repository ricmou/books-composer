using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIExemplar.Exemplars;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APIExemplar;

public interface IExemplarService
{
    Task<List<ExemplarDto>> GetAllAsync();
    Task<List<ExemplarDto>> GetByBookIdAsync(BookId id);
    Task<List<ExemplarDto>> GetBySellerIdAsync(ClientId id);
    Task<ExemplarDto> GetByIdAsync(ExemplarId id);
    Task<ExemplarDto> AddAsync(CreatingExemplarDto dto);
    Task<ExemplarDto> UpdateAsync(ExemplarDto dto);
    Task<ExemplarDto> DeleteAsync(ExemplarId id);
}