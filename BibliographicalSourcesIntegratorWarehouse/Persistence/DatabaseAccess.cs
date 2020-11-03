using BibliographicalSourcesIntegratorWarehouse.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Persistence
{
    public class DatabaseAccess
    {
        private readonly AppDbContext context;

        public DatabaseAccess(AppDbContext context)
        {
            this.context = context;
        }


        public void SaveArticle(Article article)
        {
            context.Articles.Add(article);

            context.SaveChanges();
        }
    }
}
