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


        public Article(string title, int year, string url, ICollection<Person_Publication> people, int initialPage, int finalPage, Exemplar exemplar) 
            : base(title, year, url , people)
        {
            InitialPage = initialPage;
            FinalPage = finalPage;
            Exemplar = exemplar;
        }
    }
}
