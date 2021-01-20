using BibliographicalSourcesIntegrator;
using BibliographicalSourcesIntegratorContracts;
using BibliographicalSourcesIntegratorWarehouse.Extractors;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
                (int numberOfResults, List<string> errorList) = await LoadFromSource(
                    "DBLP",
                    loadRequest,
                    extractRequest,
                    requestsManager.LoadDataFromDBLP,
                    dBLPExtractor.ExtractData);

                loadAnswer.DBLPNumberOfResults = numberOfResults;
                loadAnswer.DBLPErrors = errorList;
            }

            if (loadRequest.LoadFromIEEEXplore)
            {
                (int numberOfResults, List<string> errorList) = await LoadFromSource(
                    "IEEEXplore",
                    loadRequest,
                    extractRequest,
                    requestsManager.LoadDataFromIEEEXplore,
                    iEEEXploreExtractor.ExtractData);

                loadAnswer.IEEEXploreNumberOfResults = numberOfResults;
                loadAnswer.IEEEXploreErrors = errorList;
            }

            if (loadRequest.LoadFromGoogleScholar)
            {
                (int numberOfResults, List<string> errorList) = await LoadFromSource(
                    "Google Scholar",
                    loadRequest,
                    extractRequest,
                    requestsManager.LoadDataFromGoogleScholar,
                    bibTeXExtractor.ExtractData);

                loadAnswer.GoogleScholarNumberOfResults = numberOfResults;
                loadAnswer.GoogleScholarErrors = errorList;
            }

            return loadAnswer;
        }

        private async Task<(int, List<string>)> LoadFromSource(string sourceName, LoadRequest loadRequest, ExtractRequest extractRequest, Func<ExtractRequest, Task<string>> loadData, Func<string, string, (int, List<string>)> extractData)
        {
            int numberOfResults = 0;
            List<string> errors = new List<string>();

            try
            {
                string jsonAnswer = await loadData(extractRequest);

                if (jsonAnswer == null || jsonAnswer.Equals(""))
                {
                    string errorMessage = "The " + sourceName + " wrapper had problems.";

                    errors.Add(errorMessage);

                    _logger.LogError(errorMessage);

                    return (numberOfResults, errors);
                }

                (numberOfResults, errors) = extractData(sourceName, jsonAnswer);

                _logger.LogInformation("Publications between " + loadRequest.InitialYear + " and " + loadRequest.FinalYear + " loaded: " + numberOfResults);
            }
            catch (HttpRequestException)
            {
                string errorMessage = "There was an error communicating to the " + sourceName + " wrapper.";

                errors.Add(errorMessage);

                _logger.LogError(errorMessage);
            }
            catch (Exception e)
            {
                string errorMessage = "There was an error while extracting data from the " + sourceName + " answer.";

                errors.Add(errorMessage);

                _logger.LogError(errorMessage);
            }

            return (numberOfResults, errors);
        }
    }
}