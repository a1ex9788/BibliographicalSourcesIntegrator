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
    public class BibTeXExtractor : IExtractor
    {
        private readonly PublicationCreator publicationCreator;


        public BibTeXExtractor(PublicationCreator publicationCreator, DatabaseAccess databaseAccess, ILogger<BibTeXExtractor> logger)
            : base(databaseAccess, logger)
        {
            this.publicationCreator = publicationCreator;
        }


        public (int, List<string>) ExtractData(string sourceName, string json)
        {
            return ExtractData<BibTeXPublicationSchema>(sourceName, json);
        }


        public override string PrepareJson(string json)
        {
            return json;
        }


        public override Article CreateArticle<T>(T publication)
        {
            BibTeXPublicationSchema dBLPPublication = publication as BibTeXPublicationSchema;

            return null;
            // publicationCreator.CreateArticle();
        }

        public override Book CreateBook<T>(T publication)
        {
            BibTeXPublicationSchema dBLPPublication = publication as BibTeXPublicationSchema;

            return null;
            //return publicationCreator.CreateBook();
        }

        public override CongressComunication CreateCongressComunication<T>(T publication)
        {
            BibTeXPublicationSchema dBLPPublication = publication as BibTeXPublicationSchema;

            return null;
            //return publicationCreator.CreateCongressComunication();
        }

        public override bool IsArticle<T>(T publication)
        {
            return true;
        }

        public override bool IsBook<T>(T publication)
        {
            return false;
        }

        public override bool IsCongressComunication<T>(T publication)
        {
            return false;
        }
    }

    class BibTeXPublicationSchema
    {
        public string title { get; set; }

        public int year { get; set; }

        public string pages { get; set; }

        public List<Author> author { get; set; }

        public string journal { get; set; }

        public string booktitle { get; set; }

        public string publisher { get; set; }

        public string volume { get; set; }

        public string number { get; set; }

        public string url { get; set; }

        public string place { get; set; }

        public string type { get; set; }


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

            return pages.Substring(slashPosition + 2);
        }

        /*public List<(string name, string surnames)> GetAuthors()
        {
            List<(string name, string surnames)> authors = new List<(string name, string surnames)>();

            if (author == null)
            {
                return authors;
            }

            string[] aux = author.Split("and");

            foreach (string s in aux)
            {
                string author = s.TrimStart().TrimEnd();

                int pos = author.IndexOf(",");
                string surnames = author.Substring(0, pos);
                string name = author.Substring(pos + 2);

                authors.Add((name, surnames));
            }

            return authors;
        } */
    }

    public class Author
    {
        public string given;
        public string family;

        public Author(string given, string family)
        {
            this.given = given;
            this.family = family;

        }
        public List<Object> authors { get; set; }

    }





}
