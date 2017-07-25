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
    public class TopicOrientationsController : ApiController
    {
        // GET api/values
        public IEnumerable<TopicAgenda> Get(string url)
        {

            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            var articleData = GetArticleData(url);
            /**
             * need to remove the comment and the throw declaration when the AnalyzeArticle method return the right type
             *  |
             *  |
             *  |
             * \ /
             *  V
             */
            //return MeaningCloud.AnalyzeArticle(articleData.SiteName, articleData.Text);
            throw new AggregateException();
        }   


        private ArticleData GetArticleData(string url)
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


    public class TopicAgenda
    {
        public string Name { get; set; }

        public int Agenda  { get; set; }

        public string OppositeOpinionUrl { get; set; }
    }
}
