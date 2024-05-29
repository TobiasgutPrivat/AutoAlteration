using Newtonsoft.Json;
class Inventory {
    public List<Article> articles = new List<Article>();

    public Inventory(List<Article> articles) {this.articles = articles;}
    public Inventory(string inventoryPath,string[] Keywords) {
        string json = File.ReadAllText(inventoryPath);
        string[] lines = JsonConvert.DeserializeObject<string[]>(json);
        articles = lines.Select(line => new Article(line.Split('/')[line.Split('/').Length-1].Trim(),Keywords)).ToList();
    }
    public Inventory select(string keywordFilter) =>
        new Inventory(GetArticles(keywordFilter));

    public void add(List<Article> articles) =>
        this.articles.AddRange(articles);
    public void add(Inventory inventory) =>
        articles.AddRange(inventory.articles);
    public string[] names() =>
        articles.Select(a => a.Name).ToArray();

    public List<Article> GetArticles(string keywordFilter) =>
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
            Console.WriteLine(article.Name + ": More than one found article with keywords: " + string.Join(", ", keywords));
            Console.WriteLine("Articles: " + string.Join(", ", newArticle.Select(a => a.Name).ToArray()));
            return null;
        }
        if (newArticle.Count() == 0) {
            Console.WriteLine(article.Name + ": No found article with keywords: " + string.Join(", ", keywords));
            return null;
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
    
    public void checkInventory(){//used for Development
        articles.ForEach(article => {
            if (GetArticles(article.Keywords.ToArray()).Where(a => a.Keywords.Count == article.Keywords.Count).Count() > 1 ) {
                Console.WriteLine(article.Name + ": More than one found article with keywords: " + string.Join(", ", article.Keywords) + "\nFound Articles: " + string.Join(", ", GetArticles(article.Keywords.ToArray()).Where(a => a.Keywords.Count == article.Keywords.Count).Select(a => a.Name).ToArray()));
            }
        });
        List<string> Keywords = File.ReadAllLines("C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/src/Configuration/Keywords.txt").ToList();
        Keywords.ForEach(k => {
            int count = 0;
            foreach (Article article in articles)
            {
                if (article.Keywords.Contains(k))
                {
                    count++;
                }
            }
            Console.WriteLine($"'{k}' count: {count}");
        });
        articles.ForEach(article => {
            article.Keywords.ForEach(keyword => {
                if (Keywords.Contains(keyword)) {
                    Keywords.RemoveAll(k => k == keyword);
                }
            });
        });
        Console.WriteLine("Keywords left: " + string.Join(", ", Keywords));
    }
}