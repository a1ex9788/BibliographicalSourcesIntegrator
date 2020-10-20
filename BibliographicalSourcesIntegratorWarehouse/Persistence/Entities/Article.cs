using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Article : Publication
    {
        public int InitialPage { get; set; }

        public int FinalPage { get; set; }


        public Exemplar Exemplar { get; set; }
    }
}
