using Newtonsoft.Json;
class Alteration {
    public static Inventory inventory;
    public static Inventory Blocks;
    public static Inventory Items;

    public static void load(string projectFolder) {
        string[] Keywords = File.ReadAllLines(projectFolder + "src/Vanilla/Keywords.txt");
        Array.Sort(Keywords, (a, b) => b.Length.CompareTo(a.Length));
        inventory = importSerializedInventory(projectFolder + "src/Inventory.json");
        Blocks = importArrayInventory(projectFolder + "src/Vanilla/Blocks.json",Keywords);
        Items = importArrayInventory(projectFolder + "src/Vanilla/Items.json",Keywords);
    }

    public static Inventory importSerializedInventory(string path)
    {
        string filePath = path;
        string json = File.ReadAllText(filePath);
        List<Article> articles = JsonConvert.DeserializeObject<List<Article>>(json);
        return new Inventory(articles);
    }
    
    public static Inventory importArrayInventory(string path,string[] Keywords){
        string json = File.ReadAllText(path);
        string[] lines = JsonConvert.DeserializeObject<string[]>(json);
        return new Inventory(lines.Select(line => new Article(line,Keywords)).ToList());
    }

    public Alteration(){}
    public virtual void run(Map map) {}
}