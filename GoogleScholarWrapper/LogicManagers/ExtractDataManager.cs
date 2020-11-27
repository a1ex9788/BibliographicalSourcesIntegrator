using BibliographicalSourcesIntegratorContracts;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private string bibtexFilePath = "TempFiles\\archivoBibTeX.bib", jsonFilePath = "TempFiles\\archivoJson.json";


        public ExtractDataManager(ILogger<ExtractDataManager> logger)
        {
            _logger = logger;
        }


        public string ExtractData(string request)
        {
            ExtractRequest extractRequest = GetExtractRequest(request);

            if (extractRequest == null)
            {
                return null;
            }

            string bibTeXFile = GetBibTeXWithSelenium(extractRequest.InitialYear, extractRequest.FinalYear);

            CreateBibteXFile(bibTeXFile);

            return ConverBibteXToJson();
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

        private string GetBibTeXWithSelenium(int initialYear, int finalYear)
        {
            string bibTeXFile = "";

            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--start-maximized");
            //options.AddArguments("--incognito");
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

                List<IWebElement> elements = new List<IWebElement>();
                elements = driver.FindElements(By.XPath("//*[contains(@class,'gs_r gs_or gs_scl')]")).ToList();
                int i = 1;
                foreach (IWebElement element in elements)
                {
                    try
                    {
                        IWebElement citar = driver.FindElementByXPath("//*[@id='gs_res_ccl_mid']/div[" + i + "]/div[2]/div[3]/a[2]");

                        if (citar != null)
                        {
                            citar.Click();
                            String link_prueba = driver.FindElementByXPath("//*[@id='gs_citi']/a[1]").GetAttribute("href");
                            IWebElement BibTeX = driver.FindElementByXPath("//*[@id='gs_citi']/a[1]");
                            BibTeX.Click();
                            bibTeXFile += driver.FindElementByXPath("/html/body/pre").Text + "\n";
                            driver.Navigate().Back();
                            driver.Navigate().Back();
                        }
                    }
                    catch (Exception e)
                    {
                        driver.Url = "https://scholar.google.es/scholar?as_q=&as_epq=&as_oq=&as_eq=&as_occt=any&as_sauthors=&as_publication=&as_ylo=2000&as_yhi=2010&hl=es&as_sdt=0%2C5";
                        //driver.Navigate().Back();
                        _logger.LogError("Ruta incorrecta");
                    }
                    if (i == 4) i = i++;
                    i++;
                }
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

            return bibTeXFile;
        }

        private void CreateBibteXFile(string bibTeXFile)
        {
            Directory.CreateDirectory("TempFiles");
            File.WriteAllText(bibtexFilePath, bibTeXFile);
        }

        private string ConverBibteXToJson()
        {
            Process process = new Process();
            string command = $"pandoc-citeproc --bib2json {bibtexFilePath} > {jsonFilePath}";

            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c" + command;

            process.Start();
            process.WaitForExit();

            return File.ReadAllText(jsonFilePath);
        }
    }
}
