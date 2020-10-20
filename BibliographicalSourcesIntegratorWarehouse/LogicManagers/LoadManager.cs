using BibliographicalSourcesIntegrator;
using BibliographicalSourcesIntegratorWarehouse.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Controllers
{
    public class LoadManager
    {
        RequestsManager requestsManager;

        private readonly AppDbContext context;

        public LoadManager(AppDbContext context, RequestsManager requestsManager)
        {
            this.context = context;
            this.requestsManager = requestsManager;
        }

        public async void Load()
        {
            await requestsManager.LoadDataFrom().ConfigureAwait(false);
        }
    }
}
