using BibliographicalSourcesIntegrator;
using BibliographicalSourcesIntegratorContracts;
using BibliographicalSourcesIntegratorWarehouse.Extractors;
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
        private RequestsManager requestsManager;
        private DBLPExtractor dBLPExtractor;
        private IEEEXploreExtractor iEEEXploreExtractor;
        private BibTeXExtractor bibTeXExtractor;
        private readonly ILogger<LoadManager> _logger;

        public LoadManager(RequestsManager requestsManager, DBLPExtractor dBLPExtractor, IEEEXploreExtractor iEEEXploreExtractor, BibTeXExtractor bibTeXExtractor, ILogger<LoadManager> logger)
        {
            this.requestsManager = requestsManager;
            this.dBLPExtractor = dBLPExtractor;
            this.iEEEXploreExtractor = iEEEXploreExtractor;
            this.bibTeXExtractor = bibTeXExtractor;
            _logger = logger;
        }

        public async Task<string> Load(string request)
        {
            LoadRequest loadRequest = GetLoadRequest(request);

            if (loadRequest == null)
            {
                return null;
            }

            LoadAnswer loadAnswer = await ProcessLoadRequest(loadRequest);

            if (loadAnswer == null)
            {
                return null;
            }

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

        private async Task<LoadAnswer> ProcessLoadRequest(LoadRequest loadRequest)
        {
            // TODO: ver formato de respuestas
            string dBLPAnswer = "", iEEEXploreAnswer = "", googleScholarAnswer = "";

            ExtractRequest extractRequest = new ExtractRequest(loadRequest.InitialYear, loadRequest.FinalYear);

            if (loadRequest.LoadFromDBLP)
            {
                string jsonDBLPAnswer = await requestsManager.LoadDataFromDBLP(extractRequest);

                try
                {
                    dBLPExtractor.ExtractData(jsonDBLPAnswer);
                }
                catch (Exception)
                {
                    _logger.LogError("There was an error while extracting data from DBLP answer.");
                }
            }

            if (loadRequest.LoadFromIEEEXplore)
            {
                string jsonIEEEXploreAnswer = await requestsManager.LoadDataFromIEEEXplore(extractRequest);

                try
                {
                    iEEEXploreExtractor.ExtractData(jsonIEEEXploreAnswer);
                }
                catch (Exception)
                {
                    _logger.LogError("There was an error while extracting data from IEEEXplore answer.");
                }
            }

            if (loadRequest.LoadFromGoogleScholar)
            {
                string jsonGoogleScholarAnswer = await requestsManager.LoadDataFromGoogleScholar(extractRequest);

                try
                {
                    bibTeXExtractor.ExtractData(jsonGoogleScholarAnswer);
                }
                catch (Exception)
                {
                    _logger.LogError("There was an error while extracting data from Google Scholar answer.");
                }
            }

            return new LoadAnswer(dBLPAnswer, iEEEXploreAnswer, googleScholarAnswer);
        }
    }
}
