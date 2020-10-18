﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Entities
{
    public class Exemplar
    {
        public int Volume { get; set; }

        public int Number { get; set; }

        public int Month { get; set; }


        public ICollection<Article> Articles { get; set; }

        public Journal Journal { get; set; }
    }
}