using Newtonsoft.Json;

public class Inventory : HashSet<Article> { // represents all available articles which can be placed in a map
    public Inventory() : base() { }
    public Inventory(IEnumerable<Article> collection) : base(collection) { }
    public Dictionary<string,Inventory> cachedInventories = [];

    public Inventory Select(BlockType blockType) =>
        [.. this.Where(a => a.Type == blockType)];

    public Inventory Select(Predicate<Article> condition) =>
        [.. this.Where(a => condition(a))];
    public Inventory Any(List<string> keywords) =>
        [.. this.Where(a => keywords.Any(k => a.Keywords.Contains(k)))];

    public Inventory Select(string keyword) =>
        [.. this.Where(a => a.Keywords.Contains(keyword))];

    public Inventory Select(List<string> keywords) =>
        [.. this.Where(a => keywords.All(k => a.Keywords.Contains(k)))];

    public Inventory Not(string keyword) =>
        [.. this.Where(a => !a.Keywords.Contains(keyword))];
    public Inventory Not(List<string> keywords) =>
        [.. this.Where(a => !keywords.Any(k => a.Keywords.Contains(k)))];

    public static Inventory operator &(Inventory a, Inventory b) =>
        [.. a.Intersect(b)];

    public static Inventory operator |(Inventory a, Inventory b) =>
        [.. a.Union(b)];

    public static Inventory operator /(Inventory a, Inventory b) =>
        [.. a.Except(b)];


    public Article GetArticle(string name) {
        if (name.Contains(':')) {
            name = name.Split(':')[1]; //in case the name is given as club:Name
        }
        IEnumerable<Article> match = this.Where(a => a.Name == name);
        if (match.Count() == 0) {
            throw new Exception("No article with name: " + name);
        }
        return match.First();
    }

    public Inventory GetArticles(SList<string> names) {
        List<Article> result = [];
        IEnumerable<Article> articles = this.Where(a => names.Contains(a.Name));
        // take the first match for each name (might have duplicates)
        foreach (string name in names) {
            IEnumerable<Article> match = articles.Where(a => a.Name == name).ToList();
            if (match.Any()) {
                result.Add(match.First());
            }
        }
        return [.. result];
    }

    public Article? AlignArticle(Article article) {
        List<Article> matchArticles = this.Where(article.Match).ToList();
        if (matchArticles.Count > 1) {
            Console.WriteLine("More than one found article with keywords: " + article.KeywordString() + "\nFound Articles: " + string.Join(", ", matchArticles.Select(a => a.Name).ToArray()));
            return null;
        } else if (matchArticles.Count == 1) {
            return matchArticles.First();
        }
        return null;
    }

    public Inventory AlignMultiple(Article article, List<string> addKeywords) {
        List<Article> articles = [];
        addKeywords.ForEach(k => {
            Article clonedArticle = article.CloneArticle();
            clonedArticle.Keywords.Add(k);
            Article ?Match = AlignArticle(clonedArticle);
            if (Match != null) {
                articles.Add(Match);
            }
        });
        return [.. articles];
    }

    public KeywordEdit Edit(){
        List<Article> articleClone = this.Select(a => a.CloneArticle()).ToList();
        return new KeywordEdit(articleClone);
    }

    #region Development
    public void Export(string Name) {
        Directory.CreateDirectory(Path.Combine(AlterationConfig.devPath, "Inventory"));
        File.WriteAllText(Path.Combine(AlterationConfig.devPath, "Inventory", "Inventory" + Name + ".json"), JsonConvert.SerializeObject(this, Formatting.Indented));
    }
    
    public Inventory Print() {
        this.ToList().ForEach(article => {
            Console.WriteLine(article.Name + ": " + article.KeywordString());
        });
        return this;
    }
    #endregion
}