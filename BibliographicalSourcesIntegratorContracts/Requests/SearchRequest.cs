using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorContracts
{
    public class SearchRequest
    {
        public bool SearchArticles { get; set; }

        public bool SearchBooks { get; set; }

        public bool SearchCongressComunications { get; set; }


        public int InitialYear { get; set; }

        public int FinalYear { get; set; }

        
        public string Author { get; set; }

        public string Title { get; set; }


        public SearchRequest() { }

        public SearchRequest(bool searchArticles, bool searchBooks, bool searchCongressComunications, int initialYear, int finalYear, string author, string title)
        {
            SearchArticles = searchArticles;
            SearchBooks = searchBooks;
            SearchCongressComunications = searchCongressComunications;
            InitialYear = initialYear;
            FinalYear = finalYear;
            Author = author;
            Title = title;
        }
    }
}
