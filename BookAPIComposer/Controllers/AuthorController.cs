using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIAuthors.Authors;
using BookAPIComposer.Domain.Shared;
using BookAPIComposer.Services.APIAuthors.Authors;
using Microsoft.AspNetCore.Mvc;

namespace BookAPIComposer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IAuthorsService _authorsService;

    public AuthorController(IAuthorsService authorsService)
    {
        _authorsService = authorsService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll()
    {
        return await _authorsService.GetAllAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorDto>> GetGetById(string id)
    {
        var author = await _authorsService.GetByIdAsync(new AuthorId(id));

        if (author == null)
        {
            return NotFound();
        }

        return author;
    }
    
    [HttpPost]
    public async Task<ActionResult<AuthorDto>> Create(CreatingAuthorsDto dto)
    {
        var author = await _authorsService.AddAsync(dto);

        //return CreatedAtAction(nameof(GetGetById), new { id = author.AuthorId }, author);

        return author;
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<AuthorDto>> Update(string id, AuthorDto dto)
    {
        if (id != dto.AuthorId)
        {
            return BadRequest();
        }

        try
        {
            var publisher = await _authorsService.UpdateAsync(dto);

            if (publisher == null)
            {
                return NotFound();
            }

            return Ok(publisher);
        }
        catch (BusinessRuleValidationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<AuthorDto>> HardDelete(string id)
    {
        try
        {
            var book = await _authorsService.DeleteAsync(new AuthorId(id));

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
}