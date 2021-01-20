using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BibliographicalSourcesIntegratorContracts.Entities
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surnames { get; set; }

        public virtual ICollection<Publication> Publications { get; set; }

        public override bool Equals(object obj)
        {
            Person other = obj as Person;

            if (other == null)
            {
                return false;
            }

            bool a = Name == null ? other.Name == null : Name.Equals(other.Name);
            bool b = Surnames == null ? other.Surnames == null : Surnames.Equals(other.Surnames);

            return a && b;
        }
    }
}