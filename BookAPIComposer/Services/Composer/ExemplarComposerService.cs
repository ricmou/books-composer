using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIExemplar.Exemplars;
using BookAPIComposer.Domain.Requests.Books;
using BookAPIComposer.Domain.Requests.Exemplar;
using BookAPIComposer.Domain.Shared;
using BookAPIComposer.Services.APIClients;
using BookAPIComposer.Services.APIExemplar;

namespace BookAPIComposer.Services.Composer;

public class ExemplarComposerService : IExemplarComposerService
{
    private readonly IExemplarService _exemplarService;
    private readonly IClientsService _clientsService;
    private readonly IBookComposerService _bookComposerService;

    public ExemplarComposerService(IExemplarService exemplarService, IBookComposerService bookComposerService,
        IClientsService clientsService)
    {
        _exemplarService = exemplarService;
        _bookComposerService = bookComposerService;
        _clientsService = clientsService;
    }

    public async Task<ExemplarPackage> GetExemplarsOfBook(BookId id)
    {
        var book = await _bookComposerService.GetBookInfo(id);
        if (book == null)
        {
            return null;
        }

        var exemplars = await _exemplarService.GetByBookIdAsync(id);
        if (exemplars == null || exemplars.Count == 0)
        {
            return null;
        }

        var exemplarList = new List<Exemplar>();
        
        foreach (ExemplarDto exemplarDto in exemplars)
        {
            var existsAlready = exemplarList.FirstOrDefault(i => i.Seller.ClientId == exemplarDto.SellerId);
            if (existsAlready == null)
            {
                var client = await _clientsService.GetByIdAsync(new ClientId(exemplarDto.SellerId));
                exemplarList.Add(new Exemplar(exemplarDto.ExemplarId, exemplarDto.BookState,
                    exemplarDto.DateOfAcquisition, client));
            }
            else
            {
                exemplarList.Add(new Exemplar(exemplarDto.ExemplarId, exemplarDto.BookState,
                    exemplarDto.DateOfAcquisition, existsAlready.Seller));
            }
        }

        return new ExemplarPackage(book, exemplarList);
    }

    public async Task<List<ExemplarPackage>> GetAllExemplarsFromAllBooks()
    {
        var books = await _bookComposerService.GetAllBookInfo();

        if (books == null)
        {
            //Console.WriteLine("null book");
            return null;
        }

        List<ExemplarPackage> packages = new List<ExemplarPackage>();

        foreach (Book book in books)
        {
            var exemplars = await _exemplarService.GetByBookIdAsync(new BookId(book.Id));
            if (exemplars == null || exemplars.Count == 0)
            {
                packages.Add(new ExemplarPackage(book));
                continue;
            }

            var exemplarList = new List<Exemplar>();


            foreach (ExemplarDto exemplarDto in exemplars)
            {
                var existsAlready = exemplarList.FirstOrDefault(i => i.Seller.ClientId == exemplarDto.SellerId);
                if (existsAlready == null)
                {
                    var client = await _clientsService.GetByIdAsync(new ClientId(exemplarDto.SellerId));
                    exemplarList.Add(new Exemplar(exemplarDto.ExemplarId, exemplarDto.BookState,
                        exemplarDto.DateOfAcquisition, client));
                }
                else
                {
                    exemplarList.Add(new Exemplar(exemplarDto.ExemplarId, exemplarDto.BookState,
                        exemplarDto.DateOfAcquisition, existsAlready.Seller));
                }
            }

            if (exemplarList.Count > 0)
            {
                packages.Add(new ExemplarPackage(book, exemplarList));
            }
        }

        return packages;
    }

    public async Task<List<ExemplarPackage>> GetAllExemplarsFromClient(ClientId id)
    {
        var client = await _clientsService.GetByIdAsync(id);
        if (client == null)
        {
            return null;
        }
        
        var exemplars = await _exemplarService.GetBySellerIdAsync(id);
        if (exemplars == null)
        {
            return null;
        }
        
        List<ExemplarPackage> packages = new List<ExemplarPackage>();

        for (int i = 0; i < exemplars.Count; i++)
        {
            var existsAlready = packages.FirstOrDefault(pack => pack.Book.Id == exemplars[i].BookId);
            if (existsAlready == null)
            {
                var book = await _bookComposerService.GetBookInfo(new BookId(exemplars[i].BookId));
                var exemplarList = new List<Exemplar>
                {
                    new Exemplar(exemplars[i].ExemplarId, exemplars[i].BookState,
                        exemplars[i].DateOfAcquisition, client)
                };
                packages.Add(new ExemplarPackage(book, exemplarList));
            }
            else
            {
                existsAlready.Exemplars.Add(new Exemplar(exemplars[i].ExemplarId, exemplars[i].BookState,
                    exemplars[i].DateOfAcquisition, client));
            }
        }

        return packages;
    }

    public async Task<List<ExemplarPackage>> GetAllExemplarsFromLanguage(string language)
    {
        var books = await _bookComposerService.GetAllBookFromLanguage(language);

        if (books == null)
        {
            return null;
        }

        List<ExemplarPackage> packages = new List<ExemplarPackage>();

        foreach (Book book in books)
        {
            var exemplars = await _exemplarService.GetByBookIdAsync(new BookId(book.Id));
            if (exemplars == null || exemplars.Count == 0)
            {
                packages.Add(new ExemplarPackage(book));
                continue;
            }

            var exemplarList = new List<Exemplar>();


            foreach (ExemplarDto exemplarDto in exemplars)
            {
                var existsAlready = exemplarList.FirstOrDefault(i => i.Seller.ClientId == exemplarDto.SellerId);
                if (existsAlready == null)
                {
                    var client = await _clientsService.GetByIdAsync(new ClientId(exemplarDto.SellerId));
                    exemplarList.Add(new Exemplar(exemplarDto.ExemplarId, exemplarDto.BookState,
                        exemplarDto.DateOfAcquisition, client));
                }
                else
                {
                    exemplarList.Add(new Exemplar(exemplarDto.ExemplarId, exemplarDto.BookState,
                        exemplarDto.DateOfAcquisition, existsAlready.Seller));
                }
            }

            if (exemplarList.Count > 0)
            {
                packages.Add(new ExemplarPackage(book, exemplarList));
            }
        }

        return packages;
    }

    public async Task<List<ExemplarPackage>> GetAllExemplarsFromAuthor(AuthorId authorId)
    {
        var books = await _bookComposerService.GetAllBookFromAuthor(authorId);

        if (books == null)
        {
            return null;
        }

        List<ExemplarPackage> packages = new List<ExemplarPackage>();

        foreach (Book book in books)
        {
            var exemplars = await _exemplarService.GetByBookIdAsync(new BookId(book.Id));
            if (exemplars == null || exemplars.Count == 0)
            {
                packages.Add(new ExemplarPackage(book));
                continue;
            }

            var exemplarList = new List<Exemplar>();


            foreach (ExemplarDto exemplarDto in exemplars)
            {
                var existsAlready = exemplarList.FirstOrDefault(i => i.Seller.ClientId == exemplarDto.SellerId);
                if (existsAlready == null)
                {
                    var client = await _clientsService.GetByIdAsync(new ClientId(exemplarDto.SellerId));
                    exemplarList.Add(new Exemplar(exemplarDto.ExemplarId, exemplarDto.BookState,
                        exemplarDto.DateOfAcquisition, client));
                }
                else
                {
                    exemplarList.Add(new Exemplar(exemplarDto.ExemplarId, exemplarDto.BookState,
                        exemplarDto.DateOfAcquisition, existsAlready.Seller));
                }
            }

            if (exemplarList.Count > 0)
            {
                packages.Add(new ExemplarPackage(book, exemplarList));
            }
        }

        return packages;
    }

    public async Task<List<ExemplarPackage>> GetAllExemplarsFromPublisher(PublisherId publisherId)
    {
        var books = await _bookComposerService.GetAllBookFromPublisher(publisherId);

        if (books == null)
        {
            return null;
        }

        List<ExemplarPackage> packages = new List<ExemplarPackage>();

        foreach (Book book in books)
        {
            var exemplars = await _exemplarService.GetByBookIdAsync(new BookId(book.Id));
            if (exemplars == null || exemplars.Count == 0)
            {
                packages.Add(new ExemplarPackage(book));
                continue;
            }

            var exemplarList = new List<Exemplar>();


            foreach (ExemplarDto exemplarDto in exemplars)
            {
                var existsAlready = exemplarList.FirstOrDefault(i => i.Seller.ClientId == exemplarDto.SellerId);
                if (existsAlready == null)
                {
                    var client = await _clientsService.GetByIdAsync(new ClientId(exemplarDto.SellerId));
                    exemplarList.Add(new Exemplar(exemplarDto.ExemplarId, exemplarDto.BookState,
                        exemplarDto.DateOfAcquisition, client));
                }
                else
                {
                    exemplarList.Add(new Exemplar(exemplarDto.ExemplarId, exemplarDto.BookState,
                        exemplarDto.DateOfAcquisition, existsAlready.Seller));
                }
            }

            if (exemplarList.Count > 0)
            {
                packages.Add(new ExemplarPackage(book, exemplarList));
            }
        }

        return packages;
    }
}