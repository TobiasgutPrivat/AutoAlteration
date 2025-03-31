using GBX.NET;

public abstract class InventoryChange: PosUtils {
    public abstract void ChangeInventory(Inventory inventory, bool mapSpecific = false);
}

public class NormalizeCheckpoint: InventoryChange {
    public override void ChangeInventory(Inventory inventory, bool mapSpecific = false) {
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Straight").Align().getAligned().EditOriginal().RemoveKeyword("Straight");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("StraightX2").Align().getAligned().EditOriginal().RemoveKeyword("StraightX2");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Base").Align().getAligned().EditOriginal().RemoveKeyword("Base");
    }
}

public class CustomBlockFolder(string subFolder) : InventoryChange {
    public readonly string folder = Path.Combine(AltertionConfig.CustomBlocksFolder, subFolder);

    public override void ChangeInventory(Inventory inventory, bool mapSpecific = false) {
        inventory.AddArticles(Directory.GetFiles(folder, "*.Block.Gbx", SearchOption.AllDirectories)
            .Select(x => new Article(x.Replace(folder,"")[..^10], BlockType.CustomBlock, x, mapSpecific)).ToList());

        inventory.AddArticles(Directory.GetFiles(folder, "*.Item.Gbx", SearchOption.AllDirectories)
            .Select(x => new Article(x.Replace(folder,"")[..^9], BlockType.CustomItem, x, mapSpecific)).ToList());
    }
}

public class CustomBlockSet(CustomBlockAlteration customBlockAlteration, bool skipUnchanged = true) : InventoryChange {
    public readonly CustomBlockAlteration customBlockAlteration = customBlockAlteration;
    public readonly bool skipUnchanged = skipUnchanged;

    public virtual string GetFolder() { return Path.Combine(AltertionConfig.CacheFolder, GetSetName()); }
    public virtual string GetSetName() { return customBlockAlteration.GetType().Name; }
    public virtual string GetOrigin() { return Path.Combine(AltertionConfig.CustomBlocksFolder, "Vanilla"); }

    public override void ChangeInventory(Inventory inventory, bool mapSpecific = false) {
        if (!Directory.Exists(GetFolder())) { 
            GenerateBlockSet();
        }
        
        inventory.AddArticles(Directory.GetFiles(GetFolder(), "*.Block.Gbx", SearchOption.AllDirectories)
            .Select(x => new Article(Path.GetFileName(x)[..^10], BlockType.CustomBlock, x)).ToList());

        inventory.AddArticles(Directory.GetFiles(GetFolder(), "*.Item.Gbx", SearchOption.AllDirectories)
            .Select(x => new Article(Path.GetFileName(x)[..^9], BlockType.CustomItem, x)).ToList());
    }

    public void GenerateBlockSet() {
        Console.WriteLine("Generating " + customBlockAlteration.GetType().Name + " block set...");
        if (!Directory.Exists(GetFolder())) { 
            Directory.CreateDirectory(GetFolder() + "Temp");//Temp in case something goes wrong
        }
        AutoAlteration.AlterAll(customBlockAlteration, GetOrigin(), GetFolder() + "Temp", GetSetName(),skipUnchanged);
        Directory.Move(GetFolder() + "Temp", GetFolder());
    }
}

public class LightSurface(CustomBlockAlteration customBlockAlteration, bool skipUnchanged = true) : CustomBlockSet(customBlockAlteration, skipUnchanged) {
    public override string GetSetName() { return customBlockAlteration.GetType().Name + "Light"; }
    public override string GetOrigin() { return Path.Combine(AltertionConfig.CustomBlocksFolder, "LightSurface"); }
}

public class HeavySurface(CustomBlockAlteration customBlockAlteration, bool skipUnchanged = true) : CustomBlockSet(customBlockAlteration, skipUnchanged) {
    public override string GetSetName() { return customBlockAlteration.GetType().Name + "Heavy"; }
    public override string GetOrigin() { return Path.Combine(AltertionConfig.CustomBlocksFolder, "HeavySurface"); }
}

public class NoCPBlocks: InventoryChange {
    public override void ChangeInventory(Inventory inventory, bool mapSpecific = false) {
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
        tempInventory.AddArticles(new Article("Open" +surface+"RoadSlope2Straight",BlockType.Block,["Down","Slope2","Open" + surface + "Road"], moveChain: Move(32,0,32).Rotate(PI,0,0)));
        tempInventory.AddArticles(new Article("Open" +surface+"RoadStraightTilt2",BlockType.Block,["Left","Slope2","Open" + surface + "Road"], moveChain: Move(32,0,0).Rotate(PI*1.5f,0,0)));
        tempInventory.AddArticles(new Article("Open" +surface+"RoadStraightTilt2",BlockType.Block,["Right","Slope2","Open" + surface + "Road"], moveChain: Move(0,0,32).Rotate(PI*0.5f,0,0)));
    }

    private static void AddRoadNoCPBlocks(Inventory tempInventory, string surface){
        tempInventory.AddArticles(new Article("Road" +surface+"SlopeStraight",BlockType.Block,["Up","Slope","Road" + surface]));
        tempInventory.AddArticles(new Article("Road" +surface+"SlopeStraight",BlockType.Block,["Down","Slope","Road" + surface], moveChain: Move(32,0,32).Rotate(PI,0,0)));
        tempInventory.AddArticles(new Article("Road" +surface+"TiltStraight",BlockType.Block,["Left","Tilt","Road" + surface], moveChain: Move(32,0,32).Rotate(PI,0,0)));
        tempInventory.AddArticles(new Article("Road" +surface+"TiltStraight",BlockType.Block,["Right","Tilt","Road" + surface]));
    }
    private static void AddPlatformNoCPBlocks(Inventory tempInventory, string surface){
        tempInventory.AddArticles(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,["Up","Slope2","Platform",surface]));
        tempInventory.AddArticles(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,["Down","Slope2","Platform",surface],null,Move(32,0,32).Rotate(PI,0,0)));
        tempInventory.AddArticles(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,["Right","Slope2","Platform",surface],null,Move(32,0,0).Rotate(PI*1.5f,0,0)));
        tempInventory.AddArticles(new Article("Platform" +surface+"Slope2Straight",BlockType.Block,["Left","Slope2","Platform",surface],null,Move(0,0,32).Rotate(PI*0.5f,0,0)));
        tempInventory.AddArticles(new Article("Platform" +surface+"WallStraight4",BlockType.Block,["Up","Wall","Platform",surface],null,Move(0,0,32).Rotate(PI*0.5f,0,0)));
        tempInventory.AddArticles(new Article("Platform" +surface+"WallStraight4",BlockType.Block,["Down","Wall","Platform",surface],null,Move(0,0,32).Rotate(PI*0.5f,0,0)));
        tempInventory.AddArticles(new Article("Platform" +surface+"WallStraight4",BlockType.Block,["Right","Wall","Platform",surface],null,Move(0,0,32).Rotate(PI*0.5f,0,0)));
        tempInventory.AddArticles(new Article("Platform" +surface+"WallStraight4",BlockType.Block,["Left","Wall","Platform",surface],null,Move(0,0,32).Rotate(PI*0.5f,0,0)));
    }
    private static void AddIceRoadNoCPBlocks(Inventory tempInventory){
        tempInventory.AddArticles(new Article("RoadIceWithWallStraight",BlockType.Block,["Left","WithWall","RoadIce"]));
        tempInventory.AddArticles(new Article("RoadIceWithWallStraight",BlockType.Block,["Right","WithWall","RoadIce"],null,Move(32,0,32).Rotate(PI,0,0)));
        tempInventory.AddArticles(new Article("RoadIceDiagRightWithWallStraight",BlockType.Block,["DiagRight","Right","WithWall","RoadIce"],null));
        tempInventory.AddArticles(new Article("RoadIceDiagLeftWithWallStraight",BlockType.Block,["DiagLeft","Right","WithWall","RoadIce"],null,Move(96,0,64).Rotate(PI,0,0)));
        tempInventory.AddArticles(new Article("RoadIceDiagRightWithWallStraight",BlockType.Block,["DiagRight","Left","WithWall","RoadIce"],null,Move(96,0,64).Rotate(PI,0,0)));
        tempInventory.AddArticles(new Article("RoadIceDiagLeftWithWallStraight",BlockType.Block,["DiagLeft","Left","WithWall","RoadIce"],null));
    }
}
public class CheckpointTrigger: InventoryChange {
    public override void ChangeInventory(Inventory inventory, bool mapSpecific = false) {
        Vec3 midPlatform = new(16,2,16);
        CreateTriggerArticle(inventory,"!Wall&!Slope2&!Slope&!Tilt&!DiagRight&!DiagLeft&!RoadIce", midPlatform,Vec3.Zero);
        CreateTriggerArticle(inventory,"!WithWall&!RoadIce&DiagRight",new Vec3(48f,2,32f),new Vec3(PI * -0.148f,0f,0));
        CreateTriggerArticle(inventory,"!WithWall&!RoadIce&DiagLeft",new Vec3(48f,2,32f),new Vec3(PI * 0.148f,0,0));
        float slope2 = 0.47f;
        CreateTriggerArticle(inventory,"Slope2&Down", midPlatform + new Vec3(0,8,0),new Vec3(0,slope2,0));
        CreateTriggerArticle(inventory,"Slope2&Up", midPlatform + new Vec3(0,8,0),new Vec3(0,-slope2,0));
        CreateTriggerArticle(inventory,"Slope2&Right", midPlatform + new Vec3(0,8,0),new Vec3(0,0,slope2));
        CreateTriggerArticle(inventory,"Slope2&Left", midPlatform + new Vec3(0,8,0),new Vec3(0,0,-slope2));
        CreateTriggerArticle(inventory,"Slope&Down&!RoadIce", midPlatform + new Vec3(0,4,0),Vec3.Zero);
        CreateTriggerArticle(inventory,"Slope&Up&!RoadIce", midPlatform + new Vec3(0,4,0),Vec3.Zero);
        CreateTriggerArticle(inventory,"Tilt&Right&!RoadIce", midPlatform + new Vec3(0,4,0),Vec3.Zero);
        CreateTriggerArticle(inventory,"Tilt&Left&!RoadIce", midPlatform + new Vec3(0,4,0),Vec3.Zero);

        CreateTriggerArticle(inventory,"Slope&Down&RoadIce", midPlatform + new Vec3(0,7,0),Vec3.Zero);
        CreateTriggerArticle(inventory,"Slope&Up&RoadIce", midPlatform + new Vec3(0,7,0),Vec3.Zero);
        CreateTriggerArticle(inventory,"!Slope&!DiagRight&!DiagLeft&!WithWall&RoadIce", midPlatform + new Vec3(0,2,0),Vec3.Zero);
        CreateTriggerArticle(inventory,"WithWall&RoadIce&!DiagRight&!DiagLeft", midPlatform + new Vec3(0,2,0),Vec3.Zero);
        CreateTriggerArticle(inventory,"WithWall&RoadIce&DiagRight",new Vec3(48f,4,32f),new Vec3(PI * -0.148f,0f,0));
        CreateTriggerArticle(inventory,"WithWall&RoadIce&DiagLeft",new Vec3(48f,4,32f),new Vec3(PI * 0.148f,0,0));

        CreateTriggerArticle(inventory,"Platform&Wall&Down", new Vec3(16,16,29),new Vec3(PI,PI*0.5f,0));
        CreateTriggerArticle(inventory,"Platform&Wall&Up", new Vec3(16,16,29),new Vec3(0,-PI*0.5f,0));
        CreateTriggerArticle(inventory,"Platform&Wall&Right", new Vec3(16,16,29),new Vec3(PI,PI*0.5f,PI*0.5f));
        CreateTriggerArticle(inventory,"Platform&Wall&Left", new Vec3(16,16,29),new Vec3(PI,PI*0.5f,-PI*0.5f));
    }

    private static void CreateTriggerArticle(Inventory inventory,string selection,Vec3 offset, Vec3 rotation) {
        inventory.AddArticles(inventory.Select(BlockType.Block).Select("Checkpoint").Select(selection).RemoveKeyword("Checkpoint").AddKeyword("CheckpointTrigger").SetChain(Move(offset).Rotate(rotation)).getEdited());
        inventory.AddArticles(inventory.Select(BlockType.Block).Select("Multilap").Select(selection).RemoveKeyword("Multilap").AddKeyword("MultilapTrigger").SetChain(Move(offset).Rotate(rotation)).getEdited());
    }
}

