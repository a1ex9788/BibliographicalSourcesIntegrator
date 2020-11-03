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
            //        string preparedJson = PrepareJson(json);
            //
            //        List<DBLPPublicationSchema> publications = JsonConvert.DeserializeObject<List<DBLPPublicationSchema>>(preparedJson);
            //
            //        foreach (DBLPPublicationSchema dBLPPublication in publications)
            //        {
            //            Article publication = new Article(
            //                title: dBLPPublication.title,
            //                year: dBLPPublication.year,
            //                url: dBLPPublication.url,
            //                people: null,
            //                initialPage: GetInitialPage(dBLPPublication.pages),
            //                finalPage: GetFinalPage(dBLPPublication.pages),
            //                exemplar: null
            //            );
            //        }
                }
                catch (Exception e) { }
        }
        //
        //static string PrepareJson(string source)
        //{
        //    string aux = source;
        //
        //    string jsonWithoutSpecialChars = aux.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace("$", "dollar");
        //
        //    string jsonWithoutDBLPNode = jsonWithoutSpecialChars.Substring(21);
        //
        //    string jsonWithoutEndDBLPNodeBracket = jsonWithoutDBLPNode.Substring(0, jsonWithoutDBLPNode.Length - 2);
        //
        //    //string jsonWithSquareBrackets = AddSquareBrackets(jsonWithoutEndDBLPNodeBracket);
        //
        //    return jsonWithSquareBrackets;
        //}
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

