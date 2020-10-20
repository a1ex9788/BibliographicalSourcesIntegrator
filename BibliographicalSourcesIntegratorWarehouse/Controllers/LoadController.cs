using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        public void Get()
        {
            LoadManager.Load();
        }
    }
}
