class Inventory {
    public Article[] articles;
    string[] Keywords;

    public Inventory(string inventoryPath,string keywordsPath) {
        string[] lines = File.ReadAllLines(inventoryPath);
        this.Keywords = File.ReadAllLines(keywordsPath);
        Array.Sort(this.Keywords, (a, b) => b.Length.CompareTo(a.Length));
        articles = lines.Select(line => new Article(line.Trim(),this.Keywords)).ToArray();
    }

    public Article[] GetArticlesWithAllKeywords(string[] keywords) {
        List<Article> articlesWithAllKeywords = new List<Article>();
        foreach (var article in articles) {
            bool hasAllKeywords = true;
            foreach (var keyword in keywords) {
                if (!article.Keywords.Any(k => k.Name == keyword)) {
                    hasAllKeywords = false;
                    break;
                }
            }
            if (hasAllKeywords) {
                articlesWithAllKeywords.Add(article);
            }
        }
        Console.WriteLine($"{articlesWithAllKeywords.Count} articles with all keywords");
        return articlesWithAllKeywords.ToArray();
    }
}