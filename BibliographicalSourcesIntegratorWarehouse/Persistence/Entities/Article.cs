using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Article : Publication
    {
        public string InitialPage { get; set; }

        public string FinalPage { get; set; }


        public Exemplar Exemplar { get; set; }


        public Article() : base() { }

        public Article(string title, string year, string url, Exemplar exemplar)
            : base(title, year, url)
        {
            Exemplar = exemplar;
        }

        public Article(string title, string year, string url, string initialPage, string finalPage, Exemplar exemplar)
            : base(title, year, url)
        {
            InitialPage = initialPage;
            FinalPage = finalPage;
            Exemplar = exemplar;
        }
    }
}
