public abstract class Alteration {
    protected abstract void Run(Inventory inventory, Map map);
    public void Run(Map map)
    {
        foreach (Alteration alteration in AlterationsBefore)
        {
            alteration.Run(map);
        }
        Inventory inventory = [.. AlterationConfig.VanillaArticles.GetArticles()];
        foreach (ArticleProvider articleProvider in additionalArticles)
        {
            AlterationConfig.Keywords.AddRange(articleProvider.GetAdditionalKeywords());
            inventory &= [.. articleProvider.GetArticles()];
        }
        //logging
        if (AlterationConfig.devMode)
        {
            inventory.Export(GetType().Name);
        }

        //Map specific custom blocks
        if (map.embeddedBlocks.Count != 0)
        {
            inventory &= [.. map.embeddedBlocks.Select(x => new Article(x.Key, x.Value, ""))];
            //logging
            if (AlterationConfig.devMode)
            {
                inventory.Export(GetType().Name + "WithEmbedded");
            }
        }
        //TODO customblocksets on embedded
        Run(inventory, map);
        AlterationConfig.mapCount++;
    }

    public virtual string Description => "No description given"; // Alteration Description
    public virtual bool Published => false; // Alteration is published in Auto Alteration -> determines if shown in UI-App
    public virtual bool LikeAN => false; // made in the same way as in Altered Nadeo
    public virtual bool Complete => false; // alteration sometimes needs manual additions


    // Changes which get applied on base inventory before the Alteration is executed
    internal virtual List<ArticleProvider> additionalArticles { get; } = [];

    public virtual List<Alteration> AlterationsBefore { get; } = [];// applied in AlterFile()

    public const float PI = (float)Math.PI;
    
}
