using System.Collections.Generic;
using System.Web.Http;

namespace SubtextServer.Controllers
{
    using System.IO;
    using System.Net;
    using SemanticAnalyzer;

    public class TopicOrientationsController : ApiController
    {
        // GET api/values
        public IEnumerable<TopicAgenda> Get(string url)
        {
            var topicAgendas = new TopicOrientationsService().Get(url, Startup.SourceDic);
            return topicAgendas;
        }
    }
}
