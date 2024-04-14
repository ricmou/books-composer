using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIPublisher.Publishers;
using BookAPIComposer.Domain.Shared;
using BookAPIComposer.Services.APIPublisher.Publishers;
using Microsoft.AspNetCore.Mvc;

namespace BookAPIComposer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PublisherController : ControllerBase
{
    private readonly IPublisherService _publisherService;

    public PublisherController(IPublisherService publisherService)
    {
        _publisherService = publisherService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PublisherDto>>> GetAll()
    {
        return await _publisherService.GetAllAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<PublisherDto>> GetGetById(string id)
    {
        var publisher = await _publisherService.GetByIdAsync(new PublisherId(id));

        if (publisher == null)
        {
            return NotFound();
        }

        return publisher;
    }
    
    [HttpPost]
    public async Task<ActionResult<PublisherDto>> Create(CreatingPublisherDto dto)
    {
        var publisher = await _publisherService.AddAsync(dto);

        return publisher;

        //return CreatedAtAction(nameof(GetGetById), new { id = publisher.PublisherId }, publisher);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<PublisherDto>> Update(string id, PublisherDto dto)
    {
        if (id != dto.PublisherId)
        {
            return BadRequest();
        }

        try
        {
            var publisher = await _publisherService.UpdateAsync(dto);

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
    public async Task<ActionResult<PublisherDto>> HardDelete(string id)
    {
        try
        {
            var publisher = await _publisherService.DeleteAsync(new PublisherId(id));

            if (publisher == null)
            {
                return NotFound();
            }

            return Ok(publisher);
        }
        catch(BusinessRuleValidationException ex)
        {
            return BadRequest(new {Message = ex.Message});
        }
    }
}