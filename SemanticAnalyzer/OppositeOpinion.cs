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

            return other.entitiesIds.Where((t, i) => this.entitiesIds[i] != t).Any();
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
            return (this.entitiesIds != null ? this.entitiesIds.GetHashCode() : 0);
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
        private const int GoodScore = 1;
        private const int VeryGoodScore = 2;
        private const int BadScore = -1;
        private const int VeryBadScore = -2;

        private static Dictionary<EntitySet, Dictionary<int, string>> EntitySetToOpinionToLink = new Dictionary<EntitySet, Dictionary<int, string>>();

        public static void AddItem(List<string> entitiesIds, string url, int score)
        {
            EntitySet set = new EntitySet(entitiesIds);
            EntitySetToOpinionToLink[set][score] = url;
        }

        public static string GetOppositeLink(List<string> entitiesIds, int score)
        {
            EntitySet set = new EntitySet(entitiesIds);

            if (score == 0)
            {
                return string.Empty;
            }

            // Looking for good score
            if (score < 0)
            {
                if (EntitySetToOpinionToLink.ContainsKey(set) && EntitySetToOpinionToLink[set].ContainsKey(VeryGoodScore))
                {
                    return EntitySetToOpinionToLink[set][VeryGoodScore];
                }
                else if (EntitySetToOpinionToLink.ContainsKey(set) && EntitySetToOpinionToLink[set].ContainsKey(GoodScore))
                {
                    return EntitySetToOpinionToLink[set][GoodScore];
                }
            }

            // Looking for bed score 
            if (score > 0)
            {
                if (EntitySetToOpinionToLink.ContainsKey(set) && EntitySetToOpinionToLink[set].ContainsKey(VeryGoodScore))
                {
                    return EntitySetToOpinionToLink[set][VeryBadScore];
                }
                else if (EntitySetToOpinionToLink.ContainsKey(set) && EntitySetToOpinionToLink[set].ContainsKey(GoodScore))
                {
                    return EntitySetToOpinionToLink[set][BadScore];
                }
            }

            return string.Empty;
        }
    }
}