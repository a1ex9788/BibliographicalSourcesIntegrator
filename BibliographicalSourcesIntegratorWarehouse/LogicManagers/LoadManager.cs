using BibliographicalSourcesIntegratorWarehouse.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Controllers
{
    public class LoadManager
    {
        private readonly AppDbContext context;

        public LoadManager(AppDbContext context)
        {
            this.context = context;
        }

        public static void Load()
        {
            throw new NotImplementedException();
        }
    }
}
