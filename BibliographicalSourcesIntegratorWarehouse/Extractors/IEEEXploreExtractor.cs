using BibliographicalSourcesIntegratorContracts.Entities;
using BibliographicalSourcesIntegratorWarehouse.Persistence;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliographicalSourcesIntegratorWarehouse.Extractors
{
    public class IEEEXploreExtractor
    {
        private readonly PublicationCreator publicationCreator;
        private readonly DatabaseAccess databaseAccess;
        private readonly ILogger<IEEEXploreExtractor> logger;


        public IEEEXploreExtractor(PublicationCreator publicationCreator, DatabaseAccess databaseAccess, ILogger<IEEEXploreExtractor> logger)
        {
            this.publicationCreator = publicationCreator;
            this.databaseAccess = databaseAccess;
            this.logger = logger;
        }


        public (int, List<string>) ExtractData(string json)
        {
            List<string> errorList = new List<string>();
            List<Article> articlesToSave = new List<Article>();
            List<Book> booksToSave = new List<Book>();
            List<CongressComunication> conferencesToSave = new List<CongressComunication>();

            logger.LogInformation("Preparing the json...");

            string preparedJson = PrepareJson(json);

            logger.LogInformation("Converting the json to IEEEXplore schema...");

            List<IEEEXplorerPublicationSchema> publications = JsonConvert.DeserializeObject<List<IEEEXplorerPublicationSchema>>(preparedJson);

            logger.LogInformation("Creating the publications...");

            foreach (IEEEXplorerPublicationSchema ieeePublication in publications)
            {
                try 
                {
                    if (ieeePublication.content_type == "Journals")
                    {
                       Article article = publicationCreator.CreateArticle(
                            title: ieeePublication.title,
                            year: Convert.ToInt32(ieeePublication.publication_year),
                            url: ieeePublication.pdf_url,
                            authors: ieeePublication.GetAuthors(),
                            initialPage: ieeePublication.start_page,
                            finalPage: ieeePublication.end_page,
                            volume: ieeePublication.volume,
                            number: ieeePublication.article_number,
                            month: ieeePublication.GetMonth(),
                            journalName: ieeePublication.publisher);

                        if (databaseAccess.GetArticle(article) == null)
                        {
                            articlesToSave.Add(article);
                        }
                    }
                    else if (ieeePublication.content_type == "Conferences")
                    {
                        CongressComunication conference = publicationCreator.CreateCongressComunication(
                            title: ieeePublication.publication_title,
                            year: Convert.ToInt32(ieeePublication.publication_year),
                            url: ieeePublication.pdf_url,
                            authors: ieeePublication.GetAuthors(),
                            edition: null,
                            congress: ieeePublication.title,
                            place: ieeePublication.conference_location,
                            initialPage: ieeePublication.start_page,
                            finalPage: ieeePublication.end_page);

                        if (databaseAccess.GetCongressComunication(conference) == null)
                        {
                            conferencesToSave.Add(conference);
                        }
                    }
                    else 
                    {
                       Book book = publicationCreator.CreateBook(
                            title: ieeePublication.publication_title,
                            year: Convert.ToInt32(ieeePublication.publication_year),
                            url: ieeePublication.pdf_url,
                            authors: ieeePublication.GetAuthors(),
                            editorial: null);

                        if (databaseAccess.GetBook(book) == null)
                        {
                            booksToSave.Add(book);
                        }
                    }                    
                }
                catch (Exception e)
                {
                    errorList.Add(e.Message);
                }
            }

            logger.LogInformation("Saving the publications into the database...");

            databaseAccess.SaveBooks(booksToSave);
            databaseAccess.SaveCongressComunications(conferencesToSave);
            databaseAccess.SaveArticles(articlesToSave);

            return (booksToSave.Count + conferencesToSave.Count + articlesToSave.Count, errorList);
        }


        static string PrepareJson(string json)
        {
            string aux = json;

            int indexOfArticle = aux.IndexOf("[");
            aux = aux.Substring(indexOfArticle);
            aux = aux.Remove(aux.Length - 1, 1);
            
            return aux;
        }


        class IEEEXplorerPublicationSchema
        {
            public string title { get; set; }

            public string publisher { get; set; }

            public string volume { get; set; }

            public Author authors { get; set; }

            public string publication_title { get; set; }

            public string publication_year { get; set; }

            public string start_page { get; set; }

            public string end_page { get; set; }

            public string pdf_url { get; set; }

            public string article_number { get; set; }

            public string publication_date { get; set; }

            public string conference_dates { get; set; }

            public string conference_location { get; set; }

            public string content_type { get; set; }


            public List<(string name, string surnames)> GetAuthors()
            {
                return authors.GetAuthors();
            }

            public string GetMonth()
            {
                char separator = ' ';

                if (publication_date.Contains('-'))
                {
                    separator = '-';   
                }

                return publication_date.Substring(0, publication_date.IndexOf(separator));
            }
        }


        class Author
        {
            public List<Object> authors { get; set; }


            public List<(string name, string surnames)> GetAuthors()
            {
                List<(string name, string surnames)> authorsFinal = new List<(string name, string surnames)>();

                // o --> {{ "authorUrl": "https://ieeexplore.ieee.org/author/37600583700", "id": 37600583700,"full_name": "Ying-Nong Chen","author_order": 1}}
                foreach (Object o in authors)
                {
                    string name = "";
                    string surnames = "";
                    string completeName = "";


                    Author2 author = JsonConvert.DeserializeObject<Author2>(o.ToString());
                    completeName = author.full_name;

                    name = GetName(completeName);
                    surnames = GetSurnames(completeName);

                    authorsFinal.Add((name, surnames));
                }

                return authorsFinal;


                string GetName(string completeName)
                {
                    string[] splittedName = completeName.Split(' ');

                    if (splittedName.Length == 0)
                    {
                        return "";
                    }

                    if (splittedName.Length > 1 && splittedName[1].Contains('.'))
                    {
                        return splittedName.Length > 1 ? splittedName[0] + " " + splittedName[1] : splittedName[0];
                    }
                    else
                    {
                        return splittedName[0];
                    }
                }

                string GetSurnames(string completeName)
                {
                    string[] splittedName = completeName.Split(' ');
                    string surnames = "";

                    if (splittedName.Length <= 1)
                    {
                        return "";
                    }

                    if (splittedName[1].Contains('.'))
                    {
                        for (int i = 2; i < splittedName.Length; i++)
                        {
                            surnames += splittedName[i] + " ";
                        }
                    }
                    else
                    {
                        for (int i = 1; i < splittedName.Length; i++)
                        {
                            surnames += splittedName[i] + " ";
                        }
                    }

                    return surnames.TrimEnd();
                }

            }


            class Author2
            {
                string authorUrl;

                public string full_name;

                string id;

                string author_order;


                public Author2(string authorUrl, string full_name, string id, string author_order)
                {
                    this.authorUrl = authorUrl;
                    this.full_name = full_name;
                    this.id = id;
                    this.author_order = author_order;
                }
            }
        }
    }
}