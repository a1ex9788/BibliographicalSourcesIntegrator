﻿using BibliographicalSourcesIntegrator;
using BibliographicalSourcesIntegratorContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace DBLPWrapper.LogicManagers
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

            if (extractRequest == null)
            {
                return null;
            }

            return ExtractDataFromFile(extractRequest.InitialYear, extractRequest.FinalYear);
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

        private string ExtractDataFromFile(int initialYear, int finalYear)
        {
            try
            {
                string xml = File.ReadAllText("../DBLP.XML");

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode dblpNode = doc.ChildNodes[1];

                XmlNodeList articlesListToRemove = dblpNode.SelectNodes($"/dblp/article[year<{initialYear} or year>{finalYear}]");

                foreach (XmlNode articleToRemove in articlesListToRemove.Cast<XmlNode>())
                {
                    dblpNode.RemoveChild(articleToRemove);
                }

                string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(dblpNode);

                _logger.LogInformation("Articles found between " + initialYear + " and " + finalYear + ": " + dblpNode.ChildNodes.Count);

                return json;
            }
            catch (Exception)
            {
                _logger.LogError("There was a problem working with the XML file.");

                return null;
            }
        }
    }
}
