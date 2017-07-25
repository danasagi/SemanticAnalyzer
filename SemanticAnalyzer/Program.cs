// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using SemanticAnalyzer.Readers;

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
            // Read local DB
            /*Dictionary<EntityKey, EntityValue> sourcesBais = new Dictionary<EntityKey, EntityValue>();
            FileUtils.ReadSourcesBais(Consts.SourcesBaisPath, sourcesBais);

            // Go over news datasets
            var parser = new ArticlesParser();
            DirectoryInfo d = new DirectoryInfo(Consts.DatasetsFolder);
            foreach (var file in d.GetFiles())
            {
                parser.parse_file(file.FullName);
            }
            
            // Write local DB
            FileUtils.WriteSourcesBais(Consts.SourcesBaisPath, sourcesBais);*/

            Dictionary<EntityKey, EntityValue> sourcesBais = new Dictionary<EntityKey, EntityValue>();
            FileUtils.ReadSourcesBais(Consts.SourcesBaisPath, sourcesBais);
            MeaningCloud.AnalyzeArticle("NYTimes", "bing.com", Consts.t, sourcesBais);
            FileUtils.WriteSourcesBais(Consts.SourcesBaisPath, sourcesBais);
        }
    }
}