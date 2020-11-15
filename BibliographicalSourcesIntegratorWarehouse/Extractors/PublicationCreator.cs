using BibliographicalSourcesIntegratorWarehouse.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Extractors
{
    public class PublicationCreator
    {
        public Article CreateArticle(string title, int year, string url, List<(string name, string surnames)> authors, 
            int initialPage, int finalPage, string volume, string number, int month, string journalName)
        {
            Journal journal = new Journal(
                    name: journalName);

            Exemplar exemplar;

            if (month == -1)
            {
                exemplar = new Exemplar(
                    volume: volume,
                    number: number,
                    journal: journal);
            }
            else
            {
                exemplar = new Exemplar(
                    volume: volume,
                    number: number,
                    month: month,
                    journal: journal);
            }
            
            Article article;

            if (initialPage == -1)
            {
                article = new Article(
                    title: title,
                    year: year,
                    url: url,
                    exemplar: exemplar);
            }
            else
            {
                article = new Article(
                    title: title,
                    year: year,
                    url: url,
                    initialPage: initialPage,
                    finalPage: finalPage,
                    exemplar: exemplar);
            }

            foreach ((string name, string surnames) in authors)
            {
                Person person = new Person(
                    name: name,
                    surnames: surnames);

                Person_Publication person_Publication = new Person_Publication(
                    person: person,
                    publication: article);

                article.People.Add(person_Publication);
                person.Publications.Add(person_Publication);
            }            

            journal.Exemplars.Add(exemplar);
            exemplar.Articles.Add(article);

            return article;
        }

        public CongressComunication CreateCongressComunication(string title, int year, string url, List<(string name, string surnames)> authors,
            string congress, int edition, string place, int initialPage, int finalPage)
        {

            CongressComunication conference;

            //Esta fet per al IEEEXplorer
            if (edition == -1)
            {
                if (finalPage == -1)
                {
                    conference = new CongressComunication(
                        title: title,
                        year: year,
                        url: url,
                        congress: congress,
                        place: place);
                }
                else {
                    conference = new CongressComunication(
                        title: title,
                        year: year,
                        url: url,
                        congress: congress,
                        place: place,
                        initialPage: initialPage,
                        finalPage: finalPage);
                }
                
            }
            //Esta fet per al GoogleSchoolar
            else
            {
                conference = new CongressComunication(
                    title: title,
                    year: year,
                    url: url,
                    congress: congress,
                    edition: edition,
                    place: place,
                    initialPage: initialPage,
                    finalPage: finalPage);
            }

            

            foreach ((string name, string surnames) in authors)
            {
                Person person = new Person(
                    name: name,
                    surnames: surnames);

                Person_Publication person_Publication = new Person_Publication(
                    person: person,
                    publication: conference);

                conference.People.Add(person_Publication);
                person.Publications.Add(person_Publication);
            }

            return conference;
        }

        public Book CreateBook(string title, int year, string url, List<(string name, string surnames)> authors,
            string editorial)
        {
            Book book;
            if(editorial == null)
            {
                book = new Book(
                    title: title,
                    year: year,
                    url: url
                    );
            } else
            {
                book = new Book(
                    title: title,
                    year: year,
                    url: url,
                    editorial: editorial
                    );
            }

            foreach ((string name, string surnames) in authors)
            {
                Person person = new Person(
                    name: name,
                    surnames: surnames);

                Person_Publication person_Publication = new Person_Publication(
                    person: person,
                    publication: book);

                book.People.Add(person_Publication);
                person.Publications.Add(person_Publication);
            }

            return book;
        }
    }
}
