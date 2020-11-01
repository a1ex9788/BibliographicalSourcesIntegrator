using BibliographicalSourcesIntegratorWarehouse.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Extractors
{
    public class DBLPExtractor
    {
        public void ExtractData(string json)
        {
            // Leer json, aplicar mappings y guardar en la BD
            try
            {
                string jsonWithoutSpecialChars = json.Replace("\t", "").Replace("\n", "").Replace("\r", "");

                string jsonWithoutDBLPNode = jsonWithoutSpecialChars.Substring(9);

                string jsonWithoutEndDBLPNodeBracket = jsonWithoutDBLPNode.Remove(jsonWithoutDBLPNode.Length - 1);

                jsonWithoutEndDBLPNodeBracket.Remove(0);
                jsonWithoutEndDBLPNodeBracket.Remove(jsonWithoutEndDBLPNodeBracket.Length - 1);

                string jsonAsAListOfPublications = '[' + jsonWithoutEndDBLPNodeBracket + ']';

                List<Article> publications = JsonConvert.DeserializeObject<List<Article>>(jsonAsAListOfPublications);
            }
            catch (Exception e) { }
        }
    }
}
