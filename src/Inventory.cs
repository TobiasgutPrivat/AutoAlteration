using Newtonsoft.Json;
class Inventory {
    public List<Article> articles = new List<Article>();

    public Inventory(string inventoryPath,string[] Keywords) {
        string json = File.ReadAllText(inventoryPath);
        string[] lines = JsonConvert.DeserializeObject<string[]>(json);
        articles = lines.Select(line => new Article(line.Trim(),Keywords)).ToList();
    }
    public void addInventory(string inventoryPath,string[] Keywords) {
        string json = File.ReadAllText(inventoryPath);
        string[] lines = JsonConvert.DeserializeObject<string[]>(json);
        articles.Add(lines.Select(line => new Article(line.Trim(),Keywords)).ToList());
    }

    public List<Article> GetArticlesWithKeywords(string[] keywords) {
        List<Article> articlesWithAllKeywords = new List<Article>();
        foreach (var article in articles) {
            bool hasAllKeywords = true;
            foreach (var keyword in keywords) {
                if (keyword == "Dirt" && article.Name == "OpenTechRoadEndSlope2Straight") {
                    Console.WriteLine(!article.Keywords.Any(k => k == keyword));
                }
                if (!article.Keywords.Any(k => k == keyword)) {
                    hasAllKeywords = false;
                    break;
                }
                if (keyword == "Dirt" && article.Name == "OpenTechRoadEndSlope2Straight") {
                    article.Keywords.Select(k => k).ToList().ForEach(Console.WriteLine);
                }
            }
            if (hasAllKeywords) {
                articlesWithAllKeywords.Add(article);
            }
        }
        return articlesWithAllKeywords;
    }

    public Article GetExactArticleWithKeywords(string[] keywords) {
        foreach (var article in articles) {
            bool hasAllKeywords = true;
            foreach (var keyword in keywords) {
                if (!article.Keywords.Any(k => k == keyword)) {
                    hasAllKeywords = false;
                    break;
                }
            }
            int totalLength = keywords.Aggregate(0, (sum, keyword) => sum + keyword.Length);
            if (hasAllKeywords && totalLength == article.Name.Length) {
                return article;
            }
        }
        return null;
    }
    
    public List<Article> GetArticlesWithKeywords(List<string> keywords) {
        return GetArticlesWithKeywords(keywords.Select(k => k).ToArray());
    }
    public Article GetExactArticleWithKeywords(List<string> keywords) {
        return GetExactArticleWithKeywords(keywords.Select(k => k).ToArray());
    }

    public Article ArticleReplaceKeyword(Article article, string oldKeyword, string newKeyword) {
        List<string> keywords = article.Keywords.ToList();
        keywords.RemoveAll(k => k == oldKeyword);
        keywords.Add(newKeyword);
        Article newarticle = GetExactArticleWithKeywords(keywords);
        return newarticle;
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