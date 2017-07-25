

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;


namespace SemanticAnalyzer
{
    public class Program
    {
        public static void Main()
        {
            //SandBox.GetEntitiesByText(Consts.t);
            // SandBox.GetSentimentsByText(Consts.text);

            /**
             *this is going to be the main method: 
             */ 
            MeaningCloud.AnalyzeArticle(3, 70, Consts.t);
        }
    }
}
