using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ultraschall.Data.Abstractions;
using Ultraschall.Data.Entities;

// Only Repository
namespace Ultraschall.Api.Controllers
{
    [Route("api/[controller]")]
    public class ChaptersController : Controller
    {
        private readonly IGenericRepository<Chapter> _repository;
        public ChaptersController(IGenericRepository<Chapter> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        // GET api/categories
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Chapter>), (int)HttpStatusCode.OK)]
        public IActionResult Get()
        {
            return Ok(_repository.GetAll());
        }
        
         // GET api/categories/34bb1dbe-6267-40d3-b6a9-056453bb9b5f
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Chapter), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute]Guid id)
        {
            var result = await _repository.GetById(id);
            if (result == null) return NotFound();

            return Ok(result);
        }

        // POST api/categories
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody] Chapter chapter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (chapter == null)
            {
                return BadRequest();
            }

            try
            {
                await _repository.Create(chapter);
                return CreatedAtAction(nameof(Get), new { id = chapter.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/categories/34bb1dbe-6267-40d3-b6a9-056453bb9b5f
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Put([FromRoute]Guid id, [FromBody] Chapter chapter)
        {
            if (chapter == null || chapter.Id != id)
            {
                return BadRequest();
            }

            try { 
                await _repository.Update(id, chapter);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/categories/34bb1dbe-6267-40d3-b6a9-056453bb9b5f
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            try { 
                await _repository.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}