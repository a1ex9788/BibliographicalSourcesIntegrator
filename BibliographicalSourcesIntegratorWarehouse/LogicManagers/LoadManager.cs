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
        private readonly AppDbContext context;
        private RequestsManager requestsManager;
        private DBLPExtractor dBLPExtractor;
        private IEEEXploreExtractor iEEEXploreExtractor;
        private BibTeXExtractor bibTeXExtractor;
        private readonly ILogger<LoadManager> _logger;

        public LoadManager(AppDbContext context, RequestsManager requestsManager, DBLPExtractor dBLPExtractor, IEEEXploreExtractor iEEEXploreExtractor, BibTeXExtractor bibTeXExtractor, ILogger<LoadManager> logger)
        {
            this.context = context;
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
                string dBLPAnswerJSON = await requestsManager.LoadDataFromDBLP(extractRequest);

                _logger.LogInformation("DBLP answer:\n" + dBLPAnswer);

                dBLPExtractor.ExtractData(dBLPAnswerJSON);
            }

            if (loadRequest.LoadFromIEEEXplore)
            {
                string iEEEXploreAnswerJSON = await requestsManager.LoadDataFromIEEEXplore(extractRequest);

                _logger.LogInformation("IEEE Xplore answer:\n" + iEEEXploreAnswer);

                iEEEXploreExtractor.ExtractData(iEEEXploreAnswerJSON);
            }

            if (loadRequest.LoadFromGoogleScholar)
            {
                string googleScholarAnswerJSON = await requestsManager.LoadDataFromGoogleScholar(extractRequest);

                _logger.LogInformation("Google Scholar answer:\n" + googleScholarAnswer);

                bibTeXExtractor.ExtractData(googleScholarAnswerJSON);
            }

            return new LoadAnswer(dBLPAnswer, iEEEXploreAnswer, googleScholarAnswer);
        }
    }
}
