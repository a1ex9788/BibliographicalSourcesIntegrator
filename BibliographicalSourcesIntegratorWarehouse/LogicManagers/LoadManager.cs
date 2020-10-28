using BibliographicalSourcesIntegrator;
using BibliographicalSourcesIntegratorContracts;
using BibliographicalSourcesIntegratorWarehouse.Persistence;
using Microsoft.Extensions.Logging;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Controllers
{
    public class LoadManager
    {
        private readonly AppDbContext context;
        private RequestsManager requestsManager;
        private readonly ILogger<LoadManager> _logger;

        public LoadManager(AppDbContext context, RequestsManager requestsManager, ILogger<LoadManager> logger)
        {
            this.context = context;
            this.requestsManager = requestsManager;
            _logger = logger;
        }

        public async Task<string> Load(string request)
        {
            LoadRequest loadRequest = GetLoadRequest(request);

            LoadAnswer loadAnswer = await requestsManager.LoadDataFrom(loadRequest).ConfigureAwait(false);

            // Guarda en la DB

            return JSONHelper<LoadAnswer>.Serialize(loadAnswer);
        }


        private LoadRequest GetLoadRequest(string request)
        {
            try
            {
                return JSONHelper<LoadRequest>.Deserialize(request);
            }
            catch (Exception)
            {
                _logger.LogError("The request is not a LoadRequest.");

                return null;
            }
        }
    }
}
