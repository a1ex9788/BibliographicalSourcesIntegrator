using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Person_Publication
    {
        [Key]
        public int Id { get; set; }


        public int PersonId { get; set; }

        public Person Person { get; set; }


        public int PublicationId { get; set; }

        public Publication Publication { get; set; }


        public Person_Publication() { }

        public Person_Publication(Person person, Publication publication)
        {
            Person = person;
            Publication = publication;
        }
    }
}
