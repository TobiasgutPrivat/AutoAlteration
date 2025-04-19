class EffectUtils {
    public static string SelAllEffects = "Boost|Boost2|Turbo|Turbo2|TurboRoulette|Fragile|NoSteering|SlowMotion|NoBrake|Cruise|Reset|NoEngine";
    public static List<string> AllEffects = ["Boost","Boost2","Turbo","Turbo2","TurboRoulette","Fragile","NoSteering","SlowMotion","NoBrake","Cruise","Reset","NoEngine"];
}

public class StartEffect(string Effect = "",MoveChain ?moveChain = null, bool oriented = false): Alteration {
    public override void Run(Map map)
    {
        if (Effect == "") throw new Exception("Not intended to be used without Effect");
        moveChain ??= new();
        Inventory start = inventory.Select(BlockType.Block).Select("MapStart");
        Article GateSpecial;
        if (oriented) {
            GateSpecial = inventory.GetArticle("GateSpecial" + Effect + "Oriented");
        } else{
            GateSpecial = inventory.GetArticle("GateSpecial" + Effect);
        }
        map.PlaceRelative(start.Select("!Water&!RoadIce"), GateSpecial,Move(0,-16,0).AddChain(moveChain));
        map.PlaceRelative(start.Select("RoadIce"), GateSpecial,Move(0,-8,0).AddChain(moveChain));
        map.PlaceRelative(inventory.GetArticle("RoadWaterStart"), GateSpecial,Move(0,-16,-2).AddChain(moveChain));
        inventory.Select(BlockType.Item).Select("MapStart&Gate").AddKeyword(Effect).RemoveKeyword(["MapStart", "Left", "Right", "Center"]).PlaceRelative(map,Move(0,0,-10).AddChain(moveChain));
        map.PlaceStagedBlocks();
    }
}

public class CPEffect(string Effect = "",MoveChain ?moveChain = null, bool oriented = false, bool includeStart = true): Alteration {
    public override string Description => "places an Effect on every Checkpoint";
    public override List<InventoryChange> InventoryChanges => [new CheckpointTrigger()];
    public override void Run(Map map) {
        if (Effect == "") throw new Exception("Not intended to be used without Effect");
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
        if (includeStart) {
            new StartEffect(Effect,moveChain,oriented).Run(map);
        }
    }
}
public class Cruise: CPEffect {
    public override string Description => "places Cruise Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => false;
    public override bool Complete => false;

    public Cruise() : base("Cruise",Move(0,0,1)) {}
}

public class Fragile: CPEffect {
    public override string Description => "places Fragile Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public Fragile() : base("Fragile",includeStart: true) {}
}

//TODO 100 Fragile, Fragile + remove reset + start (Macro)block

public class FreeWheel(): CPEffect {
    public override string Description => "places Turbo and NoEngine Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public override void Run(Map map){
        new CPEffect("Turbo",includeStart: true).Run(map);
        new CPEffect("NoEngine",Move(0,0,1)).Run(map);
        new StartEffect("NoEngine",Move(0,0,3)).Run(map);
    }
}

public class Glider(): CPEffect {
    public override string Description => "places Yellow Reactor and NoEngine Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => false;
    public override bool Complete => false;

    public override void Run(Map map){
        new CPEffect("Boost",includeStart: true).Run(map);
        new CPEffect("NoEngine",Move(0,0,1)).Run(map);
        new StartEffect("NoEngine",Move(0,0,3)).Run(map);
    }
}

public class NoBrake: CPEffect {
    public override string Description => "places NoBrake Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public NoBrake() : base("NoBrake",Move(0,0,1),includeStart: true) {}
}

public class NoEffect: Alteration {
    public override string Description => "replaces all Effects with their normal Block version";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override List<InventoryChange> InventoryChanges => [new NoCPBlocks()];

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select(EffectUtils.SelAllEffects)
            .RemoveKeyword(EffectUtils.AllEffects).Replace(map);
        map.Delete(inventory.Select(BlockType.Item).Select(EffectUtils.SelAllEffects));
        map.PlaceStagedBlocks();
    }
}

//TODO no-grip (custom)block

public class NoSteer: CPEffect {
    public override string Description => "places NoSteering Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public NoSteer() : base("NoSteering",Move(0,0,1),includeStart: true) {}
}

public class RandomDankness: Alteration {
    public override string Description => "replaces Checkpoint and Effect Blocks with random Effects";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint&!((Slope|Slope2)&(Left|Right))").RemoveKeyword("Checkpoint")
            .ReplaceWithRandom(map,EffectUtils.AllEffects);
        inventory.Select(BlockType.Block).Select("Checkpoint&Slope&(Left|Right)").RemoveKeyword(["Checkpoint","Slope"]).AddKeyword("Tilt")
            .ReplaceWithRandom(map,EffectUtils.AllEffects);
        inventory.Select(BlockType.Block).Select("Checkpoint&Slope2&(Left|Right)").RemoveKeyword(["Checkpoint","Slope2"]).AddKeyword("Tilt2")
            .ReplaceWithRandom(map,EffectUtils.AllEffects);
        inventory.Select(BlockType.Item).Select("Checkpoint").RemoveKeyword(["Checkpoint","Left","Right","Center"])
            .ReplaceWithRandom(map,EffectUtils.AllEffects);
        inventory.Select(EffectUtils.SelAllEffects)
            .RemoveKeyword(EffectUtils.AllEffects)
            .ReplaceWithRandom(map,EffectUtils.AllEffects);
        map.PlaceStagedBlocks();
    }
}

public class RandomEffects: Alteration {
    public override string Description => "replaces all Effects with random Effects";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select(EffectUtils.SelAllEffects)
            .RemoveKeyword(EffectUtils.AllEffects)
            .ReplaceWithRandom(map,EffectUtils.AllEffects);
        map.PlaceStagedBlocks();
    }
}

public class Reactor: CPEffect {
    public override string Description => "places Red Reactor Effect on every Checkpoint";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public Reactor() : base("Boost2",oriented: true, includeStart: true) {}
}

public class ReactorDown: CPEffect {
    public override string Description => "places Red Reactor Effect on every Checkpoint (reactor rotated 180°)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public ReactorDown() : base("Boost2",RotateMid(PI,0,0),true,true) {}
}

public class RedEffects: Alteration {
    public override string Description => "replaces all Effects with Red Turbo";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(EffectUtils.SelAllEffects).RemoveKeyword(EffectUtils.AllEffects).AddKeyword("Turbo2").Replace(map);
        map.PlaceStagedBlocks();
    }
}

public class RngBooster: Alteration {
    public override string Description => "replaces all Effects with RNG Turbo";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select(EffectUtils.SelAllEffects).RemoveKeyword(EffectUtils.AllEffects).AddKeyword("TurboRoulette").Replace(map);
        map.PlaceStagedBlocks();
    }
}


public class SlowMo: CPEffect {
    public override string Description => "places SlowMotion Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public SlowMo() : base("SlowMotion",Move(0,0,1),includeStart: true) {}
}

//TODO WetWheels (custom)block

//TODO Worn-tires, similair to 100 fragile (macro)block