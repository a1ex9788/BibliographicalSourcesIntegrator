using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Person
    {
        [Key]
        public int Id { get; set; }


        public string Name { get; set; }

        public string Surnames { get; set; }


        public ICollection<Person_Publication> Publications { get; set; }


        public Person(string name, string surnames, ICollection<Person_Publication> publications)
        {
            Name = name;
            Surnames = surnames;
            Publications = publications;
        }
    }
}
