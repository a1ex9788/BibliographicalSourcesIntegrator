using BibliographicalSourcesIntegrator;
using BibliographicalSourcesIntegratorContracts;
using Microsoft.Extensions.Logging;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DBLPExtractor.LogicManagers
{
    public class ExtractDataManager
    {
        private readonly ILogger<ExtractDataManager> _logger;

        public ExtractDataManager(ILogger<ExtractDataManager> logger)
        {
            _logger = logger;
        }


        public string ExtractData(string request)
        {
            ExtractRequest extractRequest = GetExtractRequest(request);

            if (extractRequest != null)
            {
                return ExtractDataFromFile(extractRequest.InitialYear, extractRequest.FinalYear);
            }

            return null;
        }


        private ExtractRequest GetExtractRequest(string request)
        {
            try
            {
                return new JavaScriptSerializer().Deserialize<ExtractRequest>(request);
            }
            catch (Exception)
            {
                _logger.LogError("The request string is not an ExtractRequest.");

                return null;
            }
        }

        private string ExtractDataFromFile(int initialYear, int finalYear)
        {
            try
            {
                // TODO: Utilizar herramientas de XML para obtener los datos

                FileStream file = File.OpenRead("../../DBLP.XML");

                return "";
            }
            catch (Exception)
            {
                _logger.LogError("There was a problem working with the XML file.");

                return null;
            }
        }
    }
}
