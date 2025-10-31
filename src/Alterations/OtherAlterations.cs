using GBX.NET;

public class AirMode: Alteration {
    public override string Description => "Turn all Blocks to Air-Mode, should not change anything";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public List<string> roads = ["RoadTech", "RoadDirt", "RoadIce", "RoadBump"];

    public override void Run(Map map)
    {
        map.StageAll();
        List<Block> airBlocks = map.stagedBlocks.Where(x => x.IsAir).ToList();
        map.stagedBlocks = map.stagedBlocks.Where(x => !x.IsAir).ToList();
        map.PlaceStagedBlocks(false); //Place Air Blocks to modify

        // Platforms
        Inventory tiltedBlocks = inventory.Select(BlockType.Block).Select(x => x.Height > 1);
        tiltedBlocks.Any(roads).Edit().RemoveKeyword(roads).AddKeyword("TrackWall").PlaceRelative(map);

        Inventory Platforms = tiltedBlocks.Any(["DecoPlatform","Platform"]);
        Inventory notGrassPillar = Platforms.Select(["Dirt","Ice","Plastic"]);
        List<string> removeKeywords = ["DecoPlatform","Platform","Plastic","Grass","Tech","DecoCliff","DecoHill","To","X2"];
        notGrassPillar.Edit().RemoveKeyword(removeKeywords).AddKeyword(["DecoWall"]).PlaceRelative(map);
        (Platforms/notGrassPillar).Edit().RemoveKeyword(removeKeywords).AddKeyword(["DecoWall","Grass"]).PlaceRelative(map); //Deco-Grass is missing Keyword Grass

        // DecoHills
        Inventory DecoHills = tiltedBlocks.Select("DecoHill");
        Inventory notGrassHills = Platforms.Any(["Dirt","Ice"]);

        notGrassHills.Edit().RemoveKeyword(removeKeywords).AddKeyword("DecoWall").PlaceRelative(map,new RotateMid(new Vec3(-PI/2,0,0)));
        (DecoHills/notGrassHills).Edit().RemoveKeyword(removeKeywords).AddKeyword(["DecoWall","Grass"]).PlaceRelative(map,new RotateMid(new Vec3(-PI/2,0,0)));

        // map.StageAll(); // only set air mode if pillar could be set accordingly, can be changed if pillars done completely
        map.stagedBlocks.ForEach(x => x.IsAir = true);
        map.stagedBlocks.AddRange(airBlocks);
        map.PlaceStagedBlocks(false);
        map.StageAll();
        map.stagedBlocks.ForEach(x => x.IsGround = false);
        map.PlaceStagedBlocks();
    }
}

//flat2d (manual)

//a08 (manual)

//TODO altered-camera needs (Mediatracker)

public class AntiBooster : Alteration
{
    public override string Description => "Rotates all boosters and reactors by 180°";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map)
    {
        Inventory boosters = inventory.Any(["Boost","Boost2","Turbo","Turbo2","TurboRoulette"]);
        Inventory tiltedBoosters = boosters.Any(["Slope","Slope2","Tilt","Tilt2"]);
        tiltedBoosters.Select("Up").Edit().RemoveKeyword("Up").AddKeyword("Down").Replace(map);
        tiltedBoosters.Select("Down").Edit().RemoveKeyword("Down").AddKeyword("Up").Replace(map);
        tiltedBoosters.Select("Left").Edit().RemoveKeyword("Left").AddKeyword("Right").Replace(map);
        tiltedBoosters.Select("Right").Edit().RemoveKeyword("Right").AddKeyword("Left").Replace(map); ;
        map.PlaceStagedBlocks();
        map.Move(boosters, new RotateMid(PI, 0, 0));
        map.PlaceStagedBlocks();
    }
}

//backwards (manual)

public class Boosterless: Alteration {
    public override string Description => "Removes all boosters and reactors (replaces with base blocks)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.Delete(inventory.Select(BlockType.Item).Any(["Boost","Boost2","Turbo","Turbo2","TurboRoulette"]));
        Inventory blocks = inventory.Select(BlockType.Block);
        map.Delete(blocks);
        blocks.Select("Boost").Edit().RemoveKeyword("Boost").Replace(map);
        blocks.Select("Boost2").Edit().RemoveKeyword("Boost2").Replace(map);
        blocks.Select("Turbo").Edit().RemoveKeyword("Turbo").Replace(map);
        blocks.Select("Turbo2").Edit().RemoveKeyword("Turbo2").Replace(map);
        blocks.Select("TurboRoulette").Edit().RemoveKeyword("TurboRoulette").Replace(map);
        map.PlaceStagedBlocks();
    }
}

//TODO boss-overlayed (multiple) Maps

public class Broken: CPEffect {
    public override string Description => "Replaces all Effects with Engine Off";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Any(EffectUtils.AllEffects).Edit()
            .RemoveKeyword([.. EffectUtils.AllEffects,"Right","Left","Down","Up"])
            .AddKeyword("NoEngine").Replace(map);
        map.PlaceStagedBlocks();
    }
}

//TODO Bumper (custom)block

//Cacti (manual)

public class Checkpointnt: CPEffect { //only blocks Checkpoints
    public override string Description => "Blocks all Checkpoints with pillars";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public override void Run(Map map){
        float slope = 0.235f;
        Inventory triggers = inventory.Any(["CheckpointTrigger","MultilapTrigger"]).Not("Ring");
        Article Pillar = inventory.GetArticle("ObstaclePillar2m");
        Inventory Tilted = triggers.Select("Tilt");
        Inventory WithWall = triggers.Select("WithWall");
        Inventory nonTiltWall = triggers / Tilted / WithWall;
        Inventory Tiltleft = Tilted.Select("Left");
        Inventory Tiltright = Tilted.Select("Right");

        for (int i = 0; i < 13; i++){
            map.PlaceRelative(nonTiltWall,Pillar,new Offset(12-i*2,0,0));
            map.PlaceRelative(Tiltleft,Pillar,[new Rotate(0,0,slope), new Offset(12-i*2,0,0)]);
            map.PlaceRelative(Tiltright,Pillar,[new Rotate(0,0,-slope), new Offset(12-i*2,0,0)]);
            for (int j = 0; j < 3; j++){
                map.PlaceRelative(WithWall,Pillar,new Offset(12-i*2,j*8,0));
            }
        } 
        map.PlaceStagedBlocks();
    }
}

//TODO Cleaned (custom)block

//TODO color-combined (multiple) Maps

public class CPBoost : Alteration{
    public override string Description => "Swaps Boosters with Checkpoints";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint").Edit().RemoveKeyword("Checkpoint").AddKeyword("Turbo").Replace(map);
        inventory.Select(BlockType.Block).Select("Turbo").Edit().RemoveKeyword("Turbo").AddKeyword("Checkpoint").Replace(map);
        inventory.Select(BlockType.Block).Select("Turbo2").Edit().RemoveKeyword("Turbo2").AddKeyword("Checkpoint").Replace(map);
        inventory.Select(BlockType.Item).Select("Checkpoint").Edit().RemoveKeyword(["Right","Left","Center","Checkpoint","v2"]).AddKeyword("Turbo").Replace(map);
        inventory.Select(BlockType.Item).Select("Turbo").Edit().AddKeyword("Center").RemoveKeyword("Turbo").AddKeyword("Checkpoint").Replace(map);
        inventory.Select(BlockType.Item).Select("Turbo2").Edit().AddKeyword("Center").RemoveKeyword("Turbo2").AddKeyword("Checkpoint").Replace(map);
        map.Replace(inventory.GetArticle("GateSpecial4mTurbo"),inventory.GetArticle("GateCheckpointCenter8mv2"),new Offset(2,0,0));//untested
        map.PlaceStagedBlocks();
    }
}

//cp1 kept (manual)

public class CPFull : Alteration{
    public override string Description => "Replaces all Blocks with their checkpoint variant if available (Direction depends on original block)";
    public override bool Published => true;
    public override bool LikeAN => false;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Edit().AddKeyword("Checkpoint").Replace(map);
        map.PlaceStagedBlocks();
    }
}

public class CPLess : Alteration{
    public override string Description => "Removes all boosters and reactors (replaces with base blocks)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.Delete(inventory.Select("Checkpoint"),true);
    }
}

public class CPLink : Alteration{
    public override string Description => "Links all CP's together";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.map.Blocks.ToList().ForEach(x => {
            if (x.BlockModel.Id.Contains("Checkpoint")){
                x.WaypointSpecialProperty.Order = 1;
                x.WaypointSpecialProperty.Tag = "LinkedCheckpoint";
           }
        });
        map.map.AnchoredObjects.ToList().ForEach(x => {
            if (x.ItemModel.Id.Contains("Checkpoint")){
                x.WaypointSpecialProperty.Order = 1;
                x.WaypointSpecialProperty.Tag = "LinkedCheckpoint";
           }
        });
    }
}

public class CPsRotated : Alteration{
    public override string Description => "Rotates all CP's by 90 degrees";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.Move(inventory.Select("Checkpoint"),new RotateMid(PI*0.5f,0,0));
        map.PlaceStagedBlocks();
    }
}

//TODO Dragonyeet (Macroblock)

public class Earthquake : Alteration {
    public override string Description => "Moves the whole map by 1 million meters, making it feel like an earthquake";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map)
    {
        Vec3 offset = new(1000000, 500000, 1000000);
        map.StageAll();
        map.stagedBlocks.ForEach(x => x.position.coords += offset);
        map.PlaceStagedBlocks(false);
        map.map.ThumbnailPosition += offset;
        // map.map.ClipIntro.Tracks[0].Blocks[0].Keys[0].Position
    }
}

public class Fast: Alteration { //TODO Wall and tilted platform (check Inventory)
    public override string Description => "Replaces all checkpoints with red Turbo";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint").Edit().RemoveKeyword("Checkpoint").AddKeyword("Turbo2").Replace(map);
        inventory.Select(BlockType.Item).Select("Checkpoint").Edit().RemoveKeyword(["Checkpoint","Left","Right","Center"]).AddKeyword("Turbo2").Replace(map);
        map.PlaceStagedBlocks();
    }
}

public class Flipped: Alteration {
    public override string Description => "Flips the whole map on its head";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        //Dimensions normal Stadium
        // from (1,9,1) to (48,38,48)
        map.StageAll(new RotateCenter(0,PI,0));
        map.PlaceStagedBlocks();
        new CPEffect("Boost",new RotateMid(PI,0,0),true, true).Run(map);
    }
}

public class Holes : Alteration {
    public override string Description => "Replaces all Blocks with their hole variant if available";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Edit().AddKeyword("Hole").Replace(map);
        inventory.Select(BlockType.Block).Edit().AddKeyword(["Hole","With","24m"]).Replace(map);
        map.PlaceStagedBlocks();
    }
}

//lunatic (manual)

//mini-rpg (manual)

//TODO mirrored
public class Mirrored: CPEffect {//TODO Prototype
    public override string Description => "Mirrors the whole map on its z-axis";
    public override bool Published => false;
    public override bool LikeAN => false;
    public override bool Complete => false;

    public override void Run(Map map){
        //Dimensions normal Stadium
        // from (1,9,1) to (48,38,48)
        map.StageAll();
        map.stagedBlocks.ForEach(x => x.position.coords = new Vec3(1536-x.position.coords.X, x.position.coords.Y, x.position.coords.Z));
        map.stagedBlocks.ForEach(x => x.position.pitchYawRoll = new Vec3(x.position.pitchYawRoll.X, -x.position.pitchYawRoll.Y, -x.position.pitchYawRoll.Z)); //inverting yaw
        // Move back by Width of Block (Simulate Coords of block beeing at other Corner)
        //Switch all right and left
        map.PlaceStagedBlocks();
    }
}

public class NoItems: Alteration {
    public override string Description => "Removes all Items";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.Delete(inventory.Select(BlockType.Item).Not(["MapStart","Finish"]));
        map.PlaceStagedBlocks();
    }
}

//TODO Poolhunters (custom)block links manual //Only reasonable if asked for

public class RandomBlocks : Alteration {
    public override string Description => "Places some additional random Blocks (based on Blocks in the Map) with random Position";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        Random rand = new();
        Inventory normals = (inventory/inventory.Select(BlockType.Pillar)) & inventory.Not(["MapStart","Finish","Checkpoint","Multilap"]);
        normals.Edit().PlaceRelative(map);
        normals.Edit().PlaceRelative(map);
        map.stagedBlocks.ForEach(x => x.position.coords = new Vec3(rand.Next() % 1536, rand.Next() % 240, rand.Next() % 1536));
        map.PlaceStagedBlocks();
    }
}

public class RingCP : Alteration {
    public override string Description => "Replaces all CP's with a RingCP";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        Article GateCheckpoint = inventory.GetArticle("GateCheckpoint");
        map.PlaceRelative(inventory.Select(BlockType.Item).Select("Checkpoint"),GateCheckpoint,new Offset(-16,-12,-16));
        map.PlaceRelative(inventory.Select(BlockType.Block).Select("CheckpointTrigger"),GateCheckpoint,new Offset(0,-12,0));
        map.Delete(inventory.Select("Checkpoint"),true);
        map.PlaceStagedBlocks();
    }
}

//sections-joined (manual)

//select-del (manual)
 
public class SpeedLimit: Alteration {
    public override string Description => "Deletes all Boosters and Reactors leaving Gaps";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.Delete(inventory.Any(["Boost","Boost2","Turbo","Turbo2","TurboRoulette"]),true);
    }
}

public class StartOneDown: Alteration {
    public override string Description => "Moves the start 1 unit down";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select("MapStart").Edit().PlaceRelative(map,new Offset(0,-8,0));
        map.Delete(inventory.Select("MapStart"),true);
        map.PlaceStagedBlocks();
    }
}

public class SuperSized : Alteration{
    public override string Description => "Scales the whole map by factor 2";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;

    private float factor = 2;
    public override List<InventoryChange> InventoryChanges => [new CustomBlockSet(new SupersizedBlock(factor))];
    public SuperSized(){}
    public SuperSized(float factor) => this.factor = factor;
    public override void Run(Map map){
        inventory.Edit().AddKeyword("SupersizedBlock").Replace(map);
        map.Delete(inventory);
        map.stagedBlocks.ForEach(x => x.position.coords = new Vec3((x.position.coords.X - (32 * 24)) * factor, x.position.coords.Y * factor + 500, (x.position.coords.Z - (32 * 24)) * factor));
        map.PlaceStagedBlocks();
    }
}

public class STTF : Alteration {
    public override string Description => "Replaces all CP's with their normal Block-Variant";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.Delete(inventory.Select("Checkpoint").Any(["Ring","Gate"]));
        inventory.Select(BlockType.Block).Select("Checkpoint").Edit().RemoveKeyword("Checkpoint").Replace(map);
        map.PlaceStagedBlocks();
    }
}

//Stunt (manual)

//symmetrical (manual) (Editor)

public class Tilted: Alteration {
    public override string Description => "Tilt's the whole Map by a limited random Amount around all euler Angles";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;
    
    public Tilted() { }
    
    public override void Run(Map map)
    {
        //TODO add grass blocks at the bottom of stadium
        Random rand = new();
        Vec3 angle = new Vec3(rand.Next() % 1000f / 200f, (rand.Next() % 2 == 0 ? 1 : -1) * (rand.Next() % 100f / 500f + 0.2f), (rand.Next() % 2 == 0 ? 1 : -1) * (rand.Next() % 100f / 500f + 0.2f));
        map.StageAll(new RotateCenter(angle));
        map.stagedBlocks.ForEach(x => x.position.coords = new Vec3(x.position.coords.X, x.position.coords.Y + 300, x.position.coords.Z));
        map.PlaceStagedBlocks();

        Position thumbNailPos = new Position(map.map.ThumbnailPosition, map.map.ThumbnailPitchYawRoll);
        new RotateCenter(angle).Apply(thumbNailPos, new Article());
        map.map.ThumbnailPosition = new Vec3(thumbNailPos.coords.X, thumbNailPos.coords.Y + 300, thumbNailPos.coords.Z);
        map.map.ThumbnailPitchYawRoll = thumbNailPos.pitchYawRoll;
    }
}

public class Yeet: Alteration {
    public override string Description => "Replaces all CP's with a Red Reactor";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint").Edit().RemoveKeyword("Checkpoint").AddKeyword("Boost2").Replace(map);
        inventory.Select(BlockType.Item).Select("Checkpoint").Edit().RemoveKeyword(["Checkpoint","Left","Right","Center"]).AddKeyword("Boost2").Replace(map);
        map.PlaceStagedBlocks();
    }
}

public class YeetDown: Alteration {
    public override string Description => "Replaces all CP's with a Red Reactor rotated by 180°";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint").Edit().RemoveKeyword("Checkpoint").AddKeyword("Boost2").Replace(map,new RotateMid(PI,0,0));
        inventory.Select(BlockType.Item).Select("Checkpoint").Edit().RemoveKeyword(["Checkpoint","Left","Right","Center"]).AddKeyword("Boost2").Replace(map,new RotateMid(PI,0,0));
        map.PlaceStagedBlocks();
    }
}

public class YeetMaxUp: Alteration {
    public override string Description => "Replaces all CP's with a Red Reactor, and moves the Finish up to the build limit";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint").Edit().RemoveKeyword("Checkpoint").AddKeyword("Boost2").Replace(map);
        inventory.Select(BlockType.Item).Select("Checkpoint").Edit().RemoveKeyword(["Checkpoint","Left","Right","Center"]).AddKeyword("Boost2").Replace(map);
        map.PlaceStagedBlocks();
        inventory.Select("Finish").Edit().Replace(map);
        map.stagedBlocks.ForEach(x => x.position.coords = new Vec3(x.position.coords.X, 240, x.position.coords.Z));
        map.PlaceStagedBlocks();
    }
}

//New ------------------------------------------------------------------------------
public class RandomHoles: Alteration {
    public override string Description => "Removes 10% of all Blocks";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        Random rand = new();
        (inventory/inventory.Any(["MapStart","Finish","Multilap"])).Edit().Replace(map);
        map.stagedBlocks = map.stagedBlocks.Where(x => !(rand.Next() % 10 == 0)).ToList();
        map.PlaceStagedBlocks();
    }
}

public class YepTree: Alteration {
    public override string Description => "Places a CP on every Tree";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override List<InventoryChange> InventoryChanges => [new CustomBlockFolder("YepTree")];

    public override void Run(Map map){
        Article YepTree = inventory.GetArticle("YepTree-TreeCheckpointTrigger");
        map.PlaceRelative(inventory.Any(["Tree","Cactus","Fir","Palm","Cypress","Spring","Summer","Winter","Fall"]),YepTree);
        map.PlaceStagedBlocks();
    }
}

public class Rotated: Alteration {
    public override string Description => "Rotates all Blocks by 180°";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.Move(inventory.Select(BlockType.Block),new RotateMid(PI,0,0));
        map.PlaceStagedBlocks();
    }
}

public class Mini : Alteration {
    public override string Description => "Scales all Blocks by factor 0.5";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public override List<InventoryChange> InventoryChanges => [new CustomBlockSet(new MiniBlock())];
    public override void Run(Map map){
        inventory.Edit().AddKeyword("MiniBlock").Replace(map);
        map.Delete(inventory);
        map.stagedBlocks.ForEach(x => x.position.coords = new Vec3(x.position.coords.X / 2, x.position.coords.Y / 2 + 4, x.position.coords.Z / 2));//4 is offset for normal stadium
        map.PlaceStagedBlocks();
    }
}

public class Invisible : Alteration {
    public override string Description => "Replaces all Blocks apart from Start, Finish and Checkpoints with an invisible version";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public override List<InventoryChange> InventoryChanges => [new CustomBlockSet(new InvisibleBlock())];
    public override void Run(Map map){
        Inventory nonGameplay = inventory.Not(["MapStart", "Finish", "Checkpoint", "Gameplay"]);
        nonGameplay.Edit().AddKeyword("InvisibleBlock").Replace(map);
        map.Delete(nonGameplay);
        map.PlaceStagedBlocks();
    }
}

public class Gaps : Alteration {
    public override string Description => "Spreads all Blocks apart by ~7%";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.StageAll();
        map.stagedBlocks.ForEach(x => 
            x.position.coords = new Vec3(x.position.coords.X * 17/16, x.position.coords.Y * 17/16, x.position.coords.Z * 17/16)
        );
        map.PlaceStagedBlocks();
    }
}