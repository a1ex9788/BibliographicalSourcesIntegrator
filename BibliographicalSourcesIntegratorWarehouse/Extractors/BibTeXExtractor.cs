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

            //string pruebaJson = json;
            //string preparedJson = PrepareJson(json);

            logger.LogInformation("Converting the json to IEEEXplore schema...");
                                                                                                                    //preparedJson
            List<BibTeXPublicationSchema> publications = JsonConvert.DeserializeObject<List<BibTeXPublicationSchema>>(json);

            logger.LogInformation("Creating the publications...");

            /*foreach (BibTeXPublicationSchema googleScholarPublication in publications)
            {
                try 
                {
                    if (googleScholarPublication.content_type == "Journals")
                    {
                        articles.Add(publicationCreator.CreateArticle(
                            title: googleScholarPublication.title,
                            year: googleScholarPublication.year,
                            //url: googleScholarPublication.pdf_url,
                            authors: googleScholarPublication.GetAuthors(),
                            initialPage: googleScholarPublication.GetInitialPage(),
                            finalPage: googleScholarPublication.GetFinalPage(),
                            volume: googleScholarPublication.volume,
                            number: googleScholarPublication.article_number,
                            month: googleScholarPublication.GetMonth(),
                            journalName: googleScholarPublication.publisher)); ;
                    }
                    else if (googleScholarPublication.content_type == "Conferences")
                    {
                        conferences.Add(publicationCreator.CreateCongressComunication(
                            title: googleScholarPublication.publication_title,
                            year: googleScholarPublication.publication_year,
                            url: googleScholarPublication.pdf_url,
                            authors: googleScholarPublication.GetAuthors(),
                            edition: null,
                            congress: googleScholarPublication.title,
                            place: googleScholarPublication.conference_location,
                            initialPage: googleScholarPublication.start_page,
                            finalPage: googleScholarPublication.end_page));
                    }
                    else 
                    {
                        books.Add(publicationCreator.CreateBook(
                            title: googleScholarPublication.publication_title,
                            year: googleScholarPublication.publication_year,
                            url: googleScholarPublication.pdf_url,
                            authors: googleScholarPublication.GetAuthors(),
                            editorial: null));
                    }                    
                }
                catch (Exception e)
                {
                    errorList.Add(e.Message);
                }
            } 

            logger.LogInformation("Saving the publications into the database...");

            databaseAccess.SaveBooks(books);
            databaseAccess.SaveCongressComunications(conferences);
            databaseAccess.SaveArticles(articles); */

            return (publications.Count - errorList.Count, errorList);
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

        public string author { get; set; }

        //public List<Object> authors { get; set; }

        public string journal { get; set; }

        public string booktitle { get; set; } //Esto es para incollection y inproceedings

        public string publisher { get; set; }

        public string volume { get; set; }

        public string article_number { get; set; }


        public string GetInitialPage()
        {
            try
            {
                int slashPosition = pages.IndexOf('-');

                return pages.Substring(0, slashPosition);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetFinalPage()
        {
            try
            {
                int slashPosition = pages.IndexOf('-');

                return pages.Substring(slashPosition + 1);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }




}
