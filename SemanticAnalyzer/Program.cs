//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using RestSharp;
//using Newtonsoft.Json;


//namespace SemanticAnalyzer
//{
//    public class Program
//    {
//        public static void Main()
//        {
//            string text =
//                "When President Donald Trump enters the annual G20 Summit this week, he will begin an important dialogue about a series of pivotal issues ranging from financial regulation to trade and immigration. The conversations, which the G20 has been conducting since 1999, will involve cabinet - room style talks with all the leaders in this powerful group as well as side discussions between particular leaders focusing on the challenges that their respective nations face and tensions that exist between them.Most eagerly anticipated are the potential interactions between Presidents Donald Trump and Vladimir Putin. But Trump walks into this summit with the United States now in a much weaker position than when he started his presidency. The President, who prides himself on making America great again, brings with him a set of liabilities that will make it more difficult for him to persuade others at this crucial gathering in Hamburg, Germany, to listen to his recommendations or fear his threats-- despite all the economic and military power that the United States brings to the table.";
//            GetEntitiesByText(text);
//            GetSentimentsByText(text);
//            var myHashMap = new Dictionary<string, int>();
//            myHashMap["dana"] = 1;
//            XmlManager.SerializeObject(myHashMap, ".\\map.txt");
//            var mapAfter = XmlManager.DeSerializeObject<Dictionary<string, int>>(".\\map.txt");
//            Console.Write("danascore: " + mapAfter["dana"]);
//        }

//        private static void GetEntitiesByText(string text)
//        {
//            using (var client = new WebClient())
//            {
//                var requestUrl =
//                    "https://api.meaningcloud.com/topics-2.0?key=d56be77678d4001808974084056a1712&of=json&lang=en&ilang=en&txt=" + text + "&tt=a&uw=y";
//                var response = client.UploadValues(requestUrl, new NameValueCollection());
//                var responseString = Encoding.Default.GetString(response);
//                var result = JsonConvert.DeserializeObject<dynamic>(responseString);
//                Console.Write(result);
//            }
//        }
//        private static void GetSentimentsByText(string text)
//        {
//            using (var client = new WebClient())
//            {
//                var requestUrl =
//                    "https://api.meaningcloud.com/sentiment-2.1?key=d56be77678d4001808974084056a1712&of=json&lang=en&ilang=en&txt=" + text + "&tt=a&uw=y";
//                var response = client.UploadValues(requestUrl, new NameValueCollection());
//                var responseString = Encoding.Default.GetString(response);
//                var result = JsonConvert.DeserializeObject<dynamic>(responseString);
//                Console.Write(result);
//            }
//        }

//    }
//}




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
            GetEntitiesByText(Consts.t);
            GetSentimentsByText(Consts.text);
        }

        private static void GetEntitiesByText(string text)
        {
            using (var client = new WebClient())
            {
                var requestUrl =
                    "https://api.meaningcloud.com/topics-2.0";
                client.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                //var encodedText = Uri.EscapeUriString(text);
                var response = client.UploadString(requestUrl, "key=d56be77678d4001808974084056a1712&of=json&lang=en&ilang=en&txt=" + text + "&tt=a&uw=y");

                var result = JsonConvert.DeserializeObject<dynamic>(response);
                Console.Write(result);
            }
        }
        private static void GetSentimentsByText(string text)
        {
            using (var client = new WebClient())
            {
                var requestUrl =
                    "https://api.meaningcloud.com/sentiment-2.1";
                client.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                //var encodedText = Uri.EscapeUriString(text);
                var response = client.UploadString(requestUrl, "key=d56be77678d4001808974084056a1712&of=json&lang=en&ilang=en&txt=" + text + "&tt=a&uw=y");

                var result = JsonConvert.DeserializeObject<dynamic>(response);
                Console.Write(result);
            }
        }

    }
}
