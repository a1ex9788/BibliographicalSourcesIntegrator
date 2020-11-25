using BibliographicalSourcesIntegrator;
using BibliographicalSourcesIntegratorContracts;
using BibliographicalSourcesIntegratorWarehouse.Persistence;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Controllers
{
    public class SearchManager
    {
        private readonly ILogger<SearchManager> _logger;
        private readonly DatabaseAccess databaseAccess;


        public SearchManager(ILogger<SearchManager> logger, DatabaseAccess databaseAccess)
        {
            _logger = logger;
            this.databaseAccess = databaseAccess;
        }


        public SearchAnswer Search(string request)
        {
            SearchRequest searchRequest = GetSearchRequest(request);

            if (searchRequest == null)
            {
                return null;
            }

            return ProcessSearchRequest(searchRequest);
        }

        private SearchRequest GetSearchRequest(string request)
        {
            try
            {
                return JsonSerializer.Deserialize<SearchRequest>(request);
            }
            catch (Exception)
            {
                _logger.LogError("The request is not a LoadRequest.");

                return null;
            }
        }

        private SearchAnswer ProcessSearchRequest(SearchRequest searchRequest)
        {
            SearchAnswer searchAnswer = new SearchAnswer();

            if (searchRequest.SearchArticles)
            {
                searchAnswer.Articles = databaseAccess.GetArticles(searchRequest.Title, searchRequest.Author, searchRequest.InitialYear, searchRequest.FinalYear);
            }

            if (searchRequest.SearchBooks)
            {
                searchAnswer.Books = databaseAccess.GetBooks(searchRequest.Title, searchRequest.Author, searchRequest.InitialYear, searchRequest.FinalYear);
            }

            if (searchRequest.SearchCongressComunications)
            {
                searchAnswer.CongressComunications = databaseAccess.GetCongressComunications(searchRequest.Title, searchRequest.Author, searchRequest.InitialYear, searchRequest.FinalYear);
            }

            return searchAnswer;
        }
    }
}
