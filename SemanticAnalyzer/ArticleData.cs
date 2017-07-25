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
        string Title { get; set; }
        string Text { get; set; }
        string SiteName { get; set; }
    }
}