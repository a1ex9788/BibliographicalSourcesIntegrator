using BibliographicalSourcesIntegratorContracts.Entities;
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
            return context.Articles.FirstOrDefault(a =>
                a.Title == article.Title &&
                a.Year == article.Year &&
                a.Url == article.Url &&
                a.InitialPage == article.InitialPage);
        }

        public CongressComunication GetCongressComunication(CongressComunication congressComunication)
        {
            return context.CongressComunications.FirstOrDefault(cc =>
                cc.Title == congressComunication.Title &&
                cc.Year == congressComunication.Year &&
                cc.Url == congressComunication.Url &&
                cc.Congress == congressComunication.Congress &&
                cc.Edition == congressComunication.Edition &&
                cc.Place == congressComunication.Place &&
                cc.InitialPage == congressComunication.InitialPage &&
                cc.FinalPage == congressComunication.FinalPage);
        }

        public Book GetBook(Book book)
        {
            return context.Books.FirstOrDefault(b =>
                b.Title == book.Title &&
                b.Year == book.Year &&
                b.Url == book.Url &&
                b.Editorial == book.Editorial);
        }

        public Person GetPerson(Person person)
        {
            return context.People.FirstOrDefault(p =>
                p.Name == person.Name &&
                p.Surnames == person.Surnames);
        }

        public Exemplar GetExemplar(Exemplar exemplar)
        {
            return context.Exemplars.FirstOrDefault(e =>
                e.Volume == exemplar.Volume &&
                e.Number== exemplar.Number &&
                e.Month == exemplar.Volume &&
                e.Journal.Name == exemplar.Journal.Name);
        }

        public Journal GetJournal(Journal journal)
        {
            return context.Journals.FirstOrDefault(j =>
                j.Name == journal.Name);
        }
    }
}
