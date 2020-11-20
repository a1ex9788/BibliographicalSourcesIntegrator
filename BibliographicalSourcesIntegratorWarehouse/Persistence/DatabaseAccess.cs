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


        public Article GetArticle(Article article)
        {
            return context.Articles.FirstOrDefault(a => a.Equals(article));
        }

        public CongressComunication GetCongressComunication(CongressComunication congressComunication)
        {
            return context.CongressComunications.FirstOrDefault(cc => cc.Equals(congressComunication));
        }

        public Book GetBook(Book book)
        {
            return context.Books.FirstOrDefault(b => b.Equals(book));
        }

        public Person GetPerson(Person person)
        {
            return context.People.FirstOrDefault(p => p.Equals(person));
        }

        public Exemplar GetExemplar(Exemplar exemplar)
        {
            return context.Exemplars.FirstOrDefault(e => e.Equals(exemplar));
        }

        public Journal GetJournal(Journal journal)
        {
            return context.Journals.FirstOrDefault(j => j.Equals(journal));
        }
    }
}
