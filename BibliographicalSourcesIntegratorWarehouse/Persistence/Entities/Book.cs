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
    }
}
