using Newtonsoft.Json;
class Alteration {
    public static Inventory inventory;
    public static Inventory Blocks;
    public static Inventory Items;

    public static void load(string projectFolder) {
        string[] Keywords = File.ReadAllLines(projectFolder + "src/Configuration/Keywords.txt");
        Array.Sort(Keywords, (a, b) => b.Length.CompareTo(a.Length));
        importInventory(projectFolder + "src/Configuration/Inventory.json");
        Blocks = new Inventory(projectFolder + "src/Configuration/Blocks.json",Keywords);
        Items = new Inventory(projectFolder + "src/Configuration/Items.json",Keywords);
    }

    public static void importInventory(string path)
    {
        string filePath = path;
        string json = File.ReadAllText(filePath);
        List<Article> articles = JsonConvert.DeserializeObject<List<Article>>(json);
        inventory = new Inventory(articles);
    }

    public Alteration(){}
    public virtual void run(Map map) {}
}