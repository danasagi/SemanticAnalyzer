using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SemanticAnalyzer.Readers
{
    public class ArticlesParser
    {
        public string CleanUnicodeContent(string unicodeString)
        {
            // Convert the string into a byte[].
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(unicodeString);

            // Perform the conversion from one encoding to the other.
            byte[] asciiBytes = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, unicodeBytes);

            // Convert the new byte[] into a char[] and then into a string.
            // This is a slightly different approach to converting to illustrate
            // the use of GetCharCount/GetChars.
            char[] asciiChars = new char[Encoding.ASCII.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            Encoding.ASCII.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
            string asciiString = new string(asciiChars);

            return asciiString;
        }

        public void parse_line(string line)
        {
            var article = JsonConvert.DeserializeObject<SingleArticle>(line);
            string cleanArticle = CleanUnicodeContent(article.content);

            // Source, url, text
            MeaningCloud.AnalyzeArticle(cleanArticle);
        }

        public void parse_file(string path)
        {
            foreach (string line in File.ReadLines(path))
            {
                parse_line(line);
            }
        }
    }
}
