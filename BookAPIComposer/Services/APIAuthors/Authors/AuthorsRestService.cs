using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIAuthors.Authors;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APIAuthors.Authors;

public class AuthorsRestService : IAuthorsService
{
    private readonly HttpClient _client;

    public AuthorsRestService(Uri uri)
    {
        _client = new HttpClient();
        _client.BaseAddress = uri;
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }
    
    public async Task<List<AuthorDto>> GetAllAsync()
    {
        List<AuthorDto> auth = null;
        HttpResponseMessage response = await _client.GetAsync("api/Authors/");
        if (response.IsSuccessStatusCode)
        {
            auth = await response.Content.ReadFromJsonAsync<List<AuthorDto>>();
        }

        return auth;
    }

    public async Task<AuthorDto> GetByIdAsync(AuthorId id)
    {
        AuthorDto auth = null;
        HttpResponseMessage response = await _client.GetAsync("api/Authors/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            auth = await response.Content.ReadFromJsonAsync<AuthorDto>();
        }
        
        return auth;
    }

    public async Task<AuthorDto> AddAsync(CreatingAuthorsDto dto)
    {
        AuthorDto auth = null;
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "api/Authors", dto);
        if (response.IsSuccessStatusCode)
        {
            auth = await response.Content.ReadFromJsonAsync<AuthorDto>();
        }
        
        return auth;
    }

    public async Task<AuthorDto> UpdateAsync(AuthorDto dto)
    {
        AuthorDto auth = null;
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            "api/Authors", dto);
        if (response.IsSuccessStatusCode)
        {
            auth = await response.Content.ReadFromJsonAsync<AuthorDto>();
        }
        
        return auth;
    }

    public async Task<AuthorDto> DeleteAsync(AuthorId id)
    {
        AuthorDto auth = null;
        HttpResponseMessage response = await _client.DeleteAsync(
            "api/Authors/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            auth = await response.Content.ReadFromJsonAsync<AuthorDto>();
        }
        
        return auth;
    }
}