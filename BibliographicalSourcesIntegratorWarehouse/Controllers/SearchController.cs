using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        [HttpGet("{request}")]
        public string Search(string request)
        {
            _logger.LogInformation("A search request was received: " + request);

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            return JsonConvert.SerializeObject(searchManager.Search(request), Formatting.None, jsSettings);
        }
    }
}