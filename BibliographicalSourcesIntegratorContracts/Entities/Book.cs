using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BibliographicalSourcesIntegratorContracts.Entities
{
    public class Book : Publication
    {
        public string Editorial { get; set; }


        public Book() : base() { }

        public Book(string title, string year, string url) : base(title, year, url) { }

        public Book(string title, string year, string url, string editorial)
            : base(title, year, url)
        {
            Editorial = editorial;
        }


        public override bool Equals(object obj)
        {
            Book other = obj as Book;

            bool a = Editorial == null ? other.Editorial == null : Editorial.Equals(other.Editorial);
            bool b = base.Equals(obj);

            return a && b;
        }
    }
}
