using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace SemanticAnalyzer
{
    using System.Web;

    public class TopicOrientationsService
    {
        // GET api/values
        public IEnumerable<TopicAgenda> Get(string url, Dictionary<EntityKey, EntityValue> sourcesBais)
        {

            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            var articleData = GetArticleData(url);
            var result = MeaningCloud.AnalyzeArticle(articleData.SiteName, url, articleData.Text, sourcesBais);

            return result;
        }   

        private ArticleData GetArticleData(string url)
        {
            url = HttpUtility.UrlEncode(url);
            var requestContent = "https://api.diffbot.com/v3/article?" +
                "token=642d8c5e2e5f11ca47e3128716bea5dc&url=" +
                url +
                "&fields=title,text,siteName&timeout=50000";
            
            var request = (HttpWebRequest)WebRequest.Create(requestContent);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            var result = JsonConvert.DeserializeObject<dynamic>(responseString);
            var objects = result.objects[0];
            dynamic title = objects.title.ToString();
            var text = objects.text.ToString();
            string siteName = objects.siteName.ToString();
            if (siteName.ToLower().Contains("telegraph"))
            {
                siteName = "www.telegraph.co.uk";
            }

            return new ArticleData(title, text, siteName);
        }

    }


    public class TopicAgenda
    {
        public string Name { get; set; }

        public int Agenda  { get; set; }

        public string OppositeOpinionUrl { get; set; }
    }
}
