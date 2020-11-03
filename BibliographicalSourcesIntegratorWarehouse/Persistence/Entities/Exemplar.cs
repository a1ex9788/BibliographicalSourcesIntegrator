﻿using System;
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


        public int Volume { get; set; }

        public int Number { get; set; }

        public int Month { get; set; }


        public ICollection<Article> Articles { get; set; }

        public Journal Journal { get; set; }


        public Exemplar(int volume, int number, int month, ICollection<Article> articles, Journal journal)
        {
            Volume = volume;
            Number = number;
            Month = month;
            Articles = articles;
            Journal = journal;
        }
    }
}
