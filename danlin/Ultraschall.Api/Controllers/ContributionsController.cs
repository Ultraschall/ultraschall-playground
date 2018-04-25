using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ultraschall.Data.Abstractions;
using Ultraschall.Data.Entities;
using Ultraschall.Domain.Models;

namespace Ultraschall.Api.Controllers
{
    [Route("api/[controller]")]
    public class ContributionsController : GenericController<Contribution>
    {
        public ContributionsController(IGenericRepository<Contribution> repository) : base(repository)
        {
        }

        [ProducesResponseType(typeof(IEnumerable<Contribution>), (int)HttpStatusCode.OK)]
        public override IActionResult Get()
        {
            return base.Get();
        }

        [ProducesResponseType(typeof(Contribution), (int)HttpStatusCode.OK)]
        public override Task<IActionResult> Get(Guid id)
        {
            return base.Get(id);
        }
    }
}