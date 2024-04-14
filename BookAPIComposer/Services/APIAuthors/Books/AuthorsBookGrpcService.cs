using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using APIAuthors;
using BookAPIComposer.Domain.Fetching.APIAuthors.Authors;
using BookAPIComposer.Domain.Fetching.APIAuthors.Books;
using BookAPIComposer.Domain.Shared;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace BookAPIComposer.Services.APIAuthors.Books;

public class AuthorsBookGrpcService : IAuthorsBookService
{
    private readonly APIAuthorsBooksGRPC.APIAuthorsBooksGRPCClient _client;

    public AuthorsBookGrpcService(string url)
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
        _client = new APIAuthorsBooksGRPC.APIAuthorsBooksGRPCClient(channel);
    }

    public async Task<List<AuthorsBooksDto>> GetAllAsync()
    {
        try
        {
            using var call = _client.GetAllBooks(new Empty());
            List<AuthorsBooksDto> lstAuthBook = new List<AuthorsBooksDto>();
            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                lstAuthBook.Add(new AuthorsBooksDto(reply.Id, reply.Authors.ToList().ConvertAll(auth =>
                    new AuthorDto(auth.AuthorId, auth.FirstName, auth.LastName, auth.BirthDate, auth.Country))));
            }

            return lstAuthBook;
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<List<AuthorsBooksDto>> GetByAuthorIdAsync(AuthorId id)
    {
        try
        {
            using var call = _client.GetAllBooksFromAuthor(new RequestWithAuthorId
            {
                Id = id.AsString()
            });
            List<AuthorsBooksDto> lstAuthBook = new List<AuthorsBooksDto>();
            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                lstAuthBook.Add(new AuthorsBooksDto(reply.Id, reply.Authors.ToList().ConvertAll(auth =>
                    new AuthorDto(auth.AuthorId, auth.FirstName, auth.LastName, auth.BirthDate, auth.Country))));
            }

            return lstAuthBook;
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<AuthorsBooksDto> GetByIdAsync(BookId id)
    {
        try
        {
            var reply = await _client.GetBookByISBNAsync(new RequestWithISBN
            {
                Id = id.AsString()
            });
            return new AuthorsBooksDto(reply.Id, reply.Authors.ToList().ConvertAll(auth =>
                new AuthorDto(auth.AuthorId, auth.FirstName, auth.LastName, auth.BirthDate, auth.Country)));
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<AuthorsBooksDto> AddAsync(AuthorsCreatingBooksDto dto)
    {
        try
        {
            var reply = await _client.AddNewBookAsync(new CreatingBooksGrpcDto
            {
                Id = dto.Id,
                Authors = { dto.Authors }
            });
            return new AuthorsBooksDto(reply.Id, reply.Authors.ToList().ConvertAll(auth =>
                new AuthorDto(auth.AuthorId, auth.FirstName, auth.LastName, auth.BirthDate, auth.Country)));
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<AuthorsBooksDto> UpdateAsync(AuthorsCreatingBooksDto dto)
    {
        try
        {
            var reply = await _client.ModifyBookAsync(new CreatingBooksGrpcDto
            {
                Id = dto.Id,
                Authors = { dto.Authors }
            });
            return new AuthorsBooksDto(reply.Id, reply.Authors.ToList().ConvertAll(auth =>
                new AuthorDto(auth.AuthorId, auth.FirstName, auth.LastName, auth.BirthDate, auth.Country)));
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<AuthorsBooksDto> DeleteAsync(BookId id)
    {
        try
        {
            var reply = await _client.DeleteBookAsync(new RequestWithISBN
            {
                Id = id.AsString()
            });
            return new AuthorsBooksDto(reply.Id, reply.Authors.ToList().ConvertAll(auth =>
                new AuthorDto(auth.AuthorId, auth.FirstName, auth.LastName, auth.BirthDate, auth.Country)));
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }
}