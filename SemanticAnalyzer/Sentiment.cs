using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticAnalyzer
{
    public class Sentiment
    {
        public SentimentScore GeneralScore { get; set; }

        public List<EntitySentiment> EntitySentiments { get; set; }
    }
}
