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
        public List<Article> Articles { get; set; }

        public List<Book> Books { get; set; }

        public List<CongressComunication> CongressComunications { get; set; }
    }
}
