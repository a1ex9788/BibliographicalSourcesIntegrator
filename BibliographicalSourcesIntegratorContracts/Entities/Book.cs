using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BibliographicalSourcesIntegratorContracts.Entities
{
    public class Book : Publication
    {
        public string Editorial { get; set; }


        public override bool Equals(object obj)
        {
            Book other = obj as Book;

            bool a = Editorial == null ? other.Editorial == null : Editorial.Equals(other.Editorial);
            bool b = base.Equals(obj);

            return a && b;
        }
    }
}
