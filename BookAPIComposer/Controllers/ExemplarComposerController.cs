using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Requests.Exemplar;
using BookAPIComposer.Domain.Shared;
using BookAPIComposer.Services.Composer;
using Microsoft.AspNetCore.Mvc;

namespace BookAPIComposer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExemplarComposerController :  ControllerBase
{
    private readonly IExemplarComposerService _exemplarComposerService;
    
    public ExemplarComposerController(IExemplarComposerService exemplarComposerService)
    {
        _exemplarComposerService = exemplarComposerService;
    }

    // GET: api/Books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExemplarPackage>>> GetAll()
    {
        return await _exemplarComposerService.GetAllExemplarsFromAllBooks();
    }

    // GET: api/Books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ExemplarPackage>> GetGetById(string id)
    {
        var exemplar = await _exemplarComposerService.GetExemplarsOfBook(new BookId(id));

        if (exemplar == null)
        {
            return NotFound();
        }

        return exemplar;
    }
    
    // GET
    [HttpGet("Client/{id}")]
    public async Task<ActionResult<IEnumerable<ExemplarPackage>>> GetExemplarsOfClient(string id)
    {
        return await _exemplarComposerService.GetAllExemplarsFromClient(new ClientId(id));
    }
    
    // GET
    [HttpGet("Author/{id}")]
    public async Task<ActionResult<IEnumerable<ExemplarPackage>>> GetExemplarsOfAuthor(string id)
    {
        return await _exemplarComposerService.GetAllExemplarsFromAuthor(new AuthorId(id));
    }
    
    // GET
    [HttpGet("Lang/{id}")]
    public async Task<ActionResult<IEnumerable<ExemplarPackage>>> GetExemplarsOfLanguage(string id)
    {
        return await _exemplarComposerService.GetAllExemplarsFromLanguage(id);
    }
    
    // GET
    [HttpGet("Publisher/{id}")]
    public async Task<ActionResult<IEnumerable<ExemplarPackage>>> GetExemplarsOfPublisher(string id)
    {
        return await _exemplarComposerService.GetAllExemplarsFromPublisher(new PublisherId(id));
    }
}