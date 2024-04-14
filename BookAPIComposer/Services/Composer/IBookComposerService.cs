using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Requests.Books;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.Composer;

public interface IBookComposerService
{
    Task<Book> GetBookInfo(BookId id);

    Task<List<Book>> GetAllBookInfo();
    
    Task<List<Book>> GetAllBookFromLanguage(string language);
    
    Task<List<Book>> GetAllBookFromAuthor(AuthorId authorId);
    
    Task<List<Book>> GetAllBookFromPublisher(PublisherId publisherId);

    Task<Book> AddNewBookInfo(CreatingBook book);

    Task<Book> ModifyBookInfo(CreatingBook book);

    Task<BookDeletionDto> DeleteBook(BookId id);
}