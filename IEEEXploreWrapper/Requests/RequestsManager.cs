using BibliographicalSourcesIntegratorContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Script.Serialization;

namespace IEEEXploreWrapper.Requests
{
    public class RequestsManager
    {
        static HttpClient client = new HttpClient();


        string ApiKeyParameter { get => "&apikey=efv84mzqq6ydx4dbd59jhdcn"; }

        int InitialYearValue { get; set; }

        string StartYearParameter { get => $"&start_year={InitialYearValue}"; }

        int FinalYearValue { get; set; }

        string EndYearParameter { get => $"&end_year={FinalYearValue}"; }

        string SortFieldParameter { get => "&sort_field=article_number"; }

        string ShortOrderParameter { get => "&sort_order=asc"; }

        int MaxRecordsValue { get => 200; }

        string MaxRecordsParameter { get => "&max_records=" + MaxRecordsValue; }

        int StartRecordValue { get; set; }

        string StartRecordParameter { get => "&start_record=" + StartRecordValue; }

        string ContentTypeValue { get; set; }

        string ContentTypeParameter { get => "&content_type=" + ContentTypeValue; }

        string Request { get => "?parameter" + ApiKeyParameter + StartYearParameter + EndYearParameter + SortFieldParameter + ShortOrderParameter + MaxRecordsParameter + StartRecordParameter + ContentTypeParameter; }


        public RequestsManager()
        {
            client.BaseAddress = new Uri("http://ieeexploreapi.ieee.org/api/v1/search/articles");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
        }


        public async Task<string> LoadDataFromDataSources(int initialYear, int finalYear)
        {
            // TODO: Hablar con el profesor para ver si tenemos que hacer paginación o con los primeros 200 resultados sobra.
            bool PAGINATION = false;

            InitialYearValue = initialYear;
            FinalYearValue = finalYear;
            StartRecordValue = 1;

            string firstAnswer = await MakeARequest(Request);
            int numberOfArticlesFound = GetNumberOfArticlesFound(firstAnswer);

            string answer = "{total_records:" + numberOfArticlesFound + ",articles:[";
            AddNewAnswer(firstAnswer);


            //////////////////////////////////
            ContentTypeValue = "Journals";
            string articlesAnswer = await MakeARequest(Request);
            AddNewAnswer(articlesAnswer);

            ContentTypeValue = "Books";
            string booksAnswer = await MakeARequest(Request);
            AddNewAnswer(booksAnswer);

            ContentTypeValue = "Conferences";
            string conferencesAnswer = await MakeARequest(Request);
            AddNewAnswer(conferencesAnswer);
            //////////////////////////////////////


            for (StartRecordValue += MaxRecordsValue; PAGINATION && StartRecordValue < numberOfArticlesFound; StartRecordValue += MaxRecordsValue)
            {
                string newAnswer = await MakeARequest(Request);

                AddNewAnswer(newAnswer);
            }

            return answer.Substring(0, answer.Length - 1) + "]}";


            void AddNewAnswer(string newAnswer)
            {
                int indexOfOpeningSquareBracket = newAnswer.IndexOf('[');
                int indexOfClosingSquareBracket = newAnswer.LastIndexOf(']');

                answer += newAnswer.Substring(indexOfOpeningSquareBracket + 1, indexOfClosingSquareBracket - 1 - indexOfOpeningSquareBracket) + ',';
            }
        }

        async Task<string> MakeARequest(string path)
        {
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        int GetNumberOfArticlesFound(string ieeexploreAnswer)
        {
            int startPositionOfLabel = ieeexploreAnswer.IndexOf("total_records");

            int startPositionOfTotalRecords = ieeexploreAnswer.IndexOf(':', startPositionOfLabel) + 1;

            int endPostitionOfTotalRecords = ieeexploreAnswer.IndexOf(',', startPositionOfTotalRecords) - 1;

            return Convert.ToInt32(ieeexploreAnswer.Substring(startPositionOfTotalRecords, endPostitionOfTotalRecords - startPositionOfTotalRecords + 1));
        }
    }
}
