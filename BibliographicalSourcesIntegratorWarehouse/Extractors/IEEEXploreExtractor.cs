using BibliographicalSourcesIntegratorWarehouse.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Extractors
{
    public class IEEEXploreExtractor
    {
        public void ExtractData(string json)
        {
            try
            {
                string preparedJson = PrepareJson(json);

                List<IEEEXplorerPublicationSchema> publications = JsonConvert.DeserializeObject<List<IEEEXplorerPublicationSchema>>(preparedJson);

                foreach (IEEEXplorerPublicationSchema IEEPublication in publications)
                {
                    // Article publication = new Article(
                    // title: IEEPublication.title,
                    // year: IEEPublication.publication_year,
                    // url: IEEPublication.pdf_url,
                    // people: null,
                    // initialPage: Convert.ToInt32(IEEPublication.start_page),
                    // finalPage: Convert.ToInt32(IEEPublication.end_page),
                    // exemplar: new Exemplar(Convert.ToInt32(IEEPublication.volume), Convert.ToInt32(IEEPublication.article_number), 1, null, null));
                }

            }

            catch (Exception e) { }
        }

        static string PrepareJson(string json)
        {
            string aux = json;
            string jsonWithoutSpecialChars = aux.Replace("\t", "").Replace("\n", "").Replace("\r", "");
            string jsonWithoutRootNode = jsonWithoutSpecialChars.Substring(68);
            // jsonWithoutRootNode = '[' + jsonWithoutRootNode;
            return jsonWithoutRootNode;
        }

        class IEEEXplorerPublicationSchema
        {
            public string title { get; set; }

            public string publisher { get; set; }

            public string volume { get; set; }

            public List<Object> authors { get; set; }

            public string publication_title { get; set; }

            public int publication_year { get; set; }

            public string start_page { get; set; }

            public string end_page { get; set; }

            public string pdf_url { get; set; }

            public string article_number { get; set; }

            public string publication_date { get; set; }




        }
    }
}