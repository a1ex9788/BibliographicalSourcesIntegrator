using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Article : Publication
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Year { get; set; }

        public string Url { get; set; }

        public int InitialPage { get; set; }

        public int FinalPage { get; set; }

        public int Id_Exemplar { get; set; }

        public Exemplar Exemplar { get; set; }
    }
}
