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

            if (loadRequest == null)
            {
                return null;
            }

            LoadAnswer loadAnswer = await MakeLoadRequest(loadRequest);

            if (loadAnswer == null)
            {
                return null;
            }

            // TODO: Guardar en la DB

            return JsonSerializer.Serialize(loadAnswer);
        }


        private LoadRequest GetLoadRequest(string request)
        {
            try
            {
                return JsonSerializer.Deserialize<LoadRequest>(request);
            }
            catch (Exception)
            {
                _logger.LogError("The request is not a LoadRequest.");

                return null;
            }
        }

        private async Task<LoadAnswer> MakeLoadRequest(LoadRequest loadRequest)
        {
            string dBLPAnswer = "", iEEEXploreAnswer = "", googleScholarAnswer = "";

            ExtractRequest extractRequest = new ExtractRequest(loadRequest.InitialYear, loadRequest.FinalYear);

            if (loadRequest.loadFromDBLP)
            {
                dBLPAnswer = await requestsManager.LoadDataFromDBLP(extractRequest);

                _logger.LogInformation("DBLP answer:\n" + dBLPAnswer);
            }

            if (loadRequest.loadFromIEEEXplore)
            {
                iEEEXploreAnswer = await requestsManager.LoadDataFromIEEEXplore(extractRequest);

                _logger.LogInformation("IEEE Xplore answer:\n" + iEEEXploreAnswer);
            }

            if (loadRequest.loadFromGoogleScholar)
            {
                googleScholarAnswer = await requestsManager.LoadDataFromGoogleScholar(extractRequest);

                _logger.LogInformation("Google Scholar answer:\n" + googleScholarAnswer);
            }

            return new LoadAnswer(dBLPAnswer, iEEEXploreAnswer, googleScholarAnswer);
        }
    }
}
