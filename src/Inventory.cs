using Newtonsoft.Json;
class Inventory {
    public List<Article> articles = new List<Article>();

    public Inventory(List<Article> articles) {this.articles = articles;}
    public Inventory(string inventoryPath,string[] Keywords) {
        string json = File.ReadAllText(inventoryPath);
        string[] lines = JsonConvert.DeserializeObject<string[]>(json);
        articles = lines.Select(line => new Article(line.Trim(),Keywords)).ToList();
    }
    public Inventory select(string[] Keywords) =>
        new Inventory(GetArticles(Keywords));
    public Inventory select(string Keyword) =>
        new Inventory(GetArticles(Keyword));
    public Inventory select(string[] Keywords,string[] excludeKeywords) =>
        new Inventory(GetArticles(Keywords,excludeKeywords));
    public Inventory select(string Keyword,string[] excludeKeywords) =>
        new Inventory(GetArticles(Keyword,excludeKeywords));

    public void add(List<Article> articles) =>
        this.articles.Concat(articles).ToList();
    public void add(Inventory inventory) =>
        this.articles.Concat(inventory.articles).ToList();

    public List<Article> GetArticles(string[] keywords) =>
        articles.Where(a => keywords.All(k => a.Keywords.Contains(k))).ToList();

    public List<Article> GetArticles(string[] keywords,string[] excludeKeywords) =>
        GetArticles(keywords).Where(a => !excludeKeywords.Any(k => a.Keywords.Contains(k))).ToList();
    public List<Article> GetArticles(string keyword) =>
        articles.Where(a => a.Keywords.Contains(keyword)).ToList();

    public List<Article> GetArticles(string keyword,string[] excludeKeywords) =>
        GetArticles(keyword).Where(a => !excludeKeywords.Any(k => a.Keywords.Contains(k))).ToList();

    public Article ArticleReplaceKeyword(Article article, string oldKeyword, string newKeyword) {
        Console.WriteLine("Replace " + oldKeyword + " with " + newKeyword + " in " + article.Name);
        List<string> keywords = article.Keywords.ToList();
        keywords.RemoveAll(k => k == oldKeyword);
        keywords.Add(newKeyword);
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