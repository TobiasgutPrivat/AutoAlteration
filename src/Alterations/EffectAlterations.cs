abstract public class EffectAlteration: Alteration {
    public string SelAllEffects = "Boost|Boost2|Turbo|Turbo2|TurboRoulette|Fragile|NoSteering|SlowMotion|NoBrake|Cruise|Reset|NoEngine";
    public List<string> AllEffects = ["Boost","Boost2","Turbo","Turbo2","TurboRoulette","Fragile","NoSteering","SlowMotion","NoBrake","Cruise","Reset","NoEngine"];
    public override List<InventoryChange> InventoryChanges => [new CheckpointTrigger()];
    public static void PlaceCPEffect(Map map, string Effect,MoveChain ?moveChain = null, bool oriented = false) {
        moveChain ??= new();
        Article GateSpecial;
        if (oriented) {
            GateSpecial = inventory.GetArticle("GateSpecial" + Effect + "Oriented");
        } else{
            GateSpecial = inventory.GetArticle("GateSpecial" + Effect);
        }
        inventory.Select(BlockType.Item).Select("Checkpoint").RemoveKeyword("Checkpoint").RemoveKeyword(["Left", "Right", "Center"]).AddKeyword(Effect).PlaceRelative(map,moveChain);

        Inventory triggers = inventory.Select("(CheckpointTrigger|MultilapTrigger)");
        map.PlaceRelative(triggers.Select("!Ring&!WithWall"), GateSpecial,Move(-16,-16,-16).AddChain(moveChain));
        map.PlaceRelative(triggers.Select("WithWall&Left"), inventory.GetArticle("GateSpecial32m" + Effect),Move(0,7,0).Rotate(0,0,PI).AddChain(moveChain));
        map.PlaceRelative(triggers.Select("WithWall&Left"), inventory.GetArticle("GateSpecial32m" + Effect),Move(6,12,0).Rotate(0,0,PI*-0.5f).AddChain(moveChain));
        map.PlaceRelative(triggers.Select("WithWall&Right"), inventory.GetArticle("GateSpecial32m" + Effect),Move(0,7,0).Rotate(0,0,PI).AddChain(moveChain));
        map.PlaceRelative(triggers.Select("WithWall&Right"), inventory.GetArticle("GateSpecial32m" + Effect),Move(-6,12,0).Rotate(0,0,PI*0.5f).AddChain(moveChain));
        map.PlaceRelative(inventory.GetArticle("GateCheckpoint"), GateSpecial,moveChain);

        map.PlaceStagedBlocks();
    }
    public static void PlaceStartEffect(Map map, string Effect,MoveChain ?moveChain = null, bool oriented = false){
        moveChain ??= new();
        Inventory start = inventory.Select(BlockType.Block).Select("MapStart");
        Article GateSpecial;
        if (oriented) {
            GateSpecial = inventory.GetArticle("GateSpecial" + Effect + "Oriented");
        } else{
            GateSpecial = inventory.GetArticle("GateSpecial" + Effect);
        }
        map.PlaceRelative(start.Select("!Water&!RoadIce"), GateSpecial,Move(0,-16,0).AddChain(moveChain));
        map.PlaceRelative(start.Select("RoadIce"), GateSpecial,Move(0,-8,0).AddChain(moveChain));//TODO check if actually at spawn
        map.PlaceRelative(inventory.GetArticle("RoadWaterStart"), GateSpecial,Move(0,-16,-2).AddChain(moveChain));
        inventory.Select(BlockType.Item).Select("MapStart&Gate").AddKeyword(Effect).RemoveKeyword(["MapStart", "Left", "Right", "Center"]).PlaceRelative(map,Move(0,0,-10).AddChain(moveChain));
        map.PlaceStagedBlocks();
    }
}
public class Cruise: EffectAlteration {
    public override string Description => "places Cruise Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => false;
    public override bool Complete => false;

    public override void Run(Map map){
        PlaceCPEffect(map,"Cruise",Move(0,0,1));
    }
}

public class Fragile: EffectAlteration {
    public override string Description => "places Fragile Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public override void Run(Map map){
        PlaceCPEffect(map,"Fragile",Move(0,0,1));
        PlaceStartEffect(map,"Fragile");
    }
}

//TODO 100 Fragile, Fragile + remove reset + start (Macro)block

public class FreeWheel: EffectAlteration {
    public override string Description => "places Turbo and NoEngine Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public override void Run(Map map){
        PlaceCPEffect(map,"Turbo");
        PlaceStartEffect(map,"Turbo");
        PlaceCPEffect(map,"NoEngine",Move(0,0,1));
        PlaceStartEffect(map,"NoEngine",Move(0,0,3));
    }
}

public class Glider: EffectAlteration {
    public override string Description => "places Yellow Reactor and NoEngine Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => false;
    public override bool Complete => false;

    public override void Run(Map map){
        PlaceCPEffect(map,"Boost",oriented: true);
        PlaceStartEffect(map,"Boost",oriented: true);
        PlaceCPEffect(map,"NoEngine",Move(0,0,1));
        PlaceStartEffect(map,"NoEngine",Move(0,0,3));
    }
}

public class NoBrake: EffectAlteration {
    public override string Description => "places NoBrake Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public override void Run(Map map){
        PlaceCPEffect(map,"NoBrake",Move(0,0,1));
        PlaceStartEffect(map,"NoBrake");
    }
}

public class NoEffect: EffectAlteration {
    public override string Description => "replaces all Effects with their normal Block version";
    public override bool Published => true;
    public override bool LikeAN => true;

    public override List<InventoryChange> InventoryChanges => [new NoCPBlocks()];
    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select(SelAllEffects)
            .RemoveKeyword(AllEffects).Replace(map);
        map.Delete(inventory.Select(BlockType.Item).Select(SelAllEffects));
        map.PlaceStagedBlocks();
    }
}

//TODO no-grip (custom)block

public class NoSteer: EffectAlteration {
    public override string Description => "places NoSteering Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public override void Run(Map map){
        PlaceCPEffect(map,"NoSteering",Move(0,0,1));
        PlaceStartEffect(map,"NoSteering");
    }
}

public class RandomDankness: EffectAlteration {
    public override string Description => "replaces Checkpoint and Effect Blocks with random Effects";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint&!((Slope|Slope2)&(Left|Right))").RemoveKeyword("Checkpoint")
            .ReplaceWithRandom(map,AllEffects);
        inventory.Select(BlockType.Block).Select("Checkpoint&Slope&(Left|Right)").RemoveKeyword(["Checkpoint","Slope"]).AddKeyword("Tilt")
            .ReplaceWithRandom(map,AllEffects);
        inventory.Select(BlockType.Block).Select("Checkpoint&Slope2&(Left|Right)").RemoveKeyword(["Checkpoint","Slope2"]).AddKeyword("Tilt2")
            .ReplaceWithRandom(map,AllEffects);
        inventory.Select(BlockType.Item).Select("Checkpoint").RemoveKeyword(["Checkpoint","Left","Right","Center"])
            .ReplaceWithRandom(map,AllEffects);
        inventory.Select(SelAllEffects)
            .RemoveKeyword(AllEffects)
            .ReplaceWithRandom(map,AllEffects);
        map.PlaceStagedBlocks();
    }
}

public class RandomEffects: EffectAlteration {
    public override string Description => "replaces all Effects with random Effects";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select(SelAllEffects)
            .RemoveKeyword(AllEffects)
            .ReplaceWithRandom(map,AllEffects);
        map.PlaceStagedBlocks();
    }
}

public class Reactor: EffectAlteration {
    public override string Description => "places Red Reactor Effect on every Checkpoint";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        PlaceCPEffect(map,"Boost2",oriented: true);
        PlaceStartEffect(map,"Boost2",oriented: true);
    }
}

public class ReactorDown: EffectAlteration {
    public override string Description => "places Red Reactor Effect on every Checkpoint (reactor rotated 180°)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        PlaceCPEffect(map,"Boost2",RotateMid(PI,0,0),true);
        PlaceStartEffect(map,"Boost2",RotateMid(PI,0,0),true);
    }
}

public class RedEffects: EffectAlteration {
    public override string Description => "replaces all Effects with Red Turbo";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(SelAllEffects).RemoveKeyword(AllEffects).AddKeyword("Turbo2").Replace(map);
        map.PlaceStagedBlocks();
    }
}

public class RngBooster: EffectAlteration {
    public override string Description => "replaces all Effects with RNG Turbo";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(SelAllEffects).RemoveKeyword(AllEffects).AddKeyword("TurboRoulette").Replace(map);
        map.PlaceStagedBlocks();
    }
}


public class SlowMo: EffectAlteration {
    public override string Description => "places SlowMotion Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public override void Run(Map map){
        PlaceCPEffect(map,"SlowMotion",Move(0,0,1));
        PlaceStartEffect(map,"SlowMotion");
    }
}

//TODO WetWheels (custom)block

//TODO Worn-tires, similair to 100 fragile (macro)block