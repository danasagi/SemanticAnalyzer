﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;


namespace SemanticAnalyzer
{
    public class EntityKey
    {
        protected bool Equals(EntityKey other)
        {
            return string.Equals(Src, other.Src) && string.Equals(Entity, other.Entity);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EntityKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Src != null ? Src.GetHashCode() : 0) * 397) ^ (Entity != null ? Entity.GetHashCode() : 0);
            }
        }

        public string Src;
        public string Entity;
        public EntityKey(string src, string entity)
        {
            Src = src;
            Entity = entity;
        }

    }

    public class EntityValue
    {
        public string Name;
        public string Type;
        public int PositiveSpecific;
        public int NegativeSpecific;
        public int NeutralSpecific;
        public int PositiveGeneral;
        public int NegativeGeneral;
        public int NeutralGeneral;


        public EntityValue(string name, string type, int positiveSpecific, int negativeSpecific, int neutralSpecific, int positiveGeneral, int negativeGeneral, int neutralGeneral)
        {
            Name = name;
            Type = type;
            PositiveSpecific = positiveSpecific;
            NegativeSpecific = negativeSpecific;
            NeutralSpecific = neutralSpecific;
            PositiveGeneral = positiveGeneral;
            NegativeGeneral = negativeGeneral;
            NeutralGeneral = neutralGeneral;
        }

    }
    public class FileUtils
    {

        public static void ReadSourcesBais(string sourcesBaisFileStr, Dictionary<EntityKey, EntityValue> sourcesBais)
        {
            //var lines = File.ReadLines(sourcesBaisPath);
            var lines = sourcesBaisFileStr.Split(new [] { "\r\n"}, StringSplitOptions.None);
            foreach (var line in lines)
            {
                try
                {
                    var elementsList = line.Split(',');
                    EntityKey key = new EntityKey(elementsList[0], elementsList[1]);
                    var nameLength = elementsList.Length - 10 + 1;
                    var offset = nameLength + 1;
                    string name = elementsList[2];
                    for (int i = 1; i < nameLength; i++)
                    {
                        name += ", " + elementsList[2 + i];
                    }

                    EntityValue value = new EntityValue(name, elementsList[offset + 1], Int32.Parse(elementsList[offset + 2]), Int32.Parse(elementsList[offset + 3]), Int32.Parse(elementsList[offset + 4]), Int32.Parse(elementsList[offset + 5]), Int32.Parse(elementsList[offset + 6]), Int32.Parse(elementsList[offset + 7]));
                    sourcesBais.Add(key, value);
                }
                catch (Exception e)
                {

                    Console.Out.WriteLine("");
                }
            }
        }

        public static void UpdateSourcesBais(Dictionary<EntityKey, EntityValue> sourcesBais, string src, Sentiment articleSentiments)
        {
            foreach (var articleSentiment in articleSentiments.EntitySentiments)
            {
                EntityKey key = new EntityKey(src, articleSentiment.Id);
                EntityValue initValue = new EntityValue(articleSentiment.Name, articleSentiment.Type, 0, 0, 0, 0, 0, 0);
                if (!(sourcesBais.ContainsKey(key))) //new entity
                {
                    sourcesBais.Add(key, initValue);
                }
                switch (articleSentiments.GeneralScore)
                {
                    case SentimentScore.StrongNegative:
                        sourcesBais[key].NegativeGeneral++;
                        break;
                    case SentimentScore.Negative:
                        sourcesBais[key].NegativeGeneral++;
                        break;
                    case SentimentScore.Neutral:
                        sourcesBais[key].NeutralGeneral++;
                        break;
                    case SentimentScore.Positive:
                        sourcesBais[key].PositiveGeneral++;
                        break;
                    case SentimentScore.StrongPositive:
                        sourcesBais[key].PositiveGeneral++;
                        break;
                }
                switch (articleSentiment.SpecificScore)
                {
                    case SentimentScore.StrongNegative:
                        sourcesBais[key].NegativeSpecific++;
                        break;
                    case SentimentScore.Negative:
                        sourcesBais[key].NegativeSpecific++;
                        break;
                    case SentimentScore.Neutral:
                        sourcesBais[key].NeutralSpecific++;
                        break;
                    case SentimentScore.Positive:
                        sourcesBais[key].PositiveSpecific++;
                        break;
                    case SentimentScore.StrongPositive:
                        sourcesBais[key].PositiveSpecific++;
                        break;
                } 
            }   
        }

        private static string ConvertToCSVLine(EntityKey key, EntityValue value)
        {
            string line = key.Src + ',' + key.Entity + ',' + value.Name + ',' + value.Type + ',' + (value.PositiveSpecific).ToString() +
                          ',' + value.NegativeSpecific.ToString() + ',' + value.NeutralSpecific.ToString() +
                          ',' + value.PositiveGeneral.ToString() + ',' + value.NegativeGeneral.ToString() +
                          ',' + value.NeutralGeneral.ToString();
            return line;
        }

        public static void WriteSourcesBais(string sourcesBaisPath, Dictionary<EntityKey, EntityValue> sourcesBais)
        {
            var sourcesBaisOutput = new StringBuilder();
            foreach (var item in sourcesBais)
            {
                sourcesBaisOutput.AppendLine(ConvertToCSVLine(item.Key, item.Value));
            }
            File.WriteAllText(sourcesBaisPath, sourcesBaisOutput.ToString());
        }

        public static TopicAgenda CalculateTopicAgenda(Dictionary<EntityKey, EntityValue> sourcesBais, EntityKey key, string oppositeOpinionUrl)
        {
            EntityValue value;
            int minNumOfArticles = 20;
            if (sourcesBais.TryGetValue(key, out value) && SumNumOfTopics(value) >= minNumOfArticles && !ContainsBadTypes(value.Type))
            {
                return new TopicAgenda()
                {
                    Name = value.Name,
                    Agenda = CalculateTopicAgenda(value.PositiveGeneral, value.NegativeGeneral, value.NeutralGeneral),
                    OppositeOpinionUrl = oppositeOpinionUrl
                };
            }

            return null;
        }

        private static bool ContainsBadTypes(string value)
        {
            return value.Contains("Location") || value.Contains("FirstName");
        }

        private static int SumNumOfTopics(EntityValue value)
        {
            return value.PositiveGeneral + value.NegativeGeneral + value.NeutralGeneral;
        }

        private static int CalculateTopicAgenda(double positive, double negative, double neutral)
        {
            var relation = (positive + (neutral / 2)) / (positive + negative + neutral);
            if (relation >= 0 && relation < 0.2)
            {
                return -2;
            }

            if (relation >= 0.2 && relation < 0.4)
            {
                return -1;
            }

            if (relation >= 0.4 && relation < 0.6)
            {
                return 0;
            }

            if (relation >= 0.6 && relation < 0.8)
            {
                return 1;
            }

            return 2;
        }
		
		   public static void ReadOppositeOpinion(string path)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string line = file.ReadLine();

            while (line != null)
            {
                List<string> entitiesIds = line.Split(' ').ToList();
                EntitySet entitySet = new EntitySet(entitiesIds);
                OppositeOpinion.EntitySetToOpinionToLink.Add(entitySet, new Dictionary<SentimentScore, string>());
                line = file.ReadLine();

                if (line != null)
                {
                    var scoreToUrlList = line.Split('\t').ToList();
                    foreach (var scoreToUrl in scoreToUrlList)
                    {
                        if (scoreToUrl != string.Empty)
                        {
                            var scoreToUrlArray = scoreToUrl.Split(' ');
                            var score = (SentimentScore)Enum.Parse(typeof(SentimentScore), scoreToUrlArray[0]);
                            OppositeOpinion.EntitySetToOpinionToLink[entitySet].Add(score, scoreToUrlArray[1]);
                        }
                    }
                }

                line = file.ReadLine();
            }

            file.Close();
        }

        public static void WriteOppositeOpinion(string path)
        {
            var sourcesBaisOutput = new StringBuilder();
            foreach (var item in OppositeOpinion.EntitySetToOpinionToLink)
            {
                sourcesBaisOutput.AppendLine(item.Key.ToString());
                string line = item.Value.Aggregate(string.Empty, (score, url) => score + (url.Key.ToString() + " " + url.Value + "\t"));
                sourcesBaisOutput.AppendLine(line);
            }

            File.WriteAllText(path, sourcesBaisOutput.ToString());
        }
    }
}
