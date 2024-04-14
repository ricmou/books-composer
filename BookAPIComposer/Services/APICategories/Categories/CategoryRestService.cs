using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APICategories.Categories;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APICategories.Categories;

public class CategoryRestService : ICategoryService
{
    private readonly HttpClient _client;
    
    public CategoryRestService(Uri uri)
    {
        _client = new HttpClient();
        _client.BaseAddress = uri;
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        List<CategoryDto> lstCat = null;
        HttpResponseMessage response = await _client.GetAsync("api/Categories/");
        if (response.IsSuccessStatusCode)
        {
            lstCat = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
        }
        
        return lstCat;
        
    }

    public async Task<CategoryDto> GetByIdAsync(CategoryId id)
    {
        CategoryDto cat = null;
        HttpResponseMessage response = await _client.GetAsync("api/Categories/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            cat = await response.Content.ReadFromJsonAsync<CategoryDto>();
        }

        return cat;
    }

    public async Task<CategoryDto> AddAsync(CreatingCategoryDto dto)
    {
        CategoryDto cat = null;
        HttpResponseMessage response = await _client.PostAsJsonAsync("api/Categories", dto);
        if (response.IsSuccessStatusCode)
        {
            cat = await response.Content.ReadFromJsonAsync<CategoryDto>();
        }

        return cat;
    }

    public async Task<CategoryDto> UpdateAsync(CategoryDto dto)
    {
        CategoryDto cat = null;
        HttpResponseMessage response = await _client.PutAsJsonAsync("api/Categories"+dto.CategoryId, dto);
        if (response.IsSuccessStatusCode)
        {
            cat = await response.Content.ReadFromJsonAsync<CategoryDto>();
        }

        return cat;
    }

    public async Task<CategoryDto> DeleteAsync(CategoryId id)
    {
        CategoryDto cat = null;
        HttpResponseMessage response = await _client.DeleteAsync("api/Categories/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            cat = await response.Content.ReadFromJsonAsync<CategoryDto>();
        }

        return cat;
    }
}