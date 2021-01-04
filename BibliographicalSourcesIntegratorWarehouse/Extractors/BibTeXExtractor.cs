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
            string jsonWithoutSlashes = json.Replace("date-parts", "date_parts");

            return jsonWithoutSlashes;
        }


        public override Article CreateArticle<T>(T publication)
        {
            BibTeXPublicationSchema bibtexPublication = publication as BibTeXPublicationSchema;

            int year = bibtexPublication.GetYear();

            return publicationCreator.CreateArticle(
                title: bibtexPublication.title,
                year: bibtexPublication.GetYear(),
                url: bibtexPublication.url,
                authors: bibtexPublication.GetAuthors(),
                initialPage: bibtexPublication.GetInitialPage(),
                finalPage: bibtexPublication.GetFinalPage(),
                volume: bibtexPublication.volume,
                number: bibtexPublication.number,
                month: null,
                journalName: bibtexPublication.publisher);
        }

        public override Book CreateBook<T>(T publication)
        {
            BibTeXPublicationSchema bibtexPublication = publication as BibTeXPublicationSchema;

           
            return publicationCreator.CreateBook(
                title: bibtexPublication.title,
                year: bibtexPublication.GetYear(),
                url: bibtexPublication.url,
                authors: bibtexPublication.GetAuthors(),
                editorial: null);
        }

        public override CongressComunication CreateCongressComunication<T>(T publication)
        {
            BibTeXPublicationSchema bibtexPublication = publication as BibTeXPublicationSchema;

            return publicationCreator.CreateCongressComunication(
                title: bibtexPublication.title,
                year: bibtexPublication.GetYear(),
                url: bibtexPublication.url,
                authors: bibtexPublication.GetAuthors(),
                edition: null,
                congress: null,
                place: null,
                initialPage: bibtexPublication.GetInitialPage(),
                finalPage: bibtexPublication.GetFinalPage());
        }

        public override bool IsArticle<T>(T publication)
        {
            BibTeXPublicationSchema bibtexPublication = publication as BibTeXPublicationSchema;

            return bibtexPublication.type.Equals("article-journal");
        }

        public override bool IsBook<T>(T publication)
        {
            BibTeXPublicationSchema bibtexPublication = publication as BibTeXPublicationSchema;

            return bibtexPublication.type.Equals("book");
        }

        public override bool IsCongressComunication<T>(T publication)
        {
            BibTeXPublicationSchema bibtexPublication = publication as BibTeXPublicationSchema;

            return bibtexPublication.type.Equals("paper-conference");
        }
    }

    class BibTeXPublicationSchema
    {
        public string title { get; set; }

        public Year issued { get; set; }

        public string page { get; set; }

        public List<Author> author { get; set; }

        public string journal { get; set; }

        public string booktitle { get; set; }

        public string publisher { get; set; }

        public string volume { get; set; }

        public string number { get; set; }

        public string url { get; set; }

        public string type { get; set; }


        public string GetInitialPage()
        {
            if (page == null)
            {
                return null;
            }

            int slashPosition = page.IndexOf('-');

            if (slashPosition == -1)
            {
                return page;
            }

            return page.Substring(0, slashPosition);
        }

        public string GetFinalPage()
        {
            if (page == null)
            {
                return null;
            }

            int slashPosition = page.IndexOf('-');

            if (slashPosition == -1)
            {
                return null;
            }

            return page.Substring(slashPosition + 1);
        }

        public List<(string name, string surnames)> GetAuthors()
        {
            List<(string name, string surnames)> authors = new List<(string name, string surnames)>();

            if (author == null)
            {
                return authors;
            }

            foreach (Author a in author)
            {
                authors.Add((a.given, a.family));
            }

            return authors;
        }

        public int GetYear()
        {
            return issued.GetYear();
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


        public class Year
        {
            public List<List<int>> date_parts;

             public int GetYear()
             {
                 return date_parts[0][0];
             } 

        }
    }
}
