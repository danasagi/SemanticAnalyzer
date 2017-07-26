using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticAnalyzer
{
    public class EntitySentiment
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public SentimentScore SpecificScore { get; set; }
    }
}
