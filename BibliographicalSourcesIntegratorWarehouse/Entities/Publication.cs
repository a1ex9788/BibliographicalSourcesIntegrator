﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Publication
    {
        public string Title { get; set; }

        public int Year { get; set; }

        public string Url { get; set; }


        public ICollection<Person> People { get; set; }
    }
}
