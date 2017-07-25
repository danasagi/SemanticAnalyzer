namespace SemanticAnalyzer
{
    public class ArticleData
    {
        public ArticleData(string title, string text, string siteName)
        {
            Title = title;
            Text = text;
            SiteName = siteName;
        }
        public string Title { get; set; }
        public string Text { get; set; }
        public string SiteName { get; set; }
    }
}