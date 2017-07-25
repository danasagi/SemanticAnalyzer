// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

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
            //FileUtils.ReadOppositeOpinion(Consts.OppositePath);
            Dictionary<EntityKey, EntityValue> sourcesBais = new Dictionary<EntityKey, EntityValue>();
            FileUtils.ReadSourcesBais(Consts.SourcesBaisPath, sourcesBais);
            MeaningCloud.AnalyzeArticle("src", "url", Consts.t, sourcesBais);
            FileUtils.WriteSourcesBais(Consts.SourcesBaisPath, sourcesBais);
            FileUtils.WriteOppositeOpinion(Consts.OppositePath);
        }
    }
}