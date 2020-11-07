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
        private readonly PublicationCreator publicationConstructor;
        private readonly DatabaseAccess databaseAccess;


        public DBLPExtractor(PublicationCreator publicationConstructor, DatabaseAccess databaseAccess)
        {
            this.publicationConstructor = publicationConstructor;
            this.databaseAccess = databaseAccess;
        }


        public void ExtractData(string json)
        {
            string preparedJson = PrepareJson(json);

            List<DBLPPublicationSchema> publications = JsonConvert.DeserializeObject<List<DBLPPublicationSchema>>(preparedJson);

            List<Article> articles = new List<Article>();

            foreach (DBLPPublicationSchema dBLPPublication in publications)
            {
                articles.Add(publicationConstructor.CreateArticle(
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

            databaseAccess.SaveArticles(articles);
        }


        static string PrepareJson(string source)
        {
            string aux = source;

            string jsonWithoutSpecialChars = aux.Replace("$", "dollar");

            string jsonArticleList = SearchArticleList(jsonWithoutSpecialChars);

            string jsonWithSquareBracketsInAuthorLists = AddSquareBracketsInAuthorListsIfNeeded(jsonArticleList);

            return jsonWithSquareBracketsInAuthorLists;
        }

        static string SearchArticleList(string source)
        {
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
            string res = source;

            List<int> initialAuthorPositions = GetInitialPositionsOf(res, "author");

            int numberOfAddedSquareBrackets = 0;

            foreach (int initialAuthorPosition in initialAuthorPositions)
            {
                int currentPosition = initialAuthorPosition + 9 + numberOfAddedSquareBrackets;

                if (res[currentPosition] != '[')
                {
                    res = res.Substring(0, currentPosition) + '[' + res.Substring(currentPosition);

                    int initialTitlePosition = res.Substring(currentPosition).IndexOf("\"title") + currentPosition;

                    int beforeCommaPosition = res.Substring(0, initialTitlePosition).LastIndexOf(',');

                    res = res.Substring(0, beforeCommaPosition) + ']' + res.Substring(beforeCommaPosition);

                    numberOfAddedSquareBrackets += 2;
                }
            }

            return res;
        }

        static List<int> GetInitialPositionsOf(string source, string word)
        {
            List<int> res = new List<int>();
            string aux = source;
            int currentInitialPos = 0;

            while (aux.Contains(word))
            {
                int indexOfWord = aux.IndexOf(word);

                res.Add(currentInitialPos + indexOfWord);
                aux = aux.Substring(indexOfWord + 1);

                currentInitialPos += indexOfWord + 1;
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

        public int number { get; set; }


        public List<(string name, string surnames)> GetAuthors()
        {
            List<(string name, string surnames)> authors = new List<(string name, string surnames)>();

            foreach (Object o in author)
            {
                string name = "";
                string surnames = "";
                string completeName = "";

                completeName = o as string;

                if (completeName == null)
                {
                    Author author = JsonConvert.DeserializeObject<Author>(o.ToString());
                    completeName = author.dollar;
                }

                name = GetName(completeName);
                surnames = GetSurnames(completeName);

                authors.Add((name, surnames));
            }

            return authors;


            string GetName(string completeName)
            {
                string[] splittedName = completeName.Split(' ');

                if (splittedName[1].Contains('.'))
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
            return Convert.ToInt32(mdate.Substring(5, 2));
        }



        class Author
        {
            string type;

            public string dollar;


            public Author(string type, string dollar)
            {
                this.type = type;
                this.dollar = dollar;
            }
        }
    }
}
