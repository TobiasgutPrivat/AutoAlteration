using GBX.NET;
class EffectAlteration: Alteration {
    public static void PlaceCPEffect(Map map, string Effect,Position position = null, string RingAddition = "") {
        if (position == null) position = new Position(new Vec3(0,0,0),new Vec3(0,0,0));
        inventory.Select(BlockType.Item).Select("Checkpoint").RemoveKeyword("Checkpoint").RemoveKeyword("Left").RemoveKeyword("Right").RemoveKeyword("Center").RemoveKeyword("v2").AddKeyword(Effect).PlaceRelative(map,position);
        map.PlaceRelative(inventory.Select("CheckpointTrigger|MultilapTrigger&!Ring&!WithWall"), inventory.GetArticle("GateSpecial" + Effect + RingAddition),position.Clone().Move(new Vec3(-16,-16,-16)));
        map.PlaceRelative(inventory.Select("CheckpointTrigger|MultilapTrigger&WithWall&Left"), inventory.GetArticle("GateSpecial32m" + Effect),position.Clone().AddPosition(new Position(new Vec3(0,7,0),new Vec3(0,0,PI))));
        map.PlaceRelative(inventory.Select("CheckpointTrigger|MultilapTrigger&WithWall&Left"), inventory.GetArticle("GateSpecial32m" + Effect),position.Clone().AddPosition(new Position(new Vec3(6,12,0),new Vec3(0,0,PI*-0.5f))));
        map.PlaceRelative(inventory.Select("CheckpointTrigger|MultilapTrigger&WithWall&Right"), inventory.GetArticle("GateSpecial32m" + Effect),position.Clone().AddPosition(new Position(new Vec3(0,7,0),new Vec3(0,0,PI))));
        map.PlaceRelative(inventory.Select("CheckpointTrigger|MultilapTrigger&WithWall&Right"), inventory.GetArticle("GateSpecial32m" + Effect),position.Clone().AddPosition(new Position(new Vec3(-6,12,0),new Vec3(0,0,PI*0.5f))));
        map.PlaceRelative(inventory.GetArticle("GateCheckpoint"), inventory.GetArticle("GateSpecial" + Effect + RingAddition),position);

        map.PlaceStagedBlocks();
    }
    public static void PlaceStartEffect(Map map, string Effect,Position position = null, string RingAddition = ""){
        if (position == null) position = new Position(new Vec3(0,0,0),new Vec3(0,0,0));
        Inventory start = inventory.Select(BlockType.Block).Select("MapStart");
        Article GateSpecial = inventory.GetArticle("GateSpecial" + Effect + RingAddition);
        map.PlaceRelative(start.Select("!Water&!(RoadIce)"), GateSpecial,position.Clone().Move(new Vec3(0,-16,0)));
        map.PlaceRelative(start.Select("RoadIce"), GateSpecial,position.Clone().Move(new Vec3(0,-8,0)));
        map.PlaceRelative(inventory.GetArticle("RoadWaterStart"), GateSpecial,position.Clone().Move(new Vec3(0,-16,-2)));
        inventory.Select(BlockType.Item).Select("MapStart&Gate").AddKeyword(Effect).RemoveKeyword(new string[] {"MapStart", "Left", "Right", "Center", "v2" }).PlaceRelative(map,position.Clone().Move(new Vec3(0,0,-10)));
        map.PlaceStagedBlocks();
    }
    public static void PlaceStartGamePlay(Map map, string GamePlay){
        Inventory start = inventory.Select(BlockType.Block).Select("MapStart");
        Article GateSpecial = inventory.GetArticle("GateGameplay" + GamePlay);
        map.PlaceRelative(start.Select("!Water&!(RoadIce)"), GateSpecial,new Position(new Vec3(0,-16,0)));
        map.PlaceRelative(start.Select("RoadIce"), GateSpecial,new Position(new Vec3(0,-8,0)));
        map.PlaceRelative(inventory.GetArticle("RoadWaterStart"), GateSpecial,new Position(new Vec3(0,-16,-2)));
        inventory.Select("MapStart&Gate").AddKeyword("Gameplay").AddKeyword(GamePlay).RemoveKeyword(new string[] {"MapStart", "Left", "Right", "Center", "v2" }).PlaceRelative(map,new Position(new Vec3(0,0,-10)));
        map.PlaceStagedBlocks();
    }
}
class NoBrake: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"NoBrake",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
        PlaceStartEffect(map,"NoBrake",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
    }
}
class Cruise: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"Cruise",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
    }
}

class Fragile: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"Fragile",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
        PlaceStartEffect(map,"Fragile",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
    }
}

class SlowMo: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"SlowMotion",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
        PlaceStartEffect(map,"SlowMotion",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
    }
}

class NoSteer: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"NoSteering",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
        PlaceStartEffect(map,"NoSteering",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
    }
}

class Glider: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"Boost",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)),"Oriented");
        PlaceStartEffect(map,"Boost",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)),"Oriented");
    }
}

class Reactor: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"Boost2",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)),"Oriented");
        PlaceStartEffect(map,"Boost2",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)),"Oriented");
    }
}

class ReactorDown: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"Boost2",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)),"Oriented");
        PlaceStartEffect(map,"Boost2",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)),"Oriented");
    }
}

class FreeWheel: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"Turbo");
        PlaceStartEffect(map,"Turbo");
        PlaceCPEffect(map,"NoEngine",new Position(new Vec3(0,0,1),Vec3.Zero));
        PlaceStartEffect(map,"NoEngine",new Position(new Vec3(0,0,3),Vec3.Zero));
    }
}

class Rally: EffectAlteration {
    public override void Run(Map map){
        inventory.Select("Gameplay").RemoveKeyword("Snow").RemoveKeyword("Desert").RemoveKeyword("Rally").RemoveKeyword("Stadium").AddKeyword("Rally").Replace(map);
        map.PlaceStagedBlocks();
        PlaceStartGamePlay(map,"Rally");
    }
}
class Snow: EffectAlteration {
    public override void Run(Map map){
        inventory.Select("Gameplay").RemoveKeyword("Snow").RemoveKeyword("Desert").RemoveKeyword("Rally").RemoveKeyword("Stadium").AddKeyword("Snow").Replace(map);
        map.PlaceStagedBlocks();
        PlaceStartGamePlay(map,"Snow");
    }
}
class Desert: EffectAlteration {
    public override void Run(Map map){
        inventory.Select("Gameplay").RemoveKeyword("Snow").RemoveKeyword("Desert").RemoveKeyword("Rally").RemoveKeyword("Stadium").AddKeyword("Desert").Replace(map);
        map.PlaceStagedBlocks();
        PlaceStartGamePlay(map,"Desert");
    }
}
class Stadium: EffectAlteration {
    public override void Run(Map map){
        inventory.Select("Gameplay").RemoveKeyword("Snow").RemoveKeyword("Desert").RemoveKeyword("Rally").RemoveKeyword("Stadium").AddKeyword("Stadium").Replace(map);
        map.PlaceStagedBlocks();
        PlaceStartGamePlay(map,"Stadium");
    }
}

class SnowCarswitchToDesert: EffectAlteration {
    public override void Run(Map map){
        inventory.Select("Gameplay&Snow").RemoveKeyword("Snow").AddKeyword("Desert").Replace(map);
        map.PlaceStagedBlocks();
    }
}
class SnowCarswitchToRally: EffectAlteration {
    public override void Run(Map map){
        inventory.Select("Gameplay&Snow").RemoveKeyword("Snow").AddKeyword("Rally").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class AntiBooster: Alteration {
    public override void Run(Map map){
        inventory.Select("Boost|Boost2|Turbo|Turbo2|TurboRoulette").Edit().Replace(map,new(new Vec3(32,0,32),new Vec3(PI,0,0),true));
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
}

class Broken: Alteration {
    public override void Run(Map map){
        inventory.Select("Boost|Boost2|Turbo|Turbo2|TurboRoulette|Fragile|NoSteering|SlowMotion|NoBrake|Cruise|Reset").RemoveKeyword(new string[] { "Boost","Boost2","Turbo","Turbo2","TurboRoulette","Fragile","NoSteering","SlowMotion","NoBrake","Cruise","Reset","Right","Left","Down","Up" }).AddKeyword("NoEngine").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class Fast: Alteration { //TODO Wall and tilted platform (check Inventory)
    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Turbo2").Replace(map);
        inventory.Select(BlockType.Item).Select("Checkpoint").RemoveKeyword(new string[] { "Checkpoint","Left","Right","Center","V2"}).AddKeyword("Turbo2").Replace(map);
        map.PlaceStagedBlocks();
    }
}