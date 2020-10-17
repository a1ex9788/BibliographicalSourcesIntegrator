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
    public class PublicationController : ControllerBase
    {
        private readonly ILogger<PublicationController> _logger;

        public PublicationController(ILogger<PublicationController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Publication> Get()
        {
            return null;
        }
    }
}
