using GBX.NET;
class EffectAlteration: Alteration {
    public static void placeCPEffect(Map map, string Effect,Position position = null, string RingAddition = "") {
        if (position == null) position = new Position(new Vec3(0,0,0),new Vec3(0,0,0));
        inventory.select(BlockType.Item).select("Checkpoint").remove("Checkpoint").remove("Left").remove("Right").remove("Center").remove("v2").add(Effect).placeRelative(map,position);
        map.PlaceRelative(inventory.select("Checkpoint|Multilap&Trigger&!Ring&!WithWall"), inventory.GetArticle("GateSpecial" + Effect + RingAddition),position.Clone().Move(new Vec3(-16,-16,-16)));
        map.PlaceRelative(inventory.select("Checkpoint|Multilap&Trigger&WithWall&Left"), inventory.GetArticle("GateSpecial32m" + Effect),position.Clone().addPosition(new Position(new Vec3(0,7,0),new Vec3(0,0,PI))));
        map.PlaceRelative(inventory.select("Checkpoint|Multilap&Trigger&WithWall&Left"), inventory.GetArticle("GateSpecial32m" + Effect),position.Clone().addPosition(new Position(new Vec3(6,12,0),new Vec3(0,0,PI*-0.5f))));
        map.PlaceRelative(inventory.select("Checkpoint|Multilap&Trigger&WithWall&Right"), inventory.GetArticle("GateSpecial32m" + Effect),position.Clone().addPosition(new Position(new Vec3(0,7,0),new Vec3(0,0,PI))));
        map.PlaceRelative(inventory.select("Checkpoint|Multilap&Trigger&WithWall&Right"), inventory.GetArticle("GateSpecial32m" + Effect),position.Clone().addPosition(new Position(new Vec3(-6,12,0),new Vec3(0,0,PI*0.5f))));
        map.PlaceRelative(inventory.GetArticle("GateCheckpoint"), inventory.GetArticle("GateSpecial" + Effect + RingAddition),position);

        map.PlaceStagedBlocks();
    }
    public static void placeStartEffect(Map map, string Effect,Position position = null, string RingAddition = ""){
        if (position == null) position = new Position(new Vec3(0,0,0),new Vec3(0,0,0));
        Inventory start = inventory.select("MapStart");
        Article GateSpecial = inventory.GetArticle("GateSpecial" + Effect + RingAddition);
        map.PlaceRelative(start.select("!Water&!(RoadIce)"), GateSpecial,position.Clone().Move(new Vec3(0,-16,0)));
        map.PlaceRelative(start.select("RoadIce"), GateSpecial,position.Clone().Move(new Vec3(0,-8,0)));
        map.PlaceRelative(inventory.GetArticle("RoadWaterStart"), GateSpecial,position.Clone().Move(new Vec3(0,-16,-2)));
        inventory.select("MapStart&Gate").add(Effect).remove(new string[] {"MapStart", "Left", "Right", "Center", "v2" }).placeRelative(map,position.Clone().Move(new Vec3(0,0,-10)));
        map.PlaceStagedBlocks();
    }
    public static void placeStartGamePlay(Map map, string GamePlay){
        Inventory start = inventory.select(BlockType.Block).select("MapStart");
        Article GateSpecial = inventory.GetArticle("GateGameplay" + GamePlay);
        map.PlaceRelative(start.select("!Water&!(RoadIce)"), GateSpecial,new Position(new Vec3(0,-16,0)));
        map.PlaceRelative(start.select("RoadIce"), GateSpecial,new Position(new Vec3(0,-8,0)));
        map.PlaceRelative(inventory.GetArticle("RoadWaterStart"), GateSpecial,new Position(new Vec3(0,-16,-2)));
        inventory.select("MapStart&Gate").add("Gameplay").add(GamePlay).remove(new string[] {"MapStart", "Left", "Right", "Center", "v2" }).placeRelative(map,new Position(new Vec3(0,0,-10)));
        map.PlaceStagedBlocks();
    }
}
class NoBrake: EffectAlteration {
    public override void Run(Map map){
        placeCPEffect(map,"NoBrake",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
        placeStartEffect(map,"NoBrake",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
    }
}
class Cruise: EffectAlteration {
    public override void Run(Map map){
        placeCPEffect(map,"Cruise",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
    }
}

class Fragile: EffectAlteration {
    public override void Run(Map map){
        placeCPEffect(map,"Fragile",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
        placeStartEffect(map,"Fragile",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
    }
}

class SlowMo: EffectAlteration {
    public override void Run(Map map){
        placeCPEffect(map,"SlowMotion",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
        placeStartEffect(map,"SlowMotion",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
    }
}

class NoSteer: EffectAlteration {
    public override void Run(Map map){
        placeCPEffect(map,"NoSteering",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
        placeStartEffect(map,"NoSteering",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)));
    }
}

class Glider: EffectAlteration {
    public override void Run(Map map){
        placeCPEffect(map,"Boost",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)),"Oriented");
        placeStartEffect(map,"Boost",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)),"Oriented");
    }
}

class Reactor: EffectAlteration {
    public override void Run(Map map){
        placeCPEffect(map,"Boost2",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)),"Oriented");
        placeStartEffect(map,"Boost2",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)),"Oriented");
    }
}

class ReactorDown: EffectAlteration {
    public override void Run(Map map){
        placeCPEffect(map,"Boost2",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)),"Oriented");
        placeStartEffect(map,"Boost2",new Position(new Vec3(0,0,1),new Vec3(PI,0,0)),"Oriented");
    }
}

class FreeWheel: EffectAlteration {
    public override void Run(Map map){
        placeCPEffect(map,"Turbo");
        placeStartEffect(map,"Turbo");
        placeCPEffect(map,"NoEngine",new Position(new Vec3(0,0,1),Vec3.Zero));
        placeStartEffect(map,"NoEngine",new Position(new Vec3(0,0,1),Vec3.Zero));
    }
}

class Rally: EffectAlteration {
    public override void Run(Map map){
        inventory.select("Gameplay").remove("Snow").remove("Desert").remove("Rally").remove("Stadium").add("Rally").replace(map);
        map.PlaceStagedBlocks();
        placeStartGamePlay(map,"Rally");
    }
}
class Snow: EffectAlteration {
    public override void Run(Map map){
        inventory.select("Gameplay").remove("Snow").remove("Desert").remove("Rally").remove("Stadium").add("Snow").replace(map);
        map.PlaceStagedBlocks();
        placeStartGamePlay(map,"Snow");
    }
}
class Desert: EffectAlteration {
    public override void Run(Map map){
        inventory.select("Gameplay").remove("Snow").remove("Desert").remove("Rally").remove("Stadium").add("Desert").replace(map);
        map.PlaceStagedBlocks();
        placeStartGamePlay(map,"Desert");
    }
}
class Stadium: EffectAlteration {
    public override void Run(Map map){
        inventory.select("Gameplay").remove("Snow").remove("Desert").remove("Rally").remove("Stadium").add("Stadium").replace(map);
        map.PlaceStagedBlocks();
        placeStartGamePlay(map,"Stadium");
    }
}

class SnowCarswitchToDesert: EffectAlteration {
    public override void Run(Map map){
        inventory.select("Gameplay&Snow").remove("Snow").add("Desert").replace(map);
        map.PlaceStagedBlocks();
    }
}
class SnowCarswitchToRally: EffectAlteration {
    public override void Run(Map map){
        inventory.select("Gameplay&Snow").remove("Snow").add("Rally").replace(map);
        map.PlaceStagedBlocks();
    }
}