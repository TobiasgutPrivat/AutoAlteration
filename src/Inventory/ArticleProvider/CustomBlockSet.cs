class CustomBlockSet(CustomBlockAlteration customBlockAlteration) : ArticleProvider
{
    public readonly CustomBlockAlteration customBlockAlteration = customBlockAlteration;

    public override List<string> GetAdditionalKeywords() { return [GetSetName()]; }
    public virtual string GetFolder() { return Path.Combine(AlterationConfig.CacheFolder, GetSetName()); }
    public virtual string GetSetName() { return customBlockAlteration.GetType().Name; }
    public virtual string GetOrigin() { return Path.Combine(AlterationConfig.CustomBlocksFolder, "Vanilla"); }

    protected override List<Article> GenerateArticles()
    {
        if (!Directory.Exists(GetFolder()))
        {
            GenerateBlockSet();
        }
        List<Article> articles = [];

        articles.AddRange(Directory.GetFiles(GetFolder(), "*.Block.Gbx", SearchOption.AllDirectories).ToList().Select(x =>
            new Article(Path.GetFileName(x), BlockType.CustomBlock, x)));

        articles.AddRange(Directory.GetFiles(GetFolder(), "*.Item.Gbx", SearchOption.AllDirectories).ToList().Select(x =>
            new Article(Path.GetFileName(x), BlockType.CustomItem, x)));

        return articles;
    }

    public void GenerateBlockSet()
    {
        Console.WriteLine("Generating " + customBlockAlteration.GetType().Name + " block set...");
        if (!Directory.Exists(GetFolder()))
        {
            Directory.CreateDirectory(GetFolder() + "Temp");//Temp in case something goes wrong
        }
        AutoAlteration.AlterAll(customBlockAlteration, GetOrigin(), GetFolder() + "Temp", GetSetName());
        Directory.Move(GetFolder() + "Temp", GetFolder());
    }
}

class LightSurface(CustomBlockAlteration customBlockAlteration) : CustomBlockSet(customBlockAlteration) {
    public override List<string> GetAdditionalKeywords() { return [GetSetName(), customBlockAlteration.GetType().Name]; }
    public override string GetSetName() { return customBlockAlteration.GetType().Name + "Light"; }
    public override string GetOrigin() { return Path.Combine(AlterationConfig.CustomBlocksFolder, "LightSurface"); }
}

class HeavySurface(CustomBlockAlteration customBlockAlteration) : CustomBlockSet(customBlockAlteration) {
    public override List<string> GetAdditionalKeywords() { return [GetSetName(), customBlockAlteration.GetType().Name]; }
    public override string GetSetName() { return customBlockAlteration.GetType().Name + "Heavy"; }
    public override string GetOrigin() { return Path.Combine(AlterationConfig.CustomBlocksFolder, "HeavySurface"); }
}