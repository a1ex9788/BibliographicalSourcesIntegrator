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
                string jsonWithoutSpecialChars = json.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace("@", "");

                string jsonWithoutDBLPNode = jsonWithoutSpecialChars.Substring(21);

                string jsonWithoutEndDBLPNodeBracket = jsonWithoutDBLPNode.Substring(0, jsonWithoutDBLPNode.Length - 2);

                List<DBLPPublicationSchema> publications = JsonConvert.DeserializeObject<List<DBLPPublicationSchema>>(jsonWithoutEndDBLPNodeBracket);
            }
            catch (Exception e) { }
        }

        class DBLPPublicationSchema
        {
            public string mdate { get; set; }

            //public List<string> author { get; set; }

            public string title { get; set; }

            public string pages { get; set; }

            public int year { get; set; }

            public string volume { get; set; }

            public string journal { get; set; }
        }
    }
}
