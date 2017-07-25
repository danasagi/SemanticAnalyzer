// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace SemanticAnalyzer
{
    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main.
        /// </summary>
        public static void Main()
        {
            // SandBox.GetEntitiesByText(Consts.t);
            // SandBox.GetSentimentsByText(Consts.text);
            /**
             *this is going to be the main method: 
             */
            //  MeaningCloud.AnalyzeArticle(Consts.t);
            var url =
            "https://www.nytimes.com/2017/07/25/us/politics/trump-attacks-own-attorney-general-jeff-sessions.html";
            var textTitleSite = GetArticleData(url);
        }

        private static ArticleData GetArticleData(string url)
        {

            var requestContent = "https://api.diffbot.com/v3/article?" +
                "token=642d8c5e2e5f11ca47e3128716bea5dc&url=" +
                url +
                "&fields=title,text,siteName";
            var request = (HttpWebRequest)WebRequest.Create(requestContent);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            var result = JsonConvert.DeserializeObject<dynamic>(responseString);
            var objects = result.objects[0];
            return new ArticleData(objects.title.ToString(), objects.text.ToString(), objects.siteName.ToString());
        }
    }
}