using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using APIPublisher;
using BookAPIComposer.Domain.Fetching.APIPublisher.Publishers;
using BookAPIComposer.Domain.Shared;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace BookAPIComposer.Services.APIPublisher.Publishers;

public class PublisherGrpcService : IPublisherService
{
    private readonly APIPublisherPublisherGRPC.APIPublisherPublisherGRPCClient _client;

    public PublisherGrpcService(string url)
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
        _client = new APIPublisherPublisherGRPC.APIPublisherPublisherGRPCClient(channel);
    }

    public async Task<List<PublisherDto>> GetAllAsync()
    {
        try
        {
            using var call = _client.GetAllPublishers(new Empty());
            List<PublisherDto> lstPub = new List<PublisherDto>();
            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                lstPub.Add(new PublisherDto(reply.PublisherId, reply.Name, reply.Country));
            }

            return lstPub;
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<PublisherDto> GetByIdAsync(PublisherId id)
    {
        try
        {
            var reply = await _client.GetPublisherByIDAsync(new RequestWithPublisherId
            {
                Id = id.AsString()
            });
            //Console.WriteLine(reply);
            return new PublisherDto(reply.PublisherId, reply.Name, reply.Country);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<PublisherDto> AddAsync(CreatingPublisherDto dto)
    {
        try
        {
            var reply = await _client.AddNewPublisherAsync(new CreatingPublisherGrpcDto
            {
                PublisherId = dto.PublisherId,
                Name = dto.Name,
                Country = dto.Country
            });
            return new PublisherDto(reply.PublisherId, reply.Name, reply.Country);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<PublisherDto> UpdateAsync(PublisherDto dto)
    {
        try
        {
            var reply = await _client.ModifyPublisherAsync(new PublisherGrpcDto
            {
                PublisherId = dto.PublisherId,
                Name = dto.Name,
                Country = dto.Country
            });

            return new PublisherDto(reply.PublisherId, reply.Name, reply.Country);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<PublisherDto> DeleteAsync(PublisherId id)
    {
        try
        {
            var reply = await _client.DeletePublisherAsync(new RequestWithPublisherId
            {
                Id = id.AsString()
            });

            return new PublisherDto(reply.PublisherId, reply.Name, reply.Country);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }
}