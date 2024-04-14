using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using APIExemplar;
using BookAPIComposer.Domain.Fetching.APIExemplar.Exemplars;
using BookAPIComposer.Domain.Shared;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace BookAPIComposer.Services.APIExemplar;

public class ExemplarGrpcService : IExemplarService
{
    private readonly APIExemplarExemplarGRPC.APIExemplarExemplarGRPCClient _client;

    public ExemplarGrpcService(string url)
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
        _client = new APIExemplarExemplarGRPC.APIExemplarExemplarGRPCClient(channel);
    }

    public async Task<List<ExemplarDto>> GetAllAsync()
    {
        try
        {
            using var call = _client.GetAllExemplars(new Empty());
            List<ExemplarDto> lstExp = new List<ExemplarDto>();
            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                lstExp.Add(new ExemplarDto(reply.ExemplarId, reply.BookId, reply.BookState, reply.SellerId,
                    reply.DateOfAcquisition));
            }

            return lstExp;
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<List<ExemplarDto>> GetByBookIdAsync(BookId id)
    {
        try
        {
            using var call = _client.GetAllExemplarsFromBook(new RequestWithISBN
            {
                Id = id.AsString()
            });
            List<ExemplarDto> lstExp = new List<ExemplarDto>();
            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                lstExp.Add(new ExemplarDto(reply.ExemplarId, reply.BookId, reply.BookState, reply.SellerId,
                    reply.DateOfAcquisition));
            }

            return lstExp;
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<List<ExemplarDto>> GetBySellerIdAsync(ClientId id)
    {
        try
        {
            using var call = _client.GetAllExemplarsFromClient(new RequestWithClientId
            {
                Id = id.AsString()
            });
            List<ExemplarDto> lstExp = new List<ExemplarDto>();
            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                lstExp.Add(new ExemplarDto(reply.ExemplarId, reply.BookId, reply.BookState, reply.SellerId,
                    reply.DateOfAcquisition));
            }

            return lstExp;
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<ExemplarDto> GetByIdAsync(ExemplarId id)
    {
        try
        {
            var reply = await _client.GetExemplarByIDAsync(new RequestWithExemplarId
            {
                Id = id.AsString()
            });
            return new ExemplarDto(reply.ExemplarId, reply.BookId, reply.BookState, reply.SellerId,
                reply.DateOfAcquisition);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<ExemplarDto> AddAsync(CreatingExemplarDto dto)
    {
        try
        {
            var reply = await _client.AddNewExemplarAsync(new CreatingExemplarGrpcDto
            {
                BookId = dto.BookId,
                BookState = dto.BookState,
                SellerId = dto.SellerId,
                DateOfAcquisition = dto.DateOfAcquisition
            });
            return new ExemplarDto(reply.ExemplarId, reply.BookId, reply.BookState, reply.SellerId,
                reply.DateOfAcquisition);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<ExemplarDto> UpdateAsync(ExemplarDto dto)
    {
        try
        {
            var reply = await _client.ModifyExemplarAsync(new ExemplarGrpcDto
            {
                ExemplarId = dto.ExemplarId,
                BookId = dto.BookId,
                BookState = dto.BookState,
                SellerId = dto.SellerId,
                DateOfAcquisition = dto.DateOfAcquisition
            });
            return new ExemplarDto(reply.ExemplarId, reply.BookId, reply.BookState, reply.SellerId,
                reply.DateOfAcquisition);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<ExemplarDto> DeleteAsync(ExemplarId id)
    {
        try
        {
            var reply = await _client.DeleteExemplarAsync(new RequestWithExemplarId
            {
                Id = id.AsString()
            });
            return new ExemplarDto(reply.ExemplarId, reply.BookId, reply.BookState, reply.SellerId,
                reply.DateOfAcquisition);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }
}