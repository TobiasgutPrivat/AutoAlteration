class InventoryEdit {
    public List<Article> articles = new List<Article>();

    public InventoryEdit() {}
    public InventoryEdit(List<Article> articles) {this.articles = articles;}

    public InventoryEdit add(string Keyword) {
        articles.ForEach(a => a.Keywords.Add(Keyword));
        return this;
    }
    
    public InventoryEdit remove(string Keyword) {
        articles.ForEach(a => a.Keywords.Remove(Keyword));
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

    public Inventory align() {
        List<Article> newarticles = new List<Article>();
        articles.ForEach( a => {
            List<Article> articles = Alteration.inventory.GetArticles(a.Keywords.ToArray());
            if (articles.Count > 1) {
                Console.WriteLine(a.Name + ": More than one found article with keywords: " + string.Join(", ", a.Keywords) + "\nFound Articles: " + string.Join(", ", articles.Select(a => a.Name).ToArray()));
            } else if (articles.Count == 1) {
                newarticles.Add(articles.First());
            }
        });
        return new Inventory(newarticles);
    }

    public string[] names() =>
        align().articles.Select(a => a.Name).ToArray();

}