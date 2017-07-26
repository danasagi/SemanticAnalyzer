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
    using System.Security.Policy;
    using System.Threading;

    public class MeaningCloud
    {
        public static int Count = 0;

        private static string adisKey = "20b7d5e0ec94057b07166ac0b1565299";
        private static string niritsKey = "c42cd3f790592742da5e3ef62284c75c";

        public static List<TopicAgenda> AnalyzeArticle(string source, string url, string text, Dictionary<EntityKey, EntityValue> sourcesBais)
        {
            int numEntities = 3;
            double confidenceThreshold = 70;

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.Write("no valid input!");
                return null;
            }

            List<string> entities = GetEntitiesByText(text, numEntities, confidenceThreshold);
            var sentiment = GetSentimentsByText(text, entities);
            if (sentiment == null)
            {
                return null;
            }
            OppositeOpinion.AddItem(entities, url, sentiment.GeneralScore);
            var oppositeLink = OppositeOpinion.GetOppositeLink(entities, sentiment.GeneralScore);
			FileUtils.UpdateSourcesBais(sourcesBais, source, sentiment);

            Count++;
            if (Count % 1000 == 0)
            {
                System.Console.WriteLine("reached " + Count);
            }
            if (Count > 20000)
            {
                Consts.Key = niritsKey;
            }

            if (Count > 59000)
            {
                Consts.Key = adisKey;
            }

            return sentiment.EntitySentiments.Select(entity => FileUtils.CalculateTopicAgenda(sourcesBais, new EntityKey(source, entity.Id), oppositeLink)).ToList();
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

        public static Sentiment GetSentimentsByText(string text, List<string> entitiesIds)
        {
            var sentiment = new Sentiment { EntitySentiments = new List<EntitySentiment>() };

            using (var client = new WebClient())
            {
                var requestUrl = "https://api.meaningcloud.com/sentiment-2.1";
                client.Headers.Add(HttpRequestHeader.ContentType, Consts.Header);
                var encodedText = Uri.EscapeUriString(text);
                var response = client.UploadString(requestUrl, Consts.KeyAndLang + "txt=" + encodedText + Consts.RequestOptions);

                var result = JsonConvert.DeserializeObject<dynamic>(response);
                if (result == null || result.score_tag == null || result.score_tag.Value == null)
                {
                    return null;
                }
                sentiment.GeneralScore = ParseScore(result.score_tag.Value);
                foreach (var e in result.sentimented_entity_list)
                {
                    if (!entitiesIds.Contains(e.id.Value))
                    {
                        continue;
                    }

                    var entitySentiment = new EntitySentiment
                    {
                        Id = e.id.Value,
                        Name = e.form.Value,
                        Type = e.type.Value,
                        SpecificScore = ParseScore(e.score_tag.Value)
                    };

                    sentiment.EntitySentiments.Add(entitySentiment);
                    if (sentiment.EntitySentiments.Count == entitiesIds.Count)
                    {
                        break;
                    }
                }
            }

            return sentiment;
        }

        private static SentimentScore ParseScore(string score)
        {
            switch (score)
            {
                case "P":
                    return SentimentScore.Positive;
                case "P+":
                    return SentimentScore.StrongPositive;
                case "N":
                    return SentimentScore.Negative;
                case "N+":
                    return SentimentScore.StrongNegative;
                case "NEU":
                    return SentimentScore.Neutral;
                default:
                    return SentimentScore.Neutral;
            }
        }
    }
}
