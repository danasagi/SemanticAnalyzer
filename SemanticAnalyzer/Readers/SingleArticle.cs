using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticAnalyzer.Readers
{
    public class Keyword
    {
        public string value { get; set; }
        public string is_major { get; set; }
        public string name { get; set; }
        public string rank { get; set; }
    }

    public class SingleArticle
    {
        public string url { get; set; }
        public List<Keyword> keywords { get; set; }
        public string datePublished { get; set; }
        public string content { get; set; }
        public string headLine { get; set; }
    }
}
