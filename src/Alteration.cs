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
        alterFile(alteration,mapFile,Path.GetDirectoryName(mapFile) + "\\" +  Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + Name + ".map.gbx",Name);
    }

    public Alteration(){}
    public virtual void run(Map map) {}


    public static void createInventory(string projectFolder) {
        devMode = true;
        //Load Vanilla Articles
        Inventory items = importArrayInventory(projectFolder + "src/Vanilla/ItemNames.json");
        items.articles.ForEach(x => x.type = BlockType.Item);
        Inventory blocks = importArrayInventory(projectFolder + "src/Vanilla/BlockNames.json");
        blocks.articles.ForEach(x => x.type = BlockType.Block);

        //Fix Gate naming
        blocks.select("Gate").editOriginal().remove("Gate").add("Ring");

        //Init Inventory
        inventory.articles.AddRange(items.articles);
        inventory.articles.AddRange(blocks.articles);

        inventory.select("Special").editOriginal().remove("Special");
        inventory.select("DiagLeft|DiagRight").editOriginal().blockMove(new DiagBlockMove());

        //Add Articles with unnecessary keywords removed
        addCheckpointBlocks();
        addCheckpointTrigger();
        inventory.addArticles(inventory.select("Start&!(Slope2|Loop|DiagRight|DiagLeft|Slope|Inflatable)").remove("Start").add("MapStart"));
        
        //Control
        // inventory.checkDuplicates();

        //save
        inventory.articles.ForEach(x => x.cacheFilter.Clear());
        string json = JsonConvert.SerializeObject(inventory.articles);
        File.WriteAllText(projectFolder + "src/Inventory.json", json);

        devMode = false;
    }

    private static void addCheckpointBlocks(){
        inventory.addArticles(inventory.select("Checkpoint").remove("Checkpoint").add("Straight").align().remove("Straight"));
        inventory.addArticles(inventory.select("Checkpoint").remove("Checkpoint").add("StraightX2").align().remove("StraightX2").blockMove(new DiagBlockMove()));
        inventory.addArticles(inventory.select("Checkpoint").remove("Checkpoint").add("Base").align().remove("Base"));
        inventory.addArticles(inventory.select("Special").remove("Special"));
        addRoadNoCPBlocks("Tech");
        addRoadNoCPBlocks("Dirt");
        addRoadNoCPBlocks("Bump");
        addRoadNoCPBlocks("Ice");
        addPlatformNoCPBlocks("Tech");
        addPlatformNoCPBlocks("Dirt");
        addPlatformNoCPBlocks("Plastic");
        addPlatformNoCPBlocks("Grass");
        addPlatformNoCPBlocks("Ice");
        addIceRoadNoCPBlocks();
    }

    private static void addCheckpointTrigger(){
        Inventory CPMLBlock = inventory.select(BlockType.Block).select("Checkpoint|Multilap");
        Vec3 midPlatform = new Vec3(16,2,16);
        createTriggerArticle(CPMLBlock.select("!Wall&!Slope2&!Slope&!Tilt&!DiagRight&!DiagLeft&!(RoadIce)"), midPlatform,Vec3.Zero);
        float slope2 = 0.47f;
        createTriggerArticle(CPMLBlock.select("Slope2&Down"), midPlatform + new Vec3(0,8,0),new Vec3(0,slope2,0));
        createTriggerArticle(CPMLBlock.select("Slope2&Up"), midPlatform + new Vec3(0,8,0),new Vec3(0,-slope2,0));
        createTriggerArticle(CPMLBlock.select("Slope2&Right"), midPlatform + new Vec3(0,8,0),new Vec3(0,0,slope2));
        createTriggerArticle(CPMLBlock.select("Slope2&Left"), midPlatform + new Vec3(0,8,0),new Vec3(0,0,-slope2));
        float slope = slope2/2;
        createTriggerArticle(CPMLBlock.select("Slope&Down&!RoadIce"), midPlatform + new Vec3(0,4,0),new Vec3(0,slope,0));
        createTriggerArticle(CPMLBlock.select("Slope&Up&!RoadIce"), midPlatform + new Vec3(0,4,0),new Vec3(0,-slope,0));
        createTriggerArticle(CPMLBlock.select("Tilt&Right&!RoadIce"), midPlatform + new Vec3(0,4,0),new Vec3(0,0,-slope));
        createTriggerArticle(CPMLBlock.select("Tilt&Left&!RoadIce"), midPlatform + new Vec3(0,4,0),new Vec3(0,0,slope));

        createTriggerArticle(CPMLBlock.select("Slope&Down&RoadIce"), midPlatform + new Vec3(0,8,0),new Vec3(0,slope,0));
        createTriggerArticle(CPMLBlock.select("Slope&Up&RoadIce"), midPlatform + new Vec3(0,8,0),new Vec3(0,-slope,0));

        createTriggerArticle(CPMLBlock.select("Platform&Wall&Down"), new Vec3(16,16,29),new Vec3(PI,PI*0.5f,0));
        createTriggerArticle(CPMLBlock.select("Platform&Wall&Up"), new Vec3(16,16,29),new Vec3(0,-PI*0.5f,0));
        createTriggerArticle(CPMLBlock.select("Platform&Wall&Right"), new Vec3(16,16,29),new Vec3(PI,PI*0.5f,PI*0.5f));//should work i think, but some problem with offset
        createTriggerArticle(CPMLBlock.select("Platform&Wall&Left"), new Vec3(16,16,29),new Vec3(PI,PI*0.5f,-PI*0.5f));

        
    }

    private static void createTriggerArticle(Inventory selection,Vec3 offset, Vec3 rotation){
        inventory.addArticles(selection.add("Trigger").blockMove(new BlockMove(offset,rotation)));
    }

    private static void addRoadNoCPBlocks(string surface){
        inventory.articles.Add(new Article("Road" +surface+"SlopeStraight",BlockType.Block,new List<string> {"Up","Slope"},"Road" +surface,"",""));
        inventory.articles.Add(new Article("Road" +surface+"SlopeStraight",BlockType.Block,new List<string> {"Down","Slope"},"Road" +surface,"","",new BlockMove(new Vec3(32,0,32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("Road" +surface+"TiltStraight",BlockType.Block,new List<string> {"Left","Tilt"},"Road" +surface,"","",new BlockMove(new Vec3(32,0,32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("Road" +surface+"TiltStraight",BlockType.Block,new List<string> {"Right","Tilt"},"Road" +surface,"",""));
    }
    private static void addPlatformNoCPBlocks(string surface){
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Up","Slope2"},"Platform","",surface));
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Down","Slope2"},"Platform","",surface,new BlockMove(new Vec3(32,0,32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Right","Slope2"},"Platform","",surface,new BlockMove(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Left","Slope2"},"Platform","",surface,new BlockMove(new Vec3(32,0,0), new Vec3(PI*1.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Up","Wall"},"Platform","",surface,new BlockMove(new Vec3(32,0,0), new Vec3(-PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Down","Wall"},"Platform","",surface,new BlockMove(new Vec3(32,0,0), new Vec3(-PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Right","Wall"},"Platform","",surface,new BlockMove(new Vec3(32,0,0), new Vec3(-PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Left","Wall"},"Platform","",surface,new BlockMove(new Vec3(32,0,0), new Vec3(-PI*0.5f,0,0))));
    }
    private static void addIceRoadNoCPBlocks(){
        inventory.articles.Add(new Article("RoadIceWithWallStraight",BlockType.Block,new List<string> {"Left","WithWall"},"RoadIce","",""));
        inventory.articles.Add(new Article("RoadIceWithWallStraight",BlockType.Block,new List<string> {"Right","WithWall"},"RoadIce","","",new BlockMove(new Vec3(32,0,32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("RoadIceDiagRightWithWallStraight",BlockType.Block,new List<string> {"DiagRight","Right","WithWall"},"RoadIce","","",new DiagBlockMove()));
        inventory.articles.Add(new Article("RoadIceDiagLeftWithWallStraight",BlockType.Block,new List<string> {"DiagLeft","Right","WithWall"},"RoadIce","","",new DiagBlockMove(new Vec3(96,0,64), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("RoadIceDiagRightWithWallStraight",BlockType.Block,new List<string> {"DiagRight","Left","WithWall"},"RoadIce","","",new DiagBlockMove(new Vec3(96,0,64), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("RoadIceDiagLeftWithWallStraight",BlockType.Block,new List<string> {"DiagLeft","Left","WithWall"},"RoadIce","","",new DiagBlockMove()));
    }
}