using GBX.NET;
using Newtonsoft.Json;
class Alteration {
    public static float PI = (float)Math.PI;
    public static string[] Keywords;
    public static string[] shapeKeywords;
    public static string[] surfaceKeywords;
    public static Inventory inventory = new Inventory();
    public static bool devMode;

    public static void load(string projectFolder) {
        shapeKeywords = File.ReadAllLines(projectFolder + "src/Vanilla/shapeKeywords.txt");
        surfaceKeywords = File.ReadAllLines(projectFolder + "src/Vanilla/surfaceKeywords.txt");
        Keywords = File.ReadAllLines(projectFolder + "src/Vanilla/Keywords.txt");
        // inventory = importSerializedInventory(projectFolder + "src/Inventory.json");
        createInventory(projectFolder);
    }

    public static Inventory importSerializedInventory(string path)
    {
        string filePath = path;
        string json = File.ReadAllText(filePath);
        List<Article> articles = JsonConvert.DeserializeObject<List<Article>>(json);
        return new Inventory(articles);
    }
    
    public static Inventory importArrayInventory(string path){
        string json = File.ReadAllText(path);
        string[] lines = JsonConvert.DeserializeObject<string[]>(json);
        return new Inventory(lines.Select(line => new Article(line)).ToList());
    }
    public static void alter(List<Alteration> alterations, Map map) {
        foreach (Alteration alteration in alterations) {
            alteration.run(map);
        }
    }
    public static void alter(Alteration alteration, Map map) {
        alteration.run(map);
    }

    public static void alterFolder(List<Alteration> alterations, string mapFolder, string destinationFolder, string Name) {
        foreach (string mapFile in Directory.GetFiles(mapFolder))
        {
            alterFile(alterations,mapFile,destinationFolder + Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + Name + ".map.gbx",Name);
        }
    }
    public static void alterFolder(Alteration alteration, string mapFolder, string destinationFolder, string Name) {
        foreach (string mapFile in Directory.GetFiles(mapFolder))
        {
            alterFile(alteration,mapFile,destinationFolder + Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + Name + ".map.gbx",Name);
        }
    }
    public static void alterFolder(List<Alteration> alterations, string mapFolder, string Name) {
        alterFolder(alterations,mapFolder,mapFolder,Name);
    }
    public static void alterFolder(Alteration alteration, string mapFolder, string Name) {
        alterFolder(alteration,mapFolder,mapFolder,Name);
    }
    public static void alterFile(List<Alteration> alterations, string mapFile, string destinationFile, string Name) {
        Map map = new Map(mapFile);
        alter(alterations, map);
        map.map.MapName = Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + Name;
        map.save(destinationFile);
        Console.WriteLine(map.map.MapName);
    }
    public static void alterFile(Alteration alteration, string mapFile, string destinationFile, string Name) {
        alterFile(new List<Alteration>{alteration},mapFile,destinationFile,Name);
    }
    public static void alterFile(List<Alteration> alterations, string mapFile, string Name) {
        alterFile(alterations,mapFile,Path.GetDirectoryName(mapFile) + Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + Name + ".map.gbx",Name);
    }
    public static void alterFile(Alteration alteration, string mapFile, string Name) {
        alterFile(alteration,mapFile,Path.GetDirectoryName(mapFile) + Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + Name + ".map.gbx",Name);
    }

    public Alteration(){}
    public virtual void run(Map map) {}


    public static void createInventory(string projectFolder) {
        devMode = true;
        //Load Vanilla Articles
        Inventory items = Alteration.importArrayInventory(projectFolder + "src/Vanilla/ItemNames.json");
        items.articles.ForEach(x => x.Type = BlockType.Item);
        Inventory blocks = Alteration.importArrayInventory(projectFolder + "src/Vanilla/BlockNames.json");
        blocks.articles.ForEach(x => x.Type = BlockType.Block);

        //Fix Gate naming
        blocks.select("Gate").editOriginal().remove("Gate").add("Ring");

        //Init Inventory
        // Inventory inventory = Alteration.inventory;
        // inventory.articles.Clear();
        inventory.articles.AddRange(items.articles);
        inventory.articles.AddRange(blocks.articles);

        inventory.select("Special").editOriginal().remove("Special");
        inventory.select("DiagLeft|DiagRight").editOriginal().blockChange(new DiagBlockChange());

        //Add Articles with unnecessary keywords removed
        addCheckpointBlocks(inventory);
        inventory.addArticles(inventory.select("Start&!(Slope2|Loop|DiagRight|DiagLeft|Slope|Inflatable)").remove("Start").add("MapStart"));
        
        //Control
        // inventory.checkDuplicates();

        //Save
        // inventory.articles.ForEach(x => x.cacheFilter.Clear());
        // string json = JsonConvert.SerializeObject(inventory.articles);
        // File.WriteAllText(projectFolder + "src/Inventory.json", json);
        devMode = false;
    }

    private static void addCheckpointBlocks(Inventory inventory){
        inventory.addArticles(inventory.select("Checkpoint").remove("Checkpoint").add("Straight").align().remove("Straight"));
        inventory.addArticles(inventory.select("Checkpoint").remove("Checkpoint").add("StraightX2").align().remove("StraightX2").blockChange(new DiagBlockChange()));
        inventory.addArticles(inventory.select("Checkpoint").remove("Checkpoint").add("Base").align().remove("Base"));
        inventory.addArticles(inventory.select("Special").remove("Special"));
        addRoadNoCPBlocks(inventory, "Tech");
        addRoadNoCPBlocks(inventory, "Dirt");
        addRoadNoCPBlocks(inventory, "Bump");
        addRoadNoCPBlocks(inventory, "Ice");
        addPlatformNoCPBlocks(inventory, "Tech");
        addPlatformNoCPBlocks(inventory, "Dirt");
        addPlatformNoCPBlocks(inventory, "Plastic");
        addPlatformNoCPBlocks(inventory, "Grass");
        addPlatformNoCPBlocks(inventory, "Ice");
        addIceRoadNoCPBlocks(inventory);
    }

    private static void addRoadNoCPBlocks(Inventory inventory, string surface){
        inventory.articles.Add(new Article("Road" +surface+"SlopeStraight",BlockType.Block,new List<string> {"Up","Slope"},"Road" +surface,"",""));
        inventory.articles.Add(new Article("Road" +surface+"SlopeStraight",BlockType.Block,new List<string> {"Down","Slope"},"Road" +surface,"","",new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("Road" +surface+"TiltStraight",BlockType.Block,new List<string> {"Left","Tilt"},"Road" +surface,"","",new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("Road" +surface+"TiltStraight",BlockType.Block,new List<string> {"Right","Tilt"},"Road" +surface,"",""));
    }
    private static void addPlatformNoCPBlocks(Inventory inventory, string surface){
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Up","Slope2"},"Platform","",surface));
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Down","Slope2"},"Platform","",surface,new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Right","Slope2"},"Platform","",surface,new BlockChange(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Left","Slope2"},"Platform","",surface,new BlockChange(new Vec3(32,0,0), new Vec3(PI*1.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Up","Wall"},"Platform","",surface,new BlockChange(Vec3.Zero, new Vec3(PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Down","Wall"},"Platform","",surface,new BlockChange(Vec3.Zero, new Vec3(PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Right","Wall"},"Platform","",surface,new BlockChange(Vec3.Zero, new Vec3(PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Left","Wall"},"Platform","",surface,new BlockChange(Vec3.Zero, new Vec3(PI*0.5f,0,0))));
    }
    private static void addIceRoadNoCPBlocks(Inventory inventory){
        inventory.articles.Add(new Article("RoadIceWithWallStraight",BlockType.Block,new List<string> {"Left"},"RoadIceWithWall","",""));
        inventory.articles.Add(new Article("RoadIceWithWallStraight",BlockType.Block,new List<string> {"Right"},"RoadIceWithWall","","",new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("RoadIceDiagRightWithWallStraight",BlockType.Block,new List<string> {"DiagRight","Right"},"RoadIceWithWall","","",new DiagBlockChange()));
        inventory.articles.Add(new Article("RoadIceDiagLeftWithWallStraight",BlockType.Block,new List<string> {"DiagLeft","Right"},"RoadIceWithWall","","",new DiagBlockChange(new Vec3(96,0,64), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("RoadIceDiagRightWithWallStraight",BlockType.Block,new List<string> {"DiagRight","Left"},"RoadIceWithWall","","",new DiagBlockChange(new Vec3(96,0,64), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("RoadIceDiagLeftWithWallStraight",BlockType.Block,new List<string> {"DiagLeft","Left"},"RoadIceWithWall","","",new DiagBlockChange()));
    }
}