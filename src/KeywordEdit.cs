class KeywordEdit {
    public List<Article> articles = new List<Article>();

    public KeywordEdit() {}
    public KeywordEdit(List<Article> articles) {this.articles = articles;}

    public KeywordEdit add(string Keyword) {
        articles.ForEach(a => a.keywords.Add(Keyword));
        return this;
    }

    public KeywordEdit add(string[] Keywords) {
        articles.ForEach(a => a.keywords.AddRange(Keywords));
        return this;
    }
    
    public KeywordEdit remove(string Keyword) {
        articles.ForEach(a => a.keywords.Remove(Keyword));
        return this;
    }

    public KeywordEdit remove(string[] Keywords) {
        Keywords.ToList().ForEach(k => articles.ForEach(a => a.keywords.Remove(k)));
        return this;
    }
    
    public KeywordEdit surface(string surface) {
        articles.ForEach(a => a.surface = surface);
        return this;
    }

    public KeywordEdit shape(string shape) {
        articles.ForEach(a => a.shape = shape);
        return this;
    }

    public KeywordEdit toShape(string toShape) {
        articles.ForEach(a => a.toShape = toShape);
        return this;
    }
    public KeywordEdit changePosition(Position position) {
        articles.ForEach(a => a.position.addPosition(position));
        return this;
    }
    public KeywordEdit posCorrection(PosCorection posCorection) {
        articles.ForEach(a => a.posCorection = posCorection);
        return this;
    }

    public void replace(Map map,Position position = null){
        articles.ForEach( a => {
            Article article = Alteration.inventory.alignArticle(a);
            if (article != null) {
                map.replace(a, article, position);
            } else {
                // Console.WriteLine("No matching article found for: " + string.Join(", ", string.Join(", ", a.Keywords),a.Surface,a.Shape,a.ToShape));
            }
        });
    }

    public void placeRelative(Map map,Position position = null){
        articles.ForEach( a => {
            Article article = Alteration.inventory.alignArticle(a);
            if (article != null) {
                map.placeRelative(a, article, position);
            } else {
                // Console.WriteLine("No matching article found for: " + string.Join(", ", string.Join(", ", a.Keywords),a.Surface,a.Shape,a.ToShape));
            }
        });
    }

    public Inventory align() {
        List<Article> newarticles = new List<Article>();
        articles.ForEach( a => {
            Article article = Alteration.inventory.alignArticle(a);
            if (article != null) {
                newarticles.Add(article);
            } else {
                // Console.WriteLine("No matching article found for: " + string.Join(", ", string.Join(", ", a.Keywords),a.Surface,a.Shape,a.ToShape));
            }
        });
        return new Inventory(newarticles);
    }

    public string[] names() =>
        align().articles.Select(a => a.name).ToArray();

    public KeywordEdit print() {
        articles.ForEach(article => {
            Console.WriteLine(article.name + ": " + string.Join(", ", string.Join(", ", article.keywords),article.surface,article.shape,article.toShape));
        });
        return this;
    }
}