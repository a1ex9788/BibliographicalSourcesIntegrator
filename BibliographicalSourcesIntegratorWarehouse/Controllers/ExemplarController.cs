using System;
using System.Collections.Generic;
using System.Linq;
using BibliographicalSourcesIntegratorWarehouse.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BibliographicalSourcesIntegratorWarehouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExemplarController : ControllerBase
    {
        private readonly ILogger<ExemplarController> _logger;

        public ExemplarController(ILogger<ExemplarController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Exemplar> Get()
        {
            return null;
        }
    }
}
