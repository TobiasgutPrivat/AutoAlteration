
public abstract class Alteration: PosUtils {
    public abstract void Run(Map map);

    public virtual string Description { get{ return "No description given";} }
    public virtual bool Published { get{ return false;} }
    public virtual bool HasFlaws { get{ return true;} }

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
        inventory.articles.Where(a => a.Name.StartsWith("StructureSupportCurve2")).ToList().ForEach(a => {
            a.Width = 2;
            a.Length = 2;
        });
        inventory.articles.Where(a => a.Name.StartsWith("StructureSupportCurve3")).ToList().ForEach(a => {
            a.Width = 3;
            a.Length = 3;
        });
        inventory.Select(BlockType.Block).Select("Gate").EditOriginal().RemoveKeyword("Gate").AddKeyword("Ring");
        inventory.Select("Special").EditOriginal().RemoveKeyword("Special");
        inventory.Select("Start&!(Slope2|Loop|DiagRight|DiagLeft|Slope|Inflatable)").EditOriginal().RemoveKeyword("Start").AddKeyword("MapStart");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Straight").Align().EditOriginal().RemoveKeyword("Straight");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("StraightX2").Align().EditOriginal().RemoveKeyword("StraightX2");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Base").Align().EditOriginal().RemoveKeyword("Base");
        inventory.RemoveArticles(inventory.Select("v2").RemoveKeyword("v2").Align());
        inventory.Select("v2").EditOriginal().RemoveKeyword("v2");
        inventory.Select("Oriented").EditOriginal().RemoveKeyword("Oriented");
    }
}
