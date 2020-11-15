using BibliographicalSourcesIntegratorWarehouse.Entities;
using BibliographicalSourcesIntegratorWarehouse.Persistence;
using Microsoft.AspNetCore.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Extractors
{
    public class DBLPExtractor
    {
        private readonly PublicationCreator publicationCreator;
        private readonly DatabaseAccess databaseAccess;


        public DBLPExtractor(PublicationCreator publicationCreator, DatabaseAccess databaseAccess)
        {
            this.publicationCreator = publicationCreator;
            this.databaseAccess = databaseAccess;
        }


        public (int numberOfResults, List<string> errorList) ExtractData(string json)
        {
            List<string> errorList = new List<string>();
            List<Article> articles = new List<Article>();

            string preparedJson = PrepareJson(json);

            if (preparedJson == null)
            {
                return (0, errorList);
            }

            List<DBLPPublicationSchema> publications = JsonConvert.DeserializeObject<List<DBLPPublicationSchema>>(preparedJson);

            foreach (DBLPPublicationSchema dBLPPublication in publications)
            {
                try
                {
                    articles.Add(publicationCreator.CreateArticle(
                        title: dBLPPublication.title,
                        year: dBLPPublication.year,
                        url: dBLPPublication.url,
                        authors: dBLPPublication.GetAuthors(),
                        initialPage: dBLPPublication.GetInitialPage(),
                        finalPage: dBLPPublication.GetFinalPage(),
                        volume: dBLPPublication.volume,
                        number: dBLPPublication.number,
                        month: dBLPPublication.GetMonth(),
                        journalName: dBLPPublication.journal));
                }
                catch (Exception e)
                {
                    errorList.Add(e.Message);
                }
            }

            databaseAccess.SaveArticles(articles);

            return (publications.Count - errorList.Count, errorList);
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
        }

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
                aux = aux.Remove(aux.Length-1, 1);
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

        public int GetInitialPage()
        {
            try
            {
                int slashPosition = pages.IndexOf('-');

                return Convert.ToInt32(pages.Substring(0, slashPosition));
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int GetFinalPage()
        {
            try
            {
                int slashPosition = pages.IndexOf('-');

                return Convert.ToInt32(pages.Substring(slashPosition + 1));
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int GetMonth()
        {
            int firstSlashPos = mdate.IndexOf('-');
            int secondSlashPos = mdate.IndexOf('-', firstSlashPos + 1);

            return Convert.ToInt32(mdate.Substring(firstSlashPos + 1, secondSlashPos - firstSlashPos - 1));
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
