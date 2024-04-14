using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using APIPublisher;
using BookAPIComposer.Domain.Fetching.APIPublisher.Books;
using BookAPIComposer.Domain.Fetching.APIPublisher.Publishers;
using BookAPIComposer.Domain.Shared;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace BookAPIComposer.Services.APIPublisher.Books;

public class PublisherBookGrpcService : IPublisherBookService
{
    private readonly APIPublisherBooksGRPC.APIPublisherBooksGRPCClient _client;

    public PublisherBookGrpcService(string url)
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
        _client = new APIPublisherBooksGRPC.APIPublisherBooksGRPCClient(channel);
    }

    public async Task<List<PublisherBooksDto>> GetAllAsync()
    {
        try
        {
            using var call = _client.GetAllBooks(new Empty());
            List<PublisherBooksDto> lstPubBook = new List<PublisherBooksDto>();
            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                lstPubBook.Add(new PublisherBooksDto(reply.Id,
                    new PublisherDto(reply.Publisher.PublisherId, reply.Publisher.Name, reply.Publisher.Country)));
            }

            return lstPubBook;
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<List<PublisherBooksDto>> GetAllFromPublisherAsync(PublisherId id)
    {
        try
        {
            using var call = _client.GetAllBooksFromPublisher(new RequestWithPublisherId
            {
                Id = id.AsString()
            });
            List<PublisherBooksDto> lstPubBook = new List<PublisherBooksDto>();
            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                lstPubBook.Add(new PublisherBooksDto(reply.Id,
                    new PublisherDto(reply.Publisher.PublisherId, reply.Publisher.Name, reply.Publisher.Country)));
            }

            return lstPubBook;
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<PublisherBooksDto> GetByIdAsync(BookId id)
    {
        try
        {
            var reply = await _client.GetBookByISBNAsync(new RequestWithISBN
            {
                Id = id.AsString()
            });
            return new PublisherBooksDto(reply.Id,
                new PublisherDto(reply.Publisher.PublisherId, reply.Publisher.Name, reply.Publisher.Country));
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<PublisherBooksDto> AddAsync(PublisherCreatingBooksDto dto)
    {
        try
        {
            var reply = await _client.AddNewBookAsync(new CreatingBooksGrpcDto
            {
                Id = dto.Id,
                PublisherId = dto.PublisherId,
            });
            return new PublisherBooksDto(reply.Id,
                new PublisherDto(reply.Publisher.PublisherId, reply.Publisher.Name, reply.Publisher.Country));
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<PublisherBooksDto> UpdateAsync(PublisherCreatingBooksDto dto)
    {
        try
        {
            var reply = await _client.ModifyBookAsync(new CreatingBooksGrpcDto
            {
                Id = dto.Id,
                PublisherId = dto.PublisherId,
            });

            return new PublisherBooksDto(reply.Id,
                new PublisherDto(reply.Publisher.PublisherId, reply.Publisher.Name, reply.Publisher.Country));
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<PublisherBooksDto> DeleteAsync(BookId id)
    {
        try
        {
            var reply = await _client.DeleteBookAsync(new RequestWithISBN()
            {
                Id = id.AsString()
            });

            return new PublisherBooksDto(reply.Id,
                new PublisherDto(reply.Publisher.PublisherId, reply.Publisher.Name, reply.Publisher.Country));
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }
}