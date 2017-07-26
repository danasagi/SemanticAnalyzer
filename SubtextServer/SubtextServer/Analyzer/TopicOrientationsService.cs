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

    using HtmlAgilityPack;

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

        public ArticleData GetArticleData(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var uri = new Uri(url);
            string text = "";
            switch (uri.Host)
            {
                case "www.nytimes.com":
                    text = NYTimesGetArticleText(doc);
                    break;
                case "www.telegraph.co.uk":
                    text = TelegraphGetArticleText(doc);
                    break;
                case "www.theguardian.com":
                    text = GuardianGetArticleText(doc);
                    break;
            }
            return new ArticleData("", text, "");
        }
        private string GuardianGetArticleText(HtmlDocument doc)
        {
            var textNodes = doc.DocumentNode
                .SelectNodes("//p[not(@id) and not(@class)]");
            var text = "";
            foreach (var node in textNodes)
            {
                text += " " + node.InnerText;
            }
            return text;
        }
        private string TelegraphGetArticleText(HtmlDocument doc)
        {
            var textNodes = doc.DocumentNode
                .SelectNodes("//div[(@class='component-content')]/p");
            var text = "";
            for (int i = 0; i < textNodes.Count - 3; i++)
            {
                text += " " + textNodes[i].InnerText;
            }
            return text;
        }
        private string NYTimesGetArticleText(HtmlDocument doc)
        {
            var textNodes = doc.DocumentNode
                .SelectNodes("//p[contains(concat(' ', @class, ' '), ' story-content ')]");
            var text = "";
            foreach (var node in textNodes)
            {
                text += " " + node.InnerText;
            }
            return text;
        }

    }


    public class TopicAgenda
    {
        public string Name { get; set; }

        public int Agenda  { get; set; }

        public string OppositeOpinionUrl { get; set; }
    }
}
