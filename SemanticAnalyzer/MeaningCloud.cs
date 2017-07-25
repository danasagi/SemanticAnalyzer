using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SemanticAnalyzer
{
    class MeaningCloud
    {
        public static void AnalyzeArticle(string text = "", string url = "", string document = "")
        {
            AnalyzeArticle(3, 70, text, url, document);
        }

        public static void AnalyzeArticle(int numEntities = 3, double confidenceThreshold = 70, string text = "", string url = "", string document = "")
        {
            if (text == "" && url == "" && document == "")
            {
                Console.Write("no valid input!");
                return;
            }
            string textToAnalyze = FindNonEmptyString(text, url, document);
            List<string> entities = GetSortedTopicsFromText(textToAnalyze, numEntities, confidenceThreshold);
            //now we need to get the sentiment per entity from the list and save it to the storage and also return the info to the service according to the old and new data
        }


        /**
         * the UploadString method is problematic since it returns only the first information from the response (success + returnCode and etc). we need to extract the entity_list
         * which will give us the 'form' (entity name) and the score of each entity.
         * then parse it to JArray instead of JObject and get all the list according to the numEntities and confidenceThreshold parameters.
        */
        private static List<string> GetSortedTopicsFromText(string text, int numEntities, double confidenceThreshold)
        {
            var client = new WebClient();
            var requestUrl =
                "https://api.meaningcloud.com/sentiment-2.1";
            client.Headers.Add(HttpRequestHeader.ContentType, Consts.Header);

            var response = client.UploadString(requestUrl, Consts.KeyAndLang + text + Consts.RequestOptions);
            dynamic json = JObject.Parse(response);
            return new List<string>();
        }


        private static string FindNonEmptyString(string text, string url, string doc)
        {
            if (text != "") return "txt=" + text;
            if (url != "") return "url=" + url;
            return "doc=" + doc;
        }
    }
}
