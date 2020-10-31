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


        public async Task<string> SearchDataInWarehouse()
        {
            return await MakeARequest("no se encara");
        }

        public async Task<LoadAnswer> LoadDataFromDataSources(LoadRequest loadRequest)
        {
            string answer = await MakeARequest("Load/" + JSONHelper.Serialize(loadRequest));

            if (answer == null)
            {
                return null;
            }

            return JSONHelper.Deserialize<LoadAnswer>(answer);
        }


        private async Task<string> MakeARequest(string path)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return null;
            }
        }
    }
}
