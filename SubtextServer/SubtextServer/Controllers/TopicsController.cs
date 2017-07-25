using System.Collections.Generic;
using System.Web.Http;

namespace SubtextServer.Controllers
{
    using System.IO;
    using System.Net;

    public class TopicOrientationsController : ApiController
    {
        // GET api/values
        public IEnumerable<TopicOrientation> Get(string url)
        {   
            //Fill this Mock 
            //----------------------------------------------------------------
            return new[]
            { 
                new TopicOrientation { Orientation = 31.3, Name = "Trump" },
                new TopicOrientation { Orientation = 77.4, Name = "Hillary" }
            };
            //----------------------------------------------------------------
        }
    }

    public class TopicOrientation
    {
        public string Name { get; set; } 
        public double Orientation { get; set; } 
    }
}
