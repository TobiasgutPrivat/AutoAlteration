
public abstract class Alteration: PosUtils {
    public abstract void Run(Map map);

    public virtual string Description => "No description given"; // Alteration Description
    public virtual bool Published => false; // Alteration is published in Auto Alteration -> determines if shown in UI-App
    public virtual bool LikeAN => false; // made in the same way as in Altered Nadeo
    public virtual bool Complete => false; // alteration sometimes needs manual additions


    // Changes which get applied on base inventory before the Alteration is executed
    public virtual List<InventoryChange> InventoryChanges { get; } = [];

    public static Inventory inventory = new();
    
    public static void CreateInventory() {
        inventory = new(); //Clear inventory when regenerating
        inventory.AddArticles(ArticleImport.ImportVanillaInventory());
        if (AltertionConfig.devMode){
            inventory.Export("Vanilla");
        }
    }

    public static void DefaultInventoryChanges(){ //Some modifications for better Keyword indexing
        inventory.articles.Where(a => a.Name == "StructureSupportCurve1Out").ToList().ForEach(a => {
            a.Width = 2;
            a.Length = 2;
        });
        inventory.articles.Where(a => a.Name == "StructureSupportCurve2In").ToList().ForEach(a => {
            a.Width = 2;
            a.Length = 2;
        });
        inventory.articles.Where(a => a.Name == "StructureSupportCurve2Out").ToList().ForEach(a => {
            a.Width = 3;
            a.Length = 3;
        });
        inventory.articles.Where(a => a.Name.StartsWith("StructureSupportCurve3In")).ToList().ForEach(a => {
            a.Width = 3;
            a.Length = 3;
        });
        inventory.articles.Where(a => a.Name.StartsWith("StructureSupportCurve3Out")).ToList().ForEach(a => {
            a.Width = 4;
            a.Length = 4;
        });
        inventory.Select(BlockType.Block).Select("Gate").EditOriginal().RemoveKeyword("Gate").AddKeyword("Ring");
        inventory.Select("Special").EditOriginal().RemoveKeyword("Special");
        inventory.Select("Start&!(Slope2|Loop|DiagRight|DiagLeft|Slope|Inflatable)").EditOriginal().RemoveKeyword("Start").AddKeyword("MapStart");
        inventory.RemoveArticles(inventory.Select("v2").RemoveKeyword("v2").Align().getAligned());
        inventory.Select("v2").EditOriginal().RemoveKeyword("v2");
        inventory.Select("Oriented").EditOriginal().RemoveKeyword("Oriented");
        inventory.Select("Grasss").EditOriginal().RemoveKeyword("Grasss").AddKeyword("Grass");
    }
}
