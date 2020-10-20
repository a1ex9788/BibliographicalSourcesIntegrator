﻿using BibliographicalSourcesIntegratorContracts;
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
            client.BaseAddress = new Uri(ProgramAddresses.DBLPWrapperAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
        }


        public async Task<string> LoadDataFrom()
        {
            ExtractRequest extractRequest = new ExtractRequest(1, 5);

            return await MakeARequest("ExtractData/" + new JavaScriptSerializer().Serialize(extractRequest));
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
