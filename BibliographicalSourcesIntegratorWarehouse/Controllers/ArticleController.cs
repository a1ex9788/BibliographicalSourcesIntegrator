using System;
using System.Collections.Generic;
using BibliographicalSourcesIntegratorWarehouse.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BibliographicalSourcesIntegratorWarehouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(ILogger<ArticleController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Article> Get()
        {
            return null;
        }
    }
}
