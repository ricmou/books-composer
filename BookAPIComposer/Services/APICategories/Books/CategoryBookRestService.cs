using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APICategories.Books;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APICategories.Books;

public class CategoryBookRestService : ICategoryBookService
{
    private readonly HttpClient _client;

    public CategoryBookRestService(Uri uri)
    {
        _client = new HttpClient();
        _client.BaseAddress = uri;
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }
    
    public async Task<List<CategoryBooksDto>> GetAllAsync()
    {
        List<CategoryBooksDto> lstCatBook = null;
        HttpResponseMessage response = await _client.GetAsync("api/Books/");
        if (response.IsSuccessStatusCode)
        {
            lstCatBook = await response.Content.ReadFromJsonAsync<List<CategoryBooksDto>>();
        }
        
        return lstCatBook;
    }

    public async Task<CategoryBooksDto> GetByIdAsync(BookId id)
    {
        CategoryBooksDto catBook = null;
        HttpResponseMessage response = await _client.GetAsync("api/Books/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            catBook = await response.Content.ReadFromJsonAsync<CategoryBooksDto>();
        }

        return catBook;
    }

    public async Task<CategoryBooksDto> AddAsync(CategoryCreatingBooksDto dto)
    {
        CategoryBooksDto catBook = null;
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "api/Books", dto);
        if (response.IsSuccessStatusCode)
        {
            catBook = await response.Content.ReadFromJsonAsync<CategoryBooksDto>();
        }

        return catBook;
    }

    public async Task<CategoryBooksDto> UpdateAsync(CategoryCreatingBooksDto dto)
    {
        CategoryBooksDto catBook = null;
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"api/Books/"+dto.Id, dto);
        if (response.IsSuccessStatusCode)
        {
            catBook = await response.Content.ReadFromJsonAsync<CategoryBooksDto>();
        }

        return catBook;
    }

    public async Task<CategoryBooksDto> DeleteAsync(BookId id)
    {
        CategoryBooksDto catBook = null;
        HttpResponseMessage response = await _client.DeleteAsync(
            $"api/Books/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            catBook = await response.Content.ReadFromJsonAsync<CategoryBooksDto>();
        }

        return catBook;
    }
}