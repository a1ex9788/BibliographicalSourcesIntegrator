using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BibliographicalSourcesIntegratorWarehouse.Entities;
using BibliographicalSourcesIntegratorWarehouse.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BibliographicalSourcesIntegratorWarehouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoadController : ControllerBase
    {
        private LoadManager loadManager;

        private readonly ILogger<LoadController> _logger;

        public LoadController(ILogger<LoadController> logger, LoadManager loadManager)
        {
            _logger = logger;
            this.loadManager = loadManager;
        }

        [HttpGet("{request}")]
        public async Task<string> Load(string request)
        {
            _logger.LogInformation("A load request was received: " + request);

            return await loadManager.Load(request);
        }
    }
}
