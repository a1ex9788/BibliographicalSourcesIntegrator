using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class CongressComunication : Publication
    {
        public string Congress { get; set; }

        public int Edition { get; set; }

        public string Place { get; set; }

        public int InitialPage { get; set; }

        public int FinalPage { get; set; }
    }
}
