using Newtonsoft.Json;
public class Inventory {
    public List<Article> articles = [];
    public Inventory() {}
    public Inventory(List<Article> articles) {this.articles = articles;}

    public void Export(string Name) {
        Directory.CreateDirectory(Path.Combine(AutoAlteration.devPath,"Inventory"));
        File.WriteAllText(Path.Combine(AutoAlteration.devPath,"Inventory","Inventory" + Name + ".json"), JsonConvert.SerializeObject(articles));
    }
    
    public Inventory Select(string keywordFilter) =>
        new(GetArticles(keywordFilter));
    public Inventory Select(BlockType blockType) =>
        new(articles.Where(a => a.Type == blockType).ToList());

    public Inventory SelectString(string nameString) =>
        new(articles.Where(a => a.Name.Contains(nameString)).ToList());

    public Inventory Add(Inventory inventory) =>
        new(articles.Concat(inventory.articles).ToList());
        
    public Inventory Sub(Inventory inventory) =>
        new(articles.Except(inventory.articles).ToList());

    public string[] Names() =>
        articles.Select(a => a.Name).ToArray();

    public Article GetArticle(string name) {
        List<Article> match = articles.Where(a => a.Name == name).ToList();
        if (match.Count == 0) {
            Console.WriteLine("No article with name: " + name);
            return null;
        }
        return match.First();
    }

    public List<Article> GetArticles(string keywordFilter) =>
        articles.Where(a => a.Match(keywordFilter)).ToList();

    public Article? AlignArticle(Article article) {//TODO make Cache
        List<Article> matchArticles = articles.Where(a => article.Match(a)).ToList();
        if (matchArticles.Count > 1) {
            Console.WriteLine("More than one found article with keywords: " + article.KeywordString() + "\nFound Articles: " + string.Join(", ", matchArticles.Select(a => a.Name).ToArray()));
            return null;
        } else if (matchArticles.Count == 1) {
            return matchArticles.First();
        }
        return null;
    }

    public KeywordEdit AddKeyword(string keyword) =>
        Edit().AddKeyword(keyword);
    public KeywordEdit AddKeyword(string[] keyword) =>
        Edit().AddKeyword(keyword);
    public KeywordEdit RemoveKeyword(string keyword) =>
        Edit().RemoveKeyword(keyword);
    public KeywordEdit RemoveKeyword(string[] keyword) =>
        Edit().RemoveKeyword(keyword);
    public KeywordEdit AddToShape(string keyword) =>
        Edit().AddToShape(keyword);
    public KeywordEdit RemoveToShape(string keyword) =>
        Edit().RemoveToShape(keyword);
    public KeywordEdit SetChain(MoveChain moveChain) =>
        Edit().SetChain(moveChain);
    public KeywordEdit AddChain(MoveChain moveChain) =>
        Edit().AddChain(moveChain);
    public KeywordEdit Width(int width) =>
        Edit().Width(width);
    public KeywordEdit Length(int length) =>
        Edit().Length(length);

    public KeywordEdit Edit(){
        List<Article> articleClone = articles.Select(a => a.CloneArticle()).ToList();
        return new KeywordEdit(articleClone);
    }

    public Inventory AddArticles(List<Article> newArticles) {
        articles.AddRange(newArticles);
        return this;
    }
    public Inventory AddArticles(Article newArticle) {
        articles.Add(newArticle);
        return this;
    }
    public Inventory AddArticles(KeywordEdit inventory) =>
        AddArticles(inventory.articles);

    public Inventory RemoveArticles(Inventory removeInventory) {
        articles = articles.Where(a => !removeInventory.articles.Contains(a)).ToList();
        return this;
    }

    public KeywordEdit EditOriginal(){
        articles.ForEach(x => x.cacheFilter.Clear());
        return new KeywordEdit(articles);
    }
    //Development Section ------------------------------------------------------------------------------------------------------

    public void CheckDuplicates(){
        articles.ForEach(article => {
            if (AlignArticle(article) != article) {
                Console.WriteLine("Article " + article.Name + " aligned with diffrent Article");
            }
        });
    }

    public Inventory Print() {
        articles.ForEach(article => {
            Console.WriteLine(article.Name + ": " + string.Join(", ", string.Join(", ", article.Keywords),string.Join(", ", article.Surfaces),string.Join(", ", article.Shapes),string.Join(", ", article.ToShapes)));
        });
        return this;
    }
}