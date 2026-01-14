public abstract class Alteration {
    public virtual string Name => GetType().Name; // Alteration Name
    public virtual string Description => "No description given"; // Alteration Description
    public virtual bool Published => false; // Alteration is published in Auto Alteration -> determines if shown in UI-App
    public virtual bool LikeAN => false; // made in the same way as in Altered Nadeo
    public virtual bool Complete => false; // alteration sometimes needs manual additions

    public virtual List<Alteration> AlterationsBefore { get; } = [];
    internal virtual List<CustomBlockAlteration> customBlockAlts { get; } = [];
    internal virtual List<ArticleProvider> articleProviders { get; } = [];

    public const float PI = (float)Math.PI;
    
    protected abstract void Run(Inventory inventory, Map map);
    public void Run(Map map)
    {
        foreach (Alteration alteration in AlterationsBefore)
        {
            alteration.Run(map);
        }
        Inventory inventory = [];
        foreach (ArticleProvider provider in (List<ArticleProvider>)[AlterationConfig.VanillaArticles, .. articleProviders])
        {
            AlterationConfig.Keywords.AddRange(provider.GetAdditionalKeywords());
            inventory |= [.. provider.GetArticles()];
            foreach (CustomBlockAlteration alteration in customBlockAlts)
            {
                AlterationConfig.Keywords.Add(alteration.Name);
                inventory |= [.. provider.GetAlteredArticles(alteration)];
            }
        }
        //logging
        if (AlterationConfig.devMode) inventory.Export(Name);

        //Map specific custom blocks
        if (map.embeddedBlocks.Count != 0)
        {
            EmbeddedProvider embeddedProvider = new(map);
            inventory |= [.. embeddedProvider.GetArticles()];
            foreach (CustomBlockAlteration customBlockAlteration in customBlockAlts)
            {
                inventory |= [.. embeddedProvider.GetAlteredArticles(customBlockAlteration)];
            }
            //logging
            if (AlterationConfig.devMode) inventory.Export(Name + "WithEmbedded");
        }
        
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
