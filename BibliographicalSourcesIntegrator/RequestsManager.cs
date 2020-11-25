using BibliographicalSourcesIntegratorContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BibliographicalSourcesIntegrator
{
    public class RequestsManager
    {
        private static RequestsManager requestsManagerInstance;

        static HttpClient client = new HttpClient();

        private RequestsManager()
        {
            client.BaseAddress = new Uri(ProgramAddresses.BibliographicalSourcesIntegratorWarehouseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
        }

        public static RequestsManager GetRequestsManager()
        {
            if (requestsManagerInstance == null)
            {
                requestsManagerInstance = new RequestsManager();
            }

            return requestsManagerInstance;
        }


        public async Task<SearchAnswer> SearchDataInWarehouse(SearchRequest searchRequest)
        {
            string answer = await MakeARequest("Search/" + new JavaScriptSerializer().Serialize(searchRequest));

            if (answer == null)
            {
                return null;
            }

            return new JavaScriptSerializer().Deserialize<SearchAnswer>(answer);
        }

        public async Task<LoadAnswer> LoadDataFromDataSources(LoadRequest loadRequest)
        {
            string answer = await MakeARequest("Load/" + new JavaScriptSerializer().Serialize(loadRequest));

            if (answer == null)
            {
                return null;
            }

            return new JavaScriptSerializer().Deserialize<LoadAnswer>(answer);
        }


        private async Task<string> MakeARequest(string path)
        {
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }
    }
}
