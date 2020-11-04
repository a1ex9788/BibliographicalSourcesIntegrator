using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Article : Publication
    {
        public int InitialPage { get; set; }

        public int FinalPage { get; set; }


        public Exemplar Exemplar { get; set; }


        public Article() : base() { }

        public Article(string title, int year, string url, Exemplar exemplar)
            : base(title, year, url)
        {
            Exemplar = exemplar;
        }

        public Article(string title, int year, string url, int initialPage, int finalPage, Exemplar exemplar)
            : base(title, year, url)
        {
            InitialPage = initialPage;
            FinalPage = finalPage;
            Exemplar = exemplar;
        }
    }
}
