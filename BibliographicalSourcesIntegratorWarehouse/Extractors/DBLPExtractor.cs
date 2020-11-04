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
        private readonly PublicationConstructor publicationConstructor;
        private readonly DatabaseAccess databaseAccess;


        public DBLPExtractor(PublicationConstructor publicationConstructor, DatabaseAccess databaseAccess)
        {
            this.publicationConstructor = publicationConstructor;
            this.databaseAccess = databaseAccess;
        }


        public void ExtractData(string json)
        {
            string preparedJson = PrepareJson(json);

            List<DBLPPublicationSchema> publications = JsonConvert.DeserializeObject<List<DBLPPublicationSchema>>(preparedJson);

            foreach (DBLPPublicationSchema dBLPPublication in publications)
            {
                Journal journal = new Journal(
                    name: dBLPPublication.journal);

                Exemplar exemplar = new Exemplar(
                        volume: Convert.ToInt32(dBLPPublication.volume),
                        number: Convert.ToInt32(dBLPPublication.number),
                        month: GetMonth(dBLPPublication.mdate),
                        journal: journal);

                Article article = new Article(
                    title: dBLPPublication.title,
                    year: dBLPPublication.year,
                    url: dBLPPublication.url,
                    initialPage: GetInitialPage(dBLPPublication.pages),
                    finalPage: GetFinalPage(dBLPPublication.pages),
                    exemplar: exemplar);

                journal.Exemplars.Add(exemplar);
                exemplar.Articles.Add(article);

                //publicationConstructor.

                databaseAccess.SaveArticle(article);
            }
        }


        static string PrepareJson(string source)
        {
            string aux = source;

            string jsonWithoutSpecialChars = aux.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace("@", "").Replace("$", "dollar");

            string jsonWithoutDBLPNode = jsonWithoutSpecialChars.Substring(21);

            string jsonWithoutEndDBLPNodeBracket = jsonWithoutDBLPNode.Substring(0, jsonWithoutDBLPNode.Length - 2);

            string jsonWithSquareBrackets = AddSquareBrackets(jsonWithoutEndDBLPNodeBracket);

            return jsonWithSquareBrackets;
        }

        static string AddSquareBrackets(string source)
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


        static int GetInitialPage(string pages)
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

        static int GetFinalPage(string pages)
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

        static int GetMonth(string mdate)
        {
            return Convert.ToInt32(mdate.Substring(5, 2));
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
    }
}
