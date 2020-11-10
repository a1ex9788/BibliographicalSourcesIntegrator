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

        public int Edition { get; set; }

        public string Place { get; set; }

        public int InitialPage { get; set; }

        public int FinalPage { get; set; }


        public CongressComunication() : base() { }

        public CongressComunication(string title, int year, string url, string congress, string place)
            : base(title, year, url)
        {
            Congress = congress;
            Place = place;
        }
        public CongressComunication(string title, int year, string url, string congress, string place, int initialPage, int finalPage)
            : base(title, year, url)
        {
            Congress = congress;
            Place = place;
            InitialPage = initialPage;
            FinalPage = finalPage;
        }
        public CongressComunication(string title, int year, string url, string congress, int edition, string place, int initialPage, int finalPage)
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
