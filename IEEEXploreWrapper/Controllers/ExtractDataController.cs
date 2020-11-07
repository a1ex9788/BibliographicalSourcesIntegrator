using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BibliographicalSourcesIntegratorContracts;
using IEEEXploreWrapper.LogicManagers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IEEEXploreWrapper.Controllers
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
        public async Task<string> ExtractData(string request)
        {
            _logger.LogInformation("An extract data request was received: " + request);

            return await extractDataManager.ExtractData(request);
        }
    }
}
