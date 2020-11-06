using BibliographicalSourcesIntegratorContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegrator
{
    public class RequestsManager
    {
        static HttpClient client = new HttpClient();

        public RequestsManager()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
        }


        public async Task<string> LoadDataFromDBLP(ExtractRequest extractRequest)
        {
            client.BaseAddress = new Uri(ProgramAddresses.DBLPWrapperAddress);
            
            return await MakeARequest("ExtractData/" + JsonSerializer.Serialize(extractRequest));

            //return File.ReadAllText("../DBLPreal.json");
        }

        public async Task<string> LoadDataFromIEEEXplore(ExtractRequest extractRequest)
        {
            //client.BaseAddress = new Uri(ProgramAddresses.IEEEXploreWrapperAddress);

            //return await MakeARequest("ExtractData/" + JsonSerializer.Serialize(extractRequest));

            return File.ReadAllText("../IEEEXmini.json");
        }

        public async Task<string> LoadDataFromGoogleScholar(ExtractRequest extractRequest)
        {
            //client.BaseAddress = new Uri(ProgramAddresses.GoogleScholarWrapperAddress);

            //return await MakeARequest("ExtractData/" + JsonSerializer.Serialize(extractRequest));

            return "";
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
