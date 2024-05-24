class Alteration {
    public static Inventory Blocks;
    public static Inventory Items;

    public static void load(string projectFolder) {
        string[] Keywords = File.ReadAllLines(projectFolder + "src/Configuration/Keywords.txt");
        Array.Sort(Keywords, (a, b) => b.Length.CompareTo(a.Length));
        Blocks = new Inventory(projectFolder + "src/Configuration/Blocks.json",Keywords);
        // Blocks.addInventory(projectFolder + "src/Configuration/Macroblocks.json",Keywords);//TODO test Macroblocks
        Items = new Inventory(projectFolder + "src/Configuration/Items.json",Keywords);
    }
    // public static void checkDuplicateNames(){
    //     //TODO
    // }
    public string[] GetArticles(string[] keywords) {
        string[] articles = Blocks.GetArticles(keywords).Select(x => x.Name).ToArray();
        Items.GetArticles(keywords).ForEach(x => articles = articles.Append(x.Name).ToArray());
        return articles;
    }
    public string[] GetArticles(string[] keywords,string[] excludeKeywords) {
        string[] articles = Blocks.GetArticles(keywords).Select(x => x.Name).ToArray();
        Items.GetArticles(keywords,excludeKeywords).ForEach(x => articles = articles.Append(x.Name).ToArray());
        return articles;
    }
    public string[] GetArticles(string[] keywords,string excludeKeyword) {
        string[] articles = Blocks.GetArticles(keywords).Select(x => x.Name).ToArray();
        Items.GetArticles(keywords,new string [] {excludeKeyword}).ForEach(x => articles = articles.Append(x.Name).ToArray());
        return articles;
    }
    public string[] GetArticles(string keyword) {
        string[] articles = Blocks.GetArticles(new string [] {keyword}).Select(x => x.Name).ToArray();
        Items.GetArticles(new string [] {keyword}).ForEach(x => articles = articles.Append(x.Name).ToArray());
        return articles;
    }
    public string[] GetArticles(string keyword,string excludeKeyword) {
        string[] articles = Blocks.GetArticles(new string [] {keyword}).Select(x => x.Name).ToArray();
        Items.GetArticles(new string [] {keyword},new string [] {excludeKeyword}).ForEach(x => articles = articles.Append(x.Name).ToArray());
        return articles;
    }

    //
    public Alteration(){}
    public virtual void run(Map map) {}
}