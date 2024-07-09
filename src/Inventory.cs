using Newtonsoft.Json;
class Inventory {
    public List<Article> articles = new List<Article>();

    public Inventory() {}
    public Inventory(List<Article> articles) {this.articles = articles;}
    
    public Inventory Select(string keywordFilter) =>
        new(GetArticles(keywordFilter));
    public Inventory Select(BlockType blockType) =>
        new(articles.Where(a => a.Type == blockType).ToList());

    public Inventory SelectString(string nameString) =>
        new(articles.Where(a => a.Name.Contains(nameString)).ToList());

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

    public Article? AlignArticle(Article article) {
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
    public KeywordEdit AddSurface(string keyword) =>
        Edit().AddSurface(keyword);
    public KeywordEdit RemoveSurface(string keyword) =>
        Edit().RemoveSurface(keyword);
    public KeywordEdit AddShape(string keyword) =>
        Edit().AddShape(keyword);
    public KeywordEdit RemoveShape(string keyword) =>
        Edit().RemoveShape(keyword);
    public KeywordEdit AddToShape(string keyword) =>
        Edit().AddToShape(keyword);
    public KeywordEdit RemoveToShape(string keyword) =>
        Edit().RemoveToShape(keyword);
    public KeywordEdit ChangePosition(Position position) =>
        Edit().ChangePosition(position);
    public KeywordEdit Width(int width) =>
        Edit().Width(width);
    public KeywordEdit Length(int length) =>
        Edit().Length(length);

    public KeywordEdit Edit(){
        List<Article> articleClone = articles.Select(a => a.CloneArticle()).ToList();
        return new KeywordEdit(articleClone);
    }

    //Development Section ------------------------------------------------------------------------------------------------------
    public Inventory AddArticles(List<Article> newArticles) {
        if (!Alteration.devMode){
            Console.WriteLine("Adding Articles only available in devMode");
        } else {
            articles.AddRange(newArticles);
        }
        return this;
    }
    public Inventory AddArticles(KeywordEdit keywordEdit) {
        if (!Alteration.devMode){
            Console.WriteLine("Adding Articles only available in devMode");
        } else {
            articles.AddRange(keywordEdit.articles);
        }
        return this;
    }
    public KeywordEdit EditOriginal(){
        if (!Alteration.devMode){
            Console.WriteLine("Edit Original only available in devMode");
            return null;
        } else {
            articles.ForEach(x => x.cacheFilter.Clear());
            return new KeywordEdit(articles);
        }
    }

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