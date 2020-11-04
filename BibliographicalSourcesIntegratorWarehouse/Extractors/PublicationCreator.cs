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
            int initialPage, int finalPage, string volume, int number, int month, string journalName)
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
            

            journal.Exemplars.Add(exemplar);
            exemplar.Articles.Add(article);


            return article;
        }

        public Article CreateCongressComunication(string title, int year, string url, List<(string name, string surnames)> authors,
            string congress, int edition, string place, int initialPage, int finalPage)
        {
            return null;
        }

        public Article CreateBook(string title, int year, string url, List<(string name, string surnames)> authors,
            string editorial)
        {
            return null;
        }
    }
}
