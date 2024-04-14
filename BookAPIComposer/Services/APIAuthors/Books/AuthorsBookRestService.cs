using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIAuthors.Books;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APIAuthors.Books;

public class AuthorsBookRestService : IAuthorsBookService
{
    
    private readonly HttpClient _client;

    public AuthorsBookRestService(Uri uri)
    {
        _client = new HttpClient();
        _client.BaseAddress = uri;
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }
    
    public async Task<List<AuthorsBooksDto>> GetAllAsync()
    {
        List<AuthorsBooksDto> lstAuthBook = null;
        HttpResponseMessage response = await _client.GetAsync("api/Books/");
        if (response.IsSuccessStatusCode)
        {
            lstAuthBook = await response.Content.ReadFromJsonAsync<List<AuthorsBooksDto>>();
        }
        
        return lstAuthBook;
    }

    public async Task<List<AuthorsBooksDto>> GetByAuthorIdAsync(AuthorId id)
    {
        List<AuthorsBooksDto> lstAuthBook = null;
        HttpResponseMessage response = await _client.GetAsync("api/Books/Author/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            lstAuthBook = await response.Content.ReadFromJsonAsync<List<AuthorsBooksDto>>();
        }
        
        return lstAuthBook;
    }

    public async Task<AuthorsBooksDto> GetByIdAsync(BookId id)
    {
        AuthorsBooksDto auth = null;
        HttpResponseMessage response = await _client.GetAsync("api/Books/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            auth = await response.Content.ReadFromJsonAsync<AuthorsBooksDto>();
        }

        return auth;
    }

    public async Task<AuthorsBooksDto> AddAsync(AuthorsCreatingBooksDto dto)
    {
        AuthorsBooksDto auth = null;
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "api/Books", dto);
        if (response.IsSuccessStatusCode)
        {
            auth = await response.Content.ReadFromJsonAsync<AuthorsBooksDto>();
        }

        return auth;
    }

    public async Task<AuthorsBooksDto> UpdateAsync(AuthorsCreatingBooksDto dto)
    {
        AuthorsBooksDto auth = null;
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"api/Books/"+dto.Id, dto);
        if (response.IsSuccessStatusCode)
        {
            auth = await response.Content.ReadFromJsonAsync<AuthorsBooksDto>();
        }

        return auth;
    }

    public async Task<AuthorsBooksDto> DeleteAsync(BookId id)
    {
        AuthorsBooksDto auth = null;
        HttpResponseMessage response = await _client.DeleteAsync(
            $"api/Books/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            auth = await response.Content.ReadFromJsonAsync<AuthorsBooksDto>();
        }

        return auth;
    }
}