using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using APIAuthors;
using BookAPIComposer.Domain.Fetching.APIAuthors.Authors;
using BookAPIComposer.Domain.Shared;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace BookAPIComposer.Services.APIAuthors.Authors;

public class AuthorsGrpcService : IAuthorsService
{
    private readonly APIAuthorsAuthorGRPC.APIAuthorsAuthorGRPCClient _client;

    public AuthorsGrpcService(string url)
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
        _client = new APIAuthorsAuthorGRPC.APIAuthorsAuthorGRPCClient(channel);
    }

    public async Task<List<AuthorDto>> GetAllAsync()
    {
        try
        {
            using var call = _client.GetAllAuthors(new Empty());
            List<AuthorDto> lstAuth = new List<AuthorDto>();
            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                lstAuth.Add(new AuthorDto(reply.AuthorId, reply.FirstName, reply.LastName, reply.BirthDate,
                    reply.Country));
            }

            return lstAuth;
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<AuthorDto> GetByIdAsync(AuthorId id)
    {
        try
        {
            var reply = await _client.GetAuthorByIDAsync(new RequestWithAuthorId
            {
                Id = id.AsString()
            });
            return new AuthorDto(reply.AuthorId, reply.FirstName, reply.LastName, reply.BirthDate, reply.Country);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<AuthorDto> AddAsync(CreatingAuthorsDto dto)
    {
        try
        {
            var reply = await _client.AddNewAuthorAsync(new CreatingAuthorGrpcDto
            {
                AuthorId = dto.AuthorId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                Country = dto.Country
            });
            return new AuthorDto(reply.AuthorId, reply.FirstName, reply.LastName, reply.BirthDate, reply.Country);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<AuthorDto> UpdateAsync(AuthorDto dto)
    {
        try
        {
            var reply = await _client.ModifyAuthorAsync(new AuthorGrpcDto
            {
                AuthorId = dto.AuthorId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                Country = dto.Country
            });
            return new AuthorDto(reply.AuthorId, reply.FirstName, reply.LastName, reply.BirthDate, reply.Country);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<AuthorDto> DeleteAsync(AuthorId id)
    {
        try
        {
            var reply = await _client.DeleteAuthorAsync(new RequestWithAuthorId
            {
                Id = id.AsString()
            });
            return new AuthorDto(reply.AuthorId, reply.FirstName, reply.LastName, reply.BirthDate, reply.Country);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }
}