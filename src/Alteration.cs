public abstract class Alteration {
    public virtual string Description => "No description given"; // Alteration Description
    public virtual bool Published => false; // Alteration is published in Auto Alteration -> determines if shown in UI-App
    public virtual bool LikeAN => false; // made in the same way as in Altered Nadeo
    public virtual bool Complete => false; // alteration sometimes needs manual additions

    public virtual List<Alteration> AlterationsBefore { get; } = [];
    internal virtual List<CustomBlockAlteration> customBlockAlts { get; } = [];
    internal virtual List<CustomBlockFolder> customBlockFolders { get; } = [];

    public const float PI = (float)Math.PI;
    
    protected abstract void Run(Inventory inventory, Map map);
    public void Run(Map map)
    {
        foreach (Alteration alteration in AlterationsBefore)
        {
            alteration.Run(map);
        }
        Inventory inventory = [.. AlterationConfig.VanillaArticles.GetArticles()];
        foreach (CustomBlockAlteration alteration in customBlockAlts)
        {
            CustomBlockSet customBlockSet = new CustomBlockSet(alteration);
            AlterationConfig.Keywords.AddRange(customBlockSet.GetAdditionalKeywords());
            inventory |= [.. customBlockSet.GetArticles()];
        }
        foreach (CustomBlockFolder folder in customBlockFolders)
        {
            AlterationConfig.Keywords.AddRange(folder.GetAdditionalKeywords());
            inventory |= [.. folder.GetArticles()];
        }
        //logging
        if (AlterationConfig.devMode)
        {
            inventory.Export(GetType().Name);
        }

        //Map specific custom blocks
        if (map.embeddedBlocks.Count != 0)
        {
            inventory |= [.. map.embeddedBlocks.Select(x => new Article(x.Key, x.Value, ""))];
            string TempFolder = Path.Join(Path.GetTempPath(), "AutoAlteration");
            string ExportFolder = Path.Join(TempFolder, "Exports");
            map.ExtractEmbeddedBlocks(ExportFolder);
            foreach (CustomBlockAlteration customBlockAlteration in customBlockAlts)
            {
                string setName = customBlockAlteration.GetType().Name; //TODO refactor customblockalterations with Names
                string SetPath = Path.Join(TempFolder, setName);
                AutoAlteration.AlterAll(customBlockAlteration,ExportFolder,Path.Join(TempFolder,setName),setName);
                inventory |= [.. new CustomBlockFolder(TempFolder + setName + "\\Items").GetArticles()];
                inventory |= [.. new CustomBlockFolder(TempFolder + setName + "\\Blocks").GetArticles()];
            }
            //logging
            if (AlterationConfig.devMode)
            {
                inventory.Export(GetType().Name + "WithEmbedded");
            }
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
