using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ultraschall.Data.Abstractions;

namespace Ultraschall.Api
{
    public class GenericController<TEntity> : Controller where TEntity : class, IEntity
    {
        private Type entityType = typeof(TEntity); 
        private readonly IGenericRepository<TEntity> _repository;
        public GenericController(IGenericRepository<TEntity> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        [HttpGet]
        public virtual IActionResult Get()
        {
            return Ok(_repository.GetAll());
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<IActionResult> Get([FromRoute]Guid id)
        {
            var result = await _repository.GetById(id);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody] TEntity chapter)
        {
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

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Put([FromRoute]Guid id, [FromBody] TEntity chapter)
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