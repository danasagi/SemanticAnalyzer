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
        public static void AnalyzeArticle(string source, List<string> textList)
        {
            foreach (var text in textList)
            {
                AnalyzeArticle(text);
            }
        }

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
            GetSentimentsByText(text, entities);
            //now we need to get the sentiment per entity from the list and save it to the storage and also return the info to the service according to the old and new data
        }

        public static List<string> GetEntitiesByText(string text, int numEntities, double confidenceThreshold)
        {
            List<string> entitiesIds = new List<string>(); 
            using (var client = new WebClient())
            {
                var requestUrl =
                    "https://api.meaningcloud.com/topics-2.0";
                client.Headers.Add(HttpRequestHeader.ContentType, Consts.Header);
                //encodedText = Uri.EscapeUriString(text);
                var response = client.UploadString(requestUrl, Consts.KeyAndLang + "txt=" + text + Consts.RequestOptions);

                var result = JsonConvert.DeserializeObject<dynamic>(response);
                for (int i = 0; i < numEntities && result.entity_list[i] != null; i++)
                {
                    if (result.entity_list[i].relevance != null)
                    {
                        int relevance  = int.Parse(result.entity_list[i].relevance.Value);
                        if (relevance >= confidenceThreshold)
                        {
                            entitiesIds.Add(result.entity_list[i].id.Value);
                        }
                    }
                }
            }

            return entitiesIds;
        }

        public static void GetSentimentsByText(string text, List<string> entitiesIds)
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
