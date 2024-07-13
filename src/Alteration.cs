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
    public static Inventory ImportVanillaInventory(string path){
        string json = File.ReadAllText(path);
        json = json.Replace("PlatformGrassSlope2UTop", "PlatformGrasssSlope2UTop");
        json = json.Replace("PlatForm", "Platform");
        json = json.Replace("ShowFogger8M", "ShowFogger8m");
        json = json.Replace("ShowFogger16M", "ShowFogger16m");
        json = json.Replace("CheckPoint", "Checkpoint");
        json = json.Replace("DecoHillSlope2curve2Out", "DecoHillSlope2Curve2Out");
        json = json.Replace("RoadIceWithWallDiagLeftStraight", "RoadIceDiagLeftWithWallStraight");
        json = json.Replace("RoadIceWithWallDiagRightStraight", "RoadIceDiagRightWithWallStraight");
        json = json[..1] + "{\"Height\":1,\"Width\":1,\"Length\":1,\"type\":\"Block\",\"Name\":\"OpenIceRoadToZoneRight\",\"Theme\":false,\"DefaultRotation\":false}," + json[1..];
        
        var jsonArray = JsonConvert.DeserializeObject<dynamic[]>(json);
        Inventory temp = new();
        foreach (var item in jsonArray)
        {
            temp.articles.Add(new Article((int)item.Height, (int)item.Width, (int)item.Length, (string)item.type, (string)item.Name, (bool)item.Theme, (bool)item.DefaultRotation));
        }
        return temp;
    }

    public Alteration(){}
    public virtual void Run(Map map) {}
    public virtual void AddArticles() {}

    public static MoveChain Move(float x, float y, float z) =>
        Move(new Vec3(x,y,z));

    public static MoveChain Move(Vec3 vector) {
        MoveChain moveChain = new();
        moveChain.Move(vector);
        return moveChain;
    }

    public static MoveChain Rotate(float x, float y, float z) =>
        Rotate(new Vec3(x,y,z));

    public static MoveChain Rotate(Vec3 vector) {
        MoveChain moveChain = new();
        moveChain.Rotate(vector);
        return moveChain;
    }

    public static MoveChain RotateMid(float x, float y, float z) =>
        RotateMid(new Vec3(x,y,z));

    public static MoveChain RotateMid(Vec3 vector) {
        MoveChain moveChain = new();
        moveChain.RotateMid(vector);
        return moveChain;
    }

    public static void CreateInventory() {
        devMode = true;
        //Load Nadeo Articles
        inventory = ImportVanillaInventory(ProjectFolder + "src/Inventory/BlockData.json");

        //Inventory Changes
        inventory.Select(BlockType.Block).Select("Gate").EditOriginal().RemoveKeyword("Gate").AddKeyword("Ring");
        inventory.Select("Special").EditOriginal().RemoveKeyword("Special");
        inventory.Select("Start&!(Slope2|Loop|DiagRight|DiagLeft|Slope|Inflatable)").EditOriginal().RemoveKeyword("Start").AddKeyword("MapStart");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Straight").Align().EditOriginal().RemoveKeyword("Straight");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("StraightX2").Align().EditOriginal().RemoveKeyword("StraightX2");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Base").Align().EditOriginal().RemoveKeyword("Base");
        
        //Control
        inventory.CheckDuplicates();

        //save
        inventory.articles.ForEach(x => x.cacheFilter.Clear());
        File.WriteAllText(ProjectFolder + "dev/Inventory.json", JsonConvert.SerializeObject(inventory.articles));
        devMode = false;
    }

    public static void AddCustomBlocks(string subFolder){
        inventory.AddTemp(Directory.GetFiles(CustomBlocksFolder + subFolder, "*.Block.Gbx", SearchOption.AllDirectories).Select(x => new Article(Path.GetFileName(x)[..^10], BlockType.CustomBlock, x)).ToList());
        inventory.AddTemp(Directory.GetFiles(CustomBlocksFolder + subFolder, "*.Item.Gbx", SearchOption.AllDirectories).Select(x => new Article(Path.GetFileName(x)[..^9], BlockType.CustomItem, x)).ToList());
    }

    public static void AddCheckpointBlocks(){
        inventory.AddTemp(inventory.Select(BlockType.Item).Select("Center&(Checkpoint|Multilap|MapStart|Finish)").RemoveKeyword("Center").RemoveKeyword("v2"));
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

    public static void AddCheckpointTrigger(){
        Vec3 midPlatform = new(16,2,16);
        CreateTriggerArticle("!Wall&!Slope2&!Slope&!Tilt&!DiagRight&!DiagLeft&!RoadIce", midPlatform,Vec3.Zero);
        CreateTriggerArticle("!WithWall&!RoadIce&DiagRight",new Vec3(48f,0,32f),new Vec3(PI * -0.148f,0f,0));
        CreateTriggerArticle("!WithWall&!RoadIce&DiagLeft",new Vec3(48f,0,32f),new Vec3(PI * 0.148f,0,0));
        float slope2 = 0.47f;
        CreateTriggerArticle("Slope2&Down", midPlatform + new Vec3(0,8,0),new Vec3(0,slope2,0));
        CreateTriggerArticle("Slope2&Up", midPlatform + new Vec3(0,8,0),new Vec3(0,-slope2,0));
        CreateTriggerArticle("Slope2&Right", midPlatform + new Vec3(0,8,0),new Vec3(0,0,slope2));
        CreateTriggerArticle("Slope2&Left", midPlatform + new Vec3(0,8,0),new Vec3(0,0,-slope2));
        CreateTriggerArticle("Slope&Down&!RoadIce", midPlatform + new Vec3(0,4,0),Vec3.Zero);
        CreateTriggerArticle("Slope&Up&!RoadIce", midPlatform + new Vec3(0,4,0),Vec3.Zero);
        CreateTriggerArticle("Tilt&Right&!RoadIce", midPlatform + new Vec3(0,4,0),Vec3.Zero);
        CreateTriggerArticle("Tilt&Left&!RoadIce", midPlatform + new Vec3(0,4,0),Vec3.Zero);

        CreateTriggerArticle("Slope&Down&RoadIce", midPlatform + new Vec3(0,7,0),Vec3.Zero);
        CreateTriggerArticle("Slope&Up&RoadIce", midPlatform + new Vec3(0,7,0),Vec3.Zero);
        CreateTriggerArticle("!Slope&!DiagRight&!DiagLeft&!WithWall&RoadIce", midPlatform + new Vec3(0,2,0),Vec3.Zero);
        CreateTriggerArticle("WithWall&RoadIce&!DiagRight&!DiagLeft", midPlatform + new Vec3(0,2,0),Vec3.Zero);
        CreateTriggerArticle("WithWall&RoadIce&DiagRight",new Vec3(48f,4,32f),new Vec3(PI * -0.148f,0f,0));
        CreateTriggerArticle("WithWall&RoadIce&DiagLeft",new Vec3(48f,4,32f),new Vec3(PI * 0.148f,0,0));

        CreateTriggerArticle("Platform&Wall&Down", new Vec3(16,16,29),new Vec3(PI,PI*0.5f,0));
        CreateTriggerArticle("Platform&Wall&Up", new Vec3(16,16,29),new Vec3(0,-PI*0.5f,0));
        CreateTriggerArticle("Platform&Wall&Right", new Vec3(16,16,29),new Vec3(PI,PI*0.5f,PI*0.5f));
        CreateTriggerArticle("Platform&Wall&Left", new Vec3(16,16,29),new Vec3(PI,PI*0.5f,-PI*0.5f));
    }

    private static void CreateTriggerArticle(string selection,Vec3 offset, Vec3 rotation) {
        inventory.AddTemp(inventory.Select(BlockType.Block).Select("Checkpoint").Select(selection).RemoveKeyword("Checkpoint").AddKeyword("CheckpointTrigger").SetChain(Move(offset).Rotate(rotation)));
        inventory.AddTemp(inventory.Select(BlockType.Block).Select("Multilap").Select(selection).RemoveKeyword("Multilap").AddKeyword("MultilapTrigger").SetChain(Move(offset).Rotate(rotation)));
    }

    private static void AddRoadNoCPBlocks(string surface){
        inventory.AddTemp(new Article("Road" +surface+"SlopeStraight",BlockType.Block,new List<string> {"Up","Slope"},"Road" +surface,"",""));
        inventory.AddTemp(new Article("Road" +surface+"SlopeStraight",BlockType.Block,new List<string> {"Down","Slope"},"Road" +surface,"","",Move(32,0,32).Rotate(PI,0,0)));
        inventory.AddTemp(new Article("Road" +surface+"TiltStraight",BlockType.Block,new List<string> {"Left","Tilt"},"Road" +surface,"","",Move(32,0,32).Rotate(PI,0,0)));
        inventory.AddTemp(new Article("Road" +surface+"TiltStraight",BlockType.Block,new List<string> {"Right","Tilt"},"Road" +surface,"",""));
    }
    private static void AddPlatformNoCPBlocks(string surface){
        inventory.AddTemp(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Up","Slope2"},"Platform","",surface));
        inventory.AddTemp(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Down","Slope2"},"Platform","",surface,Move(32,0,32).Rotate(PI,0,0)));
        inventory.AddTemp(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Right","Slope2"},"Platform","",surface,Move(32,0,0).Rotate(PI*1.5f,0,0)));
        inventory.AddTemp(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,new List<string> {"Left","Slope2"},"Platform","",surface,Move(0,0,32).Rotate(PI*0.5f,0,0)));
        inventory.AddTemp(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Up","Wall"},"Platform","",surface,Move(0,0,32).Rotate(PI*0.5f,0,0)));
        inventory.AddTemp(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Down","Wall"},"Platform","",surface,Move(0,0,32).Rotate(PI*0.5f,0,0)));
        inventory.AddTemp(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Right","Wall"},"Platform","",surface,Move(0,0,32).Rotate(PI*0.5f,0,0)));
        inventory.AddTemp(new Article("Platform" +surface+"WallStraight4",BlockType.Block,new List<string> {"Left","Wall"},"Platform","",surface,Move(0,0,32).Rotate(PI*0.5f,0,0)));
    }
    private static void AddIceRoadNoCPBlocks(){
        inventory.AddTemp(new Article("RoadIceWithWallStraight",BlockType.Block,new List<string> {"Left","WithWall"},"RoadIce","",""));
        inventory.AddTemp(new Article("RoadIceWithWallStraight",BlockType.Block,new List<string> {"Right","WithWall"},"RoadIce","","",Move(32,0,32).Rotate(PI,0,0)));
        inventory.AddTemp(new Article("RoadIceDiagRightWithWallStraight",BlockType.Block,new List<string> {"DiagRight","Right","WithWall"},"RoadIce","",""));
        inventory.AddTemp(new Article("RoadIceDiagLeftWithWallStraight",BlockType.Block,new List<string> {"DiagLeft","Right","WithWall"},"RoadIce","","",Move(96,0,64).Rotate(PI,0,0)));
        inventory.AddTemp(new Article("RoadIceDiagRightWithWallStraight",BlockType.Block,new List<string> {"DiagRight","Left","WithWall"},"RoadIce","","",Move(96,0,64).Rotate(PI,0,0)));
        inventory.AddTemp(new Article("RoadIceDiagLeftWithWallStraight",BlockType.Block,new List<string> {"DiagLeft","Left","WithWall"},"RoadIce","",""));
    }
}
