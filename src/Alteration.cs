using Newtonsoft.Json;
class Alteration {
    public static Inventory inventory;
    public static Inventory Blocks;
    public static Inventory Items;

    public static void load(string projectFolder) {
        string[] Keywords = File.ReadAllLines(projectFolder + "src/Configuration/Keywords.txt");
        Array.Sort(Keywords, (a, b) => b.Length.CompareTo(a.Length));
        // Blocks = new Inventory(projectFolder + "src/Configuration/Blocks.json",Keywords);
        // Items = new Inventory(projectFolder + "src/Configuration/Items.json",Keywords);
        importInventory(projectFolder + "src/Configuration/VanillaInventory.json");
    }
    public static void exportInventory(string projectFolder) {
        Blocks.articles.ForEach(x => x.Type = ArticleType.Block);
        Items.articles.ForEach(x => x.Type = ArticleType.Item);
        List<Article> articles = Blocks.articles;
        articles.AddRange(Items.articles);
        string json = JsonConvert.SerializeObject(articles);
        File.WriteAllText(projectFolder + "src/Configuration/VanillaInventory.json", json);
    }

    public static void importInventory(string path)
    {
        string filePath = path;
        string json = File.ReadAllText(filePath);
        List<Article> articles = JsonConvert.DeserializeObject<List<Article>>(json);
        inventory = new Inventory(articles);
        inventory.articles.ForEach(x => Console.WriteLine(x.Name + x.Type));
    }
    // public static void checkDuplicateNames(){
    //     //TODO
    // }

    public Alteration(){}
    public virtual void run(Map map) {}
}