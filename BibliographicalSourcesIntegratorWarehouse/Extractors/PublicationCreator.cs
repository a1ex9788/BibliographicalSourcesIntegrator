using BibliographicalSourcesIntegratorContracts.Entities;
using BibliographicalSourcesIntegratorWarehouse.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Extractors
{
    public class PublicationCreator
    {
        private DatabaseAccess databaseAccess;

        private List<Article> alreadyCreatedArticles;
        private List<CongressComunication> alreadyCreatedCongressComunications;
        private List<Book> alreadyCreatedBooks;
        private List<Person> alreadyCreatedPeople;
        private List<Exemplar> alreadyCreatedExemplars;
        private List<Journal> alreadyCreatedJournals;

        public PublicationCreator(DatabaseAccess databaseAccess)
        {
            this.databaseAccess = databaseAccess;

            alreadyCreatedArticles = new List<Article>();
            alreadyCreatedCongressComunications = new List<CongressComunication>();
            alreadyCreatedBooks = new List<Book>();
            alreadyCreatedPeople = new List<Person>();
            alreadyCreatedExemplars = new List<Exemplar>();
            alreadyCreatedJournals = new List<Journal>();
        }


        public Article CreateArticle(string title, string year, string url, List<(string name, string surnames)> authors,
            string initialPage, string finalPage, string volume, string number, string month, string journalName)
        {
            Journal journal = CreateEntity(
                new Journal(
                    name: journalName
                ),
                alreadyCreatedJournals,
                databaseAccess.GetJournal);

            Exemplar exemplar = CreateEntity(
                new Exemplar(
                    volume: volume,
                    number: number,
                    month: month,
                    journal: journal
                ),
                alreadyCreatedExemplars,
                databaseAccess.GetExemplar);

            Article article = CreateEntity(
                new Article(
                    title: title,
                    year: year,
                    url: url,
                    initialPage: initialPage,
                    finalPage: finalPage,
                    exemplar: exemplar
                ),
                alreadyCreatedArticles,
                databaseAccess.GetArticle);

            CreateAuthors(authors, article);

            return article;
        }

        public CongressComunication CreateCongressComunication(string title, string year, string url, List<(string name, string surnames)> authors,
            string congress, string edition, string place, string initialPage, string finalPage)
        {
            CongressComunication conference = CreateEntity(
                new CongressComunication(
                    title: title,
                    year: year,
                    url: url,
                    congress: congress,
                    edition: edition,
                    place: place,
                    initialPage: initialPage,
                    finalPage: finalPage
                ),
                alreadyCreatedCongressComunications,
                databaseAccess.GetCongressComunication);

            CreateAuthors(authors, conference);

            return conference;
        }

        public Book CreateBook(string title, string year, string url, List<(string name, string surnames)> authors,
            string editorial)
        {
            Book book = CreateEntity(
                new Book(
                    title: title,
                    year: year,
                    url: url,
                    editorial: editorial
                ),
                alreadyCreatedBooks,
                databaseAccess.GetBook);

            CreateAuthors(authors, book);

            return book;
        }


        private void CreateAuthors(List<(string name, string surnames)> authors, Publication publication)
        {
            foreach ((string name, string surnames) in authors)
            {
                Person person = CreateEntity(
                    new Person(
                        name: name,
                        surnames: surnames
                    ),
                    alreadyCreatedPeople,
                    databaseAccess.GetPerson);

                Person_Publication person_Publication = new Person_Publication(
                    person: person,
                    publication: publication);

                publication.People.Add(person_Publication);
            }
        }


        private T CreateEntity<T>(T entity, List<T> alreadyCreatedEntities, Func<T, T> getEntity)
        {
            T entityFromMemory = alreadyCreatedEntities.FirstOrDefault(e => e.Equals(entity));

            if (entityFromMemory != null)
            {
                return entityFromMemory;
            }

            T entityFromDB = getEntity(entity);

            if (entityFromDB != null)
            {
                return entityFromDB;
            }

            alreadyCreatedEntities.Add(entity);

            return entity;
        }
    }
}
