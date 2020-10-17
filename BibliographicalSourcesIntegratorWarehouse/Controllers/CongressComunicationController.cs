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
    public class CongressComunicationController : ControllerBase
    {
        private readonly ILogger<CongressComunicationController> _logger;

        public CongressComunicationController(ILogger<CongressComunicationController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<CongressComunication> Get()
        {
            return null;
        }
    }
}
