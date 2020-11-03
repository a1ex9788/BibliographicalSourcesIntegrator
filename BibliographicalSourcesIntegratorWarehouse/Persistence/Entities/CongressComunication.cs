﻿using System;
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


        public CongressComunication(string title, int year, string url, ICollection<Person_Publication> people, string congress, int edition, string place, int initialPage, int finalPage)
            : base(title, year, url, people)
        {
            Congress = congress;
            Edition = edition;
            Place = place;
            InitialPage = initialPage;
            FinalPage = finalPage;
        }
    }
}
