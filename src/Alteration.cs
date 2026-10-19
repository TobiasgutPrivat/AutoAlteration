public abstract class Alteration {
    public virtual string Name => GetType().Name; // Alteration Name
    public virtual string Description => "No description given"; // Alteration Description
    public virtual bool Published => false; // Alteration is published in Auto Alteration -> determines if shown in UI-App
    public virtual bool LikeAN => false; // made in the same way as in Altered Nadeo
    public virtual bool Complete => false; // alteration sometimes needs manual additions

    public virtual List<Alteration> AlterationsBefore { get; } = [];
    internal virtual List<CustomBlockAlteration> customBlockAlts { get; } = [];
    internal virtual List<ArticleProvider> customBlocks { get; } = [];

    public const float PI = (float)Math.PI;
    
    public abstract void Run(Inventory inventory, Map map);
    public void Run(Map map, List<ArticleProvider>? customBlocks = null)
    {
        ArticleProvider collectionArticleProvider = map.map.GetEnvironment() switch
        {
            "BlueBay" => AlterationConfig.BlueBayArticleProvider,
            "GreenCoast" => AlterationConfig.GreenCoastArticleProvider,
            "RedIsland" => AlterationConfig.RedIslandArticleProvider,
            "Stadium" => AlterationConfig.StadiumArticleProvider,
            "WhiteShore" => AlterationConfig.WhiteShoreArticleProvider,
            _ => throw new NotSupportedException($"Environment '{map.map.GetEnvironment()}' is not supported")
        };
        
        Inventory inventory = [];
        List<ArticleProvider> articleProviders = [collectionArticleProvider, .. this.customBlocks, .. customBlocks ?? []];
        foreach (ArticleProvider provider in articleProviders)
        {
            AlterationConfig.Keywords.AddRange(provider.GetAdditionalKeywords());
            inventory |= [.. provider.GetArticles(customBlockAlts)];
        }

        //Map specific custom blocks
        if (map.embeddedBlocks.Count != 0)
        {
            EmbeddedProvider embeddedProvider = new(map);
            Inventory embeddedArticles = [.. embeddedProvider.GetArticles()]; // TODO add customblockAlts (currently corrupting altered reembedded blocks)
            foreach (ArticleProvider articleProvider in articleProviders)
            {
                articleProvider.EmbeddedChanges(embeddedArticles);
            }
            inventory |= embeddedArticles;
        }

        //logging
        if (AlterationConfig.devMode) inventory.Export(Name);

        foreach (Alteration alteration in AlterationsBefore)
        {
            alteration.Run(inventory, map);
        }

        
        //Run alteration
        inventory.Export("temp");
        Run(inventory, map);
        AlterationConfig.mapCount++;
            
        //cleanup
        if (map.embeddedBlocks.Count != 0)
        {
            string TempFolder = Path.Join(Path.GetTempPath(), "AutoAlteration");
            if (Directory.Exists(TempFolder))
            {
                Directory.Delete(TempFolder, true);
            }
        }

    }
}
