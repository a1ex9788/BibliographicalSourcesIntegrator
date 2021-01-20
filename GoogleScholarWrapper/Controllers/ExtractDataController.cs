using GoogleScholarWrapper.LogicManagers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GoogleScholarWrapper.Controllers
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
            _logger.LogInformation("An extract data request was received: " + request);

            return extractDataManager.ExtractData(request);
        }
    }
}