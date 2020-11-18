using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Exemplar
    {
        [Key]
        public int Id { get; set; }


        public string Volume { get; set; }

        public string Number { get; set; }

        public string Month { get; set; }


        public ICollection<Article> Articles { get; set; }

        public Journal Journal { get; set; }


        public Exemplar()
        {
            Articles = new List<Article>();
        }

        public Exemplar(string volume, string number, Journal journal)
        {
            Volume = volume;
            Number = number;
            Journal = journal;

            Articles = new List<Article>();
        }

        public Exemplar(string volume, string number, string month, Journal journal)
        {
            Volume = volume;
            Number = number;
            Month = month;
            Journal = journal;

            Articles = new List<Article>();
        }
    }
}
