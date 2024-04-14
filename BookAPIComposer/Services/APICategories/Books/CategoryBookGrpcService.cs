using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using APICategories;
using BookAPIComposer.Domain.Fetching.APICategories.Books;
using BookAPIComposer.Domain.Fetching.APICategories.Categories;
using BookAPIComposer.Domain.Shared;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace BookAPIComposer.Services.APICategories.Books;

public class CategoryBookGrpcService : ICategoryBookService
{
    private readonly APICategoriesBooksGRPC.APICategoriesBooksGRPCClient _client;

    public CategoryBookGrpcService(string url)
    {
        var handler = new SocketsHttpHandler
        {
            PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
            KeepAlivePingDelay = TimeSpan.FromSeconds(60),
            KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
            EnableMultipleHttp2Connections = true
        };
        var channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions
        {
            HttpHandler = handler
        });
        _client = new APICategoriesBooksGRPC.APICategoriesBooksGRPCClient(channel);
    }

    public async Task<List<CategoryBooksDto>> GetAllAsync()
    {
        try
        {
            using var call = _client.GetAllBooks(new Empty());
            List<CategoryBooksDto> lstCatBook = new List<CategoryBooksDto>();
            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                lstCatBook.Add(new CategoryBooksDto(reply.Id, reply.Categories.ToList().ConvertAll(cat =>
                    new CategoryDto(cat.CategoryId, cat.Name))));
            }

            return lstCatBook;
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<CategoryBooksDto> GetByIdAsync(BookId id)
    {
        try
        {
            var reply = await _client.GetBookByISBNAsync(new RequestWithISBN
            {
                Id = id.AsString()
            });
            return new CategoryBooksDto(reply.Id, reply.Categories.ToList().ConvertAll(cat =>
                new CategoryDto(cat.CategoryId, cat.Name)));
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<CategoryBooksDto> AddAsync(CategoryCreatingBooksDto dto)
    {
        try
        {
            var reply = await _client.AddNewBookAsync(new CreatingBooksGrpcDto
            {
                Id = dto.Id,
                Categories = { dto.Categories }
            });
            return new CategoryBooksDto(reply.Id, reply.Categories.ToList().ConvertAll(cat =>
                new CategoryDto(cat.CategoryId, cat.Name)));
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<CategoryBooksDto> UpdateAsync(CategoryCreatingBooksDto dto)
    {
        try
        {
            var reply = await _client.ModifyBookAsync(new CreatingBooksGrpcDto
            {
                Id = dto.Id,
                Categories = { dto.Categories }
            });
            return new CategoryBooksDto(reply.Id, reply.Categories.ToList().ConvertAll(cat =>
                new CategoryDto(cat.CategoryId, cat.Name)));
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<CategoryBooksDto> DeleteAsync(BookId id)
    {
        try
        {
            var reply = await _client.DeleteBookAsync(new RequestWithISBN
            {
                Id = id.AsString()
            });
            return new CategoryBooksDto(reply.Id, reply.Categories.ToList().ConvertAll(cat =>
                new CategoryDto(cat.CategoryId, cat.Name)));
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }
}