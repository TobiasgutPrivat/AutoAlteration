abstract class ArticleProvider
{
    private List<Article>? articles = null;
    public List<Article> GetArticles()
    {
        articles ??= GenerateArticles();
        return articles;
    }

    protected abstract List<Article> GenerateArticles();
    public virtual List<string> GetAdditionalKeywords() { return []; }
}