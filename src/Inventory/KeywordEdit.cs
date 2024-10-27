public class KeywordEdit {
    public List<Article> articles = [];

    public KeywordEdit(List<Article> articles) {
        this.articles = articles;
    }

    public KeywordEdit AddKeyword(string keyword) {
        // if (AutoAlteration.surfaceKeywords.Contains(keyword)) {
        //     articles.ForEach(a => a.Surfaces.Add(keyword));
        // } else if (AutoAlteration.shapeKeywords.Contains(keyword)) {
        //     articles.ForEach(a => a.Shapes.Add(keyword));
        if (AutoAlteration.Keywords.Contains(keyword)) {
            articles.ForEach(a => a.Keywords.Add(keyword));
        } else {
            Console.WriteLine("Keyword not found: " + keyword);
        }
        return this;
    }

    public KeywordEdit AddKeyword(List<string> keywords) {
        keywords.ToList().ForEach(a => AddKeyword(a));
        return this;
    }
    
    public KeywordEdit RemoveKeyword(string keyword) {
        // if (AutoAlteration.surfaceKeywords.Contains(keyword)) {
        //     articles.ForEach(a => a.Surfaces.Remove(keyword));
        // } else if (AutoAlteration.shapeKeywords.Contains(keyword)) {
        //     articles.ForEach(a => a.Shapes.Remove(keyword));
        if (AutoAlteration.Keywords.Contains(keyword)) {
            articles.ForEach(a => a.Keywords.Remove(keyword));
        } else {
            Console.WriteLine("Keyword not found: " + keyword);
        }
        return this;
    }

    public KeywordEdit RemoveKeyword(List<string> keywords) {
        articles.ForEach(a => keywords.ToList().ForEach(k => a.Keywords.Remove(k)));
        keywords.ToList().ForEach(k => RemoveKeyword(k));//TODO Fix loop
        return this;
    }

    public KeywordEdit AddToShape(List<string> toShape) {
        articles.ForEach(a => toShape.ForEach(s => a.ToShapes.Add(s)));
        return this;
    }
    public KeywordEdit RemoveToShape(List<string> toShape) {
        articles.ForEach(a => toShape.ForEach(s => a.ToShapes.Remove(s)));
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