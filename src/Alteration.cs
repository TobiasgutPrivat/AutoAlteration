using GBX.NET;
using Newtonsoft.Json;
public class Alteration {
    public static float PI = (float)Math.PI;
    public static Inventory inventory = new();

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
        json = json.Replace("\"GateSpecialBoost\"", "\"GateSpecialBoostOriented\"");
        json = json.Replace("\"GateSpecialBoost2\"", "\"GateSpecialBoost2Oriented\"");
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
    public virtual void ChangeInventory() {}

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
        //Load Nadeo Articles
        inventory = ImportVanillaInventory(AutoAlteration.ProjectFolder + "data/Inventory/BlockData.json");
        
        //Control
        // inventory.CheckDuplicates();

        //save
        inventory.articles.ForEach(x => x.cacheFilter.Clear());
        inventory.Export("Vanilla");
    }

    public static void InventoryChanges(){
        inventory.Select(BlockType.Block).Select("Gate").EditOriginal().RemoveKeyword("Gate").AddKeyword("Ring");
        inventory.Select("Special").EditOriginal().RemoveKeyword("Special");
        inventory.Select("Start&!(Slope2|Loop|DiagRight|DiagLeft|Slope|Inflatable)").EditOriginal().RemoveKeyword("Start").AddKeyword("MapStart");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Straight").Align().EditOriginal().RemoveKeyword("Straight");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("StraightX2").Align().EditOriginal().RemoveKeyword("StraightX2");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Base").Align().EditOriginal().RemoveKeyword("Base");
        inventory.RemoveArticles(inventory.Select("v2").RemoveKeyword("v2").Align());
        inventory.Select("v2").EditOriginal().RemoveKeyword("v2");
    }

    public static void TestInventory(){
        CreateInventory();
        AddCustomBlocks("");
        AddNoCPBlocks();
        AddCheckpointTrigger();
        inventory.CheckDuplicates();
        inventory.articles.ForEach(x => {
            if (x.Keywords.Any(y => y == "")){Console.WriteLine("Empty Keyword found in " + x.Name);}
            if (x.Shapes.Any(y => y == "")){Console.WriteLine("Empty Shape found in " + x.Name);}
            if (x.ToShapes.Any(y => y == "")){Console.WriteLine("Empty ToShape found in " + x.Name);}
            if (x.Surfaces.Any(y => y == "")){Console.WriteLine("Empty Surface found in " + x.Name);}
        });
    }

    public static void AddCustomBlocks(string subFolder){
        inventory.AddArticles(Directory.GetFiles(AutoAlteration.CustomBlocksFolder + subFolder, "*.Block.Gbx", SearchOption.AllDirectories).Select(x => new Article(Path.GetFileName(x)[..^10], BlockType.CustomBlock, x)).ToList());
        inventory.AddArticles(Directory.GetFiles(AutoAlteration.CustomBlocksFolder + subFolder, "*.Item.Gbx", SearchOption.AllDirectories).Select(x => new Article(Path.GetFileName(x)[..^9], BlockType.CustomItem, x)).ToList());
    }

    public static void AddNoCPBlocks(){
        Inventory tempInventory = new();
        tempInventory.AddArticles(inventory.Select(BlockType.Item).Select("Center&(Checkpoint|Multilap|MapStart)").RemoveKeyword("Center"));
        AddRoadNoCPBlocks(tempInventory,"Tech");
        AddRoadNoCPBlocks(tempInventory,"Dirt");
        AddRoadNoCPBlocks(tempInventory,"Bump");
        AddRoadNoCPBlocks(tempInventory,"Ice");
        AddPlatformNoCPBlocks(tempInventory,"Tech");
        AddPlatformNoCPBlocks(tempInventory,"Dirt");
        AddPlatformNoCPBlocks(tempInventory,"Plastic");
        AddPlatformNoCPBlocks(tempInventory,"Grass");
        AddPlatformNoCPBlocks(tempInventory,"Ice");
        AddIceRoadNoCPBlocks(tempInventory);
        tempInventory.Select("DiagRight|DiagLeft").articles.ForEach(x => {x.Width = 3;x.Height = 1;x.Length = 2;});
        tempInventory.Select("Slope2").articles.ForEach(x => {x.Width = 1;x.Height = 3;x.Length = 1;});
        tempInventory.Select("Slope|Tilt").articles.ForEach(x => {x.Width = 1;x.Height = 2;x.Length = 1;});
        tempInventory.Select("Wall").articles.ForEach(x => {x.Width = 1;x.Height = 4;x.Length = 1;});
        tempInventory.Select("!(DiagRight|DiagLeft|Slope2|Slope|Wall|Tilt)").articles.ForEach(x => {x.Width = 1;x.Height = 1;x.Length = 1;});
        inventory.AddArticles(tempInventory.articles);
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
        inventory.AddArticles(inventory.Select(BlockType.Block).Select("Checkpoint").Select(selection).RemoveKeyword("Checkpoint").AddKeyword("CheckpointTrigger").SetChain(Move(offset).Rotate(rotation)));
        inventory.AddArticles(inventory.Select(BlockType.Block).Select("Multilap").Select(selection).RemoveKeyword("Multilap").AddKeyword("MultilapTrigger").SetChain(Move(offset).Rotate(rotation)));
    }

    private static void AddRoadNoCPBlocks(Inventory tempInventory, string surface){
        tempInventory.AddArticles(new Article("Road" +surface+"SlopeStraight",BlockType.Block,["Up","Slope"],"Road" +surface,null,null));
        tempInventory.AddArticles(new Article("Road" +surface+"SlopeStraight",BlockType.Block,["Down","Slope"],"Road" +surface,null,null,Move(32,0,32).Rotate(PI,0,0)));
        tempInventory.AddArticles(new Article("Road" +surface+"TiltStraight",BlockType.Block,["Left","Tilt"],"Road" +surface,null,null,Move(32,0,32).Rotate(PI,0,0)));
        tempInventory.AddArticles(new Article("Road" +surface+"TiltStraight",BlockType.Block,["Right","Tilt"],"Road" +surface,null,null));
    }
    private static void AddPlatformNoCPBlocks(Inventory tempInventory, string surface){
        tempInventory.AddArticles(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,["Up","Slope2"],"Platform",null,surface));
        tempInventory.AddArticles(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,["Down","Slope2"],"Platform",null,surface,Move(32,0,32).Rotate(PI,0,0)));
        tempInventory.AddArticles(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,["Right","Slope2"],"Platform",null,surface,Move(32,0,0).Rotate(PI*1.5f,0,0)));
        tempInventory.AddArticles(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,["Left","Slope2"],"Platform",null,surface,Move(0,0,32).Rotate(PI*0.5f,0,0)));
        tempInventory.AddArticles(new Article("Platform" +surface+"WallStraight4",BlockType.Block,["Up","Wall"],"Platform",null,surface,Move(0,0,32).Rotate(PI*0.5f,0,0)));
        tempInventory.AddArticles(new Article("Platform" +surface+"WallStraight4",BlockType.Block,["Down","Wall"],"Platform",null,surface,Move(0,0,32).Rotate(PI*0.5f,0,0)));
        tempInventory.AddArticles(new Article("Platform" +surface+"WallStraight4",BlockType.Block,["Right","Wall"],"Platform",null,surface,Move(0,0,32).Rotate(PI*0.5f,0,0)));
        tempInventory.AddArticles(new Article("Platform" +surface+"WallStraight4",BlockType.Block,["Left","Wall"],"Platform",null,surface,Move(0,0,32).Rotate(PI*0.5f,0,0)));
    }
    private static void AddIceRoadNoCPBlocks(Inventory tempInventory){
        tempInventory.AddArticles(new Article("RoadIceWithWallStraight",BlockType.Block,["Left","WithWall"],"RoadIce",null,null));
        tempInventory.AddArticles(new Article("RoadIceWithWallStraight",BlockType.Block,["Right","WithWall"],"RoadIce",null,null,Move(32,0,32).Rotate(PI,0,0)));
        tempInventory.AddArticles(new Article("RoadIceDiagRightWithWallStraight",BlockType.Block,["DiagRight","Right","WithWall"],"RoadIce",null,null));
        tempInventory.AddArticles(new Article("RoadIceDiagLeftWithWallStraight",BlockType.Block,["DiagLeft","Right","WithWall"],"RoadIce",null,null,Move(96,0,64).Rotate(PI,0,0)));
        tempInventory.AddArticles(new Article("RoadIceDiagRightWithWallStraight",BlockType.Block,["DiagRight","Left","WithWall"],"RoadIce",null,null,Move(96,0,64).Rotate(PI,0,0)));
        tempInventory.AddArticles(new Article("RoadIceDiagLeftWithWallStraight",BlockType.Block,["DiagLeft","Left","WithWall"],"RoadIce",null,null));
    }
}
