class AutoAlteration {
    public static string[] Keywords;
    public static Inventory Blocks;
    public static Inventory Items;
    // private static Inventory Macroblocks; //TODO Embeded
    // private static Inventory CustomBlocks;

    public static void load(string projectFolder) {
        Keywords = File.ReadAllLines(projectFolder + "src/Inventory/Configuration/Keywords.txt");
        Array.Sort(Keywords, (a, b) => b.Length.CompareTo(a.Length));
        Blocks = new Inventory(projectFolder + "src/Inventory/Configuration/Blocks.json",Keywords);
        Items = new Inventory(projectFolder + "src/Inventory/Configuration/Items.json",Keywords);
    }

    public static void alter(List<Alteration> alterations, Map map) {
        foreach (Alteration alteration in alterations) {
            alteration.run(map);
        }
    }

    public static void alter(List<Alteration> alterations, string mapFolder, string destinationFolder, string Name) {
        string[] files = Directory.GetFiles(mapFolder);
        foreach (string file in files)
        {
            Map map = new Map(file);
            alter(alterations, map);
            map.map.MapName = Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 8) + " " + Name;
            map.save(destinationFolder + Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 8) + " " + Name + ".map.gbx");
        }
    }

    public static void checkDuplicateNames(){
        //TODO
    }
}