using BibliographicalSourcesIntegratorContracts;
using Nancy.Json;
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
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
        }


        public async Task<LoadAnswer> LoadDataFrom(LoadRequest loadRequest)
        {
            string dBLPAnswer = "", iEEEXploreAnswer = "", googleScholarAnswer = "";

            ExtractRequest extractRequest = new ExtractRequest(loadRequest.InitialYear, loadRequest.FinalYear);

            if (loadRequest.loadFromDBLP)
            {
                client.BaseAddress = new Uri(ProgramAddresses.DBLPWrapperAddress);

                dBLPAnswer = await MakeARequest("ExtractData/" + JSONHelper<ExtractRequest>.Serialize(extractRequest));
            }

            if (loadRequest.loadFromIEEEXplore)
            {
                client.BaseAddress = new Uri(ProgramAddresses.IEEEXploreWrapperAddress);

                iEEEXploreAnswer = await MakeARequest("ExtractData/" + JSONHelper<ExtractRequest>.Serialize(extractRequest));
            }

            if (loadRequest.loadFromGoogleScholar)
            {
                client.BaseAddress = new Uri(ProgramAddresses.GoogleScholarWrapperAddress);

                googleScholarAnswer = await MakeARequest("ExtractData/" + JSONHelper<ExtractRequest>.Serialize(extractRequest));
            }

            return new LoadAnswer(dBLPAnswer, iEEEXploreAnswer, googleScholarAnswer);
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
