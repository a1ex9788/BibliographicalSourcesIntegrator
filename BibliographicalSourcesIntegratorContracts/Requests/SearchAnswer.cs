using BibliographicalSourcesIntegratorContracts.Entities;
using System.Collections.Generic;

namespace BibliographicalSourcesIntegratorContracts
{
    public class SearchAnswer
    {
        public List<Article> Articles { get; set; }

        public List<Book> Books { get; set; }

        public List<CongressComunication> CongressComunications { get; set; }
    }
}