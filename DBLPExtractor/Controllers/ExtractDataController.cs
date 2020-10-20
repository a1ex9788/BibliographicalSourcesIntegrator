using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBLPExtractor.LogicManagers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DBLPExtractor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExtractDataController : ControllerBase
    {
        private ExtractDataManager extractDataManager;

        private readonly ILogger<ExtractDataController> _logger;

        public ExtractDataController(ILogger<ExtractDataController> logger, ExtractDataManager loadManager)
        {
            _logger = logger;
            this.extractDataManager = loadManager;
        }

        [HttpGet("{request}")]
        public string ExtractData(string request)
        {
            return extractDataManager.ExtractData(request);
        }
    }
}
