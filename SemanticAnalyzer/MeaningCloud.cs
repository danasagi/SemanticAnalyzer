﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SemanticAnalyzer
{
    using System.Security.Policy;

    class MeaningCloud
    {
        public static void AnalyzeArticles(string source, string url, List<string> textList)
        {
            foreach (var text in textList)
            {
                AnalyzeArticle(source, url, text);
            }
        }

        public static void AnalyzeArticle(string source, string url, string text)
        {
            int numEntities = 3;
            double confidenceThreshold = 70;

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.Write("no valid input!");
                return;
            }

            List<string> entities = GetEntitiesByText(text, numEntities, confidenceThreshold);
            var sentiments = GetSentimentsByText(text, entities);
            //now we need to get the sentiment per entity from the list and save it to the storage and also return the info to the service according to the old and new data
   //         OppositeOpinion.AddItem(entities, url, sentiments.);
        }

        public static List<string> GetEntitiesByText(string text, int numEntities, double confidenceThreshold)
        {
            List<string> entitiesIds = new List<string>();
            using (var client = new WebClient())
            {
                var requestUrl = "https://api.meaningcloud.com/topics-2.0";
                client.Headers.Add(HttpRequestHeader.ContentType, Consts.Header);
                var encodedText = Uri.EscapeUriString(text);
                var response = client.UploadString(requestUrl, Consts.KeyAndLang + "txt=" + encodedText + Consts.RequestOptions);

                var result = JsonConvert.DeserializeObject<dynamic>(response);
                if (result != null && result.entity_list != null)
                {
                    numEntities = Math.Min(numEntities, result.entity_list.Count);
                    for (int i = 0; i < numEntities && result.entity_list[i] != null; i++)
                    {
                        if (result.entity_list[i].relevance != null)
                        {
                            int relevance = int.Parse(result.entity_list[i].relevance.Value);
                            if (relevance >= confidenceThreshold)
                            {
                                entitiesIds.Add(result.entity_list[i].id.Value);
                            }
                        }
                    }
                }
            }

            return entitiesIds;
        }

        public static List<Sentiment> GetSentimentsByText(string text, List<string> entitiesIds)
        {
            var sentimentsList = new List<Sentiment>();
            using (var client = new WebClient())
            {
                var requestUrl = "https://api.meaningcloud.com/sentiment-2.1";
                client.Headers.Add(HttpRequestHeader.ContentType, Consts.Header);
                var encodedText = Uri.EscapeUriString(text);
                var response = client.UploadString(requestUrl, Consts.KeyAndLang + "txt=" + encodedText + Consts.RequestOptions);

                var result = JsonConvert.DeserializeObject<dynamic>(response);
                /*for (int i = 0; i < result && result.entity_list[i] != null; i++)
                {
                    if (result.entity_list[i].relevance != null)
                    {
                        int relevance = int.Parse(result.entity_list[i].relevance.Value);
                        if (relevance >= confidenceThreshold)
                        {
                            entitiesIds.Add(result.entity_list[i].id.Value);
                        }
                    }
                }*/
            }

            return sentimentsList;
        }
    }
}
