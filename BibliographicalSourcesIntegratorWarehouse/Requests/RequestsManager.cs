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
        HttpClient DBLPclient = new HttpClient();
        HttpClient IEEEXclient = new HttpClient();
        HttpClient BibTeXclient = new HttpClient();

        public RequestsManager()
        {
            DBLPclient.DefaultRequestHeaders.Accept.Clear();
            DBLPclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
            IEEEXclient.DefaultRequestHeaders.Accept.Clear();
            IEEEXclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
            BibTeXclient.DefaultRequestHeaders.Accept.Clear();
            BibTeXclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
        }


        public async Task<string> LoadDataFromDBLP(ExtractRequest extractRequest)
        {
            DBLPclient.BaseAddress = new Uri(ProgramAddresses.DBLPWrapperAddress);
            
            return await MakeARequest("ExtractData/" + JsonSerializer.Serialize(extractRequest), DBLPclient);

            //return File.ReadAllText("../DBLPreal.json");
        }

        public async Task<string> LoadDataFromIEEEXplore(ExtractRequest extractRequest)
        {
            IEEEXclient.BaseAddress = new Uri(ProgramAddresses.IEEEXploreWrapperAddress);

            return await MakeARequest("ExtractData/" + JsonSerializer.Serialize(extractRequest), IEEEXclient);

            //return File.ReadAllText("../IEEEXmini.json");
        }

        //public async Task<string> LoadDataFromGoogleScholar(ExtractRequest extractRequest)
         public async Task<string> LoadDataFromGoogleScholar(ExtractRequest extractRequest)
        {
            BibTeXclient.BaseAddress = new Uri(ProgramAddresses.GoogleScholarWrapperAddress);

           // return await MakeARequest("ExtractData/" + JsonSerializer.Serialize(extractRequest));

            return File.ReadAllText("../sample_array.json");
        }


        private async Task<string> MakeARequest(string path, HttpClient client)
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
