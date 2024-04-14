using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Requests.Books;
using BookAPIComposer.Domain.Shared;
using BookAPIComposer.Services.Composer;
using Microsoft.AspNetCore.Mvc;

namespace BookAPIComposer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookComposerController : ControllerBase
{
    private readonly IBookComposerService _bookComposerService;

    public BookComposerController(IBookComposerService bookComposerService)
    {
        _bookComposerService = bookComposerService;
    }

    // GET: api/Books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAll()
    {
        var lstBooks = await _bookComposerService.GetAllBookInfo();

        if (lstBooks == null || lstBooks.Count < 1)
        {
            return NotFound();
        }


        return lstBooks;
    }

    // GET: api/Books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetGetById(string id)
    {
        //Get the book from book service
        var book = await _bookComposerService.GetBookInfo(new BookId(id));

        if (book == null)
        {
            return NotFound();
        }


        return book;
    }
    
    // POST: api/Books
    [HttpPost]
    public async Task<ActionResult<Book>> Create(CreatingBook dto)
    {
        var book = await _bookComposerService.AddNewBookInfo(dto);
        
        if (book == null)
        {
            return NotFound();
        }

        //return CreatedAtAction(nameof(GetGetById), new { id = book.Id }, book);

        return book;
    }

    
    // PUT: api/Books/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Book>> Update(string id, CreatingBook dto)
    {
        if (id != dto.Id)
        {
            return BadRequest();
        }

        try
        {
            var book = await _bookComposerService.ModifyBookInfo(dto);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }
        catch (BusinessRuleValidationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // DELETE: api/Books/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<BookDeletionDto>> HardDelete(string id)
    {
        try
        {
            var book = await _bookComposerService.DeleteBook(new BookId(id));

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }
        catch(BusinessRuleValidationException ex)
        {
           return BadRequest(new {Message = ex.Message});
        }
    }
    
    [HttpGet("Lang/{id}")]
    public async Task<ActionResult<IEnumerable<Book>>> GetAllBooksOfLanguage(string id)
    {
        var lstBooks = await _bookComposerService.GetAllBookFromLanguage(id);

        if (lstBooks == null || lstBooks.Count < 1)
        {
            return NotFound();
        }


        return lstBooks;
    }
    
    [HttpGet("Author/{id}")]
    public async Task<ActionResult<IEnumerable<Book>>> GetAllBooksOfAuthor(string id)
    {
        var lstBooks = await _bookComposerService.GetAllBookFromAuthor(new AuthorId(id));

        if (lstBooks == null || lstBooks.Count < 1)
        {
            return NotFound();
        }


        return lstBooks;
    }
    
    [HttpGet("Publisher/{id}")]
    public async Task<ActionResult<IEnumerable<Book>>> GetAllBooksOfPublisher(string id)
    {
        var lstBooks = await _bookComposerService.GetAllBookFromPublisher(new PublisherId(id));

        if (lstBooks == null || lstBooks.Count < 1)
        {
            return NotFound();
        }


        return lstBooks;
    }
}