﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Publication
    {
        [Key]
        public int Id { get; set; }


        public string Title { get; set; }

        public string Year { get; set; }

        public string Url { get; set; }


        public ICollection<Person_Publication> People { get; set; }


        public Publication() : base() 
        {
            People = new List<Person_Publication>();
        }

        public Publication(string title, string year, string url)
        {
            Title = title;
            Year = year;
            Url = url;

            People = new List<Person_Publication>();
        }
    }
}
