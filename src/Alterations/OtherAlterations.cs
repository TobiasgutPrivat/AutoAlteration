using GBX.NET;

//flat2d (manual)

//a08 (manual)

//TODO altered-camera needs (Mediatracker)

class AntiBooster: Alteration {
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

class Boosterless: Alteration {
    public override List<InventoryChange> InventoryChanges => [new NoCPBlocks()];
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

class Broken: EffectAlteration {
    public override void Run(Map map){
        inventory.Select(SelAllEffects)
            .RemoveKeyword(["Boost","Boost2","Turbo","Turbo2","TurboRoulette","Fragile","NoSteering","SlowMotion","NoBrake","Cruise","Reset","Right","Left","Down","Up"])
            .AddKeyword("NoEngine").Replace(map);
        map.PlaceStagedBlocks();
    }
}

//TODO Bumper (custom)block

//Cacti (manual)

class Checkpointnt: EffectAlteration { //only blocks Checkpoints
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

class CPBoost : Alteration{
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

class CPFull : Alteration{
    public override List<InventoryChange> InventoryChanges => [new NoCPBlocks()];
    public override void Run(Map map){
        inventory.Select(BlockType.Block).AddKeyword("Checkpoint").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class CPLess : Alteration{
    public override void Run(Map map){
        map.Delete(inventory.Select("Checkpoint"),true);
    }
}

class CPLink : Alteration{
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

class CPsRotated : Alteration{
    public override void Run(Map map){
        map.Move(inventory.Select("Checkpoint"),RotateMid(PI*0.5f,0,0));
        map.PlaceStagedBlocks();
    }
}

//TODO Dragonyeet (Macroblock)

class Earthquake : Alteration{
    public override void Run(Map map){
        map.StageAll();
        map.stagedBlocks.ForEach(x => x.position.coords += new Vec3(1000000,500000,1000000));
        map.PlaceStagedBlocks();
    }
}

class Fast: Alteration { //TODO Wall and tilted platform (check Inventory)
    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Turbo2").Replace(map);
        inventory.Select(BlockType.Item).Select("Checkpoint").RemoveKeyword(["Checkpoint","Left","Right","Center"]).AddKeyword("Turbo2").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class Flipped: EffectAlteration {
    public override void Run(Map map){
        //Dimensions normal Stadium
        // from (1,9,1) to (48,38,48)
        map.StageAll(RotateCenter(0,PI,0));
        map.PlaceStagedBlocks();
        PlaceCPEffect(map,"Boost2",RotateMid(PI,0,0),true);
        PlaceStartEffect(map,"Boost2",RotateMid(PI,0,0),true);
        map.PlaceStagedBlocks();
    }
}

class Holes : Alteration{
    public override List<InventoryChange> InventoryChanges => [new NoCPBlocks()];
    public override void Run(Map map){
        inventory.Select(BlockType.Block).AddKeyword("Hole").Replace(map);
        inventory.Select(BlockType.Block).AddKeyword(["Hole","With","24m"]).Replace(map);
        map.PlaceStagedBlocks();
    }
}

//lunatic (manual)

//mini-rpg (manual)

//TODO mirrored
class Mirrored: EffectAlteration {//TODO Prototype
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

class NoItems: Alteration {
    public override void Run(Map map){
        map.Delete(inventory.Select(BlockType.Item).Select("!MapStart&!Finish"));
        map.PlaceStagedBlocks();
    }
}

//TODO Poolhunters (custom)block links manual //Only reasonable if asked for

class RandomBlocks : Alteration{
    public override void Run(Map map){
        Random rand = new();
        Inventory normals = !inventory.Select(BlockType.Pillar) & inventory.Select("!MapStart&!Finish&!Checkpoint&!Multilap");
        normals.Edit().PlaceRelative(map);
        normals.Edit().PlaceRelative(map);
        map.stagedBlocks.ForEach(x => x.position.coords = new Vec3(rand.Next() % 1536, rand.Next() % 240, rand.Next() % 1536));
        map.PlaceStagedBlocks();
    }
}

class RingCP : Alteration{
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
 
class SpeedLimit: Alteration {
    public override void Run(Map map){
        map.Delete(inventory.Select("Boost|Boost2|Turbo|Turbo2|TurboRoulette"),true);
    }
}

class StartOneDown: Alteration {
    public override void Run(Map map){
        inventory.Select("MapStart").Edit().PlaceRelative(map,Move(0,-8,0));
        map.Delete(inventory.Select("MapStart"),true);
        map.PlaceStagedBlocks();
    }
}

class SuperSized : Alteration{
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

class STTF : Alteration{
    public override List<InventoryChange> InventoryChanges => [new NoCPBlocks()];
    public override void Run(Map map){
        map.Delete(inventory.Select("Checkpoint&(Ring|Gate)"));
        inventory.Select(BlockType.Block).Select("Checkpoint").RemoveKeyword("Checkpoint").Replace(map);
        map.PlaceStagedBlocks();
    }
}

//Stunt (manual)

//symmetrical (manual) (Editor)

class Tilted: Alteration {
    public override void Run(Map map){
        Random rand = new();
        map.StageAll(RotateCenter(rand.Next() % 100f/125f - 0.4f,rand.Next() % 100f/125f - 0.4f,rand.Next() % 100f/125f - 0.4f));
        map.stagedBlocks.ForEach(x => x.position.coords = new Vec3(x.position.coords.X, x.position.coords.Y + 300, x.position.coords.Z));
        map.PlaceStagedBlocks();
    }
}

class Yeet: Alteration {
    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Boost2").Replace(map);
        inventory.Select(BlockType.Item).Select("Checkpoint").RemoveKeyword(["Checkpoint","Left","Right","Center"]).AddKeyword("Boost2").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class YeetDown: Alteration {
    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Boost2").Replace(map,RotateMid(PI,0,0));
        inventory.Select(BlockType.Item).Select("Checkpoint").RemoveKeyword(["Checkpoint","Left","Right","Center"]).AddKeyword("Boost2").Replace(map,RotateMid(PI,0,0));
        map.PlaceStagedBlocks();
    }
}

class YeetMaxUp: Alteration {
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
class RandomHoles: Alteration {
    public override void Run(Map map){
        Random rand = new();
        Inventory normals = !(inventory.Select(BlockType.Pillar)).Select("!MapStart&!Finish&!Checkpoint&!Multilap");
        normals.Edit().Replace(map);
        map.stagedBlocks = map.stagedBlocks.Where(x => !(rand.Next() % 10 == 0)).ToList();
        map.PlaceStagedBlocks();
    }
}
class YepTree: Alteration {
    public override void Run(Map map){    
        map.PlaceRelative(inventory.Select("Tree|Cactus|Fir|Palm|Cypress|Spring|Summer|Winter|Fall"),inventory.GetArticle("GateCheckpointCenter8mv2"));
        map.PlaceStagedBlocks();
    }
}

class Rotated: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select(BlockType.Block),RotateMid(PI,0,0));
        map.PlaceStagedBlocks();
    }
}

class Mini : Alteration {
    public override List<InventoryChange> InventoryChanges => [new CustomBlockSet(new MiniBlock())];
    public override void Run(Map map){
        inventory.AddKeyword("MiniBlock").Replace(map);
        map.Delete(inventory);
        map.stagedBlocks.ForEach(x => x.position.coords = new Vec3(x.position.coords.X / 2, x.position.coords.Y / 2 + 4, x.position.coords.Z / 2));//4 is offset for normal stadium
        map.PlaceStagedBlocks();
    }
}
class Invisible : Alteration {
    public override List<InventoryChange> InventoryChanges => [new CustomBlockSet(new InvisibleBlock())];
    public override void Run(Map map){
        inventory.Select("!MapStart&!Finish&!Checkpoint&!Gameplay").AddKeyword("InvisibleBlock").Replace(map);
        map.Delete(inventory.Select("!MapStart&!Finish&!Checkpoint&!Gameplay"));
        map.PlaceStagedBlocks();
    }
}

class Gaps : Alteration {
    public override void Run(Map map){
        map.StageAll();
        map.stagedBlocks.ForEach(x => 
            x.position.coords = new Vec3(x.position.coords.X * 17/16, x.position.coords.Y * 17/16, x.position.coords.Z * 17/16)
        );
        map.PlaceStagedBlocks();
    }
}