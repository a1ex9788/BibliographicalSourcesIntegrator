using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegrator
{
    public class RequestsManager
    {
        static HttpClient client = new HttpClient();

        public RequestsManager()
        {
            client.BaseAddress = new Uri("http://localhost:49845");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
        }


        public async Task<string> SearchDataInWarehouse()
        {
            return await MakeARequest("no se encara");
        }

        public async Task<string> LoadDataFromDataSources()
        {
            return await MakeARequest("no se encara");
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
            catch (Exception)
            {
                return null;
            }
        }
    }
}
