using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BibliographicalSourcesIntegratorContracts.Entities
{
    public class Exemplar
    {
        [Key]
        public int Id { get; set; }


        public string Volume { get; set; }

        public string Number { get; set; }

        public string Month { get; set; }

                
        public virtual Journal Journal { get; set; }


        public override bool Equals(object obj)
        {
            Exemplar other = obj as Exemplar;

            if (other == null)
            {
                return false;
            }

            bool a = Volume == null ? other.Volume == null : Volume.Equals(other.Volume);
            bool b = Number == null ? other.Number == null : Number.Equals(other.Number);
            bool c = Month == null ? other.Month == null : Month.Equals(other.Month);
            bool d = Journal == null ? other.Journal == null : Journal.Equals(other.Journal);

            return a && b && c && d;
        }
    }
}
