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
                    Journal journal = new Journal(
                        name: IEEPublication.publisher
                        );
                    Exemplar exemplar = new Exemplar(
                        //volume: Convert.ToInt32(IEEPublication.volume),
                        volume: "",
                        number: Convert.ToInt32(IEEPublication.article_number),
                        month: getMonth(IEEPublication.publication_date),
                        journal: journal
                        );
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
            string jsonWithoutLastBracket = jsonWithoutRootNode.Remove(jsonWithoutRootNode.Length-1);
            // jsonWithoutRootNode = '[' + jsonWithoutRootNode;
            return jsonWithoutLastBracket;
        }

        static int getMonth(string publicationDate)
        {
            if (publicationDate != null)
            {

                if (publicationDate.Contains("January"))
                {
                    return 1;
                }
                else if (publicationDate.Contains("February"))
                {
                    return 2;
                }
                else if (publicationDate.Contains("March"))
                {
                    return 3;
                }
                else if (publicationDate.Contains("April"))
                {
                    return 4;
                }
                else if (publicationDate.Contains("May"))
                {
                    return 5;
                }
                else if (publicationDate.Contains("June"))
                {
                    return 6;
                }
                else if (publicationDate.Contains("July"))
                {
                    return 7;
                }
                else if (publicationDate.Contains("August"))
                {
                    return 8;
                }
                else if (publicationDate.Contains("September"))
                {
                    return 9;
                }
                else if (publicationDate.Contains("October"))
                {
                    return 10;
                }
                else if (publicationDate.Contains("November"))
                {
                    return 11;
                }
                else
                {
                    return 12;
                }
            }
            else { return -1; }



        }


        class IEEEXplorerPublicationSchema
        {
            public string title { get; set; }

            public string publisher { get; set; }

            public string volume { get; set; }

            public Author authors { get; set; }

            public string publication_title { get; set; }

            public int publication_year { get; set; }

            public string start_page { get; set; }

            public string end_page { get; set; }

            public string pdf_url { get; set; }

            public string article_number { get; set; }

            public string publication_date { get; set; }




        }

        class Author
        {
            public List<Object> authors { get; set; }

        }
    }
}