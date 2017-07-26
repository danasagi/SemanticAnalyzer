using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SubtextServer.Startup))]

namespace SubtextServer
{
    using Resources;
    using SemanticAnalyzer;

    public partial class Startup
    {
        public static Dictionary<EntityKey, EntityValue> SourceDic = new Dictionary<EntityKey, EntityValue>();
        public void Configuration(IAppBuilder app)
        {
            if (!SourceDic.Keys.Any())
            {
                FileUtils.ReadSourcesBais(SubtextResources.SourcesBaisFile, SourceDic);
            }

            ConfigureAuth(app);
        }
    }
}
