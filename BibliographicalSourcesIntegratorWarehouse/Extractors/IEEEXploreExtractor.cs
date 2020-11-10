using BibliographicalSourcesIntegratorWarehouse.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BibliographicalSourcesIntegratorWarehouse.Persistence;

namespace BibliographicalSourcesIntegratorWarehouse.Extractors
{
    public class IEEEXploreExtractor
    {
        private readonly PublicationCreator publicationConstructor;
        private readonly DatabaseAccess databaseAccess;


        public IEEEXploreExtractor(PublicationCreator publicationConstructor, DatabaseAccess databaseAccess)
        {
            this.publicationConstructor = publicationConstructor;
            this.databaseAccess = databaseAccess;
        }

        public (int numberOfResults, List<string> errorList) ExtractData(string json)
        {
            List<string> errorList = new List<string>();
            List<Article> articles = new List<Article>();
            List<Book> books = new List<Book>();
            List<CongressComunication> conferences = new List<CongressComunication>();

            string preparedJson = PrepareJson(json);

            List<IEEEXplorerPublicationSchema> publications = JsonConvert.DeserializeObject<List<IEEEXplorerPublicationSchema>>(preparedJson);

            foreach (IEEEXplorerPublicationSchema IEEPublication in publications)
            {
                try 
                {
                    if (IEEPublication.content_type == "Journals")
                    {
                        articles.Add(publicationConstructor.CreateArticle(
                            title: IEEPublication.title,
                            year: IEEPublication.publication_year,
                            url: IEEPublication.pdf_url,
                            authors: IEEPublication.authors.GetAuthors(),
                            initialPage: getIniPage(IEEPublication),
                            finalPage: getFinalPage(IEEPublication),
                            volume: IEEPublication.volume,
                            number: IEEPublication.article_number,
                            month: getMonth(IEEPublication.publication_date),
                            journalName: IEEPublication.publisher));
                    } else if (IEEPublication.content_type == "Conferences")
                    {
                        conferences.Add(publicationConstructor.CreateCongressComunication(
                            title: IEEPublication.publication_title,
                            year: IEEPublication.publication_year,
                            url: IEEPublication.pdf_url,
                            authors: IEEPublication.authors.GetAuthors(),
                            edition: -1,
                            congress: IEEPublication.title,
                            place: IEEPublication.conference_location,
                            initialPage: getIniPage(IEEPublication),
                            finalPage: getFinalPage(IEEPublication)));
                    } else 
                    {
                        books.Add(publicationConstructor.CreateBook(
                            title: IEEPublication.publication_title,
                            year: IEEPublication.publication_year,
                            url: IEEPublication.pdf_url,
                            authors: IEEPublication.authors.GetAuthors(),
                            editorial: null
                            ));
                    }
                    
                }
                catch (Exception e)
                {
                    errorList.Add(e.Message);
                }
            }

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

        static int getIniPage(IEEEXplorerPublicationSchema IEEPublication) 
        {
            if (IEEPublication.start_page != null)
            {
                return Convert.ToInt32(IEEPublication.start_page);
            } else return -1;
        }
        static int getFinalPage(IEEEXplorerPublicationSchema IEEPublication) 
        {
            if (IEEPublication.end_page != null)
            {
                return Convert.ToInt32(IEEPublication.end_page);
            }
            else return -1;
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

            public string conference_dates { get; set; }

            public string conference_location { get; set; }

            public string content_type { get; set; }

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

                    if (splittedName[1].Contains('.'))
                    {
                        return splittedName.Length > 1 ? splittedName[0] + splittedName[1] : splittedName[0];
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