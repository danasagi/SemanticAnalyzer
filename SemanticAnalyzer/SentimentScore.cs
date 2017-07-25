using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticAnalyzer
{
    public enum SentimentScore
    {
        Neutral = 0,
        Positive = 1,
        StrongPositive = 2,
        Negative = -1,
        StrongNegative = -2
    }
}
