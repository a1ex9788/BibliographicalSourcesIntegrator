using BibliographicalSourcesIntegrator;
using BibliographicalSourcesIntegratorContracts;
using BibliographicalSourcesIntegratorWarehouse.Extractors;
using BibliographicalSourcesIntegratorWarehouse.Persistence;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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


        public async Task<LoadAnswer> Load(string request)
        {
            LoadRequest loadRequest = GetLoadRequest(request);

            if (loadRequest == null)
            {
                return null;
            }

            return await ProcessLoadRequest(loadRequest);
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
            LoadAnswer loadAnswer = new LoadAnswer();

            ExtractRequest extractRequest = new ExtractRequest(loadRequest.InitialYear, loadRequest.FinalYear);

            if (loadRequest.LoadFromDBLP)
            {
                try
                {
                    string jsonDBLPAnswer = await requestsManager.LoadDataFromDBLP(extractRequest);

                    (int numberOfResults, List<string> errorList) = dBLPExtractor.ExtractData(jsonDBLPAnswer);

                    loadAnswer.DBLPNumberOfResults = numberOfResults;
                    loadAnswer.DBLPErrors = errorList;

                    _logger.LogInformation("Publications between " + loadRequest.InitialYear + " and " + loadRequest.FinalYear + " loaded: " + numberOfResults);
                }
                catch (HttpRequestException)
                {
                    _logger.LogError("There was an error communicating to the DBLP wrapper.");
                }
                catch (Exception)
                {
                    _logger.LogError("There was an error while extracting data from the DBLP answer.");
                }
            }

            if (loadRequest.LoadFromIEEEXplore)
            {
                try
                {
                    string jsonIEEEXploreAnswer = await requestsManager.LoadDataFromIEEEXplore(extractRequest);

                    (int numberOfResults, List<string> errorList) = iEEEXploreExtractor.ExtractData(jsonIEEEXploreAnswer);

                    loadAnswer.IEEEXploreNumberOfResults = numberOfResults;
                    loadAnswer.IEEEXploreErrors = errorList;

                    _logger.LogInformation("Publications between " + loadRequest.InitialYear + " and " + loadRequest.FinalYear + " loaded: " + numberOfResults);
                }
                catch (HttpRequestException)
                {
                    _logger.LogError("There was an error communicating to the IEEEXplore wrapper.");
                }
                catch (Exception)
                {
                    _logger.LogError("There was an error while extracting data from the IEEEXplore answer.");
                }
            }

            if (loadRequest.LoadFromGoogleScholar)
            {
                try
                {
                    string jsonGoogleScholarAnswer = await requestsManager.LoadDataFromGoogleScholar(extractRequest);

                    (int numberOfResults, List<string> errorList) = bibTeXExtractor.ExtractData(jsonGoogleScholarAnswer);

                    loadAnswer.GoogleScholarNumberOfResults = numberOfResults;
                    loadAnswer.GoogleScholarErrors = errorList;

                    _logger.LogInformation("Publications between " + loadRequest.InitialYear + " and " + loadRequest.FinalYear + " loaded: " + numberOfResults);
                }
                catch (HttpRequestException)
                {
                    _logger.LogError("There was an error communicating to the Google Scholar wrapper.");
                }
                catch (Exception)
                {
                    _logger.LogError("There was an error while extracting data from the Google Scholar answer.");
                }
            }

            return loadAnswer;
        }
    }
}
