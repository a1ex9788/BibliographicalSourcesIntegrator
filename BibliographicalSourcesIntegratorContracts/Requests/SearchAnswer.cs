using BibliographicalSourcesIntegratorContracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorContracts
{
    public class SearchAnswer
    {
        public List<Article> SearchArticles { get; set; }

        public List<Book> SearchBooks { get; set; }

        public List<CongressComunication> SearchCongressComunications { get; set; }
    }
}
