class KeywordEdit {
    public List<Article> articles = [];

    public KeywordEdit() {}
    public KeywordEdit(List<Article> articles) {
        this.articles = articles;
    }

    public KeywordEdit AddKeyword(string keyword) {
        if (AutoAlteration.surfaceKeywords.Contains(keyword)) {
            articles.ForEach(a => a.Surfaces.Add(keyword));
        } else if (AutoAlteration.shapeKeywords.Contains(keyword)) {
            articles.ForEach(a => a.Shapes.Add(keyword));
        } else if (AutoAlteration.Keywords.Contains(keyword)) {
            articles.ForEach(a => a.Keywords.Add(keyword));
        } else {
            Console.WriteLine("Keyword not found: " + keyword);
        }
        return this;
    }

    public KeywordEdit AddKeyword(string[] keywords) {
        keywords.ToList().ForEach(a => AddKeyword(a));
        return this;
    }
    
    public KeywordEdit RemoveKeyword(string keyword) {
        if (AutoAlteration.surfaceKeywords.Contains(keyword)) {
            articles.ForEach(a => a.Surfaces.Remove(keyword));
        } else if (AutoAlteration.shapeKeywords.Contains(keyword)) {
            articles.ForEach(a => a.Shapes.Remove(keyword));
        } else if (AutoAlteration.Keywords.Contains(keyword)) {
            articles.ForEach(a => a.Keywords.Remove(keyword));
        } else {
            Console.WriteLine("Keyword not found: " + keyword);
        }
        return this;
    }

    public KeywordEdit RemoveKeyword(string[] keywords) {
        keywords.ToList().ForEach(k => RemoveKeyword(k));
        return this;
    }

    public KeywordEdit AddToShape(string toShape) {
        articles.ForEach(a => a.ToShapes.Add(toShape));
        return this;
    }
    public KeywordEdit RemoveToShape(string toShape) {
        articles.ForEach(a => a.ToShapes.Remove(toShape));
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

    public string[] Names() =>
        Align().articles.Select(a => a.Name).ToArray();

    public KeywordEdit Print() {
        articles.ForEach(article => {
            Console.WriteLine(article.Name + ": " + article.KeywordString());
        });
        return this;
    }
}