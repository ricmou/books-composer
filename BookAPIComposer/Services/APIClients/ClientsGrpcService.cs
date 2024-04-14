using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using APIClients;
using BookAPIComposer.Domain.Fetching.APIClients.Clients;
using BookAPIComposer.Domain.Shared;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace BookAPIComposer.Services.APIClients;

public class ClientsGrpcService : IClientsService
{
    private readonly APIClientsClientGRPC.APIClientsClientGRPCClient _client;

    public ClientsGrpcService(string url)
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
        _client = new APIClientsClientGRPC.APIClientsClientGRPCClient(channel);
    }

    public async Task<List<ClientDto>> GetAllAsync()
    {
        try
        {
            using var call = _client.GetAllClients(new Empty());
            List<ClientDto> lstClt = new List<ClientDto>();
            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                lstClt.Add(new ClientDto(reply.ClientId, reply.Name, reply.Street, reply.Local, reply.PostalCode,
                    reply.Country));
            }

            return lstClt;
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<ClientDto> GetByIdAsync(ClientId id)
    {
        try
        {
            var reply = await _client.GetClientByIDAsync(new RequestWithClientId
            {
                Id = id.AsString()
            });
            return new ClientDto(reply.ClientId, reply.Name, reply.Street, reply.Local, reply.PostalCode,
                reply.Country);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<ClientDto> AddAsync(CreatingClientDto dto)
    {
        try
        {
            var reply = await _client.AddNewClientAsync(new CreatingClientGrpcDto
            {
                Name = dto.Name,
                Street = dto.Street,
                Local = dto.Local,
                PostalCode = dto.PostalCode,
                Country = dto.Country
            });
            return new ClientDto(reply.ClientId, reply.Name, reply.Street, reply.Local, reply.PostalCode,
                reply.Country);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<ClientDto> UpdateAsync(ClientDto dto)
    {
        try
        {
            var reply = await _client.ModifyClientAsync(new ClientGrpcDto
            {
                ClientId = dto.ClientId,
                Name = dto.Name,
                Street = dto.Street,
                Local = dto.Local,
                PostalCode = dto.PostalCode,
                Country = dto.Country
            });
            return new ClientDto(reply.ClientId, reply.Name, reply.Street, reply.Local, reply.PostalCode,
                reply.Country);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<ClientDto> DeleteAsync(ClientId id)
    {
        try
        {
            var reply = await _client.DeleteClientAsync(new RequestWithClientId
            {
                Id = id.AsString()
            });
            return new ClientDto(reply.ClientId, reply.Name, reply.Street, reply.Local, reply.PostalCode,
                reply.Country);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }
}