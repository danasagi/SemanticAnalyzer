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
        public static void AnalyzeArticle(string text)
        {
            int numEntities = 3;
            double confidenceThreshold = 70;

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.Write("no valid input!");
                return;
            }
            List<string> entities = GetEntitiesByText(text, numEntities, confidenceThreshold);
            //now we need to get the sentiment per entity from the list and save it to the storage and also return the info to the service according to the old and new data
        }

        /*    private static List<string> GetSortedTopicsFromText(string text, int numEntities, double confidenceThreshold)
            {
                var client = new WebClient();
                var requestUrl =
                    "https://api.meaningcloud.com/sentiment-2.1";
                client.Headers.Add(HttpRequestHeader.ContentType, Consts.Header);

                var response = client.UploadString(requestUrl, Consts.KeyAndLang + text + Consts.RequestOptions);
                dynamic json = JObject.Parse(response);
                return new List<string>();
            }*/

        public static List<string> GetEntitiesByText(string text, int numEntities, double confidenceThreshold)
        {
            using (var client = new WebClient())
            {
                var requestUrl =
                    "https://api.meaningcloud.com/topics-2.0";
                client.Headers.Add(HttpRequestHeader.ContentType, Consts.Header);
                //encodedText = Uri.EscapeUriString(text);
                var response = client.UploadString(requestUrl, Consts.KeyAndLang + "txt=" + text + Consts.RequestOptions);

                var result = JsonConvert.DeserializeObject<dynamic>(response);
                Console.Write(result);
            }

            return new List<string>();
        }

        public static void GetSentimentsByText(string text)
        {
            using (var client = new WebClient())
            {
                var requestUrl =
                    "https://api.meaningcloud.com/sentiment-2.1";
                client.Headers.Add(HttpRequestHeader.ContentType, Consts.Header);
                //encodedText = Uri.EscapeUriString(text);
                var response = client.UploadString(requestUrl, Consts.KeyAndLang + "txt=" + text + Consts.RequestOptions);

                var result = JsonConvert.DeserializeObject<dynamic>(response);
                Console.Write(result);
            }
        }
    }
}
