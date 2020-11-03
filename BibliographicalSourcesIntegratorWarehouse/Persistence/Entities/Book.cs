using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Book : Publication
    {
        public string Editorial { get; set; }

        public int Pages { get; set; }


        public Book(string title, int year, string url, ICollection<Person_Publication> people, string editorial, int pages)
            : base(title, year, url, people)
        {
            Editorial = editorial;
            Pages = pages;
        }
    }
}
