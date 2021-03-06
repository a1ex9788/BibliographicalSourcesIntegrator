﻿using BibliographicalSourcesIntegratorContracts;
using IEEEXploreWrapper.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace IEEEXploreWrapper.LogicManagers
{
    public class ExtractDataManager
    {
        private readonly ILogger<ExtractDataManager> _logger;
        private readonly RequestsManager requestsManager;

        public ExtractDataManager(ILogger<ExtractDataManager> logger, RequestsManager requestsManager)
        {
            _logger = logger;
            this.requestsManager = requestsManager;
        }

        public async Task<string> ExtractData(string request)
        {
            ExtractRequest extractRequest = GetExtractRequest(request);

            if (extractRequest == null)
            {
                return null;
            }

            return await ExtractDataFromIEEEXploreAPIAsync(extractRequest.InitialYear, extractRequest.FinalYear);
        }

        private ExtractRequest GetExtractRequest(string request)
        {
            try
            {
                return JsonSerializer.Deserialize<ExtractRequest>(request);
            }
            catch (Exception)
            {
                _logger.LogError("The request is not an ExtractRequest.");

                return null;
            }
        }

        private async Task<string> ExtractDataFromIEEEXploreAPIAsync(int initialYear, int finalYear)
        {
            try
            {
                return await requestsManager.LoadDataFromDataSources(initialYear, finalYear);
            }
            catch (HttpRequestException)
            {
                _logger.LogError("There was a problem communicating to the IEEEXplore API.");

                return null;
            }
            catch (Exception)
            {
                _logger.LogError("There was a problem working with the IEEEXpore API answer.");

                return null;
            }
        }
    }
}