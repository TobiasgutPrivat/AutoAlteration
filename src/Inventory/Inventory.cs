using Newtonsoft.Json;

public class Inventory { // represents all available articles which can be placed in a map
    public List<Article> articles = [];
    public Inventory() {}
    public Inventory(List<Article> articles) {this.articles = articles;}
    private Dictionary<string,Inventory> cachedInventories = [];

    public void Export(string Name) {
        Directory.CreateDirectory(Path.Combine(AlterationConfig.devPath,"Inventory"));
        File.WriteAllText(Path.Combine(AlterationConfig.devPath,"Inventory","Inventory" + Name + ".json"), JsonConvert.SerializeObject(articles));
    }
    
    public Inventory Select(string keywordFilter)  {
        if (cachedInventories.ContainsKey(keywordFilter)) {
            return cachedInventories[keywordFilter];
        }

        List<Article> result = [];
        bool[] matches = Matches(keywordFilter);
        for (int i = 0; i < articles.Count; i++){
            if (matches[i]){
                result.Add(articles[i]);
            }
        }

        Inventory resultInventory = new(result);
        cachedInventories[keywordFilter] = resultInventory;
        return resultInventory;
    }

    #region Match
    public bool[] Matches(string keywordFilter){
        int length = articles.Count;
        bool[] current = new bool[length];
        bool and = false;
        bool or = false;
        bool invert = false;
        bool[] result;
        while (keywordFilter.Length > 0) {
            switch (keywordFilter[0]) {
            case '&':
                and = true;
                keywordFilter = keywordFilter[1..];
                break;
            case '|':
                or = true;
                keywordFilter = keywordFilter[1..];
                break;
            case '!':
                invert = true;
                keywordFilter = keywordFilter[1..];
                break;
            case '(':
                result = Matches(keywordFilter[1..GetEndBracePos(keywordFilter)]);
                if (invert) {
                    result = result.Select(r => !r).ToArray();
                    invert = false;
                }
                if (and) {
                    current = current.Zip(result, (a, b) => a && b).ToArray();
                    and = false;
                } else if (or) {
                    current = current.Zip(result, (a, b) => a || b).ToArray();
                    or = false;
                } else {
                    current = result;
                }
                keywordFilter = keywordFilter[(GetEndBracePos(keywordFilter) + 1)..];
                break;
            default:
                int next = NextPos(keywordFilter);
                string keyword = keywordFilter[..next];
                result = articles.Select(a => a.HasKeyword(keyword)).ToArray();
                if (invert) {
                    result = result.Select(r => !r).ToArray();
                    invert = false;
                }
                if (and) {
                    current = current.Zip(result, (a, b) => a && b).ToArray();
                    and = false;
                } else if (or) {
                    current = current.Zip(result, (a, b) => a || b).ToArray();
                    or = false;
                } else {
                    current = result;
                }
                keywordFilter = keywordFilter[next..];
                break;
            }
        }
        return current;
    }

    private static int NextPos(string text) {
        char[] systemCharacters = ['&', '|', '!', '(', ')'];
        if(text.IndexOfAny(systemCharacters) == -1) {return text.Length;}
        return text.IndexOfAny(systemCharacters);
    }

    private static int GetEndBracePos(string text) {
        int depth = 0;
        int i = 0;
        foreach (char character in text)
        {
            if (character == '(') {
                depth++;
            }
            if (character == ')'){
                depth--;
            }
            if(depth == 0){
                return i;
            }
            i++;
        }
        return 0;
    }
    #endregion

    public Inventory Select(BlockType blockType) =>
        new(articles.Where(a => a.Type == blockType).ToList());

    public Inventory Select(Predicate<Article> condition) =>
        new(articles.Where(a => condition(a)).ToList());

    public static Inventory operator &(Inventory a, Inventory b)
    {
        return new(a.articles.Intersect(b.articles).ToList());
    }

    public static Inventory operator |(Inventory a, Inventory b)
    {
        return new(a.articles.Union(b.articles).ToList());
    }

    public static Inventory operator !(Inventory a)
    {
        return new(Alteration.inventory.articles.Except(a.articles).ToList());
    }

    public Inventory Except(Inventory inventory) =>
        new(articles.Except(inventory.articles).ToList());

    public Article GetArticle(string name) {
        List<Article> match = articles.Where(a => a.Name == name).ToList();
        if (match.Count == 0) {
            throw new Exception("No article with name: " + name);
        }
        return match.First();
    }

    public Inventory GetArticles(SList<string> names) {
        List<Article> result = [];
        foreach (string name in names) {
            List<Article> match = articles.Where(a => a.Name == name).ToList();
            if (match.Count > 0) {
                result.Add(match.First());
            }
        }
        return new Inventory(result);
    }

    public Article? AlignArticle(Article article) {
        List<Article> matchArticles = articles.Where(article.Match).ToList();
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
        return new Inventory(articles);
    }

    #region Edit
    public Inventory AddArticles(SList<Article> newArticles) {
        articles.AddRange(newArticles);
        return this;
    }

    public Inventory AddArticles(Inventory inventory) {
        articles.AddRange(inventory.articles);
        return this;
    }

    public Inventory RemoveArticles(Inventory removeInventory) {
        articles = articles.Where(a => !removeInventory.articles.Contains(a)).ToList();
        return this;
    }

    public KeywordEdit AddKeyword(SList<string> keyword) =>
        Edit().AddKeyword(keyword);
    public KeywordEdit RemoveKeyword(SList<string> keyword) =>
        Edit().RemoveKeyword(keyword);
    public KeywordEdit AddToShape(SList<string> keyword) =>
        Edit().AddToShape(keyword);
    public KeywordEdit RemoveToShape(SList<string> keyword) =>
        Edit().RemoveToShape(keyword);

    public KeywordEdit Edit(){
        List<Article> articleClone = articles.Select(a => a.CloneArticle()).ToList();
        return new KeywordEdit(articleClone);
    }

    public KeywordEdit EditOriginal() =>
        new KeywordEdit(articles);

    public void ClearSpecific() =>
        articles = articles.Where(a => !a.MapSpecific).ToList();
        
    #endregion
    
    #region Development
    public void CheckDuplicates(){
        articles.ForEach(article => {
            if (AlignArticle(article) != article) {
                Console.WriteLine("Article " + article.Name + " aligned with diffrent Article");
            }
        });
    }

    public Inventory Print() {
        articles.ForEach(article => {
            Console.WriteLine(article.Name + ": " + article.KeywordString());
        });
        return this;
    }
    #endregion
}