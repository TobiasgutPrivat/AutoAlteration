class EffectUtils {
    public static List<string> AllEffects = ["Boost","Boost2","Turbo","Turbo2","TurboRoulette","Fragile","NoSteering","SlowMotion","NoBrake","Cruise","Reset","NoEngine"];
}

public class StartEffect(string Effect = "",MoveChain ?moveChain = null, bool oriented = false): Alteration {
    public override void Run(Map map)
    {
        if (Effect == "") throw new Exception("Not intended to be used without Effect");
        moveChain ??= [];
        Inventory start = inventory.Select(BlockType.Block).Select("MapStart");
        Article GateSpecial = oriented ? inventory.GetArticle("GateSpecial" + Effect + "Oriented") 
            : inventory.GetArticle("GateSpecial" + Effect);
        Inventory startRoadIce = start.Select("RoadIce");
        Inventory startRoadBump = start.Select("RoadBump");
        map.PlaceRelative((start/startRoadBump/startRoadIce).Not("Water"), GateSpecial,[new Offset(0,-16,0),.. moveChain]);
        map.PlaceRelative(startRoadIce, GateSpecial,[new Offset(0,-8,-2), ..moveChain]);
        map.PlaceRelative(startRoadBump, GateSpecial,[new Offset(0,-16,2), ..moveChain]);
        map.PlaceRelative(inventory.GetArticle("RoadWaterStart"), GateSpecial,[new Offset(0,-16,-2), ..moveChain]);
        inventory.Select(BlockType.Item).Select("MapStart").Select("Gate").Edit()
            .AddKeyword(Effect).RemoveKeyword(["MapStart", "Left", "Right", "Center"]).PlaceRelative(map,[new Offset(0,0,-10), ..moveChain]);
        map.PlaceStagedBlocks();
    }
}

public class CPEffect(string Effect = "",MoveChain ?moveChain = null, bool oriented = false, bool includeStart = true): Alteration {
    public override string Description => "places an Effect on every Checkpoint";
    public override void Run(Map map) {
        if (Effect == "") throw new Exception("Not intended to be used without Effect");
        moveChain ??= [];
        Article GateSpecial = oriented ? inventory.GetArticle("GateSpecial" + Effect + "Oriented") 
            : inventory.GetArticle("GateSpecial" + Effect);
        inventory.Select(BlockType.Item).Select("Checkpoint").Edit().RemoveKeyword("Checkpoint").RemoveKeyword(["Left", "Right", "Center"]).AddKeyword(Effect).PlaceRelative(map,moveChain);

        Inventory triggers = inventory.Select("CheckpointTrigger") | inventory.Select("MultilapTrigger)");
        Inventory withWall = triggers.Select("WithWall");
        map.PlaceRelative((triggers / withWall).Not("Ring"), GateSpecial, [new Offset(-16, -16, -16), .. moveChain]);
        Article GateSpecial32 = inventory.GetArticle("GateSpecial32m" + Effect);
        map.PlaceRelative(withWall.Select("Left"), GateSpecial32,[new Offset(0,7,0), new Rotate(0,0,PI), ..moveChain]);
        map.PlaceRelative(withWall.Select("Left"), GateSpecial32,[new Offset(6,12,0), new Rotate(0,0,PI*-0.5f), ..moveChain]);
        map.PlaceRelative(withWall.Select("Right"), GateSpecial32,[new Offset(0,7,0), new Rotate(0,0,PI), ..moveChain]);
        map.PlaceRelative(withWall.Select("Right"), GateSpecial32,[new Offset(-6,12,0), new Rotate(0,0,PI*0.5f), ..moveChain]);
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

    public Cruise() : base("Cruise",[new Offset(0,0,1)]) {}
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
        new CPEffect("Turbo").Run(map);
        new CPEffect("NoEngine",[new Offset(0,0,1)],includeStart: false).Run(map);
        new StartEffect("NoEngine",[new Offset(0,0,3)]).Run(map);
    }
}

public class Glider(): CPEffect {
    public override string Description => "places Yellow Reactor and NoEngine Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => false;
    public override bool Complete => false;

    public override void Run(Map map){
        new CPEffect("Boost").Run(map);
        new CPEffect("NoEngine",[new Offset(0,0,1)],includeStart: false).Run(map);
        new StartEffect("NoEngine",[new Offset(0,0,3)]).Run(map);
    }
}

public class NoBrake: CPEffect {
    public override string Description => "places NoBrake Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public NoBrake() : base("NoBrake",[new Offset(0,0,1)],includeStart: true) {}
}

public class NoEffect: Alteration {
    public override string Description => "replaces all Effects with their normal Block version";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;


    public override void Run(Map map){
        inventory.Select(BlockType.Block).Any(EffectUtils.AllEffects).Edit()
            .RemoveKeyword(EffectUtils.AllEffects).Replace(map);
        map.Delete(inventory.Select(BlockType.Item).Any(EffectUtils.AllEffects));
        map.PlaceStagedBlocks();
    }
}

//TODO no-grip (custom)block

public class NoSteer: CPEffect {
    public override string Description => "places NoSteering Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public NoSteer() : base("NoSteering",[new Offset(0,0,1)],includeStart: true) {}
}

public class RandomDankness: Alteration {
    public override string Description => "replaces Checkpoint and Effect Blocks with random Effects";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        Inventory blocks = inventory.Select(BlockType.Block);
        Inventory checkpoints = blocks.Select("Checkpoint");
        (checkpoints / (checkpoints.Any(["Slope","Slope2"]) & checkpoints.Any(["Left","Right"])))
            .Edit().RemoveKeyword("Checkpoint")
            .ReplaceWithRandom(map,EffectUtils.AllEffects);
        checkpoints.Select("Slope").Any(["Left","Right"]).Edit().RemoveKeyword(["Checkpoint","Slope"]).AddKeyword("Tilt")
            .ReplaceWithRandom(map,EffectUtils.AllEffects);
        checkpoints.Select("Slope2").Any(["Left","Right"]).Edit().RemoveKeyword(["Checkpoint","Slope2"]).AddKeyword("Tilt2")
            .ReplaceWithRandom(map,EffectUtils.AllEffects);
        inventory.Select(BlockType.Item).Select("Checkpoint").Edit().RemoveKeyword(["Checkpoint","Left","Right","Center"])
            .ReplaceWithRandom(map,EffectUtils.AllEffects);
        inventory.Any(EffectUtils.AllEffects).Edit()
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
        inventory.Select(BlockType.Block).Any(EffectUtils.AllEffects).Edit()
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

    public ReactorDown() : base("Boost2",[new RotateMid(PI,0,0)],true,true) {}
}

public class RedEffects: Alteration {
    public override string Description => "replaces all Effects with Red Turbo";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Any(EffectUtils.AllEffects).Edit().RemoveKeyword(EffectUtils.AllEffects).AddKeyword("Turbo2").Replace(map);
        map.PlaceStagedBlocks();
    }
}

public class RngBooster: Alteration {
    public override string Description => "replaces all Effects with RNG Turbo";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Any(EffectUtils.AllEffects).Edit().RemoveKeyword(EffectUtils.AllEffects).AddKeyword("TurboRoulette").Replace(map);
        map.PlaceStagedBlocks();
    }
}


public class SlowMo: CPEffect {
    public override string Description => "places SlowMotion Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public SlowMo() : base("SlowMotion",new Offset(0,0,1),includeStart: true) {}
}

//TODO WetWheels (custom)block

//TODO Worn-tires, similair to 100 fragile (macro)block