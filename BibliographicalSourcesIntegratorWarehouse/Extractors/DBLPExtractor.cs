using BibliographicalSourcesIntegratorContracts.Entities;
using BibliographicalSourcesIntegratorWarehouse.Persistence;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliographicalSourcesIntegratorWarehouse.Extractors
{
    public class DBLPExtractor
    {
        private readonly PublicationCreator publicationCreator;
        private readonly DatabaseAccess databaseAccess;
        private readonly ILogger<DBLPExtractor> logger;


        public DBLPExtractor(PublicationCreator publicationCreator, DatabaseAccess databaseAccess, ILogger<DBLPExtractor> logger)
        {
            this.publicationCreator = publicationCreator;
            this.databaseAccess = databaseAccess;
            this.logger = logger;
        }


        public (int, List<string>) ExtractData(string json)
        {
            List<string> errorList = new List<string>();
            List<Article> articlesToSave = new List<Article>();

            logger.LogInformation("Preparing the json...");

            string preparedJson = PrepareJson(json);

            if (preparedJson == null)
            {
                return (0, errorList);
            }

            logger.LogInformation("Converting the json to DBLP schema...");

            List<DBLPPublicationSchema> publications = JsonConvert.DeserializeObject<List<DBLPPublicationSchema>>(preparedJson);

            logger.LogInformation("Creating the publications...");

            foreach (DBLPPublicationSchema dBLPPublication in publications)
            {
                try
                {
                    Article article = publicationCreator.CreateArticle(
                        title: dBLPPublication.title,
                        year: dBLPPublication.year,
                        url: dBLPPublication.url,
                        authors: dBLPPublication.GetAuthors(),
                        initialPage: dBLPPublication.GetInitialPage(),
                        finalPage: dBLPPublication.GetFinalPage(),
                        volume: dBLPPublication.volume,
                        number: dBLPPublication.number,
                        month: dBLPPublication.GetMonth(),
                        journalName: dBLPPublication.journal);

                    if (databaseAccess.GetArticle(article) == null)
                    {
                        articlesToSave.Add(article);
                    }
                }
                catch (Exception e)
                {
                    errorList.Add(e.Message);
                }
            }

            logger.LogInformation("Saving the publications into the database...");

            databaseAccess.SaveArticles(articlesToSave);

            return (articlesToSave.Count(), errorList);
        }


        static string PrepareJson(string source)
        {
            string aux = source;

            string jsonWithoutSpecialChars = aux.Replace("@", "").Replace("#", "").Replace("$", "dollar");

            string jsonArticleList = SearchArticleList(jsonWithoutSpecialChars);

            if (jsonArticleList == null)
            {
                return null;
            }

            string jsonWithSquareBracketsInAuthorLists = AddSquareBracketsInAuthorListsIfNeeded(jsonArticleList);

            return jsonWithSquareBracketsInAuthorLists;


            static string SearchArticleList(string source)
            {
                if (!source.Contains('[') || !source.Contains(']'))
                {
                    return null;
                }

                string aux = source;

                while (aux.First() != '[')
                {
                    aux = aux.Remove(0, 1);
                }

                while (aux.Last() != ']')
                {
                    aux = aux.Remove(aux.Length - 1, 1);
                }

                return aux;
            }

            static string AddSquareBracketsInAuthorListsIfNeeded(string source)
            {
                string auxToSearchAuthorPos = source;
                string res = source;
                int currentInitialAuthorPos = 0, numberOfAddedSquareBrackets = 0;

                while (auxToSearchAuthorPos.Contains("author\":"))
                {
                    int indexOfAuthor = auxToSearchAuthorPos.IndexOf("author\":");
                    int posToInvestigate = currentInitialAuthorPos + indexOfAuthor;

                    auxToSearchAuthorPos = auxToSearchAuthorPos.Substring(indexOfAuthor + 1);
                    currentInitialAuthorPos += indexOfAuthor + 1;


                    int currentFirstSquareBracketPos = source.IndexOf(':', posToInvestigate) + numberOfAddedSquareBrackets + 1;

                    if (res[currentFirstSquareBracketPos] != '[')
                    {
                        res = res.Substring(0, currentFirstSquareBracketPos) + '[' + res.Substring(currentFirstSquareBracketPos);

                        int initialTitlePosition = res.Substring(currentFirstSquareBracketPos).IndexOf("\"title") + currentFirstSquareBracketPos;

                        int beforeCommaPosition = res.Substring(0, initialTitlePosition).LastIndexOf(',');

                        res = res.Substring(0, beforeCommaPosition) + ']' + res.Substring(beforeCommaPosition);

                        numberOfAddedSquareBrackets += 2;
                    }
                }

                return res;
            }
        }
    }


    class DBLPPublicationSchema
    {
        public string mdate { get; set; }

        public List<Object> author { get; set; }

        public string title { get; set; }

        public string pages { get; set; }

        public int year { get; set; }

        public string volume { get; set; }

        public string journal { get; set; }

        public string url { get; set; }

        public string number { get; set; }


        public List<(string name, string surnames)> GetAuthors()
        {
            List<(string name, string surnames)> authors = new List<(string name, string surnames)>();

            if (author == null || author.Count == 0)
            {
                return authors;
            }

            foreach (Object o in author)
            {
                string name = "";
                string surnames = "";
                string completeName = "";

                completeName = o as string;

                if (completeName == null)
                {
                    Author author = JsonConvert.DeserializeObject<Author>(o.ToString());
                    completeName = author.text;
                }

                name = GetName(completeName);
                surnames = GetSurnames(completeName);

                authors.Add((name, surnames));
            }

            return authors;


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

        public string GetInitialPage()
        {
            if (pages == null)
            {
                return null;
            }

            int slashPosition = pages.IndexOf('-');

            if (slashPosition == -1)
            {
                return pages;
            }

            return pages.Substring(0, slashPosition);
        }

        public string GetFinalPage()
        {
            if (pages == null)
            {
                return null;
            }

            int slashPosition = pages.IndexOf('-');

            if (slashPosition == -1)
            {
                return null;
            }

            return pages.Substring(slashPosition + 1);
        }

        public string GetMonth()
        {
            int firstSlashPos = mdate.IndexOf('-');
            int secondSlashPos = mdate.IndexOf('-', firstSlashPos + 1);

            switch (mdate.Substring(firstSlashPos + 1, secondSlashPos - firstSlashPos - 1))
            {
                case "01":
                    return "January";
                case "02":
                    return "February";
                case "03":
                    return "March";
                case "04":
                    return "April";
                case "05":
                    return "May";
                case "06":
                    return "June";
                case "07":
                    return "July";
                case "08":
                    return "August";
                case "09":
                    return "September";
                case "10":
                    return "October";
                case "11":
                    return "November";
                case "12":
                    return "December";
                default:
                    return null;
            }
        }


        class Author
        {
            string type;

            // Si se utiliza el json de poliformat hay que llamar a este atributo 'dollar'
            public string text;


            public Author(string type, string text)
            {
                this.type = type;
                this.text = text;
            }
        }
    }
}
