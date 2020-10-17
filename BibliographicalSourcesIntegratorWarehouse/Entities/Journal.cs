using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Journal
    {
        public string Name { get; set; }


        public ICollection<Exemplar> Exemplars { get; set; }
    }
}
