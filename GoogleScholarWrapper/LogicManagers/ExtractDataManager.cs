using BibliographicalSourcesIntegratorContracts;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
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

            if (bibTeXFile == null) 
            {
                return null;
            }

            CreateBibtexFile(bibTeXFile);

            return ConvertBibteXToJson();
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
            string bibtexFile = "";

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

                IWebElement initYear = driver.FindElement(By.Id("gs_asd_ylo"));
                initYear.SendKeys(Convert.ToString(initialYear));
                IWebElement finYear = driver.FindElement(By.Id("gs_asd_yhi"));
                finYear.SendKeys(Convert.ToString(finalYear));
                IWebElement search = driver.FindElement(By.Id("gs_asd_psb"));
                search.Click();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

                for (int page = 1; page <= 5; page++)
                {
                    int element = 1;                

                    try
                    {
                        while (element < 4)
                        {
                            try
                            {
                                IWebElement citar = driver.FindElementByXPath("//*[@id='gs_res_ccl_mid']/div[" + element + "]/div[2]/div[3]/a[2]");
                                if (citar != null)
                                {
                                    citar.Click();
                                    Thread.Sleep(200);
                                    IWebElement BibTeX = driver.FindElementByXPath("//*[@id='gs_citi']/a[1]");
                                    BibTeX.Click();
                                    bibtexFile += driver.FindElementByXPath("/html/body/pre").Text + "\n";
                                    Debug.WriteLine("Contenido:  \n" + bibtexFile);
                                    Thread.Sleep(200);
                                    driver.Navigate().Back();
                                    driver.Navigate().Back();
                                }
                            }
                            catch (Exception) { }

                            element++;
                        }
                    }
                    catch (Exception)
                    {
                        element++;
                    }

                    driver.Url = "https://scholar.google.es/scholar?start=" + page + "0&hl=es&as_sdt=0,5&as_ylo=" + initialYear + "&as_yhi=" + finalYear;
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
                driver.Close();
            }

            return bibtexFile;
        }

        private void CreateBibtexFile(string bibtexFile)
        {
            Directory.CreateDirectory("TempFiles");
            File.WriteAllText(bibtexFilePath, bibtexFile);
        }

        private string ConvertBibteXToJson()
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
