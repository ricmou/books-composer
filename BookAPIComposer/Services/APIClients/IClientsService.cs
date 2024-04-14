using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIClients.Clients;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APIClients;

public interface IClientsService
{
    Task<List<ClientDto>> GetAllAsync();
    Task<ClientDto> GetByIdAsync(ClientId id);
    Task<ClientDto> AddAsync(CreatingClientDto dto);
    Task<ClientDto> UpdateAsync(ClientDto dto);
    Task<ClientDto> DeleteAsync(ClientId id);
}