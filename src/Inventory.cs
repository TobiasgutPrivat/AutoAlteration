using Newtonsoft.Json;
class Inventory {
    public List<Article> articles = new List<Article>();

    public Inventory(List<Article> articles) {this.articles = articles;}
    public Inventory(string inventoryPath,string[] Keywords) {
        string json = File.ReadAllText(inventoryPath);
        string[] lines = JsonConvert.DeserializeObject<string[]>(json);
        articles = lines.Select(line => new Article(line.Trim(),Keywords)).ToList();
    }
    public Inventory select(string keywordFilter) =>
        new Inventory(GetArticles(keywordFilter));

    public void add(List<Article> articles) =>
        this.articles.AddRange(articles);
    public void add(Inventory inventory) =>
        articles.AddRange(inventory.articles);
    public string[] names() =>
        articles.Select(a => a.Name).ToArray();

    public List<Article> GetArticles(string keywordFilter) => //TODO bad performance
        articles.Where(a => a.match(keywordFilter)).ToList();

    public List<Article> GetArticles(string[] keywords) =>
        articles.Where(a => keywords.All(k => a.Keywords.Contains(k))).ToList();

    public Article ArticleReplaceKeyword(Article article, string addKeyword, string removeKeyword) =>
        ArticleReplaceKeyword(article, new[] {addKeyword}, new[] {removeKeyword});
    
    public Article ArticleReplaceKeyword(Article article, string[] addKeywords, string[] removeKeywords) {
        List<string> keywords = article.Keywords.ToList();
        keywords.AddRange(addKeywords);
        keywords.RemoveAll(k => removeKeywords.Contains(k));
        List<Article> newArticle = GetArticles(keywords.ToArray());
        if (newArticle.Count() > 1) {
            Console.WriteLine("ArticleReplaceKeyword: More than one found article with keywords: " + keywords + " -> " + newArticle.Select(a => a.Name).ToArray());
        }
        if (newArticle.Count() == 0) {
            Console.WriteLine("ArticleReplaceKeyword: No found article with keywords: " + keywords);
        }
        return newArticle.First();
    }
    public List<Article> ArticleReplaceKeyword(List<Article> articles, string oldKeyword, string newKeyword) {
        List<Article> newArticles = new List<Article>();
        foreach (var article in articles) {
            Article newarticle = ArticleReplaceKeyword(article, oldKeyword, newKeyword);
            if (newarticle != null)
            {
                newArticles.Add(newarticle);
            }
        }
        return newArticles;
    }

    public bool hasArticle(string name) {
        return articles.Any(article => article.Name == name);
    }
}