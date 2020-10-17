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
    public class JournalController : ControllerBase
    {
        private readonly ILogger<JournalController> _logger;

        public JournalController(ILogger<JournalController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Journal> Get()
        {
            return null;
        }
    }
}
