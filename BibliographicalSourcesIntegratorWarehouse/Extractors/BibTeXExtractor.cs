using BibliographicalSourcesIntegratorWarehouse.Entities;
using BibliographicalSourcesIntegratorWarehouse.Persistence;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Extractors
{
    public class BibTeXExtractor
    {
        private readonly PublicationCreator publicationCreator;
        private readonly DatabaseAccess databaseAccess;
        private readonly ILogger<BibTeXExtractor> logger;


        public BibTeXExtractor(PublicationCreator publicationCreator, DatabaseAccess databaseAccess, ILogger<BibTeXExtractor> logger)
        {
            this.publicationCreator = publicationCreator;
            this.databaseAccess = databaseAccess;
            this.logger = logger;
        }


        public (int, List<string>) ExtractData(string json)
        {
            List<string> errorList = new List<string>();
            List<Article> articles = new List<Article>();
            List<Book> books = new List<Book>();
            List<CongressComunication> conferences = new List<CongressComunication>(); //inproceedings

            //Faltaría saber que son los "incollection"

            logger.LogInformation("Preparing the json...");

            string preparedJson = PrepareJson(json);

            logger.LogInformation("Converting the json to IEEEXplore schema...");

            List<BibTeXPublicationSchema> publications = JsonConvert.DeserializeObject<List<BibTeXPublicationSchema>>(preparedJson);

            logger.LogInformation("Creating the publications...");


            // Leer json, aplicar mappings y guardar en la BD
            return (0, null);
        }

        private string PrepareJson(string json)
        {
            throw new NotImplementedException();
        }
    }

    class BibTeXPublicationSchema
    {
        public string title { get; set; }

        public int year { get; set; }

        public string pages { get; set; }

        public List<Object> authors { get; set; }

        public string journal { get; set; }

        public string book_title { get; set; } //Esto es para incollection y inproceedings

        public string publisher { get; set; }

        public string volume { get; set; }

        public string article_number { get; set; }




    }


}
