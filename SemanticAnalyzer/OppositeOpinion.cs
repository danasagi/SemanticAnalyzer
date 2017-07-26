// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OppositeOpinion.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>
//   The opposite opinion.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SemanticAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EntitySet
    {
        protected bool Equals(EntitySet other)
        {
            if (this.entitiesIds.Count != other.entitiesIds.Count)
            {
                return false;
            }

            for (int i=0; i< other.entitiesIds.Count; i++)
            {
                if (!this.entitiesIds[i].Equals(other.entitiesIds[i]))
                {
                    return false;
                }
            }

            return true;
            //    return other.entitiesIds.Where((t, i) => !this.entitiesIds[i].Equals(t)).Any();
        }

        public override string ToString()
        {
            return string.Join(" ", this.entitiesIds);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return this.Equals((EntitySet)obj);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            if (this.entitiesIds != null)
            {
                foreach (var entity in this.entitiesIds)
                {
                    hash *= entity.GetHashCode();
                }
            }
            return hash;
        }

        private readonly List<string> entitiesIds ;

        public EntitySet(List<string> entitiesIds)
        {
            entitiesIds.Sort();
            this.entitiesIds = entitiesIds;
        }
    }

    /// <summary>
    /// The opposite opinion.
    /// </summary>
    public class OppositeOpinion
    {
        public static Dictionary<EntitySet, Dictionary<SentimentScore, string>> EntitySetToOpinionToLink = new Dictionary<EntitySet, Dictionary<SentimentScore, string>>();

        public static void AddItem(List<string> entitiesIds, string url, SentimentScore score)
        {
            if (entitiesIds == null || entitiesIds.Count < 2)
            {
                return;
            }

            EntitySet set = new EntitySet(entitiesIds);
            if (!EntitySetToOpinionToLink.ContainsKey(set))
            {
                EntitySetToOpinionToLink.Add(set, new Dictionary<SentimentScore, string>());
            }
          
            EntitySetToOpinionToLink[set][score] = url;
        }

        public static string GetOppositeLink(List<string> entitiesIds, SentimentScore score)
        {
            if (entitiesIds == null || entitiesIds.Count < 2)
            {
                return null;
            }

            EntitySet set = new EntitySet(entitiesIds);

            if (score == 0)
            {
                return null;
            }

            // Looking for good score
            if (score < 0)
            {
                if (EntitySetToOpinionToLink.ContainsKey(set) && EntitySetToOpinionToLink[set].ContainsKey(SentimentScore.StrongPositive))
                {
                    return EntitySetToOpinionToLink[set][SentimentScore.StrongPositive];
                }
                else if (EntitySetToOpinionToLink.ContainsKey(set) && EntitySetToOpinionToLink[set].ContainsKey(SentimentScore.Positive))
                {
                    return EntitySetToOpinionToLink[set][SentimentScore.Positive];
                }
            }

            // Looking for bed score 
            if (score > 0)
            {
                if (EntitySetToOpinionToLink.ContainsKey(set) && EntitySetToOpinionToLink[set].ContainsKey(SentimentScore.StrongNegative))
                {
                    return EntitySetToOpinionToLink[set][SentimentScore.StrongNegative];
                }
                else if (EntitySetToOpinionToLink.ContainsKey(set) && EntitySetToOpinionToLink[set].ContainsKey(SentimentScore.Negative))
                {
                    return EntitySetToOpinionToLink[set][SentimentScore.Negative];
                }
            }

            return null;
        }
    }
}