using Newtonsoft.Json;
class Alteration {
    public static Inventory inventory;

    public static void load(string projectFolder) {
        string[] Keywords = File.ReadAllLines(projectFolder + "src/Configuration/Keywords.txt");
        Array.Sort(Keywords, (a, b) => b.Length.CompareTo(a.Length));
        importInventory(projectFolder + "src/Configuration/VanillaInventory.json");
    }
    // public static void exportInventory(string projectFolder) {
    //     inventory.articles.ForEach(x => x.Type = ArticleType.Block);
        
    // }

    public static void importInventory(string path)
    {
        string filePath = path;
        string json = File.ReadAllText(filePath);
        List<Article> articles = JsonConvert.DeserializeObject<List<Article>>(json);
        inventory = new Inventory(articles);
        // inventory.articles.ForEach(x => Console.WriteLine(x.Name + x.Type));
    }
    // public static void checkDuplicateNames(){
    //     //TODO
    // }

    public Alteration(){}
    public virtual void run(Map map) {}
}