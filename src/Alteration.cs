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

    public Alteration(){}
    public virtual void run(Map map) {}
}