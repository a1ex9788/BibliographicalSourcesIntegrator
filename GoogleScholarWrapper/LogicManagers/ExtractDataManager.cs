using BibliographicalSourcesIntegratorContracts;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace GoogleScholarWrapper.LogicManagers
{
    public class ExtractDataManager
    {
        private readonly ILogger<ExtractDataManager> _logger;      

        public ExtractDataManager(ILogger<ExtractDataManager> logger)
        {
            _logger = logger;
        }


        public async Task<string> ExtractData(string request)
        {
            ExtractRequest extractRequest = GetExtractRequest(request);

            if (extractRequest == null)
            {
                return null;
            }

            return await ExtractDataFromGoogleScholarWithSelenium(extractRequest.InitialYear, extractRequest.FinalYear);
        }


        private ExtractRequest GetExtractRequest(string request)
        {
            try
            {
                return JsonSerializer.Deserialize<ExtractRequest>(request);
            }
            catch (Exception)
            {
                _logger.LogError("The request is not an ExtractRequest.");

                return null;
            }
        }

        private async Task<string> ExtractDataFromGoogleScholarWithSelenium(int initialYear, int finalYear)
        {

            //String exePath = "";
            //System.setProperty("webdriver.chrome.driver", exePath);
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--start-maximized");
            ChromeDriver driver = new ChromeDriver(options);

            try
            {
                driver.Url = "https://scholar.google.es/";


                IWebElement optionsTool = driver.FindElement(By.Id("gs_hdr_mnu"));
                optionsTool.Click();
                IWebElement advancedSearch = driver.FindElement(By.Id("gs_hp_drw_adv"));
                advancedSearch.Click();
                



               /* IWebElement ventanaBuscador = driver.FindElement(By.Name("q"));
                ventanaBuscador.SendKeys("hola");
                ventanaBuscador.Submit(); */




                return "";
            }          
            catch (Exception e)
            {
                _logger.LogError("There was a problem working with Selenium.");

                return null;
            }

            finally
            {
                //Thread.Sleep(2000);
               // driver.Close(); //Cerrar Chrome con Selenium
            }
        }
    }
}
