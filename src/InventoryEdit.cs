class InventoryEdit {
    public List<Article> articles = new List<Article>();

    public InventoryEdit() {}
    public InventoryEdit(List<Article> articles) {this.articles = articles;}

    public InventoryEdit add(string Keyword) {
        articles.ForEach(a => a.Keywords.Add(Keyword));
        return this;
    }

    public InventoryEdit add(string[] Keywords) {
        articles.ForEach(a => a.Keywords.AddRange(Keywords));
        return this;
    }
    
    public InventoryEdit remove(string Keyword) {
        articles.ForEach(a => a.Keywords.Remove(Keyword));
        return this;
    }

    public InventoryEdit remove(string[] Keywords) {
        Keywords.ToList().ForEach(k => articles.ForEach(a => a.Keywords.Remove(k)));
        return this;
    }
    
    public InventoryEdit surface(string surface) {
        articles.ForEach(a => a.Surface = surface);
        return this;
    }

    public InventoryEdit shape(string shape) {
        articles.ForEach(a => a.Shape = shape);
        return this;
    }

    public InventoryEdit toShape(string toShape) {
        articles.ForEach(a => a.ToShape = toShape);
        return this;
    }
    public InventoryEdit blockChange(BlockChange blockChange) {
        articles.ForEach(a => a.blockChange = blockChange);
        return this;
    }

    public void replace(Map map,BlockChange blockChange = null){
        articles.ForEach( a => {
            Article article = Alteration.inventory.alignArticle(a);
            if (article != null) {
                map.replace(a, article, blockChange);
            } else {
                // Console.WriteLine("No matching article found for: " + string.Join(", ", string.Join(", ", a.Keywords),a.Surface,a.Shape,a.ToShape));
            }
        });
    }

    public void placeRelative(Map map,BlockChange blockChange = null){
        articles.ForEach( a => {
            Article article = Alteration.inventory.alignArticle(a);
            if (article != null) {
                map.placeRelative(a, article, blockChange);
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
        align().articles.Select(a => a.Name).ToArray();

    public InventoryEdit print() {
        articles.ForEach(article => {
            Console.WriteLine(article.Name + ": " + string.Join(", ", string.Join(", ", article.Keywords),article.Surface,article.Shape,article.ToShape));
        });
        return this;
    }
}