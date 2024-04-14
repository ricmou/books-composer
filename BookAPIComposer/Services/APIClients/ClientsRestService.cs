using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIClients.Clients;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APIClients;

public class ClientsRestService : IClientsService
{
    private readonly HttpClient _client;

    public ClientsRestService(Uri uri)
    {
        _client = new HttpClient();
        _client.BaseAddress = uri;
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<List<ClientDto>> GetAllAsync()
    {
        List<ClientDto> lstCli = null;
        HttpResponseMessage response = await _client.GetAsync("api/Clients/");
        if (response.IsSuccessStatusCode)
        {
            lstCli = await response.Content.ReadFromJsonAsync<List<ClientDto>>();
        }
        
        return lstCli;
    }

    public async Task<ClientDto> GetByIdAsync(ClientId id)
    {
        ClientDto cli = null;
        HttpResponseMessage response = await _client.GetAsync("api/Clients/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            cli = await response.Content.ReadFromJsonAsync<ClientDto>();
        }

        return cli;
    }

    public async Task<ClientDto> AddAsync(CreatingClientDto dto)
    {
        ClientDto cli = null;
        HttpResponseMessage response = await _client.PostAsJsonAsync("api/Clients", dto);
        if (response.IsSuccessStatusCode)
        {
            cli = await response.Content.ReadFromJsonAsync<ClientDto>();
        }

        return cli;
    }

    public async Task<ClientDto> UpdateAsync(ClientDto dto)
    {
        ClientDto cli = null;
        HttpResponseMessage response = await _client.PutAsJsonAsync("api/Clients/"+dto.ClientId, dto);
        if (response.IsSuccessStatusCode)
        {
            cli = await response.Content.ReadFromJsonAsync<ClientDto>();
        }

        return cli;
    }

    public async Task<ClientDto> DeleteAsync(ClientId id)
    {
        ClientDto cli = null;
        HttpResponseMessage response = await _client.DeleteAsync("api/Clients/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            cli = await response.Content.ReadFromJsonAsync<ClientDto>();
        }

        return cli;
    }
}