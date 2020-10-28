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
    public class SearchController : ControllerBase
    {
        private SearchManager searchManager;

        private readonly ILogger<SearchController> _logger;

        public SearchController(ILogger<SearchController> logger, SearchManager searchManager)
        {
            _logger = logger;
            this.searchManager = searchManager;
        }

        [HttpGet("request")]
        public void Search(string request)
        {
            _logger.LogInformation("A search request was received: " + request);

            searchManager.Search();
        }
    }
}
