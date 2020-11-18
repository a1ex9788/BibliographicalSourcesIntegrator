using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class CongressComunication : Publication
    {
        public string Congress { get; set; }

        public string Edition { get; set; }

        public string Place { get; set; }

        public string InitialPage { get; set; }

        public string FinalPage { get; set; }


        public CongressComunication() : base() { }

        public CongressComunication(string title, string year, string url, string congress, string place)
            : base(title, year, url)
        {
            Congress = congress;
            Place = place;
        }

        public CongressComunication(string title, string year, string url, string congress, string place, string initialPage, string finalPage)
            : base(title, year, url)
        {
            Congress = congress;
            Place = place;
            InitialPage = initialPage;
            FinalPage = finalPage;
        }

        public CongressComunication(string title, string year, string url, string congress, string edition, string place, string initialPage, string finalPage)
            : base(title, year, url)
        {
            Congress = congress;
            Edition = edition;
            Place = place;
            InitialPage = initialPage;
            FinalPage = finalPage;
        }
    }
}
