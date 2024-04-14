using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIBooks.Books;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.APIBooks;

public class BookRestService : IBookService
{
    private readonly HttpClient _client;

    public BookRestService(Uri uri)
    {
        _client = new HttpClient();
        _client.BaseAddress = uri;
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<List<BooksBooksDto>> GetAllAsync()
    {
        List<BooksBooksDto> lstBook = null;
        HttpResponseMessage response = await _client.GetAsync("api/books/");
        if (response.IsSuccessStatusCode)
        {
            lstBook = await response.Content.ReadFromJsonAsync<List<BooksBooksDto>>();
        }

        /*if (lstBook == null)
            throw new NullApiException("Bad Response from Book API to a GET Request");*/
        return lstBook;
    }

    public async Task<List<BooksBooksDto>> GetAllOfLanguage(string language)
    {
        List<BooksBooksDto> lstBook = null;
        HttpResponseMessage response = await _client.GetAsync("api/books/language/"+language);
        if (response.IsSuccessStatusCode)
        {
            lstBook = await response.Content.ReadFromJsonAsync<List<BooksBooksDto>>();
        }

        /*if (lstBook == null)
            throw new NullApiException("Bad Response from Book API to a GET Request");*/
        return lstBook;
    }

    public async Task<BooksBooksDto> GetByIdAsync(BookId id)
    {
        BooksBooksDto book = null;
        HttpResponseMessage response = await _client.GetAsync("api/books/" + id.AsString());
        if (response.IsSuccessStatusCode)
        {
            book = await response.Content.ReadFromJsonAsync<BooksBooksDto>();
        }

        return book;
    }

    public async Task<BooksBooksDto> AddAsync(BooksCreatingBooksDto dto)
    {
        //Console.WriteLine("write");
        BooksBooksDto book = null;
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "api/books", dto);
        if (response.IsSuccessStatusCode)
        {
            book = await response.Content.ReadFromJsonAsync<BooksBooksDto>();
        }

        return book;
    }

    public async Task<BooksBooksDto> UpdateAsync(BooksBooksDto dto)
    {
        BooksBooksDto book = null;
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"api/books/" + dto.isbn, dto);
        if (response.IsSuccessStatusCode)
        {
            book = await response.Content.ReadFromJsonAsync<BooksBooksDto>();
        }

        return book;
    }

    public async Task<bool> DeleteAsync(BookId id)
    {
        HttpResponseMessage response = await _client.DeleteAsync(
            $"api/books/" + id.AsString());
        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }
}