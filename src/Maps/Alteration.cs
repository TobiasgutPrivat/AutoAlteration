public abstract class Alteration {
    public abstract void Run(Map map);

    public virtual string Description => "No description given"; // Alteration Description
    public virtual bool Published => false; // Alteration is published in Auto Alteration -> determines if shown in UI-App
    public virtual bool LikeAN => false; // made in the same way as in Altered Nadeo
    public virtual bool Complete => false; // alteration sometimes needs manual additions


    // Changes which get applied on base inventory before the Alteration is executed
    public virtual List<InventoryChange> InventoryChanges { get; } = [];

    public virtual List<Alteration> AlterationsBefore { get; } = [];// applied in AlterFile()

    public const float PI = (float)Math.PI;
    
    public static void CreateInventory() {
        inventory = new(VanillaArticleProvider.GetArticles()); //Clear inventory when regenerating
        if (AlterationConfig.devMode){
            inventory.Export("Vanilla");
        }
    }
}
