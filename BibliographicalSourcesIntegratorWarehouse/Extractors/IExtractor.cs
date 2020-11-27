using BibliographicalSourcesIntegratorContracts.Entities;
using BibliographicalSourcesIntegratorWarehouse.Persistence;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Extractors
{
    public abstract class IExtractor
    {
        private readonly DatabaseAccess databaseAccess;
        private readonly ILogger<IExtractor> logger;


        public IExtractor(DatabaseAccess databaseAccess, ILogger<IExtractor> logger)
        {
            this.databaseAccess = databaseAccess;
            this.logger = logger;
        }


        public (int, List<string>) ExtractData<T>(string extractorName, string json)
        {
            List<string> errorList = new List<string>();
            List<Article> articlesToSave = new List<Article>();
            List<Book> booksToSave = new List<Book>();
            List<CongressComunication> congressComunicationsToSave = new List<CongressComunication>();

            logger.LogInformation(extractorName + "extractor: Preparing the json...");

            string preparedJson = PrepareJson(json);

            if (preparedJson == null)
            {
                string errorMessage = extractorName + "extractor: There was a problem preparing the json";

                logger.LogInformation(errorMessage);
                errorList.Add(errorMessage);

                return (0, errorList);
            }

            logger.LogInformation(extractorName + "extractor: Converting the json to the schema...");

            List<T> publications = JsonConvert.DeserializeObject<List<T>>(preparedJson);

            logger.LogInformation(extractorName + "extractor: Creating the publications...");

            foreach (T publication in publications)
            {
                try
                {
                    if (IsArticle(publication))
                    {
                        Article article = CreateArticle(publication);

                        if (!articlesToSave.Contains(article) && databaseAccess.GetArticle(article) == null)
                        {
                            articlesToSave.Add(article);
                        }
                    }
                    else if (IsBook(publication))
                    {
                        Book book = CreateBook(publication);

                        if (!booksToSave.Contains(book) && databaseAccess.GetBook(book) == null)
                        {
                            booksToSave.Add(book);
                        }
                    }
                    else if (IsCongressComunication(publication))
                    {
                        CongressComunication conference = CreateCongressComunication(publication);

                        if (!congressComunicationsToSave.Contains(conference) && databaseAccess.GetCongressComunication(conference) == null)
                        {
                            congressComunicationsToSave.Add(conference);
                        }
                    }
                }
                catch (Exception e)
                {
                    errorList.Add(e.Message);
                }
            }

            logger.LogInformation(extractorName + "extractor: Saving the publications into the database...");

            databaseAccess.SaveArticles(articlesToSave);
            databaseAccess.SaveBooks(booksToSave);
            databaseAccess.SaveCongressComunications(congressComunicationsToSave);

            return (articlesToSave.Count + booksToSave.Count + congressComunicationsToSave.Count, errorList);
        }


        public abstract string PrepareJson(string source);

        public abstract Article CreateArticle<T>(T publication);

        public abstract Book CreateBook<T>(T publication);

        public abstract CongressComunication CreateCongressComunication<T>(T publication);

        public abstract bool IsArticle<T>(T publication);

        public abstract bool IsBook<T>(T publication);

        public abstract bool IsCongressComunication<T>(T publication);
    }
}
