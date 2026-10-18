class EffectUtils
{
    public static List<string> AllEffects = ["Boost", "Boost2", "Turbo", "Turbo2", "TurboRoulette", "Fragile", "NoSteering", "SlowMotion", "NoBrake", "Cruise", "Reset", "NoEngine"];
    public static List<string> AllBoosters = ["Boost", "Boost2", "Turbo", "Turbo2", "TurboRoulette"];
}

public abstract class WaypointEffect(string effect, MoveChain? moveChain, bool oriented) : Alteration
{
    protected string Effect => effect;
    protected bool Oriented => oriented;
    protected MoveChain Chain => moveChain ??= [];

    protected Article Ring(Inventory inventory) =>
        inventory.GetArticle("GateSpecial" + effect + (oriented ? "Oriented" : ""));

    protected Article Gate(Inventory inventory, string width) =>
        inventory.GetArticle("GateSpecial" + width + "m" + effect);


    protected void PlaceOnGateItem(Inventory inventory, Map map, string gateItem, string width, MoveChain move) =>
        map.PlaceRelative(inventory.GetArticle(gateItem), Gate(inventory, width), move);

    protected static void PlaceStaged(Map map)
    {
        map.stagedBlocks.ForEach(b => b.WaypointSpecialProperty = null);
        map.PlaceStagedBlocks();
    }
}

public class CPEffect(string effect = "", MoveChain? moveChain = null, bool oriented = false, bool includeStart = true)
    : WaypointEffect(effect, moveChain, oriented)
{
    public override string Description => "places an effect on every Checkpoint";

    public override void Run(Inventory inventory, Map map)
    {
        if (Effect == "") throw new Exception("Not intended to be used without Effect");

        PlaceOnCheckpointGateItems(inventory, map);
        PlaceRingsOnCheckpointBlocks(inventory, map);
        PlaceGatesOnWallCheckpoints(inventory, map);
        PlaceOnRingCheckpoints(inventory, map);
        PlaceStaged(map);

        if (includeStart) new StartEffect(Effect, Chain, Oriented).Run(inventory, map);
    }

    private void PlaceOnCheckpointGateItems(Inventory inventory, Map map)
    {
        void Place(string gate, string width) => PlaceOnGateItem(inventory, map, gate, width, Chain);

        Place("GateCheckpointLeft8m", "8");
        Place("GateCheckpointRight8m", "8");
        Place("GateCheckpointCenter8mv2", "8");
        Place("GateCheckpointLeft16m", "16");
        Place("GateCheckpointRight16m", "16");
        Place("GateCheckpointCenter16mv2", "16");
        Place("GateCheckpointCenter24m", "24");
        Place("GateCheckpointLeft32m", "32");
        Place("GateCheckpointRight32m", "32");
        Place("GateCheckpointCenter32mv2", "32");
    }

    private void PlaceRingsOnCheckpointBlocks(Inventory inventory, Map map)
    {
        var triggers = CheckpointTriggers(inventory);
        var onTheGround = (triggers / triggers.Select("WithWall")).Not("Ring");
        map.PlaceRelative(onTheGround, Ring(inventory), [new Offset(-16, -16, -16), .. Chain]);
    }

    private void PlaceGatesOnWallCheckpoints(Inventory inventory, Map map)
    {
        var withWall = CheckpointTriggers(inventory).Select("WithWall");
        var gate32 = Gate(inventory, "32");
        map.PlaceRelative(withWall.Select("Left"),  gate32, [new Offset(0, 7, 0),   new Rotate(0, 0, PI),         .. Chain]);
        map.PlaceRelative(withWall.Select("Left"),  gate32, [new Offset(6, 12, 0),  new Rotate(0, 0, PI * -0.5f), .. Chain]);
        map.PlaceRelative(withWall.Select("Right"), gate32, [new Offset(0, 7, 0),   new Rotate(0, 0, PI),         .. Chain]);
        map.PlaceRelative(withWall.Select("Right"), gate32, [new Offset(-6, 12, 0), new Rotate(0, 0, PI * 0.5f),  .. Chain]);
    }

    private void PlaceOnRingCheckpoints(Inventory inventory, Map map) =>
        map.PlaceRelative(inventory.GetArticle("GateCheckpoint"), Ring(inventory), Chain);

    private static Inventory CheckpointTriggers(Inventory inventory) =>
        inventory.Select("CheckpointTrigger") | inventory.Select("MultilapTrigger");
}

public class StartEffect(string effect = "", MoveChain? moveChain = null, bool oriented = false)
    : WaypointEffect(effect, moveChain, oriented)
{
    public override string Description => "places an effect on the start";

    public override void Run(Inventory inventory, Map map)
    {
        if (Effect == "") throw new Exception("Not intended to be used without Effect");
        PlaceRingOnStartBlocks(inventory, map);
        PlaceOnStartGateItems(inventory, map);
        PlaceStaged(map);
    }

    private void PlaceRingOnStartBlocks(Inventory inventory, Map map)
    {
        var ring = Ring(inventory);
        var start = inventory.Select(BlockType.Block).Select("MapStart");
        var ice = start.Select("RoadIce");
        var bump = start.Select("RoadBump");
        map.PlaceRelative((start / bump / ice).Not("Water"), ring, [new Offset(0, -16, 0),  .. Chain]);
        map.PlaceRelative(ice,  ring, [new Offset(0, -8, -2), .. Chain]);
        map.PlaceRelative(bump, ring, [new Offset(0, -16, 2), .. Chain]);
        map.PlaceRelative(inventory.GetArticle("RoadWaterStart"), ring, [new Offset(0, -16, -2), .. Chain]);
    }

    private void PlaceOnStartGateItems(Inventory inventory, Map map)
    {
        MoveChain onTheSpawn = [new Offset(0, 0, -10), .. Chain];
        void Place(string gate, string width) => PlaceOnGateItem(inventory, map, gate, width, onTheSpawn);
        Place("GateStartLeft8m", "8");   Place("GateStartRight8m", "8");   Place("GateStartCenter8m", "8");
        Place("GateStartLeft16m", "16"); Place("GateStartRight16m", "16"); Place("GateStartCenter16m", "16");
        Place("GateStartLeft32m", "32"); Place("GateStartRight32m", "32"); Place("GateStartCenter32m", "32");
    }
}


public class Cruise : CPEffect
{
    public override string Description => "places Cruise Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => false;
    public override bool Complete => false;

    public Cruise() : base("Cruise", [new Offset(0, 0, 1)], includeStart: false) { }
}

public class Fragile : CPEffect
{
    public override string Description => "places Fragile Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public Fragile() : base("Fragile", includeStart: true) { }
}

//TODO 100 Fragile, Fragile + remove reset + start (Macro)block

public class FreeWheel() : CPEffect
{
    public override string Description => "places Turbo and NoEngine Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public override void Run(Inventory inventory, Map map)
    {
        new CPEffect("Turbo", [new Offset(0, 0, 1)], includeStart: false).Run(inventory, map);
        new CPEffect("NoEngine", [new Offset(0, 0, 2)], includeStart: false).Run(inventory, map);
        new StartEffect("Turbo", [new Offset(0, 0, 1)]).Run(inventory, map);
        new StartEffect("NoEngine", [new Offset(0, 0, 2)]).Run(inventory, map);
    }
}

public class Glider() : CPEffect
{
    public override string Description => "places Yellow Reactor and NoEngine Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => false;
    public override bool Complete => false;

    public override void Run(Inventory inventory, Map map)
    {
        new CPEffect("Boost").Run(inventory, map);
        new CPEffect("NoEngine", [new Offset(0, 0, 1)], includeStart: false).Run(inventory, map);
        new StartEffect("NoEngine", [new Offset(0, 0, 3)]).Run(inventory, map);
    }
}

public class NoBrake : CPEffect
{
    public override string Description => "places NoBrake Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public NoBrake() : base("NoBrake", [new Offset(0, 0, 1)], includeStart: true) { }
}

public class NoEffect : Alteration
{
    public override string Description => "replaces all Effects with their normal Block version";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;


    public override void Run(Inventory inventory, Map map)
    {
        inventory.Select(BlockType.Block).Any(EffectUtils.AllEffects).Edit()
            .RemoveKeyword(EffectUtils.AllEffects).Replace(inventory, map);
        map.Delete(inventory.Select(BlockType.Item).Any(EffectUtils.AllEffects));
        map.PlaceStagedBlocks();
    }
}

//TODO no-grip (custom)block

public class NoSteer : CPEffect
{
    public override string Description => "places NoSteering Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public NoSteer() : base("NoSteering", [new Offset(0, 0, 1)], includeStart: true) { }
}

public class RandomDankness : Alteration
{
    public override string Description => "replaces Checkpoint and Effect Blocks with random Effects";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Inventory inventory, Map map)
    {
        Inventory blocks = inventory.Select(BlockType.Block);
        Inventory checkpoints = blocks.Select("Checkpoint");
        (checkpoints / (checkpoints.Any(["Slope", "Slope2"]) & checkpoints.Any(["Left", "Right"])))
            .Edit().RemoveKeyword("Checkpoint")
            .ReplaceWithRandom(inventory, map, EffectUtils.AllEffects);
        checkpoints.Select("Slope").Any(["Left", "Right"]).Edit().RemoveKeyword(["Checkpoint", "Slope"]).AddKeyword("Tilt")
            .ReplaceWithRandom(inventory, map, EffectUtils.AllEffects);
        checkpoints.Select("Slope2").Any(["Left", "Right"]).Edit().RemoveKeyword(["Checkpoint", "Slope2"]).AddKeyword("Tilt2")
            .ReplaceWithRandom(inventory, map, EffectUtils.AllEffects);
        inventory.Select(BlockType.Item).Select("Checkpoint").Edit().RemoveKeyword(["Checkpoint", "Left", "Right", "Center"])
            .ReplaceWithRandom(inventory, map, EffectUtils.AllEffects);
        inventory.Any(EffectUtils.AllEffects).Edit()
            .RemoveKeyword(EffectUtils.AllEffects)
            .ReplaceWithRandom(inventory, map, EffectUtils.AllEffects);
        map.PlaceStagedBlocks();
    }
}

public class RandomEffects : Alteration
{
    public override string Description => "replaces all Effects with random Effects";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Inventory inventory, Map map)
    {
        inventory.Select(BlockType.Block).Any(EffectUtils.AllEffects).Edit()
            .RemoveKeyword(EffectUtils.AllEffects)
            .ReplaceWithRandom(inventory, map, EffectUtils.AllEffects);
        map.PlaceStagedBlocks();
    }
}

public class Reactor : CPEffect
{
    public override string Description => "places Red Reactor Effect on every Checkpoint";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public Reactor() : base("Boost2", oriented: true, includeStart: true) { }
}

public class ReactorDown : CPEffect
{
    public override string Description => "places Red Reactor Effect on every Checkpoint (reactor rotated 180°)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public ReactorDown() : base("Boost2", [new RotateMid(PI, 0, 0)], true, true) { }
}

public class RedEffects : Alteration
{
    public override string Description => "replaces all Effects with Red Turbo";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Inventory inventory, Map map)
    {
        inventory.Any(EffectUtils.AllEffects).Edit().RemoveKeyword(EffectUtils.AllEffects).AddKeyword("Turbo2").Replace(inventory, map);
        map.PlaceStagedBlocks();
    }
}

public class RngBooster : Alteration
{
    public override string Description => "replaces all Effects with RNG Turbo";
    public override bool Published => true;
    public override bool LikeAN => false;
    public override bool Complete => true;

    public override void Run(Inventory inventory, Map map)
    {
        inventory.Any(EffectUtils.AllBoosters).Edit().RemoveKeyword(EffectUtils.AllBoosters).AddKeyword("TurboRoulette").Replace(inventory, map);
        map.PlaceStagedBlocks();
    }
}


public class SlowMo : CPEffect
{
    public override string Description => "places SlowMotion Effect on every Checkpoint (small offset to avoid skip)";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public SlowMo() : base("SlowMotion", new Offset(0, 0, 1), includeStart: true) { }
}

//TODO WetWheels (custom)block

//TODO Worn-tires, similair to 100 fragile (macro)block