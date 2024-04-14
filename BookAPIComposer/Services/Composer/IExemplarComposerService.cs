using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Requests.Exemplar;
using BookAPIComposer.Domain.Shared;

namespace BookAPIComposer.Services.Composer;

public interface IExemplarComposerService
{
    Task<ExemplarPackage> GetExemplarsOfBook(BookId id);
    
    Task<List<ExemplarPackage>> GetAllExemplarsFromClient(ClientId id);

    Task<List<ExemplarPackage>> GetAllExemplarsFromAllBooks();
    
    Task<List<ExemplarPackage>> GetAllExemplarsFromLanguage(string language);
    
    Task<List<ExemplarPackage>> GetAllExemplarsFromAuthor(AuthorId authorId);
    
    Task<List<ExemplarPackage>> GetAllExemplarsFromPublisher(PublisherId publisherId);
    
}