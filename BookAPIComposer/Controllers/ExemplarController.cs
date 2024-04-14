using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPIComposer.Domain.Fetching.APIExemplar.Exemplars;
using BookAPIComposer.Domain.Shared;
using BookAPIComposer.Services.APIExemplar;
using Microsoft.AspNetCore.Mvc;

namespace BookAPIComposer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExemplarController : ControllerBase
    {
        private readonly IExemplarService _exemplarService;

        public ExemplarController(IExemplarService exemplarService)
        {
            _exemplarService = exemplarService;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExemplarDto>>> GetAll()
        {
            return await _exemplarService.GetAllAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExemplarDto>> GetGetById(Guid id)
        {
            var exemplar = await _exemplarService.GetByIdAsync(new ExemplarId(id));

            if (exemplar == null)
            {
                return NotFound();
            }

            return exemplar;
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<ExemplarDto>> Create(CreatingExemplarDto dto)
        {
            var exemplar = await _exemplarService.AddAsync(dto);

            return exemplar;
        }

        
        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ExemplarDto>> Update(string id, ExemplarDto dto)
        {
            if (id != dto.ExemplarId)
            {
                return BadRequest();
            }

            try
            {
                var exemplar = await _exemplarService.UpdateAsync(dto);

                if (exemplar == null)
                {
                    return NotFound();
                }

                return Ok(exemplar);
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ExemplarDto>> HardDelete(Guid id)
        {
            try
            {
                var exemplar = await _exemplarService.DeleteAsync(new ExemplarId(id));

                if (exemplar == null)
                {
                    return NotFound();
                }

                return Ok(exemplar);
            }
            catch(BusinessRuleValidationException ex)
            {
               return BadRequest(new {Message = ex.Message});
            }
        }
    }
}