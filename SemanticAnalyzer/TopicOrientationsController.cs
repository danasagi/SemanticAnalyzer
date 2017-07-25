using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

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
            string articleText = GetArticleText(url);
            NewsPaper paperName = GetNewsPaper();
            
            
            /**
             * need to remove the comment and the throw declaration when the AnalyzeArticle method return the right type
             *  |
             *  |
             *  |
             * \ /
             *  V
             */
            //return MeaningCloud.AnalyzeArticle(articleText);
            throw new NotImplementedException();
        }

        private NewsPaper GetNewsPaper()
        {
            throw new NotImplementedException();
        }

        private string GetArticleText(string url)
        {
            throw new NotImplementedException();
        }
    }


    public class TopicAgenda
    {
        public string Name { get; set; }
        public int Ogenda  { get; set; }
    }



    public enum NewsPaper
    {
        NewYorkTimes,
        Telegraph,
        Gardian,
        WallStreetJournal,
        Other,
    };
}
