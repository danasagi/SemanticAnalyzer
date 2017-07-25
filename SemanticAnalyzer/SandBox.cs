using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SemanticAnalyzer
{
    public static class SandBox
    {
        public static void GetEntitiesByText(string text)
        {
            using (var client = new WebClient())
            {
                var requestUrl =
                    "https://api.meaningcloud.com/topics-2.0";
                client.Headers.Add(HttpRequestHeader.ContentType, Consts.Header);
                //encodedText = Uri.EscapeUriString(text);
                var response = client.UploadString(requestUrl, Consts.KeyAndLang + "txt=" + text + Consts.RequestOptions);

                var result = JsonConvert.DeserializeObject<dynamic>(response);
                Console.Write(result);
            }
        }
        public static void GetSentimentsByText(string text)
        {
            using (var client = new WebClient())
            {
                var requestUrl =
                    "https://api.meaningcloud.com/sentiment-2.1";
                client.Headers.Add(HttpRequestHeader.ContentType, Consts.Header);
                //encodedText = Uri.EscapeUriString(text);
                var response = client.UploadString(requestUrl, Consts.KeyAndLang + "txt=" + text + Consts.RequestOptions);

                var result = JsonConvert.DeserializeObject<dynamic>(response);
                Console.Write(result);
            }
        }
    }
}
