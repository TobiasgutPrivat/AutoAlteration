class AutoAlteration {
    public static Inventory Blocks;
    public static Inventory Items;

    public static void load(string projectFolder) {
        string[] Keywords = File.ReadAllLines(projectFolder + "src/Configuration/Keywords.txt");
        Array.Sort(Keywords, (a, b) => b.Length.CompareTo(a.Length));
        Blocks = new Inventory(projectFolder + "src/Configuration/Blocks.json",Keywords);
        // Blocks.addInventory(projectFolder + "src/Configuration/Macroblocks.json",Keywords);//TODO test Macroblocks
        Items = new Inventory(projectFolder + "src/Configuration/Items.json",Keywords);
    }

    public static void alter(List<Alteration> alterations, Map map) {
        foreach (Alteration alteration in alterations) {
            alteration.run(map);
        }
        map.placeStagedBlocks();//TODO check if reasonable here
    }
    public static void alter(Alteration alteration, Map map) {
        alteration.run(map);
        map.placeStagedBlocks();
    }

    public static void alterFolder(List<Alteration> alterations, string mapFolder, string destinationFolder, string Name) {
        foreach (string mapFile in Directory.GetFiles(mapFolder))
        {
            alterFile(alterations,mapFile,destinationFolder,Name);
        }
    }
    public static void alterFolder(Alteration alteration, string mapFolder, string destinationFolder, string Name) {
        foreach (string mapFile in Directory.GetFiles(mapFolder))
        {
            alterFile(alteration,mapFile,destinationFolder,Name);
        }
    }
    public static void alterFolder(List<Alteration> alterations, string mapFolder, string Name) {
        alterFolder(alterations,mapFolder,mapFolder,Name);
    }
    public static void alterFolder(Alteration alteration, string mapFolder, string Name) {
        alterFolder(alteration,mapFolder,mapFolder,Name);
    }
    public static void alterFile(List<Alteration> alterations, string mapFile, string destinationFolder, string Name) {
        Map map = new Map(mapFile);
        alter(alterations, map);
        map.map.MapName = Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + Name;
        map.save(destinationFolder + Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + Name + ".map.gbx");
    }
    public static void alterFile(Alteration alteration, string mapFile, string destinationFolder, string Name) {
        Map map = new Map(mapFile);
        alter(alteration, map);
        map.map.MapName = Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + Name;
        map.save(destinationFolder + Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + Name + ".map.gbx");
    }
    public static void alterFile(List<Alteration> alterations, string mapFile, string Name) {
        alterFile(alterations,mapFile,Path.GetDirectoryName(mapFile),Name);
    }
    public static void alterFile(Alteration alteration, string mapFile, string Name) {
        alterFile(alteration,mapFile,Path.GetDirectoryName(mapFile),Name);
    }

    public static void checkDuplicateNames(){
        //TODO
    }
}