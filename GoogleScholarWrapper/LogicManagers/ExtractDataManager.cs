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
            Process process = new Process();

            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c" + "dir > pene.txt";

            process.Start();
            process.WaitForExit();





            String BibTeX_file = "@book{halbwachs2004memoria,title ={ La memoria colectiva}, author ={ Halbwachs, Maurice}, volume ={ 6},year ={ 2004}, publisher ={ Prensas de la Universidad de Zaragoza}}"; // @article{hernandez2010metodologia, title ={Metodologia de la},author ={Hern{\'a}ndez, Roberto and Fern{\'a}ndez, Carlos and Baptista, Pilar},journal ={Ciudad de M{\'e}xico: Mc Graw Hill},volume ={ 12},pages ={ 20},year ={ 2010}}";
            //BibtexFile file = BibtexLibrary.BibtexImporter.FromString(BibTeX_file);
            StreamWriter sw = new StreamWriter("..\\archivoBibTeX.bib");
            sw.WriteLine(BibTeX_file);
            sw.Close();
            //pandoc-citeproc --bib2json archivoBibTeX.bib > archivoBibTeX.json;
            return BibTeX_file;
            /*  //String exePath = "";
              //System.setProperty("webdriver.chrome.driver", exePath);

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

                  String BibTeX_file = "";
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
                              BibTeX_file += driver.FindElementByXPath("/html/body/pre").Text + "\n";
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

                  // pandoc-citeproc --bib2json
                  BibtexFile file = BibtexLibrary.BibtexImporter.FromString(BibTeX_file);
                   //Latext document =  File.WriteAllText("../", BibTeX_file);   
                  //BibTeX_file = BibTeX_file.Replace("@", "").Replace("title=", "\"title\":").Replace("author=", "\"author\":").Replace("journal=", "\"journal\":").Replace("pages=", "\"pages\":").Replace("year=", "\"year\":");
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
             */



        }
    }
}
