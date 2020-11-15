using BibliographicalSourcesIntegrator;
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

            return ExtractDataFromXmlFile(extractRequest.InitialYear, extractRequest.FinalYear);
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

        private string ExtractDataFromXmlFile(int initialYear, int finalYear)
        {
            try
            {
                string xml = File.ReadAllText("../DBLP.XML");

                xml = FixXml(xml);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode dblpNode = doc.ChildNodes[1];

                XmlNodeList articlesListToRemove = dblpNode.SelectNodes($"/dblp/article[year<{initialYear} or year>{finalYear}]");

                foreach (XmlNode articleToRemove in articlesListToRemove.Cast<XmlNode>())
                {
                    dblpNode.RemoveChild(articleToRemove);
                }

                string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(dblpNode);

                _logger.LogInformation("Articles between " + initialYear + " and " + finalYear + " found: " + dblpNode.ChildNodes.Count);

                return json;
            }
            catch (Exception)
            {
                _logger.LogError("There was a problem working with the XML file.");

                return null;
            }


            // <title>Numerical analysis of CO<sub>2</sub> concentration and recovery from flue gas by a novel vacuum swing adsorption cycle.</title>
            // <sup>+</sup>
            // <i>in silico</i>
            string FixXml(string source)
            {
                return source.Replace("<sub>", "").Replace("</sub>", "").Replace("<sup>", "").Replace("</sup>", "").Replace("<i>", "").Replace("</i>", "");
            }
        }
    }
}
