using BibliographicalSourcesIntegratorWarehouse.Entities;
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


        public (int numberOfResults, List<string> errorList) ExtractData(string json)
        {
            List<string> errorList = new List<string>();
            List<Article> articles = new List<Article>();
            List<Book> books = new List<Book>();
            List<CongressComunication> conferences = new List<CongressComunication>();

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
                        articles.Add(publicationCreator.CreateArticle(
                            title: ieeePublication.title,
                            year: ieeePublication.publication_year,
                            url: ieeePublication.pdf_url,
                            authors: ieeePublication.GetAuthors(),
                            initialPage: ieeePublication.GetIniPage(),
                            finalPage: ieeePublication.GetFinalPage(),
                            volume: ieeePublication.volume,
                            number: ieeePublication.article_number,
                            month: ieeePublication.GetMonth(),
                            journalName: ieeePublication.publisher));
                    }
                    else if (ieeePublication.content_type == "Conferences")
                    {
                        conferences.Add(publicationCreator.CreateCongressComunication(
                            title: ieeePublication.publication_title,
                            year: ieeePublication.publication_year,
                            url: ieeePublication.pdf_url,
                            authors: ieeePublication.GetAuthors(),
                            edition: -1,
                            congress: ieeePublication.title,
                            place: ieeePublication.conference_location,
                            initialPage: ieeePublication.GetIniPage(),
                            finalPage: ieeePublication.GetFinalPage()));
                    }
                    else 
                    {
                        books.Add(publicationCreator.CreateBook(
                            title: ieeePublication.publication_title,
                            year: ieeePublication.publication_year,
                            url: ieeePublication.pdf_url,
                            authors: ieeePublication.GetAuthors(),
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
            databaseAccess.SaveArticles(articles);

            return (publications.Count - errorList.Count, errorList);
        }


        static string PrepareJson(string json)
        {
            string aux = json;
            int currentInitialArticlesPos = 0;

            int indexOfArticle = aux.IndexOf("[{\"doi\"");
            int posToInvestigate = currentInitialArticlesPos + indexOfArticle;
            aux = aux.Substring(posToInvestigate);
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

            public int publication_year { get; set; }

            public string start_page { get; set; }

            public string end_page { get; set; }

            public string pdf_url { get; set; }

            public string article_number { get; set; }

            public string publication_date { get; set; }

            public string conference_dates { get; set; }

            public string conference_location { get; set; }

            public string content_type { get; set; }


            public int GetIniPage()
            {
                int number;
                bool success = Int32.TryParse(start_page, out number);

                if (success)
                {
                    return number;
                }
                else
                {
                    return -1;
                }
            }

            public int GetFinalPage()
            {
                int number;
                bool success = Int32.TryParse(end_page, out number);

                if (success)
                {
                    return number;
                }
                else
                {
                    return -1;
                }
            }

            public int GetMonth()
            {
                if (publication_date != null)
                {
                    if (publication_date.Contains("January"))
                    {
                        return 1;
                    }
                    else if (publication_date.Contains("February"))
                    {
                        return 2;
                    }
                    else if (publication_date.Contains("March"))
                    {
                        return 3;
                    }
                    else if (publication_date.Contains("April"))
                    {
                        return 4;
                    }
                    else if (publication_date.Contains("May"))
                    {
                        return 5;
                    }
                    else if (publication_date.Contains("June"))
                    {
                        return 6;
                    }
                    else if (publication_date.Contains("July"))
                    {
                        return 7;
                    }
                    else if (publication_date.Contains("August"))
                    {
                        return 8;
                    }
                    else if (publication_date.Contains("September"))
                    {
                        return 9;
                    }
                    else if (publication_date.Contains("October"))
                    {
                        return 10;
                    }
                    else if (publication_date.Contains("November"))
                    {
                        return 11;
                    }
                    else
                    {
                        return 12;
                    }
                }
                else
                {
                    return -1;
                }
            }

            public List<(string name, string surnames)> GetAuthors()
            {
                return authors.GetAuthors();
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