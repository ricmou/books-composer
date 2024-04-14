using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIExemplar.Exemplars;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APIExemplar;

public class ExemplarRestService : IExemplarService
{
    private readonly HttpClient _client;

    public ExemplarRestService(Uri uri)
    {
        _client = new HttpClient();
        _client.BaseAddress = uri;
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }
    
    public async Task<List<ExemplarDto>> GetAllAsync()
    {
        List<ExemplarDto> lstExp = null;
        HttpResponseMessage response = await _client.GetAsync("api/Exemplar/");
        if (response.IsSuccessStatusCode)
        {
            lstExp = await response.Content.ReadFromJsonAsync<List<ExemplarDto>>();
        }
        
        return lstExp;
    }

    public async Task<List<ExemplarDto>> GetByBookIdAsync(BookId id)
    {
        List<ExemplarDto> lstExp = null;
        HttpResponseMessage response = await _client.GetAsync("api/Exemplar/Book/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            lstExp = await response.Content.ReadFromJsonAsync<List<ExemplarDto>>();
        }
        
        return lstExp;
    }

    public async Task<List<ExemplarDto>> GetBySellerIdAsync(ClientId id)
    {
        List<ExemplarDto> lstExp = null;
        HttpResponseMessage response = await _client.GetAsync("api/Exemplar/Client/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            lstExp = await response.Content.ReadFromJsonAsync<List<ExemplarDto>>();
        }
        
        return lstExp;
    }

    public async Task<ExemplarDto> GetByIdAsync(ExemplarId id)
    {
        ExemplarDto exp = null;
        HttpResponseMessage response = await _client.GetAsync("api/Exemplar/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            exp = await response.Content.ReadFromJsonAsync<ExemplarDto>();
        }
        
        return exp;
    }

    public async Task<ExemplarDto> AddAsync(CreatingExemplarDto dto)
    {
        ExemplarDto exp = null;
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "api/Exemplar", dto);
        if (response.IsSuccessStatusCode)
        {
            exp = await response.Content.ReadFromJsonAsync<ExemplarDto>();
        }

        return exp;
    }

    public async Task<ExemplarDto> UpdateAsync(ExemplarDto dto)
    {
        ExemplarDto exp = null;
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"api/Exemplar/"+dto.ExemplarId, dto);
        if (response.IsSuccessStatusCode)
        {
            exp = await response.Content.ReadFromJsonAsync<ExemplarDto>();
        }

        return exp;
    }

    public async Task<ExemplarDto> DeleteAsync(ExemplarId id)
    {
        ExemplarDto exp = null;
        HttpResponseMessage response = await _client.DeleteAsync("api/Exemplar/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            exp = await response.Content.ReadFromJsonAsync<ExemplarDto>();
        }
        
        return exp;
    }
}