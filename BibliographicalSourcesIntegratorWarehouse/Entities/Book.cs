using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Book : Publication
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Year { get; set; }

        public string Url { get; set; }

        public string Editorial { get; set; }

        public int Pages { get; set; }
    }
}
