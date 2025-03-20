public class KeywordEdit {
    public List<Article> articles = [];

    public KeywordEdit(List<Article> articles) {
        this.articles = articles;
    }

    public KeywordEdit AddKeyword(SList<string> keywords) {
        articles.ForEach(a => keywords.ToList().ForEach(k => a.Keywords.Add(k)));
        return this;
    }
    
    public KeywordEdit RemoveKeyword(SList<string> keywords) {
        articles.ForEach(a => keywords.ToList().ForEach(k => a.Keywords.Remove(k)));
        return this;
    }

    public KeywordEdit AddToShape(SList<string> toShape) {
        articles.ForEach(a => toShape.ToList().ForEach(s => a.ToShapes.Add(s)));
        return this;
    }
    public KeywordEdit RemoveToShape(SList<string> toShape) {
        articles.ForEach(a => toShape.ToList().ForEach(s => a.ToShapes.Remove(s)));
        return this;
    }

    public KeywordEdit SetChain(MoveChain moveChain) {
        articles.ForEach(a => a.MoveChain = moveChain.Clone());
        return this;
    }

    public KeywordEdit AddChain(MoveChain moveChain) {
        articles.ForEach(a => a.MoveChain.AddChain(moveChain));
        return this;
    }

    public KeywordEdit Width(int width) {
        articles.ForEach(a => a.Width = width);
       
        return this;
    }
    public KeywordEdit Length(int length) {
        articles.ForEach(a => a.Length = length);
        return this;
    }

    public void Replace(Map map,MoveChain ?moveChain = null){
        articles.ForEach( a => {
            Article ?article = Alteration.inventory.AlignArticle(a);
            if (article != null) {
                map.Replace(a, article, moveChain);
            } else {
                // Console.WriteLine("No matching article found for: " + a.KeywordString());
            }
        });
    }
    public void ReplaceWithRandom(Map map,List<string> addKeywords,MoveChain ?moveChain = null){
        articles.ForEach( a => {
            map.ReplaceWithRandom(a, AlignMultiple(a, addKeywords), moveChain);
        });
    }
    public void PlaceRelativeWithRandom(Map map,List<string> addKeywords,MoveChain ?moveChain = null){
        articles.ForEach( a => {
            map.PlaceRelativeWithRandom(a, AlignMultiple(a, addKeywords), moveChain);
        });
    }

    public static Inventory AlignMultiple(Article a,List<string> addKeywords) {
        List<Article> articles = [];
        addKeywords.ForEach(k => {
            Article article = a.CloneArticle();
            article.Keywords.Add(k);
            Article ?Match = Alteration.inventory.AlignArticle(article);
            if (Match != null) {
                articles.Add(Match);
            }
        });
        return new Inventory(articles);
    }

    public void PlaceRelative(Map map,MoveChain ?moveChain = null){
        articles.ForEach( a => {
            Article ?article = Alteration.inventory.AlignArticle(a);
            if (article != null) {
                map.PlaceRelative(a, article, moveChain);
                // Console.WriteLine("Matching article found for: " + a.KeywordString());
            } else {
                // Console.WriteLine("No matching article found for: " + a.KeywordString());
            }
        });
    }

    public Inventory Align() {
        List<Article> newarticles = [];
        articles.ForEach( a => {
            Article ?article = Alteration.inventory.AlignArticle(a);
            if (article != null) {
                newarticles.Add(article);
            } else {
                // Console.WriteLine("No matching article found for: " + a.KeywordString());
            }
        });
        return new Inventory(newarticles);
    }

    public KeywordEdit Print() {
        articles.ForEach(article => {
            Console.WriteLine(article.Name + ": " + article.KeywordString());
        });
        return this;
    }
}