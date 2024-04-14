using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIPublisher.Books;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APIPublisher.Books;

public class PublisherBookRestService : IPublisherBookService
{
    private readonly HttpClient _client;

    public PublisherBookRestService(Uri uri)
    {
        _client = new HttpClient();
        _client.BaseAddress = uri;
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }
    
    public async Task<List<PublisherBooksDto>> GetAllAsync()
    {
        List<PublisherBooksDto> lstPubBook = null;
        HttpResponseMessage response = await _client.GetAsync("api/Books/");
        if (response.IsSuccessStatusCode)
        {
            lstPubBook = await response.Content.ReadFromJsonAsync<List<PublisherBooksDto>>();
        }
        
        return lstPubBook;
    }

    public async Task<List<PublisherBooksDto>> GetAllFromPublisherAsync(PublisherId id)
    {
        List<PublisherBooksDto> lstPubBook = null;
        HttpResponseMessage response = await _client.GetAsync("api/Books/Publisher/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            lstPubBook = await response.Content.ReadFromJsonAsync<List<PublisherBooksDto>>();
        }
        
        return lstPubBook;
    }

    public async Task<PublisherBooksDto> GetByIdAsync(BookId id)
    {
        PublisherBooksDto pubBook = null;
        HttpResponseMessage response = await _client.GetAsync("api/Books/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            pubBook = await response.Content.ReadFromJsonAsync<PublisherBooksDto>();
        }
        
        return pubBook;
    }

    public async Task<PublisherBooksDto> AddAsync(PublisherCreatingBooksDto dto)
    {
        PublisherBooksDto pubBook = null;
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "api/Books", dto);
        if (response.IsSuccessStatusCode)
        {
            pubBook = await response.Content.ReadFromJsonAsync<PublisherBooksDto>();
        }

        return pubBook;
    }

    public async Task<PublisherBooksDto> UpdateAsync(PublisherCreatingBooksDto dto)
    {
        PublisherBooksDto pubBook = null;
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"api/Books/"+dto.Id, dto);
        if (response.IsSuccessStatusCode)
        {
            pubBook = await response.Content.ReadFromJsonAsync<PublisherBooksDto>();
        }

        return pubBook;
    }

    public async Task<PublisherBooksDto> DeleteAsync(BookId id)
    {
        PublisherBooksDto pubBook = null;
        HttpResponseMessage response = await _client.DeleteAsync(
            $"api/Books/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            pubBook = await response.Content.ReadFromJsonAsync<PublisherBooksDto>();
        }

        return pubBook;
    }
}