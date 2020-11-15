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
                //IWebElement cookiesWindow = driver.FindElement(By.Id("introAgreeButton"));
                // if (cookiesWindow != null) { cookiesWindow.Click(); }
                IWebElement optionsTool = driver.FindElement(By.Id("gs_hdr_mnu"));
                optionsTool.Click();
                
                IWebElement advancedSearch = driver.FindElement(By.Id("gs_hp_drw_adv"));
                advancedSearch.Click();
               
                IWebElement initYear = driver.FindElement(By.Id("gs_asd_ylo"));
                initYear.SendKeys(Convert.ToString(initialYear));
                IWebElement finYear = driver.FindElement(By.Id("gs_asd_yhi"));
                finYear.SendKeys(Convert.ToString(finalYear));
                IWebElement search = driver.FindElement(By.Id("gs_asd_psb"));
                search.Click();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

                String BibTeX_file = "";
                List<IWebElement> elements = new List<IWebElement>();
                elements = driver.FindElements(By.XPath("//*[contains(@class,'gs_r gs_or gs_scl')]")).ToList();
                int i = 1;
                foreach (IWebElement element in elements)
                {
                    IWebElement citar = driver.FindElementByXPath("//*[@id='gs_res_ccl_mid']/div["+i+"]/div[2]/div[3]/a[2]");
                    citar.Click();
                    String link_prueba = driver.FindElementByXPath("//*[@id='gs_citi']/a[1]").GetAttribute("href");
                    IWebElement BibTeX = driver.FindElementByXPath("//*[@id='gs_citi']/a[1]");
                    BibTeX.Click();
                    BibTeX_file += driver.FindElementByXPath("/html/body/pre").Text + "\n";
                    driver.Navigate().Back();
                    driver.Navigate().Back();
                    i++;
                }

                return BibTeX_file;
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
