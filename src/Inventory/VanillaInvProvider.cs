using GBX.NET;
using MathNet.Numerics.Statistics;
using Newtonsoft.Json;

/// <summary>
/// Loads vanilla articles from json data files.
/// </summary>
class VanillaArticleProvider
{
    static List<Article>? articles = null;
    static readonly float PI = (float)Math.PI;
    public static List<Article> GetArticles()
    {
        articles ??= GenerateArticles();
        return articles;
    }

    private static List<Article> GenerateArticles()
    {
        string BlockJson = File.ReadAllText(AlterationConfig.BlockDataPath);
        string ItemJson = File.ReadAllText(AlterationConfig.ItemDataPath);
        // expected json Format:
        // [
        //     {
        //         "size": {
        //             "x": int,
        //             "y": int,
        //             "z": int
        //         },
        //         "type": "block"/"item"/"pillar"
        //         "name": string,
        //     },
        //     ...
        // ]

        BlockJson = BlockPropertiesCorrections(BlockJson);
        ItemJson = BlockPropertiesCorrections(ItemJson);

        dynamic[]? BlockJsonArray = JsonConvert.DeserializeObject<dynamic[]>(BlockJson) ?? [];
        dynamic[]? ItemJsonArray = JsonConvert.DeserializeObject<dynamic[]>(ItemJson) ?? [];

        var jsonArray = BlockJsonArray.Concat(ItemJsonArray);
        Dictionary<string, Article> articles = [];
        foreach (var item in jsonArray)
        {
            BlockType blockType = BlockType.Block;
            switch ((string)item.type)
            {
                case "block":
                    articles[(string)item.name] = new Article((int)item.size.y, (int)item.size.x, (int)item.size.z, BlockType.Block, (string)item.name);
                    foreach (var pillar in item.pillar)
                    {
                        if (articles.ContainsKey((string)pillar.name)) continue;
                        //TODO fix sizes of pillars in data, temporarly using item size
                        articles[(string)pillar.name] = new Article((int)item.size.y, (int)item.size.x, (int)item.size.z, BlockType.Pillar, (string)pillar.name);
                    }
                    break;
                case "item":
                    articles[(string)item.name] = new Article((int)item.size.y, (int)item.size.x, (int)item.size.z, BlockType.Item, (string)item.name);
                    break;
                default:
                    Console.WriteLine("Blocktype missing");
                    break;
            }

        }
        Inventory inventory = new(articles.Values.ToList());
        AddCheckpointTriggers(inventory);
        AddNonCPBlocks(inventory);
        NormalizeCheckpoint(inventory);
        return inventory.articles;
    }

    private static string BlockPropertiesCorrections(string json)
    { //Hardcoded corrections, depends on imported Data
        //TODO split into block and item corrections, check whats still needed
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
        return json;
    }

    public static void AddCheckpointTriggers(Inventory inventory) {
        Vec3 midPlatform = new(16,2,16);
        AddTriggerArticle(inventory,"!Wall&!Slope2&!Slope&!Tilt&!DiagRight&!DiagLeft&!RoadIce", midPlatform,Vec3.Zero);
        AddTriggerArticle(inventory,"!WithWall&!RoadIce&DiagRight",new Vec3(48f,2,32f),new Vec3(PI * -0.148f,0f,0));
        AddTriggerArticle(inventory,"!WithWall&!RoadIce&DiagLeft",new Vec3(48f,2,32f),new Vec3(PI * 0.148f,0,0));
        float slope2 = 0.47f;
        AddTriggerArticle(inventory,"Slope2&Down", midPlatform + new Vec3(0,8,0),new Vec3(0,slope2,0));
        AddTriggerArticle(inventory,"Slope2&Up", midPlatform + new Vec3(0,8,0),new Vec3(0,-slope2,0));
        AddTriggerArticle(inventory,"Slope2&Right", midPlatform + new Vec3(0,8,0),new Vec3(0,0,slope2));
        AddTriggerArticle(inventory,"Slope2&Left", midPlatform + new Vec3(0,8,0),new Vec3(0,0,-slope2));
        AddTriggerArticle(inventory,"Slope&Down&!RoadIce", midPlatform + new Vec3(0,4,0),Vec3.Zero);
        AddTriggerArticle(inventory,"Slope&Up&!RoadIce", midPlatform + new Vec3(0,4,0),Vec3.Zero);
        AddTriggerArticle(inventory,"Tilt&Right&!RoadIce", midPlatform + new Vec3(0,4,0),Vec3.Zero);
        AddTriggerArticle(inventory,"Tilt&Left&!RoadIce", midPlatform + new Vec3(0,4,0),Vec3.Zero);

        AddTriggerArticle(inventory,"Slope&Down&RoadIce", midPlatform + new Vec3(0,7,0),Vec3.Zero);
        AddTriggerArticle(inventory,"Slope&Up&RoadIce", midPlatform + new Vec3(0,7,0),Vec3.Zero);
        AddTriggerArticle(inventory,"!Slope&!DiagRight&!DiagLeft&!WithWall&RoadIce", midPlatform + new Vec3(0,2,0),Vec3.Zero);
        AddTriggerArticle(inventory,"WithWall&RoadIce&!DiagRight&!DiagLeft", midPlatform + new Vec3(0,2,0),Vec3.Zero);
        AddTriggerArticle(inventory,"WithWall&RoadIce&DiagRight",new Vec3(48f,4,32f),new Vec3(PI * -0.148f,0f,0));
        AddTriggerArticle(inventory,"WithWall&RoadIce&DiagLeft",new Vec3(48f,4,32f),new Vec3(PI * 0.148f,0,0));

        AddTriggerArticle(inventory,"Platform&Wall&Down", new Vec3(16,16,29),new Vec3(PI,PI*0.5f,0));
        AddTriggerArticle(inventory,"Platform&Wall&Up", new Vec3(16,16,29),new Vec3(0,-PI*0.5f,0));
        AddTriggerArticle(inventory,"Platform&Wall&Right", new Vec3(16,16,29),new Vec3(PI,PI*0.5f,PI*0.5f));
        AddTriggerArticle(inventory,"Platform&Wall&Left", new Vec3(16,16,29),new Vec3(PI,PI*0.5f,-PI*0.5f));
    }

    private static void AddTriggerArticle(Inventory inventory, string selection, Vec3 offset, Vec3 rotation)
    {
        inventory.AddArticles(inventory.Select(BlockType.Block).Select("Checkpoint").Select(selection).RemoveKeyword("Checkpoint").AddKeyword("CheckpointTrigger").SetChain([new Offset(offset), new Rotate(rotation)]).getEdited());
        inventory.AddArticles(inventory.Select(BlockType.Block).Select("Multilap").Select(selection).RemoveKeyword("Multilap").AddKeyword("MultilapTrigger").SetChain([new Offset(offset), new Rotate(rotation)]).getEdited());
    }
    
    /// <summary>
    /// For Checkpoints, adds blocks without checkpoint keyword but with orientation Keywords.
    /// </summary>
    private static void AddNonCPBlocks(Inventory inventory) {
        Inventory tempInventory = new();
        tempInventory.AddArticles(inventory.Select(BlockType.Item).Select("Center&(Checkpoint|Multilap|MapStart)").RemoveKeyword("Center").Align().getAligned());
        AddRoadNoCPBlocks(tempInventory,"Tech");
        AddRoadNoCPBlocks(tempInventory,"Dirt");
        AddRoadNoCPBlocks(tempInventory,"Bump");
        AddRoadNoCPBlocks(tempInventory,"Ice");
        AddOpenRoadNoCPBlocks(tempInventory,"Tech");
        AddOpenRoadNoCPBlocks(tempInventory,"Dirt");
        AddOpenRoadNoCPBlocks(tempInventory,"Bump");
        AddOpenRoadNoCPBlocks(tempInventory,"Ice");
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

    private static void AddOpenRoadNoCPBlocks(Inventory tempInventory, string surface){
        tempInventory.AddArticles(new Article("Open" +surface+"RoadSlope2Straight",BlockType.Block,["Up","Slope2","Open" + surface + "Road"]));
        tempInventory.AddArticles(new Article("Open" +surface+"RoadSlope2Straight",BlockType.Block,["Down","Slope2","Open" + surface + "Road"], moveChain: [new Offset(32,0,32),new Rotate(PI,0,0)]));
        tempInventory.AddArticles(new Article("Open" +surface+"RoadStraightTilt2",BlockType.Block,["Left","Slope2","Open" + surface + "Road"], moveChain: [new Offset(32,0,0),new Rotate(PI*1.5f,0,0)]));
        tempInventory.AddArticles(new Article("Open" +surface+"RoadStraightTilt2",BlockType.Block,["Right","Slope2","Open" + surface + "Road"], moveChain: [new Offset(0,0,32),new Rotate(PI*0.5f,0,0)]));
    }

    private static void AddRoadNoCPBlocks(Inventory tempInventory, string surface){
        tempInventory.AddArticles(new Article("Road" +surface+"SlopeStraight",BlockType.Block,["Up","Slope","Road" + surface]));
        tempInventory.AddArticles(new Article("Road" +surface+"SlopeStraight",BlockType.Block,["Down","Slope","Road" + surface], moveChain: [new Offset(32,0,32),new Rotate(PI,0,0)]));
        tempInventory.AddArticles(new Article("Road" +surface+"TiltStraight",BlockType.Block,["Left","Tilt","Road" + surface], moveChain: [new Offset(32,0,32),new Rotate(PI,0,0)]));
        tempInventory.AddArticles(new Article("Road" +surface+"TiltStraight",BlockType.Block,["Right","Tilt","Road" + surface]));
    }
    private static void AddPlatformNoCPBlocks(Inventory tempInventory, string surface){
        tempInventory.AddArticles(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,["Up","Slope2","Platform",surface]));
        tempInventory.AddArticles(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,["Down","Slope2","Platform",surface],null,[new Offset(32,0,32),new Rotate(PI,0,0)]));
        tempInventory.AddArticles(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,["Right","Slope2","Platform",surface],null,[new Offset(32,0,0),new Rotate(PI*1.5f,0,0)]));
        tempInventory.AddArticles(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,["Left","Slope2","Platform",surface],null,[new Offset(0,0,32),new Rotate(PI*0.5f,0,0)]));
        tempInventory.AddArticles(new Article("Platform" +surface+"WallStraight4",BlockType.Block,["Up","Wall","Platform",surface],null,[new Offset(0,0,32),new Rotate(PI*0.5f,0,0)]));
        tempInventory.AddArticles(new Article("Platform" +surface+"WallStraight4",BlockType.Block,["Down","Wall","Platform",surface],null,[new Offset(0,0,32),new Rotate(PI*0.5f,0,0)]));
        tempInventory.AddArticles(new Article("Platform" +surface+"WallStraight4",BlockType.Block,["Right","Wall","Platform",surface],null,[new Offset(0,0,32),new Rotate(PI*0.5f,0,0)]));
        tempInventory.AddArticles(new Article("Platform" +surface+"WallStraight4",BlockType.Block,["Left","Wall","Platform",surface],null,[new Offset(0,0,32),new Rotate(PI*0.5f,0,0)]));
    }
    private static void AddIceRoadNoCPBlocks(Inventory tempInventory)
    {
        tempInventory.AddArticles(new Article("RoadIceWithWallStraight", BlockType.Block, ["Left", "WithWall", "RoadIce"]));
        tempInventory.AddArticles(new Article("RoadIceWithWallStraight", BlockType.Block, ["Right", "WithWall", "RoadIce"], null, [new Offset(32, 0, 32), new Rotate(PI, 0, 0)]));
        tempInventory.AddArticles(new Article("RoadIceDiagRightWithWallStraight", BlockType.Block, ["DiagRight", "Right", "WithWall", "RoadIce"], null));
        tempInventory.AddArticles(new Article("RoadIceDiagLeftWithWallStraight", BlockType.Block, ["DiagLeft", "Right", "WithWall", "RoadIce"], null, [new Offset(96, 0, 64), new Rotate(PI, 0, 0)]));
        tempInventory.AddArticles(new Article("RoadIceDiagRightWithWallStraight", BlockType.Block, ["DiagRight", "Left", "WithWall", "RoadIce"], null, [new Offset(96, 0, 64), new Rotate(PI, 0, 0)]));
        tempInventory.AddArticles(new Article("RoadIceDiagLeftWithWallStraight", BlockType.Block, ["DiagLeft", "Left", "WithWall", "RoadIce"], null));
    }
    
    private static void NormalizeCheckpoint(Inventory inventory)
    {
        // TODO check if wanted, could cause issues with custom block set aligning
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Straight").Align().getAligned().EditOriginal().RemoveKeyword("Straight");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("StraightX2").Align().getAligned().EditOriginal().RemoveKeyword("StraightX2");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Base").Align().getAligned().EditOriginal().RemoveKeyword("Base");
    }
}