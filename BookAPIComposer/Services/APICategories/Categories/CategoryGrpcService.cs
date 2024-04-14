using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using APICategories;
using BookAPIComposer.Domain.Fetching.APICategories.Categories;
using BookAPIComposer.Domain.Shared;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace BookAPIComposer.Services.APICategories.Categories;

public class CategoryGrpcService : ICategoryService
{
    private readonly APICategoriesCategoryGRPC.APICategoriesCategoryGRPCClient _client;

    public CategoryGrpcService(string url)
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
        _client = new APICategoriesCategoryGRPC.APICategoriesCategoryGRPCClient(channel);
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        try
        {
            using var call = _client.GetAllCategories(new Empty());
            List<CategoryDto> lstCat = new List<CategoryDto>();
            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                lstCat.Add(new CategoryDto(reply.CategoryId, reply.Name));
            }

            return lstCat;
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<CategoryDto> GetByIdAsync(CategoryId id)
    {
        try
        {
            var reply = await _client.GetCategoryByIDAsync(new RequestWithCategoryId
            {
                Id = id.AsString()
            });
            return new CategoryDto(reply.CategoryId, reply.Name);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<CategoryDto> AddAsync(CreatingCategoryDto dto)
    {
        try
        {
            var reply = await _client.AddNewCategoryAsync(new CreatingCategoryGrpcDto
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name
            });
            return new CategoryDto(reply.CategoryId, reply.Name);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<CategoryDto> UpdateAsync(CategoryDto dto)
    {
        try
        {
            var reply = await _client.ModifyCategoryAsync(new CategoryGrpcDto
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name
            });
            return new CategoryDto(reply.CategoryId, reply.Name);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<CategoryDto> DeleteAsync(CategoryId id)
    {
        try
        {
            var reply = await _client.DeleteCategoryAsync(new RequestWithCategoryId
            {
                Id = id.AsString()
            });
            return new CategoryDto(reply.CategoryId, reply.Name);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }
}