using BibliographicalSourcesIntegratorContracts.Entities;
using System.Collections.Generic;
using System.Linq;

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
                e.Number == exemplar.Number &&
                e.Month == exemplar.Volume &&
                e.Journal.Name == exemplar.Journal.Name);
        }

        public Journal GetJournal(Journal journal)
        {
            return context.Journals.FirstOrDefault(j =>
                j.Name == journal.Name);
        }

        public List<Article> GetArticles(string title, string author, int initialYear, int finalYear)
        {
            List<Article> articles = context.Articles.Where(a =>
                a.Title.Contains(title) &&
                a.People.Any(p => (p.Name + " " + p.Surnames).Contains(author)) &&
                a.Year >= initialYear &&
                a.Year <= finalYear).ToList();

            return articles;
        }

        public List<Book> GetBooks(string title, string author, int initialYear, int finalYear)
        {
            List<Book> books = context.Books.Where(b =>
                b.Title.Contains(title) &&
                b.People.Any(p => (p.Name + " " + p.Surnames).Contains(author)) &&
                b.Year >= initialYear &&
                b.Year <= finalYear).ToList();

            return books;
        }

        public List<CongressComunication> GetCongressComunications(string title, string author, int initialYear, int finalYear)
        {
            List<CongressComunication> congressComunications = context.CongressComunications.Where(cc =>
                cc.Title.Contains(title) &&
                cc.People.Any(p => (p.Name + " " + p.Surnames).Contains(author)) &&
                cc.Year >= initialYear &&
                cc.Year <= finalYear).ToList();

            return congressComunications;
        }
    }
}