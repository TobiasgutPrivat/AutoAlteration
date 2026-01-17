// base implementation designed to provide articles from a specific customblocks folder
public class ArticleProvider(string? subFolder = null)
{
    protected virtual string GetOrigin() { return Path.Combine(AlterationConfig.CustomBlocksFolder, subFolder!); }
    private Dictionary<string, List<Article>> articleDict = [];
    public List<Article> GetArticles(List<CustomBlockAlteration>? customBlockAlterations = null) //TODO maybe merge with GetAlteredArticles?
    {
        customBlockAlterations ??= [];
        string key;
        if (customBlockAlterations.Count == 0) {
            key = "";
        } else {
            key = customBlockAlterations?.Select(x => x.GetType().Name).Aggregate((a, b) => a + ";" + b) ?? "";
        }

        if (articleDict.TryGetValue(key, out List<Article>? articles) && articles != null) {
            return articles;
        } else {
            articles = GenerateArticles();
            foreach (CustomBlockAlteration customBlockAlteration in customBlockAlterations!) {
                articles.AddRange(GetAlteredArticles(customBlockAlteration));
            }
            InventoryChanges([.. articles]);
            articleDict[key] = articles;
            return articles;
        } 
    }

    protected virtual List<Article> GenerateArticles() {
        string folder = GetOrigin();
        if (!Directory.Exists(folder))
        {
            return [];
        }
        List<Article> articles = [];
        articles.AddRange(Directory.GetFiles(folder, "*.Block.Gbx", SearchOption.AllDirectories).ToList().Select(x =>
            new Article(x.Replace(folder + "\\", ""), BlockType.CustomBlock, x)));

        articles.AddRange(Directory.GetFiles(folder, "*.Item.Gbx", SearchOption.AllDirectories).ToList().Select(x =>
            new Article(x.Replace(folder + "\\", ""), BlockType.CustomItem, x)));

        return articles;
    }

    // base implementation alters and returns Customblocks from origin folder
    protected virtual List<Article> GetAlteredArticles(CustomBlockAlteration customBlockAlteration)
    {
        string Name = customBlockAlteration.GetType().Name;
        string folder = Path.Combine(AlterationConfig.CacheFolder, subFolder! , Name);
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
        List<Article> alteredArticles = [];

        alteredArticles.AddRange(Directory.GetFiles(folder, "*.Block.Gbx", SearchOption.AllDirectories).ToList().Select(x =>
            new Article(x.Replace(folder + "\\", ""), BlockType.CustomBlock, x)));

        alteredArticles.AddRange(Directory.GetFiles(folder, "*.Item.Gbx", SearchOption.AllDirectories).ToList().Select(x =>
            new Article(x.Replace(folder + "\\", ""), BlockType.CustomItem, x)));

        // InventoryChanges(alteredArticles);
        return alteredArticles;
    }

    protected virtual void InventoryChanges(Inventory inventory) { }
    public virtual void EmbeddedChanges(Inventory inventory) { }
    public virtual List<string> GetAdditionalKeywords() { return []; }
}