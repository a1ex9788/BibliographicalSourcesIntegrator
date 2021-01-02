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

            bool a = Title == null ? (obj as Publication).Title == null : Title.Equals((obj as Publication).Title);
            bool b = Year == 0 ? (obj as Publication).Year == 0 : Year.Equals((obj as Publication).Year);
            bool c = Url == null ? (obj as Publication).Url == null : Url.Equals((obj as Publication).Url);

            return a && b && c;
        }
    }
}
