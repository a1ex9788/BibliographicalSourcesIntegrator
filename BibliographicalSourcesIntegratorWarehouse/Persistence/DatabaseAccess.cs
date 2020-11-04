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


        public void SaveArticles(List<Article> articles)
        {
            context.Articles.AddRange(articles);

            context.SaveChanges();
        }

        public void SaveCongressComunications(List<CongressComunication> congressComunication)
        {
            context.CongressComunications.AddRange(congressComunication);

            context.SaveChanges();
        }

        public void SaveBooks(List<Book> books)
        {
            context.Books.AddRange(books);

            context.SaveChanges();
        }
    }
}
