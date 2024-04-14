using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using APIBooksRPC;
using BookAPIComposer.Domain.Fetching.APIBooks.Books;
using BookAPIComposer.Domain.Shared;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace BookAPIComposer.Services.APIBooks;

public class BookGrpcService : IBookService
{
    private readonly APIBooksBookGRPC.APIBooksBookGRPCClient _client;

    public BookGrpcService(string url)
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
        _client = new APIBooksBookGRPC.APIBooksBookGRPCClient(channel);
    }

    public async Task<List<BooksBooksDto>> GetAllAsync()
    {
        try
        {
            using var call = _client.GetAllBooks(new Empty());
            List<BooksBooksDto> lstPubBook = new List<BooksBooksDto>();
            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                List<BookDescriptionsDto> descriptions = reply.Descriptions.ToList().ConvertAll<BookDescriptionsDto>(
                    description => new BookDescriptionsDto(
                        description.Text, description.Language));
                lstPubBook.Add(new BooksBooksDto(reply.ISBN, reply.Name, reply.Language, descriptions));
            }

            return lstPubBook;
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<List<BooksBooksDto>> GetAllOfLanguage(string language)
    {
        try
        {
            using var call = _client.GetAllBooksOfLanguage(new RequestWithLanguage
            {
                Language = language
            });
            List<BooksBooksDto> lstPubBook = new List<BooksBooksDto>();
            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                List<BookDescriptionsDto> descriptions = reply.Descriptions.ToList().ConvertAll<BookDescriptionsDto>(
                    description => new BookDescriptionsDto(
                        description.Text, description.Language));
                lstPubBook.Add(new BooksBooksDto(reply.ISBN, reply.Name, reply.Language, descriptions));
            }

            return lstPubBook;
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<BooksBooksDto> GetByIdAsync(BookId id)
    {
        try
        {
            var reply = await _client.GetBookByISBNAsync(new RequestWithISBN()
            {
                ISBN = id.AsString()
            });
            //Console.Write(reply);
            List<BookDescriptionsDto> descriptions = reply.Descriptions.ToList().ConvertAll<BookDescriptionsDto>(
                description => new BookDescriptionsDto(description.Text, description.Language));
            return new BooksBooksDto(reply.ISBN, reply.Name, reply.Language, descriptions);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<BooksBooksDto> AddAsync(BooksCreatingBooksDto dto)
    {
        try
        {
            var request = new CreatingBookGrpcDto
            {
                ISBN = dto.isbn,
                Name = dto.title,
                Descriptions =
                {
                    dto.descriptions.ConvertAll<DescriptionGrpcDto>(desc => new DescriptionGrpcDto
                    {
                        Text = desc.text,
                        Language = desc.language
                    })
                }
            };

            var reply = await _client.AddNewBookAsync(request);

            List<BookDescriptionsDto> descriptions = reply.Descriptions.ToList().ConvertAll<BookDescriptionsDto>(
                description => new BookDescriptionsDto(description.Text, description.Language));
            return new BooksBooksDto(reply.ISBN, reply.Name, reply.Language,descriptions);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<BooksBooksDto> UpdateAsync(BooksBooksDto dto)
    {
        try
        {
            var request = new BookGrpcDto
            {
                ISBN = dto.isbn,
                Name = dto.title,
                Descriptions =
                {
                    dto.descriptions.ConvertAll<DescriptionGrpcDto>(desc => new DescriptionGrpcDto
                    {
                        Text = desc.text,
                        Language = desc.language
                    })
                }
            };

            var reply = await _client.ModifyBookAsync(request);

            List<BookDescriptionsDto> descriptions = reply.Descriptions.ToList().ConvertAll<BookDescriptionsDto>(
                description => new BookDescriptionsDto(description.Text, description.Language));
            return new BooksBooksDto(reply.ISBN, reply.Name, reply.Language, descriptions);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return null;
        }
    }

    public async Task<bool> DeleteAsync(BookId id)
    {
        try
        {
            var reply = await _client.DeleteBookAsync(new RequestWithISBN()
            {
                ISBN = id.AsString()
            });

            return reply.Success;
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: {0}: {1}", e.Status.StatusCode, e.Status.Detail);
            return false;
        }
    }
}