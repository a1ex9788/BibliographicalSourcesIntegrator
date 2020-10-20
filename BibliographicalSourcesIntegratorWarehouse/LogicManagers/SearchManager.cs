using BibliographicalSourcesIntegrator;
using BibliographicalSourcesIntegratorWarehouse.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Controllers
{
    public class SearchManager
    {
        private readonly AppDbContext context;

        public SearchManager(AppDbContext context)
        {
            this.context = context;
        }

        public void Search()
        {
            // preguntar a la BD
        }
    }
}
