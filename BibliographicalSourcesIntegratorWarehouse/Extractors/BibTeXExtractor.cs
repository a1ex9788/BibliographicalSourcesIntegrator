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


            string preparedJsonBooks = PrepareJson(json, "books");
            string preparedJsonArticles = PrepareJson(json, "articles");
            string preparedJsonInproceedings = PrepareJson2(json, "inproceedings");
            string preparedJsonIncollection = PrepareJson2(json, "incollection");


            logger.LogInformation("Converting the json to IEEEXplore schema...");
                                                                                                                    
            List<BibTeXPublicationSchema> booksPublications = JsonConvert.DeserializeObject<List<BibTeXPublicationSchema>>(preparedJsonBooks);
            List<BibTeXPublicationSchema> articlesPublications = JsonConvert.DeserializeObject<List<BibTeXPublicationSchema>>(preparedJsonArticles);
            List<BibTeXPublicationSchema> inproceedingsPublications = JsonConvert.DeserializeObject<List<BibTeXPublicationSchema>>(preparedJsonInproceedings);
            List<BibTeXPublicationSchema> incollectionPublications = JsonConvert.DeserializeObject<List<BibTeXPublicationSchema>>(preparedJsonIncollection);

            logger.LogInformation("Creating the publications...");

            foreach (BibTeXPublicationSchema googleScholarPublication in booksPublications)
            {
                try 
                {
                    books.Add(publicationCreator.CreateBook(
                            title: googleScholarPublication.title,
                            year: googleScholarPublication.year,
                            url: googleScholarPublication.url,
                            authors:  null, //googleScholarPublication.GetAuthors(), Falta tratar
                            editorial: null));                 
                }
                catch (Exception e)
                {
                    errorList.Add(e.Message);
                }
            }

            foreach (BibTeXPublicationSchema googleScholarPublication in articlesPublications)
            {
                try
                {
                    articles.Add(publicationCreator.CreateArticle(
                        title: googleScholarPublication.title,
                        year: googleScholarPublication.year,
                        url: googleScholarPublication.url,
                        authors: null, //googleScholarPublication.GetAuthors(), 
                        initialPage: googleScholarPublication.GetInitialPage(),
                        finalPage: googleScholarPublication.GetFinalPage(),
                        volume: googleScholarPublication.volume,
                        number: googleScholarPublication.number,
                        month: null,
                        journalName: googleScholarPublication.publisher));

                }
                catch (Exception e)
                {
                    errorList.Add(e.Message);
                }
            }

            foreach (BibTeXPublicationSchema googleScholarPublication in inproceedingsPublications)
            {
                try
                {
                    conferences.Add(publicationCreator.CreateCongressComunication(
                        title: googleScholarPublication.title,
                        year: googleScholarPublication.year,
                        url: googleScholarPublication.url,
                        authors: null, //googleScholarPublication.GetAuthors(),
                        edition: null,
                        congress: googleScholarPublication.booktitle, //Este no estic segur
                        place: googleScholarPublication.place,
                        initialPage: googleScholarPublication.GetInitialPage(),
                        finalPage: googleScholarPublication.GetFinalPage()));

                }
                catch (Exception e)
                {
                    errorList.Add(e.Message);
                }
            }

            //Faltaria saber que són els "incollection"

            logger.LogInformation("Saving the publications into the database...");

          /*  databaseAccess.SaveBooks(books);
            databaseAccess.SaveCongressComunications(conferences);
            databaseAccess.SaveArticles(articles); */

            return ((booksPublications.Count + articlesPublications.Count + inproceedingsPublications.Count) - errorList.Count, errorList);
        }

       /*private string PrepareJsonBooks(string json, string tipo)
        {
            string aux = json;
            string articlesList = "";
           
           //int pos = aux.IndexOf("books");
           int pos = aux.IndexOf(tipo);

            if (pos == -1) //No hay libros
            {
                return null;
            }
            else
            {
                articlesList = aux.Substring(pos);
                logger.LogInformation(articlesList);
                int pos4 = articlesList.IndexOf(":");
                int pos5 = articlesList.IndexOf("[");
                logger.LogInformation("pos4:   **************   " + pos4  + "      pos5: **************    " + pos5);
                if (articlesList.IndexOf("[") == pos4 + 5)
                {
                    logger.LogInformation(articlesList);
                    int pos2 = articlesList.IndexOf("[");
                    int pos3 = articlesList.IndexOf("]");
                    logger.LogInformation("pos2:   " + pos2 + "  pos3:  " + pos3);
                    articlesList = articlesList.Substring(pos2, (pos3 - pos2) + 1);
                }
                

            }

            return articlesList;
        } */

        private string PrepareJson(string json, string tipo)   //FUNCIONA  para Books y Articles
        {
            string aux = json;
            string articlesList = "";

            int pos = aux.IndexOf(tipo);

            if (pos == -1) //No hay publicaciones
            {
                return null;
            }
            else
            {
                articlesList = aux.Substring(pos);
                int pos2 = articlesList.IndexOf("["); //Inicio de la lista
                int pos3 = articlesList.IndexOf("]"); //Final de la lista
                articlesList = articlesList.Substring(pos2, (pos3 - pos2) + 1);
            }
            return articlesList;
        }

        private string PrepareJson2(string json, string tipo)   //FUNCIONA  para inproceedings y incollection
        {
            string aux = json;
            string articlesList = "";

            int pos = aux.IndexOf(tipo);

            if (pos == -1) //No hay publicaciones
            {
                return null;
            }
            else
            {
                aux = aux.Substring(pos);
                int pos2 = aux.IndexOf("{"); //Inicio de la lista
                articlesList = "[";
                int pos3 = aux.IndexOf("}"); //Final de la lista
                articlesList = articlesList + aux.Substring(pos2, (pos3 - pos2) + 1) + "]";
            }
            return articlesList;
        }

    }

    class BibTeXPublicationSchema
    {
        public string title { get; set; }

        public string year { get; set; }

        public string pages { get; set; }

        public string author { get; set; }

        //public List<Object> authors { get; set; }

        public string journal { get; set; }

        public string booktitle { get; set; } //Esto es para incollection y inproceedings

        public string publisher { get; set; }

        public string volume { get; set; }

        public string number { get; set; }

        public string url { get; set; }

        public string place { get; set; }


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
