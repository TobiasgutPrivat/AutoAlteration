class ArticleProvider(string? subFolder = null)
{
    protected virtual string GetOrigin() { return Path.Combine(AlterationConfig.CustomBlocksFolder, subFolder ?? ""); }
    private List<Article>? articles = null;
    public List<Article> GetArticles()
    {
        articles ??= GenerateArticles();
        return articles;
    }

    public virtual List<Article> GetAlteredArticles(CustomBlockAlteration customBlockAlteration)
    {
        string Name = customBlockAlteration.GetType().Name;
        string folder = Path.Combine(AlterationConfig.CacheFolder, Name);
        if (!Directory.Exists(folder))
        {
            Console.WriteLine("Generating " + customBlockAlteration.GetType().Name + " block set...");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder + "Temp");//Tempfolder used in case something goes wrong
            }
            AutoAlteration.AlterAll(customBlockAlteration, GetOrigin(), folder + "Temp", Name);
            Directory.Move(folder + "Temp", folder);
        }
        List<Article> articles = [];

        articles.AddRange(Directory.GetFiles(folder, "*.Block.Gbx", SearchOption.AllDirectories).ToList().Select(x =>
            new Article(Path.GetFileName(x), BlockType.CustomBlock, x)));

        articles.AddRange(Directory.GetFiles(folder, "*.Item.Gbx", SearchOption.AllDirectories).ToList().Select(x =>
            new Article(Path.GetFileName(x), BlockType.CustomItem, x)));
        InventoryChanges([.. articles]);
        return articles;
    }

    protected virtual void InventoryChanges(Inventory inventory) { }
    protected virtual List<Article> GenerateArticles() {
        string folder = GetOrigin();
        if (!Directory.Exists(folder))
        {
            return [];
        }
        List<Article> articles = [];
        articles.AddRange(Directory.GetFiles(folder, "*.Block.Gbx", SearchOption.AllDirectories).ToList().Select(x =>
            new Article(x.Replace(folder, ""), BlockType.CustomBlock, x)));

        articles.AddRange(Directory.GetFiles(folder, "*.Item.Gbx", SearchOption.AllDirectories).ToList().Select(x =>
            new Article(x.Replace(folder, ""), BlockType.CustomItem, x)));

        return articles;
    }
    public virtual List<string> GetAdditionalKeywords() { return []; }
}