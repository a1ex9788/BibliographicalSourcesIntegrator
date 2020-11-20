using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Journal
    {
        [Key]
        public int Id { get; set; }


        public string Name { get; set; }


        public ICollection<Exemplar> Exemplars { get; set; }


        public Journal()
        {
            Exemplars = new List<Exemplar>();
        }

        public Journal(string name)
        {
            Name = name;

            Exemplars = new List<Exemplar>();
        }


        public override bool Equals(object obj)
        {
            Journal other = obj as Journal;

            bool a = Name == null ? other.Name == null : Name.Equals((obj as Journal).Name);

            return a;
        }
    }
}
