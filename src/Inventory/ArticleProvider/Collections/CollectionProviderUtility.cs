using GBX.NET;

public static class CollectionProviderUtility
{
    private static readonly float PI = (float)Math.PI;

    public static string NormalizeItemData(string json)
    {
        json = json.Replace("ShowFogger8M", "ShowFogger8m");
        json = json.Replace("ShowFogger16M", "ShowFogger16m");
        return json;
    }

    public static string NormalizeBlockData(string json)
    {
        json = json.Replace("PlatformGrassSlope2UTop", "PlatformGrasssSlope2UTop"); //ingame spelled with "sss"
        json = json.Replace("PlatForm", "Platform");
        json = json.Replace("CheckPoint", "Checkpoint");
        json = json.Replace("DecoHillSlope2curve2Out", "DecoHillSlope2Curve2Out");
        json = json.Replace("RoadIceWithWallDiagLeftStraight", "RoadIceDiagLeftWithWallStraight");
        json = json.Replace("RoadIceWithWallDiagRightStraight", "RoadIceDiagRightWithWallStraight");
        json = json.Replace("\"GateSpecialBoost\"", "\"GateSpecialBoostOriented\"");
        json = json.Replace("\"GateSpecialBoost2\"", "\"GateSpecialBoost2Oriented\"");
        return json;
    }

    public static List<Article> CreateCheckpointTriggers(Inventory inventory)
    {
        Inventory blocks = inventory.Select(BlockType.Block);
        Inventory checkpoints = blocks.Any(["Checkpoint", "Multilap"]);
        Vec3 midPlatform = new(16, 2, 16);
        List<Article> articles = [];
        Inventory platformWall = blocks.Select(["Platform", "Wall"]);
        Inventory Slope = blocks.Select("Slope");
        Inventory Slope2 = blocks.Select("Slope2");
        Inventory Tilt = blocks.Select("Tilt");
        Inventory Wall = blocks.Select("Wall");
        Inventory DiagRight = blocks.Select("DiagRight");
        Inventory DiagLeft = blocks.Select("DiagLeft");
        Inventory RoadIce = blocks.Select("RoadIce");
        Inventory WithWall = RoadIce.Select("WithWall"); // 
        articles.AddRange(CreateTriggerArticles(checkpoints / Wall / Slope2 / Slope / Tilt / DiagRight / DiagLeft / RoadIce, midPlatform, Vec3.Zero));
        articles.AddRange(CreateTriggerArticles((checkpoints / RoadIce) & DiagRight, new Vec3(48f, 2, 32f), new Vec3((float)(Math.PI * -0.148f), 0, 0)));
        articles.AddRange(CreateTriggerArticles((checkpoints / RoadIce) & DiagLeft, new Vec3(48f, 2, 32f), new Vec3((float)(Math.PI * 0.148f), 0, 0)));
        float slope2 = 0.47f;
        articles.AddRange(CreateTriggerArticles(Slope2.Select("Down"), midPlatform + new Vec3(0, 8, 0), new Vec3(0, slope2, 0)));
        articles.AddRange(CreateTriggerArticles(Slope2.Select("Up"), midPlatform + new Vec3(0, 8, 0), new Vec3(0, -slope2, 0)));
        articles.AddRange(CreateTriggerArticles(Slope2.Select("Right"), midPlatform + new Vec3(0, 8, 0), new Vec3(0, 0, slope2)));
        articles.AddRange(CreateTriggerArticles(Slope2.Select("Left"), midPlatform + new Vec3(0, 8, 0), new Vec3(0, 0, -slope2)));
        articles.AddRange(CreateTriggerArticles((Slope / RoadIce).Select("Down"), midPlatform + new Vec3(0, 4, 0), Vec3.Zero));
        articles.AddRange(CreateTriggerArticles((Slope / RoadIce).Select("Up"), midPlatform + new Vec3(0, 4, 0), Vec3.Zero));
        articles.AddRange(CreateTriggerArticles((Tilt / RoadIce).Select("Right"), midPlatform + new Vec3(0, 4, 0), Vec3.Zero));
        articles.AddRange(CreateTriggerArticles((Tilt / RoadIce).Select("Left"), midPlatform + new Vec3(0, 4, 0), Vec3.Zero));

        articles.AddRange(CreateTriggerArticles((Slope & RoadIce).Select("Down"), midPlatform + new Vec3(0, 7, 0), Vec3.Zero));
        articles.AddRange(CreateTriggerArticles((Slope & RoadIce).Select("Up"), midPlatform + new Vec3(0, 7, 0), Vec3.Zero));
        articles.AddRange(CreateTriggerArticles(RoadIce / Slope / DiagRight / DiagLeft / WithWall, midPlatform + new Vec3(0, 2, 0), Vec3.Zero));
        articles.AddRange(CreateTriggerArticles(WithWall / DiagRight / DiagLeft, midPlatform + new Vec3(0, 2, 0), Vec3.Zero));
        articles.AddRange(CreateTriggerArticles(WithWall & DiagRight, new Vec3(48f, 4, 32f), new Vec3(PI * -0.148f, 0f, 0)));
        articles.AddRange(CreateTriggerArticles(WithWall & DiagLeft, new Vec3(48f, 4, 32f), new Vec3(PI * 0.148f, 0, 0)));

        articles.AddRange(CreateTriggerArticles(platformWall.Select("Down"), new Vec3(16, 16, 29), new Vec3(PI, PI * 0.5f, 0)));
        articles.AddRange(CreateTriggerArticles(platformWall.Select("Up"), new Vec3(16, 16, 29), new Vec3(0, -PI * 0.5f, 0)));
        articles.AddRange(CreateTriggerArticles(platformWall.Select("Right"), new Vec3(16, 16, 29), new Vec3(PI, PI * 0.5f, PI * 0.5f)));
        articles.AddRange(CreateTriggerArticles(platformWall.Select("Left"), new Vec3(16, 16, 29), new Vec3(PI, PI * 0.5f, -PI * 0.5f)));
        return articles;
    }

    private static List<Article> CreateTriggerArticles(Inventory inventory, Vec3 offset, Vec3 rotation)
    {
        List<Article> articles = [];
        articles.AddRange(inventory.Select("Checkpoint").Edit().RemoveKeyword("Checkpoint").AddKeyword("CheckpointTrigger").SetChain([new Offset(offset), new Rotate(rotation)]).getEdited());
        articles.AddRange(inventory.Select("Multilap").Edit().RemoveKeyword("Multilap").AddKeyword("MultilapTrigger").SetChain([new Offset(offset), new Rotate(rotation)]).getEdited());
        return articles;
    }

    /// <summary>
    /// For Checkpoints, adds blocks without checkpoint keyword but with orientation Keywords.
    /// Needed for matching when Checkpoint Keyword removed
    /// </summary>
    public static List<Article> CreateNonCheckpointBlocks()
    {
        List<Article> articles = [];
        articles.AddRange(CreateRoadNonCheckpointBlocks("Tech"));
        articles.AddRange(CreateRoadNonCheckpointBlocks("Dirt"));
        articles.AddRange(CreateRoadNonCheckpointBlocks("Bump"));
        articles.AddRange(CreateRoadNonCheckpointBlocks("Ice"));
        articles.AddRange(CreateOpenRoadNoCPBlocks("Tech"));
        articles.AddRange(CreateOpenRoadNoCPBlocks("Dirt"));
        articles.AddRange(CreateOpenRoadNoCPBlocks("Bump"));
        articles.AddRange(CreateOpenRoadNoCPBlocks("Ice"));
        articles.AddRange(CreatePlatformNonCheckpointBlocks("Tech"));
        articles.AddRange(CreatePlatformNonCheckpointBlocks("Dirt"));
        articles.AddRange(CreatePlatformNonCheckpointBlocks("Plastic"));
        articles.AddRange(CreatePlatformNonCheckpointBlocks("Grass"));
        articles.AddRange(CreatePlatformNonCheckpointBlocks("Ice"));
        articles.AddRange(CreateIceRoadNonCheckpointBlocks());
        Inventory tempInventory = [.. articles];
        //set sizes
        tempInventory.Any(["DiagRight", "DiagLeft"]).ToList().ForEach(x => { x.Width = 3; x.Height = 1; x.Length = 2; });
        tempInventory.Select("Slope2").ToList().ForEach(x => { x.Width = 1; x.Height = 3; x.Length = 1; });
        tempInventory.Any(["Slope", "Tilt"]).ToList().ForEach(x => { x.Width = 1; x.Height = 2; x.Length = 1; });
        tempInventory.Select("Wall").ToList().ForEach(x => { x.Width = 1; x.Height = 4; x.Length = 1; });
        (tempInventory / tempInventory.Select(["DiagRight", "DiagLeft", "Slope2", "Slope", "Wall", "Tilt"]))
            .ToList().ForEach(x => { x.Width = 1; x.Height = 1; x.Length = 1; });

        return [.. tempInventory];
    }

    private static List<Article> CreateOpenRoadNoCPBlocks(string surface)
    {
        List<Article> articles = [];
        articles.Add(new Article("Open" + surface + "RoadSlope2Straight", BlockType.Block, ["Up", "Slope2", "Open" + surface + "Road"]));
        articles.Add(new Article("Open" + surface + "RoadSlope2Straight", BlockType.Block, ["Down", "Slope2", "Open" + surface + "Road"], moveChain: [new Offset(32, 0, 32), new Rotate(PI, 0, 0)]));
        articles.Add(new Article("Open" + surface + "RoadStraightTilt2", BlockType.Block, ["Left", "Slope2", "Open" + surface + "Road"], moveChain: [new Offset(32, 0, 0), new Rotate(PI * 1.5f, 0, 0)]));
        articles.Add(new Article("Open" + surface + "RoadStraightTilt2", BlockType.Block, ["Right", "Slope2", "Open" + surface + "Road"], moveChain: [new Offset(0, 0, 32), new Rotate(PI * 0.5f, 0, 0)]));
        return articles;
    }

    private static List<Article> CreateRoadNonCheckpointBlocks(string surface)
    {
        List<Article> articles = [];
        articles.Add(new Article("Road" + surface + "SlopeStraight", BlockType.Block, ["Up", "Slope", "Road" + surface]));
        articles.Add(new Article("Road" + surface + "SlopeStraight", BlockType.Block, ["Down", "Slope", "Road" + surface], moveChain: [new Offset(32, 0, 32), new Rotate(PI, 0, 0)]));
        articles.Add(new Article("Road" + surface + "TiltStraight", BlockType.Block, ["Left", "Tilt", "Road" + surface], moveChain: [new Offset(32, 0, 32), new Rotate(PI, 0, 0)]));
        articles.Add(new Article("Road" + surface + "TiltStraight", BlockType.Block, ["Right", "Tilt", "Road" + surface]));
        return articles;
    }
    private static List<Article> CreatePlatformNonCheckpointBlocks(string surface)
    {
        List<Article> articles = [];
        articles.Add(new Article("Platform" + surface + "Slope2Straight", BlockType.Block, ["Up", "Slope2", "Platform", surface]));
        articles.Add(new Article("Platform" + surface + "Slope2Straight", BlockType.Block, ["Down", "Slope2", "Platform", surface], null, [new Offset(32, 0, 32), new Rotate(PI, 0, 0)]));
        articles.Add(new Article("Platform" + surface + "Slope2Straight", BlockType.Block, ["Right", "Slope2", "Platform", surface], null, [new Offset(32, 0, 0), new Rotate(PI * 1.5f, 0, 0)]));
        articles.Add(new Article("Platform" + surface + "Slope2Straight", BlockType.Block, ["Left", "Slope2", "Platform", surface], null, [new Offset(0, 0, 32), new Rotate(PI * 0.5f, 0, 0)]));
        articles.Add(new Article("Platform" + surface + "WallStraight4", BlockType.Block, ["Up", "Wall", "Platform", surface], null, [new Offset(0, 0, 32), new Rotate(PI * 0.5f, 0, 0)]));
        articles.Add(new Article("Platform" + surface + "WallStraight4", BlockType.Block, ["Down", "Wall", "Platform", surface], null, [new Offset(0, 0, 32), new Rotate(PI * 0.5f, 0, 0)]));
        articles.Add(new Article("Platform" + surface + "WallStraight4", BlockType.Block, ["Right", "Wall", "Platform", surface], null, [new Offset(0, 0, 32), new Rotate(PI * 0.5f, 0, 0)]));
        articles.Add(new Article("Platform" + surface + "WallStraight4", BlockType.Block, ["Left", "Wall", "Platform", surface], null, [new Offset(0, 0, 32), new Rotate(PI * 0.5f, 0, 0)]));
        return articles;
    }
    private static List<Article> CreateIceRoadNonCheckpointBlocks()
    {
        List<Article> articles = [];
        articles.Add(new Article("RoadIceWithWallStraight", BlockType.Block, ["Left", "WithWall", "RoadIce"]));
        articles.Add(new Article("RoadIceWithWallStraight", BlockType.Block, ["Right", "WithWall", "RoadIce"], null, [new Offset(32, 0, 32), new Rotate(PI, 0, 0)]));
        articles.Add(new Article("RoadIceDiagRightWithWallStraight", BlockType.Block, ["DiagRight", "Right", "WithWall", "RoadIce"], null));
        articles.Add(new Article("RoadIceDiagLeftWithWallStraight", BlockType.Block, ["DiagLeft", "Right", "WithWall", "RoadIce"], null, [new Offset(96, 0, 64), new Rotate(PI, 0, 0)]));
        articles.Add(new Article("RoadIceDiagRightWithWallStraight", BlockType.Block, ["DiagRight", "Left", "WithWall", "RoadIce"], null, [new Offset(96, 0, 64), new Rotate(PI, 0, 0)]));
        articles.Add(new Article("RoadIceDiagLeftWithWallStraight", BlockType.Block, ["DiagLeft", "Left", "WithWall", "RoadIce"], null));
        return articles;
    }

    public static void PerformInventoryChanges(Inventory inventory)
    {
        // Some modifications for better Keyword indexing
        // Danger: does change naming of Articles, can cause compatibility issues and confusion
        inventory.SelectWord("GateCheckpoint").Select(BlockType.Block).ToList()
            .ForEach(a => { a.Keywords.Remove("Gate"); a.Keywords.Add("Ring"); });
        inventory.Select("Special").ToList()
            .ForEach(a => a.Keywords.Remove("Special"));
        (inventory.Select("Start") / inventory.Any(["Slope2", "Loop", "DiagRight", "DiagLeft", "Slope", "Inflatable"])).ToList()
            .ForEach(a => { a.Keywords.Remove("Start"); a.Keywords.Add("MapStart"); });
        inventory.Select("v2").Edit().RemoveKeyword("v2").Align(inventory).getAligned().ToList()
            .ForEach(a => inventory.Remove(a));
        inventory.Select("v2").ToList().ForEach(a => a.Keywords.Remove("v2"));
        inventory.Select("Oriented").ToList().ForEach(a => a.Keywords.Remove("Oriented"));
        inventory.Select("Grasss").ToList()
            .ForEach(a => { a.Keywords.Remove("Grasss"); a.Keywords.Add("Grass"); });

        Inventory DecoWall = inventory.Select("DecoWall");
        DecoWall.Select("LoopEnd").Not(["Center", "Side"]).ToList().ForEach(a => a.DefaultRotation = new RotateMid(new(PI * 0.5f, 0, 0)));
        DecoWall.Select(["Arch", "Slope2"]).Any(["UTop", "End"]).ToList().ForEach(a => a.DefaultRotation = new RotateMid(new(PI * 0.5f, 0, 0)));
        DecoWall.Select(["Arch", "Slope2", "Straight"]).ToList().ForEach(a => a.DefaultRotation = new RotateMid(new(-PI * 0.5f, 0, 0)));

        // NormalizeCheckpoint
        inventory.Select("Checkpoint").Edit().RemoveKeyword("Checkpoint").AddKeyword("Straight").Align(inventory).getAligned()
            .ToList().ForEach(a => a.Keywords.Remove("Straight"));
        inventory.Select("Checkpoint").Edit().RemoveKeyword("Checkpoint").AddKeyword("StraightX2").Align(inventory).getAligned()
            .ToList().ForEach(a => a.Keywords.Remove("StraightX2"));
        inventory.Select("Checkpoint").Edit().RemoveKeyword("Checkpoint").AddKeyword("Base").Align(inventory).getAligned()
            .ToList().ForEach(a => a.Keywords.Remove("Base"));
    }
}