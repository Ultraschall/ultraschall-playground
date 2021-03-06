﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ultraschall.Data.Abstractions;
using Ultraschall.Data.Entities;
using Ultraschall.Domain.Abstractions;
using Ultraschall.Domain.Models;


// With Service and Repository
namespace Ultraschall.Api.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _service;
        public CategoriesController(ICategoriesService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }
        
        // GET api/categories
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryModel>), (int)HttpStatusCode.OK)]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        // GET api/categories/34bb1dbe-6267-40d3-b6a9-056453bb9b5f
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute]Guid id)
        {
            var result = await _service.GetById(id);
            if (result == null) return NotFound();

            return Ok(result);
        }

        // POST api/categories
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody] CategoryModel category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            try
            {
                await _service.Create(category);
                return CreatedAtAction(nameof(Get),new { id = category.Id });
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
        public async Task<IActionResult> Put([FromRoute]Guid id, [FromBody] CategoryModel category)
        {
            if (category == null || category.Id != id)
            {
                return BadRequest();
            }

            try { 
                await _service.Update(id, category);
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
                await _service.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Patch api/categories/34bb1dbe-6267-40d3-b6a9-056453bb9b5f
        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetParent([FromRoute]Guid id, [FromBody] CategoryPatchModel patchModel)
        {
            try
            {
                await _service.Patch(id, patchModel);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}