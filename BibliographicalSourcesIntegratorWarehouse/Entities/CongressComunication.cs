using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class CongressComunication : Publication
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Year { get; set; }

        public string Url { get; set; }

        public string Congress { get; set; }

        public int Edition { get; set; }

        public string Place { get; set; }

        public int InitialPage { get; set; }

        public int FinalPage { get; set; }
    }
}
