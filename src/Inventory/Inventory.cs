using Newtonsoft.Json;
using SharpLzo;
public class Inventory {
    public List<Article> articles = [];
    public Inventory() {}
    public Inventory(List<Article> articles) {this.articles = articles;}

    public void Export(string Name) {
        Directory.CreateDirectory(Path.Combine(AutoAlteration.devPath,"Inventory"));
        File.WriteAllText(Path.Combine(AutoAlteration.devPath,"Inventory","Inventory" + Name + ".json"), JsonConvert.SerializeObject(articles));
    }
    
    public Inventory Select(string keywordFilter) =>
        new(SelectArticles(keywordFilter));
    public Inventory Select(BlockType blockType) =>
        new(articles.Where(a => a.Type == blockType).ToList());

    public Inventory SelectString(string nameString) =>
        new(articles.Where(a => a.Name.Contains(nameString)).ToList());

    public Inventory Add(Inventory inventory) =>
        new(articles.Concat(inventory.articles).ToList());
        
    public Inventory Sub(Inventory inventory) =>
        new(articles.Except(inventory.articles).ToList());

    public Article GetArticle(string name) {
        List<Article> match = articles.Where(a => a.Name == name).ToList();
        if (match.Count == 0) {
            Console.WriteLine("No article with name: " + name);
            return null;
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

    public List<Article> SelectArticles(string keywordFilter) =>
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
            Console.WriteLine(article.Name + ": " + article.KeywordString());
        });
        return this;
    }
}