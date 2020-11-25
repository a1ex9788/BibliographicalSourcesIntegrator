using BibliographicalSourcesIntegrator;
using BibliographicalSourcesIntegratorContracts;
using BibliographicalSourcesIntegratorWarehouse.Persistence;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Controllers
{
    public class SearchManager
    {
        private readonly ILogger<SearchManager> _logger;


        public SearchManager(ILogger<SearchManager> logger)
        {
            _logger = logger;
        }


        public SearchAnswer Search(string request)
        {
            SearchRequest searchRequest = GetSearchRequest(request);

            if (searchRequest == null)
            {
                return null;
            }

            return ProcessSearchRequest(searchRequest);
        }

        private SearchRequest GetSearchRequest(string request)
        {
            try
            {
                return JsonSerializer.Deserialize<SearchRequest>(request);
            }
            catch (Exception)
            {
                _logger.LogError("The request is not a LoadRequest.");

                return null;
            }
        }

        private SearchAnswer ProcessSearchRequest(SearchRequest searchRequest)
        {
            //Llamar BD
            return null;
        }
    }
}
