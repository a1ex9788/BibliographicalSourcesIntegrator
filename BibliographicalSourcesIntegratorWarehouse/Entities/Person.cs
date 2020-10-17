using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Person
    {
        public string Name { get; set; }

        public string Surnames { get; set; }


        public ICollection<Publication> Publications { get; set; }
    }
}
