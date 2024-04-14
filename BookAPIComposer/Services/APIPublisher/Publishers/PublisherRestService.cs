using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIPublisher.Publishers;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APIPublisher.Publishers;

public class PublisherRestService : IPublisherService
{
    private readonly HttpClient _client;

    public PublisherRestService(Uri uri)
    {
        _client = new HttpClient();
        _client.BaseAddress = uri;
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }
    
    public async Task<List<PublisherDto>> GetAllAsync()
    {
        List<PublisherDto> lstPub = null;
        HttpResponseMessage response = await _client.GetAsync("api/Publishers/");
        if (response.IsSuccessStatusCode)
        {
            lstPub = await response.Content.ReadFromJsonAsync<List<PublisherDto>>();
        }
        
        return lstPub;
    }

    public async Task<PublisherDto> GetByIdAsync(PublisherId id)
    {
        PublisherDto pub = null;
        HttpResponseMessage response = await _client.GetAsync("api/Publishers/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            pub = await response.Content.ReadFromJsonAsync<PublisherDto>();
        }
        
        return pub;
    }

    public async Task<PublisherDto> AddAsync(CreatingPublisherDto dto)
    {
        PublisherDto pub = null;
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "api/Publishers", dto);
        if (response.IsSuccessStatusCode)
        {
            pub = await response.Content.ReadFromJsonAsync<PublisherDto>();
        }

        return pub;
    }

    public async Task<PublisherDto> UpdateAsync(PublisherDto dto)
    {
        PublisherDto pub = null;
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"api/Publishers/"+dto.PublisherId, dto);
        if (response.IsSuccessStatusCode)
        {
            pub = await response.Content.ReadFromJsonAsync<PublisherDto>();
        }

        return pub;
    }

    public async Task<PublisherDto> DeleteAsync(PublisherId id)
    {
        PublisherDto pub = null;
        HttpResponseMessage response = await _client.DeleteAsync(
            $"api/Publishers/"+id.AsString());
        if (response.IsSuccessStatusCode)
        {
            pub = await response.Content.ReadFromJsonAsync<PublisherDto>();
        }

        return pub;
    }
}