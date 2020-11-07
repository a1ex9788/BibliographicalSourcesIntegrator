using BibliographicalSourcesIntegratorContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace IEEEXploreWrapper.Requests
{
    public class RequestsManager
    {
        static HttpClient client = new HttpClient();

        public RequestsManager()
        {
            client.BaseAddress = new Uri(ProgramAddresses.IEEEXploreAPIAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
        }


        public async Task<string> LoadDataFromDataSources(int initialYear, int finalYear)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/v1/search/articles?parameter&apikey=efv84mzqq6ydx4dbd59jhdcn");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return null;
            }
        }
    }
}
