using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ultraschall.Data.Abstractions;
using Ultraschall.Data.Entities;
using Ultraschall.Domain.Abstractions;
using Ultraschall.Domain.Models;

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
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<IActionResult> Get([FromRoute]Guid id)
        {
            return Ok(await _service.GetById(id));
        }

        // POST api/categories
        [HttpPost]
        public async Task Post([FromBody] CategoryModel category)
        {
            await _service.Create(category);
        }

        // PUT api/categories/34bb1dbe-6267-40d3-b6a9-056453bb9b5f
        [HttpPut("{id}")]
        public async Task Put([FromRoute]Guid id, [FromBody] CategoryModel category)
        {
            await _service.Update(id, category);
        }

        // DELETE api/categories/34bb1dbe-6267-40d3-b6a9-056453bb9b5f
        [HttpDelete("{id}")]
        public void Delete([FromRoute]Guid id)
        {
            _service.Delete(id);
        }
    }
}