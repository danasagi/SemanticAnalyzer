// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
            MeaningCloud.AnalyzeArticle("src", Consts.t);
        }
    }
}