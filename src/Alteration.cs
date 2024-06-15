using GBX.NET;
using Newtonsoft.Json;
class Alteration {
    public static float PI = (float)Math.PI;
    public static string ProjectFolder = "";
    public static string CustomBlocksFolder = "";
    public static string[] Keywords;
    public static string[] shapeKeywords;
    public static string[] surfaceKeywords;
    public static Inventory inventory = new Inventory();
    public static bool devMode;

    public static void load(string projectFolder) {
        ProjectFolder = projectFolder;
        CustomBlocksFolder = ProjectFolder + "src/CustomBlocks/";
        shapeKeywords = File.ReadAllLines(ProjectFolder + "src/Vanilla/shapeKeywords.txt");
        surfaceKeywords = File.ReadAllLines(ProjectFolder + "src/Vanilla/surfaceKeywords.txt");
        Keywords = File.ReadAllLines(ProjectFolder + "src/Vanilla/Keywords.txt");
        createInventory();
    }

    public static Inventory importSerializedInventory(string path)
    {
        string filePath = path;
        string json = File.ReadAllText(filePath);
        List<Article> articles = JsonConvert.DeserializeObject<List<Article>>(json);
        return new Inventory(articles);
    }
    
    public static Inventory importArrayInventory(string path,BlockType blockType){
        string json = File.ReadAllText(path);
        string[] lines = JsonConvert.DeserializeObject<string[]>(json);
        return new Inventory(lines.Select(line => new Article(line,blockType)).ToList());
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
        foreach (string mapFile in Directory.GetFiles(mapFolder, "*.map.gbx", SearchOption.TopDirectoryOnly))
        {
            alterFile(alterations,mapFile,destinationFolder + Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + Name + ".map.gbx",Name);
        }
    }
    public static void alterFolder(Alteration alteration, string mapFolder, string destinationFolder, string Name) {
        alterFolder(new List<Alteration>{alteration},mapFolder,destinationFolder,Name);
    }
    public static void alterAll(List<Alteration> alterations, string mapFolder, string destinationFolder, string Name) {
        alterFolder(alterations,mapFolder,destinationFolder + Path.GetFileName(mapFolder) + " - " + Name + "/",Name);
        foreach (string Directory in Directory.GetDirectories(mapFolder, "*", SearchOption.TopDirectoryOnly))
        {
            alterAll(alterations,Directory,destinationFolder + Directory.Substring(mapFolder.Length) + "/",Name);
        }
    }
    public static void alterAll(Alteration alteration, string mapFolder, string destinationFolder, string Name) {
        alterAll(new List<Alteration>{alteration},mapFolder,destinationFolder,Name);
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
        Console.WriteLine(destinationFile);
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

    

    public static void createInventory() {
        devMode = true;
        //Load Vanilla Articles
        Inventory items = importArrayInventory(ProjectFolder + "src/Vanilla/ItemNames.json",BlockType.Item);
        Inventory blocks = importArrayInventory(ProjectFolder + "src/Vanilla/BlockNames.json",BlockType.Block);

        //Fix Gate naming
        blocks.select("Gate").editOriginal().remove("Gate").add("Ring");

        //Init Inventory
        inventory.addArticles(items.articles);
        inventory.addArticles(blocks.articles);
        //CustomBlocks
        inventory.addArticles(Directory.GetFiles(CustomBlocksFolder, "*.Block.Gbx", SearchOption.AllDirectories).Select(x => new Article(Path.GetFileName(x), BlockType.CustomBlock)).ToList());
        inventory.addArticles(Directory.GetFiles(CustomBlocksFolder, "*.Item.Gbx", SearchOption.AllDirectories).Select(x => new Article(Path.GetFileName(x), BlockType.CustomItem)).ToList());

        inventory.select("Special").editOriginal().remove("Special");

        //Add Articles with unnecessary keywords removed
        addCheckpointBlocks();
        addCheckpointTrigger();
        inventory.addArticles(inventory.select("Start&!(Slope2|Loop|DiagRight|DiagLeft|Slope|Inflatable)").remove("Start").add("MapStart"));
        setSizes();
        
        //Control
        // inventory.checkDuplicates();

        //save
        inventory.articles.ForEach(x => x.cacheFilter.Clear());
        string json = JsonConvert.SerializeObject(inventory.articles);
        File.WriteAllText(ProjectFolder + "src/Inventory.json", json);

        devMode = false;
    }

    private static void setSizes(){
        // Road Type
        inventory.select(BlockType.Block).select("Road&Curve2&(!In|!Out)").editOriginal().width(2).length(2); // Might need a `&(!In|!Out)`, not tested yet... // Also catches 3-nn
        inventory.select(BlockType.Block).select("Road&Curve3&(!In|!Out)").editOriginal().width(3).length(3); // Might need a `&(!In|!Out)`, not tested yet... // Also catches 3-nn
        inventory.select(BlockType.Block).select("Road&Curve4&(!In|!Out)").editOriginal().width(4).length(4); // Might need a `&(!In|!Out)`, not tested yet... // Also catches 3-nn
        inventory.select(BlockType.Block).select("Road&Curve5&(!In|!Out)").editOriginal().width(5).length(5); // Might need a `&(!In|!Out)`, not tested yet... // Also catches 3-nn

        inventory.select(BlockType.Block).select("Road&Chicane&X2").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("Road&Chicane&X3").editOriginal().width(2).length(3);

        inventory.select(BlockType.Block).select("Road&Branch&Straigt&X4").editOriginal().width(2).length(4);
        inventory.select(BlockType.Block).select("Road&Branch&Curve3").editOriginal().width(3).length(3); // Redundant because of Curve3
        inventory.select(BlockType.Block).select("Road&Branch&YShaped&2X3").editOriginal().width(2).length(3); // Redundant because of 2X3
        inventory.select(BlockType.Block).select("Road&BranchTo&Diag&(Left|Right)").editOriginal().width(1).length(2);

        inventory.select(BlockType.Block).select("Road&Slope&(Base|Start|End)&2x1").editOriginal().width(2).length(1); // Redundant because of 2x1
        inventory.select(BlockType.Block).select("Road&Slope&(UTop|UBottom)&2X").editOriginal().width(2).length(2); // Redundant because of 2X // InGround vaiant not included, but should still be caught by UBottom

        inventory.select(BlockType.Block).select("Road&Tilt&Transition2&((Up|Down)&(Left|Right))").editOriginal().width(1).length(2);
        inventory.select(BlockType.Block).select("Road&Tilt&Transition2&Curve&(In|Out)").editOriginal().width(1).length(2);

        inventory.select(BlockType.Block).select("Road&DiagLeft|DiagRight").editOriginal().width(3).length(2); // Original example by Tobias

        inventory.select(BlockType.Block).select("Road&Diag&(Left|Right)&Chicane&Right").editOriginal().width(2).length(1);
        inventory.select(BlockType.Block).select("Road&Diag&(Left|Right)&Chicane&Left").editOriginal().width(3).length(1);

        inventory.select(BlockType.Block).select("Road&Diag&(Left|Right)&Start&Straight&X2").editOriginal().width(2).length(1);
        inventory.select(BlockType.Block).select("Road&Diag&(Left|Right)&Start&Curve1&In").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("Road&Diag&(Left|Right)&Start&Curve2&In").editOriginal().width(2).length(3);

        inventory.select(BlockType.Block).select("Road&Diag&(Left|Right)&Start&Curve1&Out").editOriginal().width(2).length(1);
        inventory.select(BlockType.Block).select("Road&Diag&(Left|Right)&Start&Curve2&Out").editOriginal().width(3).length(2);

        inventory.select(BlockType.Block).select("Road&Diag&Switch&Curve1").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("Road&Diag&Switch&Curve2").editOriginal().width(3).length(3);

        inventory.select(BlockType.Block).select("Road&Loop6X").editOriginal().width(3).length(1);
        inventory.select(BlockType.Block).select("Road&Loop11X").editOriginal().width(4).length(2);


        // Platform
        inventory.select(BlockType.Block).select("Platform&Curve2In").editOriginal().width(1).length(1); // Should be included "Platform[Surface]Curve2In" is 1x1
        inventory.select(BlockType.Block).select("Platform&Curve3In").editOriginal().width(2).length(2);

        inventory.select(BlockType.Block).select("Platform&To&Road&Tech&Diag&(Right|Left)").editOriginal().width(2).length(1); // Don't rememver how the 'To' works :xdd:

        inventory.select(BlockType.Block).select("Platform&Slope2&Curve2&(Out|In)").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("Platform&Slope2&Curve3&(Out|In)").editOriginal().width(3).length(3);

        inventory.select(BlockType.Block).select("Platform&Tilt&Transition2").editOriginal().width(1).length(2);
        inventory.select(BlockType.Block).select("Platform&Slope2&(Start2|End2)").editOriginal().width(1).length(2);

        inventory.select(BlockType.Block).select("Platform&Loop&Start&Curve2&In").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("Platform&Loop&Start&Curve3&In").editOriginal().width(3).length(3);

        inventory.select(BlockType.Block).select("Platform&Loop&Start&Curve1&Out").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("Platform&Loop&Start&Curve2&Out").editOriginal().width(3).length(3);
        inventory.select(BlockType.Block).select("Platform&Loop&Start&Curve3&Out").editOriginal().width(4).length(4);

        inventory.select(BlockType.Block).select("Platform&Loop&Start&1x2&Curve2&In").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("Platform&Loop&Start&1x2&Curve3&In").editOriginal().width(3).length(3);
        inventory.select(BlockType.Block).select("Platform&Loop&Start&1x2&Curve1&Out").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("Platform&Loop&Start&1x2&Curve2&Out").editOriginal().width(3).length(3);
        inventory.select(BlockType.Block).select("Platform&Loop&Start&1x1&Curve2&In").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("Platform&Loop&Start&1x1&Curve3&In").editOriginal().width(3).length(3);
        inventory.select(BlockType.Block).select("Platform&Loop&Start&1x1&Curve1&Out").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("Platform&Loop&Start&1x1&Curve2&Out").editOriginal().width(3).length(3);

        inventory.select(BlockType.Block).select("Platform&Loop&Out&Start&Curve1").editOriginal().width(2).length(2); // Counts for Curve1In too
        inventory.select(BlockType.Block).select("Platform&Loop&Out&Start&Curve2").editOriginal().width(3).length(3); // Counts for Curve2In too
        inventory.select(BlockType.Block).select("Platform&Loop&Out&Start&Curve3").editOriginal().width(4).length(4); // Counts for Curve3In too

        inventory.select(BlockType.Block).select("Platform&Loop&End&Curve2&In").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("Platform&Loop&End&Curve3&In").editOriginal().width(3).length(3);
        inventory.select(BlockType.Block).select("Platform&Loop&End&Curve1&Out").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("Platform&Loop&End&Curve2&Out").editOriginal().width(3).length(3);
        inventory.select(BlockType.Block).select("Platform&Loop&End&Curve3&Out").editOriginal().width(4).length(4);

        inventory.select(BlockType.Block).select("Platform&Wall&Curve2").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("Platform&Wall&Curve3").editOriginal().width(3).length(3);



        // Deco platform
        inventory.select(BlockType.Block).select("DecoPlatform&Curve2").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("DecoPlatform&Curve3").editOriginal().width(3).length(3);
        inventory.select(BlockType.Block).select("DecoPlatform&Curve4").editOriginal().width(4).length(4);
        inventory.select(BlockType.Block).select("DecoPlatform&Curve5").editOriginal().width(5).length(5);
        //                                        DecoPlatform Curve
        inventory.select(BlockType.Block).select("DecoPlatform&Curve2&In").editOriginal().width(1).length(1); // Should be included "Platform[Surface]Curve2In" is 1x1

        inventory.select(BlockType.Block).select("DecoPlatform&Curve3&In").editOriginal().width(2).length(2);

        inventory.select(BlockType.Block).select("DecoPlatform&Slope2&Start&Curve2&(In|Out)").editOriginal().width(2).length(2);

        inventory.select(BlockType.Block).select("DecoPlatform&Slope2&(Start2|End2)&!Curve").editOriginal().width(1).length(2);
        inventory.select(BlockType.Block).select("DecoPlatform&Slope2&(Start2|End2)&Curve2").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("DecoPlatform&Slope2&(Start2|End2)&Curve4").editOriginal().width(4).length(4);

        inventory.select(BlockType.Block).select("DecoPlatform&Slope2&Base&To&Slope2&Base2").editOriginal().width(1).length(2);
        //                                        DecoPlatform Slope2 Base To Slope2 Base2 (right / left)
        inventory.select(BlockType.Block).select("DecoPlatform&Slope2&Curve2").editOriginal().width(2).length(2);
        //                                        DecoPlatform Slope2 Start Curve2In (right / left)

        inventory.select(BlockType.Block).select("DecoPlatform&Slope2&Curve2").editOriginal().width(2).length(2);
        // Deco Hill
        inventory.select(BlockType.Block).select("Deco&Hill&Slope2&StraightX2").editOriginal().width(2).length(1);
        inventory.select(BlockType.Block).select("Deco&Hill&Slope2&Curve2").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("Deco&Hill&Slope2&ChicaneX2").editOriginal().width(2).length(2); // Might be &Chicane&X2 or &((Chicane&X2)| (ChicaneX2), not tested yet
        //                                        Deco Hill Slope2 ChicaneX2
        inventory.select(BlockType.Block).select("Deco&Hill&Slope4&Base4&Curve").editOriginal().width(4).length(4);
        inventory.select(BlockType.Block).select("Deco&Hill&Slope2&Start2&Base5").editOriginal().width(4).length(4);
        
        // Deco Cliff
        inventory.select(BlockType.Block).select("Deco&Cliff10&Straight&!Small").editOriginal().width(4).length(2); // Original example by Tobias
        inventory.select(BlockType.Block).select("Deco&Cliff10&Straight&Small").editOriginal().width(1).length(2); // Original example by Tobias

        inventory.select(BlockType.Block).select("Deco&Cliff10&Corner&In&!Small").editOriginal().width(4).length(4);
        inventory.select(BlockType.Block).select("Deco&Cliff10&Corner&In&Small").editOriginal().width(2).length(2);
        //                                        Deco Cliff10 Corner In
        inventory.select(BlockType.Block).select("Deco&Cliff10&Corner&Out&!Small").editOriginal().width(2).length(2); // Testing 2x2, might be 3x3 xdd
        inventory.select(BlockType.Block).select("Deco&Cliff10&Corner&Out&Small").editOriginal().width(2).length(2);

        inventory.select(BlockType.Block).select("Deco&Cliff10&DiagOut&!Small").editOriginal().width(3).length(3);
        inventory.select(BlockType.Block).select("Deco&Cliff10&DiagOut&Small").editOriginal().width(2).length(2);
        inventory.select(BlockType.Block).select("Deco&Cliff10&DiagIn&!Small").editOriginal().width(4).length(4);
        inventory.select(BlockType.Block).select("Deco&Cliff10&DiagIn&Small").editOriginal().width(2).length(2);

        inventory.select(BlockType.Block).select("Deco&Cliff10&End").editOriginal().width(6).length(2);
        inventory.select(BlockType.Block).select("Deco&Cliff10&End&!Mirror").editOriginal().width(6).length(2); // Is redundant

        inventory.select(BlockType.Block).select("Deco&Cliff8&No&Hill&Straight&!Small").editOriginal().width(4).length(2);
        inventory.select(BlockType.Block).select("Deco&Cliff8&No&Hill&Straight&Small").editOriginal().width(1).length(2);
        //                                        Deco Cliff8 No Hill Straight

        inventory.select(BlockType.Block).select("Deco&Cliff8&No&Hill&Corner&In&!Small").editOriginal().width(4).length(4);
        inventory.select(BlockType.Block).select("Deco&Cliff8&No&Hill&Corner&In&Small").editOriginal().width(2).length(2);


        // inventory.select(BlockType.Block).select("Deco&Cliff8&No&Hill&Corner&Out&!Simple").editOriginal().width(4).length(4); // Does not actually exist, but might in the future
        inventory.select(BlockType.Block).select("Deco&Cliff8&No&Hill&Corner&Out").editOriginal().width(2).length(2);
        //                                        Deco Cliff8 No Hill Corner Out  Simple

        inventory.select(BlockType.Block).select("Deco&Cliff8&No&Hill&Diag&Out&!Small").editOriginal().width(4).length(4); // Does not actually exist, but might in the future
        inventory.select(BlockType.Block).select("Deco&Cliff8&No&Hill&Diag&Out&Small").editOriginal().width(2).length(2);
        //                                        Deco Cliff8 No Hill Diag Out Small
        //                                        Deco Cliff8 No Hill Diag Out

        inventory.select(BlockType.Block).select("Deco&Cliff8&No&Hill&DiagIn&!Small").editOriginal().width(4).length(4);
        inventory.select(BlockType.Block).select("Deco&Cliff8&No&Hill&DiagIn").editOriginal().width(2).length(2);
    }

    private static void addCheckpointBlocks(){
        inventory.addArticles(inventory.select("Checkpoint").remove("Checkpoint").add("Straight").align().remove("Straight"));
        inventory.addArticles(inventory.select("Checkpoint").remove("Checkpoint").add("StraightX2").align().remove("StraightX2"));
        inventory.addArticles(inventory.select("Checkpoint").remove("Checkpoint").add("Base").align().remove("Base"));
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
        inventory.addArticles(selection.add("Trigger").changePosition(new Position(offset,rotation)));
    }

    private static void addRoadNoCPBlocks(string surface){
        inventory.articles.Add(new Article("Road" +surface+"SlopeStraight",BlockType.Block,new List<string> {"Up","Slope"},"Road" +surface,"",""));
        inventory.articles.Add(new Article("Road" +surface+"SlopeStraight",BlockType.Block,new List<string> {"Down","Slope"},"Road" +surface,"","",new Position(new Vec3(32,0,-32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("Road" +surface+"TiltStraight",BlockType.Block,new List<string> {"Left","Tilt"},"Road" +surface,"","",new Position(new Vec3(32,0,-32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("Road" +surface+"TiltStraight",BlockType.Block,new List<string> {"Right","Tilt"},"Road" +surface,"",""));
    }
    private static void addPlatformNoCPBlocks(string surface){
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Up","Slope2"},"Platform","",surface));
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Down","Slope2"},"Platform","",surface,new Position(new Vec3(32,0,32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Right","Slope2"},"Platform","",surface,new Position(new Vec3(32,0,0), new Vec3(PI*1.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Left","Slope2"},"Platform","",surface,new Position(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Up","Wall"},"Platform","",surface,new Position(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Down","Wall"},"Platform","",surface,new Position(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Right","Wall"},"Platform","",surface,new Position(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Left","Wall"},"Platform","",surface,new Position(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0))));
    }
    private static void addIceRoadNoCPBlocks(){
        inventory.articles.Add(new Article("RoadIceWithWallStraight",BlockType.Block,new List<string> {"Left","WithWall"},"RoadIce","",""));
        inventory.articles.Add(new Article("RoadIceWithWallStraight",BlockType.Block,new List<string> {"Right","WithWall"},"RoadIce","","",new Position(new Vec3(32,0,32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("RoadIceDiagRightWithWallStraight",BlockType.Block,new List<string> {"DiagRight","Right","WithWall"},"RoadIce","",""));
        inventory.articles.Add(new Article("RoadIceDiagLeftWithWallStraight",BlockType.Block,new List<string> {"DiagLeft","Right","WithWall"},"RoadIce","","",new Position(new Vec3(96,0,64), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("RoadIceDiagRightWithWallStraight",BlockType.Block,new List<string> {"DiagRight","Left","WithWall"},"RoadIce","","",new Position(new Vec3(96,0,64), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("RoadIceDiagLeftWithWallStraight",BlockType.Block,new List<string> {"DiagLeft","Left","WithWall"},"RoadIce","",""));
    }
}
