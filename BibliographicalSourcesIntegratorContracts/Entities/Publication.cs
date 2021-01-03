using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BibliographicalSourcesIntegratorContracts.Entities
{
    public class Publication
    {
        [Key]
        public int Id { get; set; }


        public string Title { get; set; }

        public int Year { get; set; }

        public string Url { get; set; }


        public virtual ICollection<Person> People { get; set; }


        public override bool Equals(object obj)
        {
            Publication other = obj as Publication;

            if (other == null)
            {
                return false;
            }

            bool a = Title == null ? other.Title == null : Title.Equals(other.Title);
            bool b = Year == 0 ? other.Year == 0 : Year.Equals(other.Year);
            bool c = Url == null ? other.Url == null : Url.Equals(other.Url);

            return a && b && c;
        }
    }
}
