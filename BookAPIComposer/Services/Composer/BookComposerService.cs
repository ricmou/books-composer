using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIAuthors.Books;
using BookAPIComposer.Domain.Fetching.APIBooks.Books;
using BookAPIComposer.Domain.Fetching.APICategories.Books;
using BookAPIComposer.Domain.Fetching.APIPublisher.Books;
using BookAPIComposer.Domain.Requests.Authors;
using BookAPIComposer.Domain.Requests.Books;
using BookAPIComposer.Domain.Requests.Categories;
using BookAPIComposer.Domain.Requests.Publishers;
using BookAPIComposer.Domain.Shared;
using BookAPIComposer.Services.APIAuthors.Books;
using BookAPIComposer.Services.APIBooks;
using BookAPIComposer.Services.APICategories.Books;
using BookAPIComposer.Services.APIPublisher.Books;

namespace BookAPIComposer.Services.Composer;

public class BookComposerService : IBookComposerService
{
    private readonly IBookService _bookService;
    private readonly IAuthorsBookService _authorsBookService;
    private readonly ICategoryBookService _categoryBookService;
    private readonly IPublisherBookService _publisherBookService;

    public BookComposerService(IBookService bookService, IAuthorsBookService authorsBookService,
        ICategoryBookService categoryBookService, IPublisherBookService publisherBookService)
    {
        _bookService = bookService;
        _authorsBookService = authorsBookService;
        _categoryBookService = categoryBookService;
        _publisherBookService = publisherBookService;
    }

    public async Task<Book> GetBookInfo(BookId id)
    {
        //Fetch each part of the book
        var authorRequest = _authorsBookService.GetByIdAsync(id);
        var bookRequest = _bookService.GetByIdAsync(id);
        var catRequest = _categoryBookService.GetByIdAsync(id);
        var pubRequest = _publisherBookService.GetByIdAsync(id);

        await Task.WhenAll(authorRequest, bookRequest, catRequest, pubRequest);

        if (authorRequest.Result == null || bookRequest.Result == null || catRequest.Result == null ||
            pubRequest.Result == null)
        {
            return null;
        }

        return new Book(id.AsString(),
            bookRequest.Result.title,
            bookRequest.Result.language,
            bookRequest.Result.descriptions,
            new Publisher(pubRequest.Result.Publisher.PublisherId, pubRequest.Result.Publisher.Name,
                pubRequest.Result.Publisher.Country),
            authorRequest.Result.Authors.ConvertAll(auth =>
                new Author(auth.AuthorId, auth.FirstName, auth.LastName, auth.BirthDate, auth.Country)),
            catRequest.Result.Categories.ConvertAll(cat =>
                new Category(cat.CategoryId, cat.Name)));
    }

    public async Task<List<Book>> GetAllBookInfo()
    {
        //Fetch all books from the APIs
        var authorRequest = _authorsBookService.GetAllAsync();
        var bookRequest = _bookService.GetAllAsync();
        var catRequest = _categoryBookService.GetAllAsync();
        var pubRequest = _publisherBookService.GetAllAsync();

        await Task.WhenAll(authorRequest, bookRequest, catRequest, pubRequest);

        if (authorRequest.Result == null || bookRequest.Result == null || catRequest.Result == null ||
            pubRequest.Result == null)
        {
            //Issues on connecting somewhere.
            return null;
        }

        List<Book> lstBooks = new List<Book>();

        foreach (var book in bookRequest.Result)
        {
            //TODO: Feels like theres a better way to do this without continue
            var author = authorRequest.Result.FirstOrDefault(i => i.Id == book.isbn);
            if (author == null)
            {
                continue;
            }

            var category = catRequest.Result.FirstOrDefault(i => i.Id == book.isbn);
            if (category == null)
            {
                continue;
            }

            var publisher = pubRequest.Result.FirstOrDefault(i => i.Id == book.isbn);
            if (publisher == null)
            {
                continue;
            }

            lstBooks.Add(new Book(book.isbn,
                book.title,
                book.language,
                book.descriptions,
                new Publisher(publisher.Publisher.PublisherId, publisher.Publisher.Name,
                    publisher.Publisher.Country),
                author.Authors.ConvertAll(auth =>
                    new Author(auth.AuthorId, auth.FirstName, auth.LastName, auth.BirthDate, auth.Country)),
                category.Categories.ConvertAll(cat =>
                    new Category(cat.CategoryId, cat.Name))));
        }

        return lstBooks;
    }

    public async Task<Book> AddNewBookInfo(CreatingBook book)
    {
        //First, check if the book exists on any of the repositories
        var bookRequest = _bookService.AddAsync(new BooksCreatingBooksDto(book.Id, book.Title, book.Language,
            book.Description));

        var authorRequest =  _authorsBookService.AddAsync(new AuthorsCreatingBooksDto(book.Id, book.Authors));


        var catRequest = _categoryBookService.AddAsync(new CategoryCreatingBooksDto(book.Id, book.Categories));


        var pubRequest = _publisherBookService.AddAsync(new PublisherCreatingBooksDto(book.Id,
                book.PublisherId));
        
        await Task.WhenAll(authorRequest, bookRequest, catRequest, pubRequest);

        if (authorRequest.Result == null || bookRequest.Result == null || catRequest.Result == null ||
            pubRequest.Result == null)
        {
            //Issues on adding somewhere
            return null;
        }

        return new Book(bookRequest.Result.isbn,
            bookRequest.Result.title,
            bookRequest.Result.language,
            bookRequest.Result.descriptions,
            new Publisher(pubRequest.Result.Publisher.PublisherId, pubRequest.Result.Publisher.Name,
                pubRequest.Result.Publisher.Country),
            authorRequest.Result.Authors.ConvertAll(auth =>
                new Author(auth.AuthorId, auth.FirstName, auth.LastName, auth.BirthDate, auth.Country)),
            catRequest.Result.Categories.ConvertAll(cat =>
                new Category(cat.CategoryId, cat.Name)));
    }
    
    /*public async Task<Book> AddNewBookInfo(CreatingBook book)
    {
        //First, check if the book exists on any of the repositories
        var bookRequest = await _bookService.GetByIdAsync(new BookId(book.Id));
        if (bookRequest != null)
        {
            //Console.WriteLine("Book exists already, discard operation");
            return null;
        }

        var authorRequest = await _authorsBookService.GetByIdAsync(new BookId(book.Id));
        if (authorRequest != null)
        {
            //Console.WriteLine("Book exists, check if authors list is similar");
            //TODO: NOT EFFICIENT, likely there is a LINQ method that is better
            //Check sizes
            if (authorRequest.Authors.Count != book.Authors.Count)
            {
                //Console.WriteLine("Not the same author lenght, so not the same");
                return null;
            }

            foreach (string authId in book.Authors)
            {
                //Console.WriteLine("If it cant find author them, theres issues");
                if (authorRequest.Authors.FirstOrDefault(i => i.AuthorId == authId) == null)
                {
                    return null;
                }
            }
            //So, the book already exists, could be due to a partial add operation that failed on other port, not worth
            //re adding
        }
        else
        {
            authorRequest = await _authorsBookService.AddAsync(new AuthorsCreatingBooksDto(book.Id, book.Authors));
            if (authorRequest == null)
            {
                //Console.WriteLine("It failed to add the book auth, a ID could be a issue.");
                return null;
            }
        }

        var catRequest = await _categoryBookService.GetByIdAsync(new BookId(book.Id));
        if (catRequest != null)
        {
            //Console.WriteLine("Book exists, check if categories list is similar");
            //TODO: NOT EFFICIENT, likely there is a LINQ method that is better
            //Check sizes
            if (catRequest.Categories.Count != book.Categories.Count)
            {
                //Console.WriteLine("Not the same cat lenght, so not the same");
                return null;
            }

            foreach (string catId in book.Categories)
            {
                //Console.WriteLine("If it cant find cat them, theres issues");
                if (catRequest.Categories.FirstOrDefault(i => i.CategoryId == catId) == null)
                {
                    return null;
                }
            }
            //So, the book already exists, could be due to a partial add operation that failed on other port, not worth
            //re adding
        }
        else
        {
            catRequest = await _categoryBookService.AddAsync(new CategoryCreatingBooksDto(book.Id, book.Categories));
            if (catRequest == null)
            {
                //Console.WriteLine("It failed to add the book cat, a ID could be a issue.");
                return null;
            }
        }

        var pubRequest = await _publisherBookService.GetByIdAsync(new BookId(book.Id));
        if (pubRequest != null)
        {
            //Console.WriteLine("Book exists, check if same publisher");
            if (pubRequest.Publisher.PublisherId != book.PublisherId)
                return null;

            //Exists, no re adding
        }
        else
        {
            pubRequest = await _publisherBookService.AddAsync(new PublisherCreatingBooksDto(book.Id,
                book.PublisherId));
            if (pubRequest == null)
            {
                //Console.WriteLine("Rejected on pubrequest");
                return null;
            }
        }

        bookRequest =
            await _bookService.AddAsync(new BooksCreatingBooksDto(book.Id, book.Title, book.Language,
                book.Description));
        if (bookRequest == null)
        {
            //Console.WriteLine("Rejected on bookrequest");
            return null;
        }

        return new Book(bookRequest.isbn,
            bookRequest.title,
            bookRequest.language,
            bookRequest.descriptions,
            new Publisher(pubRequest.Publisher.PublisherId, pubRequest.Publisher.Name,
                pubRequest.Publisher.Country),
            authorRequest.Authors.ConvertAll(auth =>
                new Author(auth.AuthorId, auth.FirstName, auth.LastName, auth.BirthDate, auth.Country)),
            catRequest.Categories.ConvertAll(cat =>
                new Category(cat.CategoryId, cat.Name)));
    }*/

    public async Task<Book> ModifyBookInfo(CreatingBook book)
    {
        //This step is TECHNICALLY not required, as the Update methods on all services will inherently check for a objects
        //existence, but better safe than sorry.
        var authorRequest = _authorsBookService.GetByIdAsync(new BookId(book.Id));
        var bookRequest = _bookService.GetByIdAsync(new BookId(book.Id));
        var catRequest = _categoryBookService.GetByIdAsync(new BookId(book.Id));
        var pubRequest = _publisherBookService.GetByIdAsync(new BookId(book.Id));

        await Task.WhenAll(authorRequest, bookRequest, catRequest, pubRequest);

        //If instances do not exist in some of the services, don't edit
        if (authorRequest.Result == null || bookRequest.Result == null || catRequest.Result == null ||
            pubRequest.Result == null)
        {
            return null;
        }

        authorRequest = _authorsBookService.UpdateAsync(new AuthorsCreatingBooksDto(book.Id, book.Authors));
        bookRequest = _bookService.UpdateAsync(new BooksBooksDto(book.Id, book.Title, book.Language, book.Description));
        catRequest = _categoryBookService.UpdateAsync(new CategoryCreatingBooksDto(book.Id, book.Categories));
        pubRequest = _publisherBookService.UpdateAsync(new PublisherCreatingBooksDto(book.Id,
            book.PublisherId));

        await Task.WhenAll(authorRequest, bookRequest, catRequest, pubRequest);

        //If any of the instances fail, well, theres issues
        if (authorRequest.Result == null || bookRequest.Result == null || catRequest.Result == null ||
            pubRequest.Result == null)
        {
            return null;
        }

        return new Book(bookRequest.Result.isbn,
            bookRequest.Result.title,
            bookRequest.Result.language,
            bookRequest.Result.descriptions,
            new Publisher(pubRequest.Result.Publisher.PublisherId, pubRequest.Result.Publisher.Name,
                pubRequest.Result.Publisher.Country),
            authorRequest.Result.Authors.ConvertAll(auth =>
                new Author(auth.AuthorId, auth.FirstName, auth.LastName, auth.BirthDate, auth.Country)),
            catRequest.Result.Categories.ConvertAll(cat =>
                new Category(cat.CategoryId, cat.Name)));
    }

    public async Task<BookDeletionDto> DeleteBook(BookId id)
    {
        //Fetch each part of the book
        var authorRequest = _authorsBookService.DeleteAsync(id);
        var bookRequest = _bookService.DeleteAsync(id);
        var catRequest = _categoryBookService.DeleteAsync(id);
        var pubRequest = _publisherBookService.DeleteAsync(id);

        await Task.WhenAll(authorRequest, bookRequest, catRequest, pubRequest);

        if (authorRequest.Result == null && bookRequest.Result == false && catRequest.Result == null &&
            pubRequest.Result == null)
        {
            return null; //Nothing was deleted
        }

        return new BookDeletionDto((authorRequest.Result != null), bookRequest.Result, (catRequest.Result != null),
            (pubRequest.Result != null));
    }

    public async Task<List<Book>> GetAllBookFromLanguage(string language)
    {
        //Fetch all books from the API
        var bookRequest = await _bookService.GetAllOfLanguage(language);
        
        if (bookRequest == null)
        {
            //Issues on connecting somewhere.
            return null;
        }
        List<Book> lstBooks = new List<Book>();

        foreach (var book in bookRequest)
        {
            //TODO: Feels like theres a better way to do this without continue
            var author = await _authorsBookService.GetByIdAsync(new BookId(book.isbn));
            if (author == null)
            {
                continue;
            }

            var category = await _categoryBookService.GetByIdAsync(new BookId(book.isbn));
            if (category == null)
            {
                continue;
            }

            var publisher = await _publisherBookService.GetByIdAsync(new BookId(book.isbn));
            if (publisher == null)
            {
                continue;
            }

            lstBooks.Add(new Book(book.isbn,
                book.title,
                book.language,
                book.descriptions,
                new Publisher(publisher.Publisher.PublisherId, publisher.Publisher.Name,
                    publisher.Publisher.Country),
                author.Authors.ConvertAll(auth =>
                    new Author(auth.AuthorId, auth.FirstName, auth.LastName, auth.BirthDate, auth.Country)),
                category.Categories.ConvertAll(cat =>
                    new Category(cat.CategoryId, cat.Name))));
        }

        return lstBooks;
    }

    public async Task<List<Book>> GetAllBookFromAuthor(AuthorId authorId)
    {
        var authorRequest = await _authorsBookService.GetByAuthorIdAsync(authorId);
        if (authorRequest == null)
        {
            //Issues on connecting somewhere.
            return null;
        }
        
        List<Book> lstBooks = new List<Book>();

        foreach (var authBook in authorRequest)
        {
            //TODO: Feels like theres a better way to do this without continue
            var book = await _bookService.GetByIdAsync(new BookId(authBook.Id));
            if (book == null)
            {
                continue;
            }

            var category = await _categoryBookService.GetByIdAsync(new BookId(authBook.Id));
            if (category == null)
            {
                continue;
            }

            var publisher = await _publisherBookService.GetByIdAsync(new BookId(authBook.Id));
            if (publisher == null)
            {
                continue;
            }

            lstBooks.Add(new Book(book.isbn,
                book.title,
                book.language,
                book.descriptions,
                new Publisher(publisher.Publisher.PublisherId, publisher.Publisher.Name,
                    publisher.Publisher.Country),
                authBook.Authors.ConvertAll(auth =>
                    new Author(auth.AuthorId, auth.FirstName, auth.LastName, auth.BirthDate, auth.Country)),
                category.Categories.ConvertAll(cat =>
                    new Category(cat.CategoryId, cat.Name))));
        }

        return lstBooks;
    }

    public async Task<List<Book>> GetAllBookFromPublisher(PublisherId publisherId)
    {
        var publisherRequest = await _publisherBookService.GetAllFromPublisherAsync(publisherId);
        if (publisherRequest == null)
        {
            //Issues on connecting somewhere.
            return null;
        }
        
        List<Book> lstBooks = new List<Book>();

        foreach (var pubBook in publisherRequest)
        {
            //TODO: Feels like theres a better way to do this without continue
            var book = await _bookService.GetByIdAsync(new BookId(pubBook.Id));
            if (book == null)
            {
                continue;
            }

            var category = await _categoryBookService.GetByIdAsync(new BookId(pubBook.Id));
            if (category == null)
            {
                continue;
            }

            var author = await _authorsBookService.GetByIdAsync(new BookId(pubBook.Id));
            if (author == null)
            {
                continue;
            }

            lstBooks.Add(new Book(book.isbn,
                book.title,
                book.language,
                book.descriptions,
                new Publisher(pubBook.Publisher.PublisherId, pubBook.Publisher.Name,
                    pubBook.Publisher.Country),
                author.Authors.ConvertAll(auth =>
                    new Author(auth.AuthorId, auth.FirstName, auth.LastName, auth.BirthDate, auth.Country)),
                category.Categories.ConvertAll(cat =>
                    new Category(cat.CategoryId, cat.Name))));
        }

        return lstBooks;
    }
}