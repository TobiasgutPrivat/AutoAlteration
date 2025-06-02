using GBX.NET;

//flat2d (manual)

//a08 (manual)

//TODO altered-camera needs (Mediatracker)

public class AntiBooster: Alteration {
    public override string Description => "Rotates all boosters and reactors by 180°";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        Inventory boosters = inventory.Select("Boost|Boost2|Turbo|Turbo2|TurboRoulette");
        Inventory tiltedBoosters = boosters.Select("Slope|Slope2|Tilt|Tilt2");
        tiltedBoosters.Select("Up").RemoveKeyword("Up").AddKeyword("Down").Replace(map);        
        tiltedBoosters.Select("Down").RemoveKeyword("Down").AddKeyword("Up").Replace(map);
        tiltedBoosters.Select("Left").RemoveKeyword("Left").AddKeyword("Right").Replace(map);
        tiltedBoosters.Select("Right").RemoveKeyword("Right").AddKeyword("Left").Replace(map);;
        map.PlaceStagedBlocks();
        map.Move(boosters,RotateMid(PI,0,0));
        map.PlaceStagedBlocks();
    }
}

//backwards (manual)

public class Boosterless: Alteration {
    public override string Description => "Removes all boosters and reactors (replaces with base blocks)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override List<InventoryChange> InventoryChanges => [new NormalizeCheckpoint(), new NoCPBlocks()];
    public override void Run(Map map){
        map.Delete(inventory.Select(BlockType.Item).Select("Boost|Boost2|Turbo|Turbo2|TurboRoulette"));
        Inventory blocks = inventory.Select(BlockType.Block);
        map.Delete(blocks.Select("GateExpandable&(Boost|Boost2|Turbo|Turbo2|TurboRoulette)"));
        blocks.Select("Boost").RemoveKeyword("Boost").Replace(map);
        blocks.Select("Boost2").RemoveKeyword("Boost2").Replace(map);
        blocks.Select("Turbo").RemoveKeyword("Turbo").Replace(map);
        blocks.Select("Turbo2").RemoveKeyword("Turbo2").Replace(map);
        blocks.Select("TurboRoulette").RemoveKeyword("TurboRoulette").Replace(map);
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
        inventory.Select(EffectUtils.SelAllEffects)
            .RemoveKeyword(["Boost","Boost2","Turbo","Turbo2","TurboRoulette","Fragile","NoSteering","SlowMotion","NoBrake","Cruise","Reset","Right","Left","Down","Up"])
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
        Inventory triggers = inventory.Select("CheckpointTrigger|MultilapTrigger&!Ring");
        Article Pillar = inventory.GetArticle("ObstaclePillar2m");
        for (int i = 0; i < 13; i++){
            map.PlaceRelative(triggers.Select("!Tilt&!WithWall"),Pillar,Move(12-i*2,0,0));
            map.PlaceRelative(triggers.Select("Tilt&Left"),Pillar,Rotate(0,0,slope).Move(12-i*2,0,0));
            map.PlaceRelative(triggers.Select("Tilt&Right"),Pillar,Rotate(0,0,-slope).Move(12-i*2,0,0));
            for (int j = 0; j < 3; j++){
                map.PlaceRelative(triggers.Select("WithWall"),Pillar,Move(12-i*2,j*8,0));
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

    public override List<InventoryChange> InventoryChanges => [new NormalizeCheckpoint()];
    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Turbo").Replace(map);
        inventory.Select(BlockType.Block).Select("Turbo").RemoveKeyword("Turbo").AddKeyword("Checkpoint").Replace(map);
        inventory.Select(BlockType.Block).Select("Turbo2").RemoveKeyword("Turbo2").AddKeyword("Checkpoint").Replace(map);
        inventory.Select(BlockType.Item).Select("Checkpoint").RemoveKeyword(["Right","Left","Center","Checkpoint","v2"]).AddKeyword("Turbo").Replace(map);
        inventory.Select(BlockType.Item).Select("Turbo").AddKeyword("Center").RemoveKeyword("Turbo").AddKeyword("Checkpoint").Replace(map);
        inventory.Select(BlockType.Item).Select("Turbo2").AddKeyword("Center").RemoveKeyword("Turbo2").AddKeyword("Checkpoint").Replace(map);
        map.Replace(inventory.GetArticle("GateSpecial4mTurbo"),inventory.GetArticle("GateCheckpointCenter8mv2"),Move(2,0,0));//untested
        map.PlaceStagedBlocks();
    }
}

//cp1 kept (manual)

public class CPFull : Alteration{
    public override string Description => "Replaces all Blocks with their checkpoint variant if available (Direction depends on original block)";
    public override bool Published => true;
    public override bool LikeAN => false;
    public override bool Complete => true;

    public override List<InventoryChange> InventoryChanges => [new NormalizeCheckpoint(), new NoCPBlocks()];
    public override void Run(Map map){
        inventory.Select(BlockType.Block).AddKeyword("Checkpoint").Replace(map);
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
        map.Move(inventory.Select("Checkpoint"),RotateMid(PI*0.5f,0,0));
        map.PlaceStagedBlocks();
    }
}

//TODO Dragonyeet (Macroblock)

public class Earthquake : Alteration {
    public override string Description => "Moves the whole map by 1 million meters, making it feel like an earthquake";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.StageAll();
        map.stagedBlocks.ForEach(x => x.position.coords += new Vec3(1000000,500000,1000000));
        map.PlaceStagedBlocks(false);
    }
}

public class Fast: Alteration { //TODO Wall and tilted platform (check Inventory)
    public override string Description => "Replaces all checkpoints with red Turbo";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Turbo2").Replace(map);
        inventory.Select(BlockType.Item).Select("Checkpoint").RemoveKeyword(["Checkpoint","Left","Right","Center"]).AddKeyword("Turbo2").Replace(map);
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
        map.StageAll(RotateCenter(0,PI,0));
        map.PlaceStagedBlocks();
        new CPEffect("Boost",RotateMid(PI,0,0),true, true).Run(map);
    }
}

public class Holes : Alteration {
    public override string Description => "Replaces all Blocks with their hole variant if available";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override List<InventoryChange> InventoryChanges => [new NormalizeCheckpoint(), new NoCPBlocks()];
    public override void Run(Map map){
        inventory.Select(BlockType.Block).AddKeyword("Hole").Replace(map);
        inventory.Select(BlockType.Block).AddKeyword(["Hole","With","24m"]).Replace(map);
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
        map.Delete(inventory.Select(BlockType.Item).Select("!MapStart&!Finish"));
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
        Inventory normals = !inventory.Select(BlockType.Pillar) & inventory.Select("!MapStart&!Finish&!Checkpoint&!Multilap");
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

    public override List<InventoryChange> InventoryChanges => [new CheckpointTrigger()];
    public override void Run(Map map){
        Article GateCheckpoint = inventory.GetArticle("GateCheckpoint");
        map.PlaceRelative(inventory.Select(BlockType.Item).Select("Checkpoint"),GateCheckpoint,Move(-16,-12,-16));
        map.PlaceRelative(inventory.Select(BlockType.Block).Select("CheckpointTrigger"),GateCheckpoint,Move(0,-12,0));
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
        map.Delete(inventory.Select("Boost|Boost2|Turbo|Turbo2|TurboRoulette"),true);
    }
}

public class StartOneDown: Alteration {
    public override string Description => "Moves the start 1 unit down";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select("MapStart").Edit().PlaceRelative(map,Move(0,-8,0));
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
        inventory.AddKeyword("SupersizedBlock").Replace(map);
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

    public override List<InventoryChange> InventoryChanges => [new NormalizeCheckpoint(), new NoCPBlocks()];
    public override void Run(Map map){
        map.Delete(inventory.Select("Checkpoint&(Ring|Gate)"));
        inventory.Select(BlockType.Block).Select("Checkpoint").RemoveKeyword("Checkpoint").Replace(map);
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

    public override void Run(Map map){
        Random rand = new();
        map.StageAll(RotateCenter(rand.Next() % 100f/125f - 0.4f,rand.Next() % 100f/125f - 0.4f,rand.Next() % 100f/125f - 0.4f));
        map.stagedBlocks.ForEach(x => x.position.coords = new Vec3(x.position.coords.X, x.position.coords.Y + 300, x.position.coords.Z));
        map.PlaceStagedBlocks();
    }
}

public class Yeet: Alteration {
    public override string Description => "Replaces all CP's with a Red Reactor";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Boost2").Replace(map);
        inventory.Select(BlockType.Item).Select("Checkpoint").RemoveKeyword(["Checkpoint","Left","Right","Center"]).AddKeyword("Boost2").Replace(map);
        map.PlaceStagedBlocks();
    }
}

public class YeetDown: Alteration {
    public override string Description => "Replaces all CP's with a Red Reactor rotated by 180°";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Boost2").Replace(map,RotateMid(PI,0,0));
        inventory.Select(BlockType.Item).Select("Checkpoint").RemoveKeyword(["Checkpoint","Left","Right","Center"]).AddKeyword("Boost2").Replace(map,RotateMid(PI,0,0));
        map.PlaceStagedBlocks();
    }
}

public class YeetMaxUp: Alteration {
    public override string Description => "Replaces all CP's with a Red Reactor, and moves the Finish up to the build limit";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Boost2").Replace(map);
        inventory.Select(BlockType.Item).Select("Checkpoint").RemoveKeyword(["Checkpoint","Left","Right","Center"]).AddKeyword("Boost2").Replace(map);
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
        Inventory specials = inventory.Select("MapStart|Finish|Multilap");
        (!specials).Edit().Replace(map);
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
        map.PlaceRelative(inventory.Select("Tree|Cactus|Fir|Palm|Cypress|Spring|Summer|Winter|Fall"),inventory.GetArticle("YepTree-TreeCheckpointTrigger"));
        map.PlaceStagedBlocks();
    }
}

public class Rotated: Alteration {
    public override string Description => "Rotates all Blocks by 180°";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.Move(inventory.Select(BlockType.Block),RotateMid(PI,0,0));
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
        inventory.AddKeyword("MiniBlock").Replace(map);
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
        inventory.Select("!MapStart&!Finish&!Checkpoint&!Gameplay").AddKeyword("InvisibleBlock").Replace(map);
        map.Delete(inventory.Select("!MapStart&!Finish&!Checkpoint&!Gameplay"));
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