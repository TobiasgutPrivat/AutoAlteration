using GBX.NET;


class YepTree: Alteration {
    public override void Run(Map map){    
        string[] Trees = inventory.Select("Small&(Tree|Cactus|Fir|Palm|Cypress)").Names();
        Trees = Trees.Append("Spring").ToArray();
        Trees = Trees.Append("Summer").ToArray();
        Trees = Trees.Append("Winter").ToArray();
        Trees = Trees.Append("Fall").ToArray();
        map.PlaceRelative(Trees,"GateCheckpointCenter8mv2");
        map.PlaceStagedBlocks();
    }
}

class Flipped: Alteration {
    public override void Run(Map map){//Prototype TODO a lot
        //Dimensions normal Stadium
        // (1,9,1)
        // (48,38,48)
        inventory.Edit().Replace(map);
        map.stagedBlocks.ForEach(x => x.position.coords = new Vec3(x.position.coords.X, 240-x.position.coords.Y, 1536-x.position.coords.Z));  //X: 1536-
        map.stagedBlocks.ForEach(x => x.position.Rotate(new Vec3(0, PI, 0)));
        map.PlaceStagedBlocks();
    }
}

class NoItems: Alteration {
    public override void Run(Map map){
        map.Delete(inventory.Select(BlockType.Item).Select("!MapStart&!Finish").Names());
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
    public override void Run(Map map){
        inventory.AddKeyword("MiniBlock").Replace(map);
        map.Delete(inventory);
        map.stagedBlocks.ForEach(x => x.position.coords = new Vec3(x.position.coords.X / 2, x.position.coords.Y / 2 + 4, x.position.coords.Z / 2));//4 is offset for normal stadium
        map.PlaceStagedBlocks();
    }

    public override void ChangeInventory()
    {
        AddCustomBlocks("MiniBlock");
    }
}
class Invisible : Alteration {
    public override void Run(Map map){
        inventory.Select("!MapStart&!Finish&!Checkpoint&!Gameplay").AddKeyword("InvisibleBlock").Replace(map);
        map.Delete(inventory.Select("!MapStart&!Finish&!Checkpoint&!Gameplay"));
        map.PlaceStagedBlocks();
    }

    public override void ChangeInventory()
    {
        AddCustomBlocks("InvisibleBlock");
    }
}

class AntiBooster: Alteration {
    public override void Run(Map map){
        Inventory boosters = inventory.Select("Boost|Boost2|Turbo|Turbo2|TurboRoulette");
        Inventory tiltedBoosters = boosters.Select("Slope|Slope2|Tilt|Tilt2");
        map.Replace(tiltedBoosters.Select("Up").RemoveKeyword("Up").AddKeyword("Down"));
        map.Replace(tiltedBoosters.Select("Down").RemoveKeyword("Down").AddKeyword("Up"));
        map.Replace(tiltedBoosters.Select("Left").RemoveKeyword("Left").AddKeyword("Right"));
        map.Replace(tiltedBoosters.Select("Right").RemoveKeyword("Right").AddKeyword("Left"));
        map.PlaceStagedBlocks();
        map.Move(boosters,RotateMid(PI,0,0));
        map.PlaceStagedBlocks();
    }
}

class Boosterless: Alteration {
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

    public override void ChangeInventory(){
        AddNoCPBlocks();
    }
}
 
class SpeedLimit: Alteration {
    public override void Run(Map map){
        map.Delete(inventory.Select("Boost|Boost2|Turbo|Turbo2|TurboRoulette"));
    }
}

class Broken: EffectAlteration {
    public override void Run(Map map){
        inventory.Select(AllEffects)
            .RemoveKeyword(["Boost","Boost2","Turbo","Turbo2","TurboRoulette","Fragile","NoSteering","SlowMotion","NoBrake","Cruise","Reset","Right","Left","Down","Up"])
            .AddKeyword("NoEngine").Replace(map);
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
