using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Publication_Person
    {
        [Key]
        public int Id { get; set; }

        public int Id_Article { get; set; }

        public int Id_Book { get; set; }

        public int Id_CongressComunication { get; set; }

        public int Id_Person { get; set; }

        public ICollection<Person> People { get; set; }
    }
}
