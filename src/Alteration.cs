using GBX.NET;
using Newtonsoft.Json;
class Alteration {
    public static float PI = (float)Math.PI;
    public static string ProjectFolder = "";
    public static string CustomBlocksFolder = "";
    public static string[] Keywords = Array.Empty<string>();
    public static string[] shapeKeywords = Array.Empty<string>();
    public static string[] surfaceKeywords = Array.Empty<string>();
    public static Inventory inventory = new();
    public static bool devMode = false;
    public static int mapCount;

    public static void Load(string projectFolder) {
        ProjectFolder = projectFolder;
        CustomBlocksFolder = ProjectFolder + "src/CustomBlocks/";
        shapeKeywords = File.ReadAllLines(ProjectFolder + "src/Inventory/shapeKeywords.txt");
        surfaceKeywords = File.ReadAllLines(ProjectFolder + "src/Inventory/surfaceKeywords.txt");
        Keywords = File.ReadAllLines(ProjectFolder + "src/Inventory/Keywords.txt");
        CreateInventory();
    }

    public static Inventory ImportSerializedInventory(string path)
    {
        string filePath = path;
        string json = File.ReadAllText(filePath);
        List<Article> articles = JsonConvert.DeserializeObject<List<Article>>(json);
        return new Inventory(articles);
    }
    
    public static Inventory ImportArrayInventory(string path,BlockType blockType){
        string json = File.ReadAllText(path);
        string[] lines = JsonConvert.DeserializeObject<string[]>(json);
        return new Inventory(lines.Select(line => new Article(line,blockType)).ToList());
    }
    public static void Alter(List<Alteration> alterations, Map map) {
        foreach (Alteration alteration in alterations) {
            alteration.Run(map);
        }
        mapCount++;
    }
    public static void Alter(Alteration alteration, Map map) {
        alteration.Run(map);
        mapCount++;
    }

    public static void AlterFolder(List<Alteration> alterations, string mapFolder, string destinationFolder, string Name) {
        foreach (string mapFile in Directory.GetFiles(mapFolder, "*.map.gbx", SearchOption.TopDirectoryOnly)){
            AlterFile(alterations,mapFile,destinationFolder + Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + Name + ".map.gbx",Name);
        }
    }
    public static void AlterFolder(Alteration alteration, string mapFolder, string destinationFolder, string Name) =>
        AlterFolder(new List<Alteration>{alteration},mapFolder,destinationFolder,Name);
    
    public static void AlterAll(List<Alteration> alterations, string mapFolder, string destinationFolder, string Name) {
        AlterFolder(alterations,mapFolder,destinationFolder + Path.GetFileName(mapFolder) + " - " + Name + "/",Name);
        foreach (string Directory in Directory.GetDirectories(mapFolder, "*", SearchOption.TopDirectoryOnly))
        {
            AlterAll(alterations,Directory,destinationFolder + Directory.Substring(mapFolder.Length) + "/",Name);
        }
    }
    public static void AlterAll(Alteration alteration, string mapFolder, string destinationFolder, string Name) =>
        AlterAll(new List<Alteration>{alteration},mapFolder,destinationFolder,Name);
    
    public static void AlterFolder(List<Alteration> alterations, string mapFolder, string Name) =>
        AlterFolder(alterations,mapFolder,mapFolder,Name);
    
    public static void AlterFolder(Alteration alteration, string mapFolder, string Name) =>
        AlterFolder(alteration,mapFolder,mapFolder,Name);
    
    public static void AlterFile(List<Alteration> alterations, string mapFile, string destinationFile, string Name) {
        Map map = new Map(mapFile);
        Alter(alterations, map);
        map.map.MapName = Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + Name;
        map.Save(destinationFile);
        Console.WriteLine(destinationFile);
    }
    public static void AlterFile(Alteration alteration, string mapFile, string destinationFile, string Name) =>
        AlterFile(new List<Alteration>{alteration},mapFile,destinationFile,Name);
    
    public static void AlterFile(List<Alteration> alterations, string mapFile, string Name) =>
        AlterFile(alterations,mapFile,Path.GetDirectoryName(mapFile) + Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + Name + ".map.gbx",Name);
    
    public static void AlterFile(Alteration alteration, string mapFile, string Name) =>
        AlterFile(alteration,mapFile,Path.GetDirectoryName(mapFile) + "\\" +  Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + Name + ".map.gbx",Name);

    public Alteration(){}
    public virtual void Run(Map map) {}

    public static void CreateInventory() {
        devMode = true;
        //Load Nadeo Articles
        Inventory items = ImportArrayInventory(ProjectFolder + "src/Inventory/ItemNames.json",BlockType.Item);
        Inventory blocks = ImportArrayInventory(ProjectFolder + "src/Inventory/BlockNames.json",BlockType.Block);
        //Fix Gate naming
        blocks.Select("Gate").EditOriginal().RemoveKeyword("Gate").AddKeyword("Ring");

        //Init Inventory
        inventory.AddArticles(items.articles);
        inventory.AddArticles(blocks.articles);
        //CustomBlocks
        inventory.AddArticles(Directory.GetFiles(CustomBlocksFolder, "*.Block.Gbx", SearchOption.AllDirectories).Select(x => new Article(Path.GetFileName(x)[..^10], BlockType.CustomBlock, x)).ToList());
        inventory.AddArticles(Directory.GetFiles(CustomBlocksFolder, "*.Item.Gbx", SearchOption.AllDirectories).Select(x => new Article(Path.GetFileName(x)[..^9], BlockType.CustomItem, x)).ToList());

        inventory.Select("Special").EditOriginal().RemoveKeyword("Special");

        inventory.articles.ForEach(x => x.Original = true);
        //save Dev Inventory
        inventory.articles.ForEach(x => x.cacheFilter.Clear());
        string json = JsonConvert.SerializeObject(inventory.articles);
        File.WriteAllText(ProjectFolder + "dev/Initial_Inventory.json", json);

        //Add Articles with unnecessary keywords removed
        AddCheckpointBlocks();
        AddCheckpointTrigger();
        inventory.AddArticles(inventory.Select("Start&!(Slope2|Loop|DiagRight|DiagLeft|Slope|Inflatable)").RemoveKeyword("Start").AddKeyword("MapStart"));
        SetSizes();
        
        //Control
        // inventory.checkDuplicates();

        //save
        inventory.articles.ForEach(x => x.cacheFilter.Clear());
        json = JsonConvert.SerializeObject(inventory.articles);
        File.WriteAllText(ProjectFolder + "dev/Inventory.json", json);

        devMode = false;
    }


    private static void AddCheckpointBlocks(){
        inventory.AddArticles(inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Straight").Align().RemoveKeyword("Straight"));
        inventory.AddArticles(inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("StraightX2").Align().RemoveKeyword("StraightX2"));
        inventory.AddArticles(inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Base").Align().RemoveKeyword("Base"));
        AddRoadNoCPBlocks("Tech");
        AddRoadNoCPBlocks("Dirt");
        AddRoadNoCPBlocks("Bump");
        AddRoadNoCPBlocks("Ice");
        AddPlatformNoCPBlocks("Tech");
        AddPlatformNoCPBlocks("Dirt");
        AddPlatformNoCPBlocks("Plastic");
        AddPlatformNoCPBlocks("Grass");
        AddPlatformNoCPBlocks("Ice");
        AddIceRoadNoCPBlocks();
    }

    private static void AddCheckpointTrigger(){
        Vec3 midPlatform = new(16,2,16);
        CreateTriggerArticle("!Wall&!Slope2&!Slope&!Tilt&!DiagRight&!DiagLeft&!RoadIce", midPlatform,Vec3.Zero);
        CreateTriggerArticle("!WithWall&!RoadIce&DiagRight",new Vec3(48f,0,32f),new Vec3(PI * -0.148f,0f,0));
        CreateTriggerArticle("!WithWall&!RoadIce&DiagLeft",new Vec3(48f,0,32f),new Vec3(PI * 0.148f,0,0));
        float slope2 = 0.47f;
        CreateTriggerArticle("Slope2&Down", midPlatform + new Vec3(0,8,0),new Vec3(0,slope2,0));
        CreateTriggerArticle("Slope2&Up", midPlatform + new Vec3(0,8,0),new Vec3(0,-slope2,0));
        CreateTriggerArticle("Slope2&Right", midPlatform + new Vec3(0,8,0),new Vec3(0,0,slope2));
        CreateTriggerArticle("Slope2&Left", midPlatform + new Vec3(0,8,0),new Vec3(0,0,-slope2));
        float slope = 0;//slope2/2
        CreateTriggerArticle("Slope&Down&!RoadIce", midPlatform + new Vec3(0,4,0),new Vec3(0,slope,0));
        CreateTriggerArticle("Slope&Up&!RoadIce", midPlatform + new Vec3(0,4,0),new Vec3(0,-slope,0));
        CreateTriggerArticle("Tilt&Right&!RoadIce", midPlatform + new Vec3(0,4,0),new Vec3(0,0,-slope));
        CreateTriggerArticle("Tilt&Left&!RoadIce", midPlatform + new Vec3(0,4,0),new Vec3(0,0,slope));

        CreateTriggerArticle("Slope&Down&RoadIce", midPlatform + new Vec3(0,7,0),new Vec3(0,slope,0));
        CreateTriggerArticle("Slope&Up&RoadIce", midPlatform + new Vec3(0,7,0),new Vec3(0,-slope,0));
        CreateTriggerArticle("!Slope&!DiagRight&!DiagLeft&!WithWall&RoadIce", midPlatform + new Vec3(0,2,0),new Vec3(0,0,0));
        CreateTriggerArticle("WithWall&RoadIce&!DiagRight&!DiagLeft", midPlatform + new Vec3(0,2,0),new Vec3(0,0,0));
        CreateTriggerArticle("WithWall&RoadIce&DiagRight",new Vec3(48f,4,32f),new Vec3(PI * -0.148f,0f,0));
        CreateTriggerArticle("WithWall&RoadIce&DiagLeft",new Vec3(48f,4,32f),new Vec3(PI * 0.148f,0,0));

        CreateTriggerArticle("Platform&Wall&Down", new Vec3(16,16,29),new Vec3(PI,PI*0.5f,0));
        CreateTriggerArticle("Platform&Wall&Up", new Vec3(16,16,29),new Vec3(0,-PI*0.5f,0));
        CreateTriggerArticle("Platform&Wall&Right", new Vec3(16,16,29),new Vec3(PI,PI*0.5f,PI*0.5f));
        CreateTriggerArticle("Platform&Wall&Left", new Vec3(16,16,29),new Vec3(PI,PI*0.5f,-PI*0.5f));
    }

    private static void CreateTriggerArticle(string selection,Vec3 offset, Vec3 rotation) {
        inventory.AddArticles(inventory.Select(BlockType.Block).Select("Checkpoint").Select(selection).RemoveKeyword("Checkpoint").AddKeyword("CheckpointTrigger").ChangePosition(new Position(offset,rotation)));
        inventory.AddArticles(inventory.Select(BlockType.Block).Select("Multilap").Select(selection).RemoveKeyword("Multilap").AddKeyword("MultilapTrigger").ChangePosition(new Position(offset,rotation)));
    }

    private static void AddRoadNoCPBlocks(string surface){
        inventory.articles.Add(new Article("Road" +surface+"SlopeStraight",BlockType.Block,new List<string> {"Up","Slope"},"Road" +surface,"",""));
        inventory.articles.Add(new Article("Road" +surface+"SlopeStraight",BlockType.Block,new List<string> {"Down","Slope"},"Road" +surface,"","",new Position(new Vec3(32,0,32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("Road" +surface+"TiltStraight",BlockType.Block,new List<string> {"Left","Tilt"},"Road" +surface,"","",new Position(new Vec3(32,0,32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("Road" +surface+"TiltStraight",BlockType.Block,new List<string> {"Right","Tilt"},"Road" +surface,"",""));
    }
    private static void AddPlatformNoCPBlocks(string surface){
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Up","Slope2"},"Platform","",surface));
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Down","Slope2"},"Platform","",surface,new Position(new Vec3(32,0,32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Right","Slope2"},"Platform","",surface,new Position(new Vec3(32,0,0), new Vec3(PI*1.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Left","Slope2"},"Platform","",surface,new Position(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Up","Wall"},"Platform","",surface,new Position(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Down","Wall"},"Platform","",surface,new Position(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Right","Wall"},"Platform","",surface,new Position(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0))));
        inventory.articles.Add(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Left","Wall"},"Platform","",surface,new Position(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0))));
    }
    private static void AddIceRoadNoCPBlocks(){
        inventory.articles.Add(new Article("RoadIceWithWallStraight",BlockType.Block,new List<string> {"Left","WithWall"},"RoadIce","",""));
        inventory.articles.Add(new Article("RoadIceWithWallStraight",BlockType.Block,new List<string> {"Right","WithWall"},"RoadIce","","",new Position(new Vec3(32,0,32), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("RoadIceDiagRightWithWallStraight",BlockType.Block,new List<string> {"DiagRight","Right","WithWall"},"RoadIce","",""));
        inventory.articles.Add(new Article("RoadIceDiagLeftWithWallStraight",BlockType.Block,new List<string> {"DiagLeft","Right","WithWall"},"RoadIce","","",new Position(new Vec3(96,0,64), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("RoadIceDiagRightWithWallStraight",BlockType.Block,new List<string> {"DiagRight","Left","WithWall"},"RoadIce","","",new Position(new Vec3(96,0,64), new Vec3(PI,0,0))));
        inventory.articles.Add(new Article("RoadIceDiagLeftWithWallStraight",BlockType.Block,new List<string> {"DiagLeft","Left","WithWall"},"RoadIce","",""));
    }
    
    private static void SetSizes(){
        // Road Type
        inventory.Select(BlockType.Block).Select("Road&Curve2&(!In|!Out)").EditOriginal().Width(2).Length(2); // Might need a `&(!In|!Out)`, not tested yet... // Also catches 3-nn
        inventory.Select(BlockType.Block).Select("Road&Curve3&(!In|!Out)").EditOriginal().Width(3).Length(3); // Might need a `&(!In|!Out)`, not tested yet... // Also catches 3-nn
        inventory.Select(BlockType.Block).Select("Road&Curve4&(!In|!Out)").EditOriginal().Width(4).Length(4); // Might need a `&(!In|!Out)`, not tested yet... // Also catches 3-nn
        inventory.Select(BlockType.Block).Select("Road&Curve5&(!In|!Out)").EditOriginal().Width(5).Length(5); // Might need a `&(!In|!Out)`, not tested yet... // Also catches 3-nn

        inventory.Select(BlockType.Block).Select("Road&ChicaneX2").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("Road&ChicaneX3").EditOriginal().Width(2).Length(3);

        inventory.Select(BlockType.Block).Select("Road&Branch&Straight&X4").EditOriginal().Width(2).Length(4);
        inventory.Select(BlockType.Block).Select("Road&Branch&Curve3").EditOriginal().Width(3).Length(3); // Redundant because of Curve3
        inventory.Select(BlockType.Block).Select("Road&Branch&YShaped&2X3").EditOriginal().Width(2).Length(3); // Redundant because of 2X3
        inventory.Select(BlockType.Block).Select("Road&Branch&Diag&(Left|Right)").EditOriginal().Width(1).Length(2);

        inventory.Select(BlockType.Block).Select("Road&Slope&(Base|Start|End)&2x1").EditOriginal().Width(2).Length(1); // Redundant because of 2x1
        inventory.Select(BlockType.Block).Select("Road&Slope&(UTopX2|UBottomX2)").EditOriginal().Width(2).Length(2); // Redundant because of 2X // InGround vaiant not included, but should still be caught by UBottom

        inventory.Select(BlockType.Block).Select("Road&Tilt&Transition2&((Up|Down)&(Left|Right))").EditOriginal().Width(1).Length(2);
        inventory.Select(BlockType.Block).Select("Road&Tilt&Transition2&Curve&(In|Out)").EditOriginal().Width(1).Length(2);

        inventory.Select(BlockType.Block).Select("(RoadTech|RoadDirt|RoadBump|RoadIce)&(DiagLeft|DiagRight)").EditOriginal().Width(3).Length(2); // Original example by Tobias

        inventory.Select(BlockType.Block).Select("Road&Diag&(Left|Right)&Chicane&Right").EditOriginal().Width(2).Length(1);
        inventory.Select(BlockType.Block).Select("Road&Diag&(Left|Right)&Chicane&Left").EditOriginal().Width(3).Length(1);

        inventory.Select(BlockType.Block).Select("Road&Diag&(Left|Right)&Start&Straight&X2").EditOriginal().Width(2).Length(1);
        inventory.Select(BlockType.Block).Select("Road&Diag&(Left|Right)&Start&Curve1&In").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("Road&Diag&(Left|Right)&Start&Curve2&In").EditOriginal().Width(2).Length(3);

        inventory.Select(BlockType.Block).Select("Road&Diag&(Left|Right)&Start&Curve1&Out").EditOriginal().Width(2).Length(1);
        inventory.Select(BlockType.Block).Select("Road&Diag&(Left|Right)&Start&Curve2&Out").EditOriginal().Width(3).Length(2);

        inventory.Select(BlockType.Block).Select("Road&Diag&Switch&Curve1").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("Road&Diag&Switch&Curve2").EditOriginal().Width(3).Length(3);

        inventory.Select(BlockType.Block).Select("Road&Loop6X").EditOriginal().Width(3).Length(1);
        inventory.Select(BlockType.Block).Select("Road&Loop11X").EditOriginal().Width(4).Length(2);


        // Platform
        inventory.Select(BlockType.Block).Select("Platform&Curve2&In").EditOriginal().Width(1).Length(1); // Should be included "Platform[Surface]Curve2In" is 1x1
        inventory.Select(BlockType.Block).Select("Platform&Curve3&In").EditOriginal().Width(2).Length(2);

        inventory.Select(BlockType.Block).Select("Platform&Road&Tech&Diag&(Right|Left)").EditOriginal().Width(2).Length(1); // Don't rememver how the 'To' works :xdd:

        inventory.Select(BlockType.Block).Select("Platform&Slope2&Curve2&(Out|In)").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("Platform&Slope2&Curve3&(Out|In)").EditOriginal().Width(3).Length(3);

        inventory.Select(BlockType.Block).Select("Platform&Tilt&Transition2").EditOriginal().Width(1).Length(2);
        inventory.Select(BlockType.Block).Select("Platform&Slope2&(Start2|End2)").EditOriginal().Width(1).Length(2);

        inventory.Select(BlockType.Block).Select("Platform&Loop&Start&Curve2&In").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("Platform&Loop&Start&Curve3&In").EditOriginal().Width(3).Length(3);

        inventory.Select(BlockType.Block).Select("Platform&Loop&Start&Curve1&Out").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("Platform&Loop&Start&Curve2&Out").EditOriginal().Width(3).Length(3);
        inventory.Select(BlockType.Block).Select("Platform&Loop&Start&Curve3&Out").EditOriginal().Width(4).Length(4);

        inventory.Select(BlockType.Block).Select("Platform&Loop&Start&1x2&Curve2&In").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("Platform&Loop&Start&1x2&Curve3&In").EditOriginal().Width(3).Length(3);
        inventory.Select(BlockType.Block).Select("Platform&Loop&Start&1x2&Curve1&Out").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("Platform&Loop&Start&1x2&Curve2&Out").EditOriginal().Width(3).Length(3);
        inventory.Select(BlockType.Block).Select("Platform&Loop&Start&1x1&Curve2&In").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("Platform&Loop&Start&1x1&Curve3&In").EditOriginal().Width(3).Length(3);
        inventory.Select(BlockType.Block).Select("Platform&Loop&Start&1x1&Curve1&Out").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("Platform&Loop&Start&1x1&Curve2&Out").EditOriginal().Width(3).Length(3);

        inventory.Select(BlockType.Block).Select("Platform&Loop&Out&Start&Curve1").EditOriginal().Width(2).Length(2); // Counts for Curve1In too
        inventory.Select(BlockType.Block).Select("Platform&Loop&Out&Start&Curve2").EditOriginal().Width(3).Length(3); // Counts for Curve2In too
        inventory.Select(BlockType.Block).Select("Platform&Loop&Out&Start&Curve3").EditOriginal().Width(4).Length(4); // Counts for Curve3In too

        inventory.Select(BlockType.Block).Select("Platform&Loop&End&Curve2&In").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("Platform&Loop&End&Curve3&In").EditOriginal().Width(3).Length(3);
        inventory.Select(BlockType.Block).Select("Platform&Loop&End&Curve1&Out").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("Platform&Loop&End&Curve2&Out").EditOriginal().Width(3).Length(3);
        inventory.Select(BlockType.Block).Select("Platform&Loop&End&Curve3&Out").EditOriginal().Width(4).Length(4);

        inventory.Select(BlockType.Block).Select("Platform&Wall&Curve2").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("Platform&Wall&Curve3").EditOriginal().Width(3).Length(3);



        // Deco platform
        inventory.Select(BlockType.Block).Select("DecoPlatform&Curve2").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("DecoPlatform&Curve3").EditOriginal().Width(3).Length(3);
        inventory.Select(BlockType.Block).Select("DecoPlatform&Curve4").EditOriginal().Width(4).Length(4);
        inventory.Select(BlockType.Block).Select("DecoPlatform&Curve5").EditOriginal().Width(5).Length(5);
        //                                        DecoPlatform Curve
        inventory.Select(BlockType.Block).Select("DecoPlatform&Curve2&In").EditOriginal().Width(1).Length(1); // Should be included "Platform[Surface]Curve2In" is 1x1

        inventory.Select(BlockType.Block).Select("DecoPlatform&Curve3&In").EditOriginal().Width(2).Length(2);

        inventory.Select(BlockType.Block).Select("DecoPlatform&Slope2&Start&Curve2&(In|Out)").EditOriginal().Width(2).Length(2);

        inventory.Select(BlockType.Block).Select("DecoPlatform&Slope2&(Start2|End2)&!Curve").EditOriginal().Width(1).Length(2);
        inventory.Select(BlockType.Block).Select("DecoPlatform&Slope2&(Start2|End2)&Curve2").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("DecoPlatform&Slope2&(Start2|End2)&Curve4").EditOriginal().Width(4).Length(4);

        inventory.Select(BlockType.Block).Select("DecoPlatform&Slope2&Base&Slope2&Base2").EditOriginal().Width(1).Length(2);
        //                                        DecoPlatform Slope2 Base To Slope2 Base2 (right / left)
        inventory.Select(BlockType.Block).Select("DecoPlatform&Slope2&Curve2").EditOriginal().Width(2).Length(2);
        //                                        DecoPlatform Slope2 Start Curve2In (right / left)

        inventory.Select(BlockType.Block).Select("DecoPlatform&Slope2&Curve2").EditOriginal().Width(2).Length(2);
        // Deco Hill
        inventory.Select(BlockType.Block).Select("Deco&Hill&Slope2&StraightX2").EditOriginal().Width(2).Length(1);
        inventory.Select(BlockType.Block).Select("Deco&Hill&Slope2&Curve2").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("Deco&Hill&Slope2&ChicaneX2").EditOriginal().Width(2).Length(2); // Might be &Chicane&X2 or &((Chicane&X2)| (ChicaneX2), not tested yet
        //                                        Deco Hill Slope2 ChicaneX2
        inventory.Select(BlockType.Block).Select("Deco&Hill&Slope4&Base4&Curve").EditOriginal().Width(4).Length(4);
        inventory.Select(BlockType.Block).Select("Deco&Hill&Slope2&Start2&Base5").EditOriginal().Width(4).Length(4);
        
        // Deco Cliff
        inventory.Select(BlockType.Block).Select("Deco&Cliff10&Straight&!Small").EditOriginal().Width(4).Length(2); // Original example by Tobias
        inventory.Select(BlockType.Block).Select("Deco&Cliff10&Straight&Small").EditOriginal().Width(1).Length(2); // Original example by Tobias

        inventory.Select(BlockType.Block).Select("Deco&Cliff10&Corner&In&!Small").EditOriginal().Width(4).Length(4);
        inventory.Select(BlockType.Block).Select("Deco&Cliff10&Corner&In&Small").EditOriginal().Width(2).Length(2);
        //                                        Deco Cliff10 Corner In
        inventory.Select(BlockType.Block).Select("Deco&Cliff10&Corner&Out&!Small").EditOriginal().Width(2).Length(2); // Testing 2x2, might be 3x3 xdd
        inventory.Select(BlockType.Block).Select("Deco&Cliff10&Corner&Out&Small").EditOriginal().Width(2).Length(2);

        inventory.Select(BlockType.Block).Select("Deco&Cliff10&DiagOut&!Small").EditOriginal().Width(3).Length(3);
        inventory.Select(BlockType.Block).Select("Deco&Cliff10&DiagOut&Small").EditOriginal().Width(2).Length(2);
        inventory.Select(BlockType.Block).Select("Deco&Cliff10&DiagIn&!Small").EditOriginal().Width(4).Length(4);
        inventory.Select(BlockType.Block).Select("Deco&Cliff10&DiagIn&Small").EditOriginal().Width(2).Length(2);

        inventory.Select(BlockType.Block).Select("Deco&Cliff10&End").EditOriginal().Width(6).Length(2);
        inventory.Select(BlockType.Block).Select("Deco&Cliff10&End&!Mirror").EditOriginal().Width(6).Length(2); // Is redundant

        inventory.Select(BlockType.Block).Select("Deco&Cliff8&No&Hill&Straight&!Small").EditOriginal().Width(4).Length(2);
        inventory.Select(BlockType.Block).Select("Deco&Cliff8&No&Hill&Straight&Small").EditOriginal().Width(1).Length(2);
        //                                        Deco Cliff8 No Hill Straight

        inventory.Select(BlockType.Block).Select("Deco&Cliff8&No&Hill&Corner&In&!Small").EditOriginal().Width(4).Length(4);
        inventory.Select(BlockType.Block).Select("Deco&Cliff8&No&Hill&Corner&In&Small").EditOriginal().Width(2).Length(2);


        // inventory.select(BlockType.Block).select("Deco&Cliff8&No&Hill&Corner&Out&!Simple").editOriginal().width(4).length(4); // Does not actually exist, but might in the future
        inventory.Select(BlockType.Block).Select("Deco&Cliff8&No&Hill&Corner&Out").EditOriginal().Width(2).Length(2);
        //                                        Deco Cliff8 No Hill Corner Out  Simple

        inventory.Select(BlockType.Block).Select("Deco&Cliff8&No&Hill&Diag&Out&!Small").EditOriginal().Width(4).Length(4); // Does not actually exist, but might in the future
        inventory.Select(BlockType.Block).Select("Deco&Cliff8&No&Hill&Diag&Out&Small").EditOriginal().Width(2).Length(2);
        //                                        Deco Cliff8 No Hill Diag Out Small
        //                                        Deco Cliff8 No Hill Diag Out

        inventory.Select(BlockType.Block).Select("Deco&Cliff8&No&Hill&DiagIn&!Small").EditOriginal().Width(4).Length(4);
        inventory.Select(BlockType.Block).Select("Deco&Cliff8&No&Hill&DiagIn").EditOriginal().Width(2).Length(2);
    }
}
