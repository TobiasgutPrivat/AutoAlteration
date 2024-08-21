class EffectAlteration: Alteration {
    public string AllEffects = "Boost|Boost2|Turbo|Turbo2|TurboRoulette|Fragile|NoSteering|SlowMotion|NoBrake|Cruise|Reset|NoEngine";
    public override void ChangeInventory() {
        AddCheckpointTrigger();
    }
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
        map.PlaceRelative(start.Select("!Water&!(RoadIce)"), GateSpecial,Move(0,-16,0).AddChain(moveChain));
        map.PlaceRelative(start.Select("RoadIce"), GateSpecial,Move(0,-8,0).AddChain(moveChain));
        map.PlaceRelative(inventory.GetArticle("RoadWaterStart"), GateSpecial,Move(0,-16,-2).AddChain(moveChain));
        inventory.Select(BlockType.Item).Select("MapStart&Gate").AddKeyword(Effect).RemoveKeyword(["MapStart", "Left", "Right", "Center"]).PlaceRelative(map,Move(0,0,-10).AddChain(moveChain));
        map.PlaceStagedBlocks();
    }
}
class Cruise: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"Cruise",Move(0,0,1));
    }
}

class Fragile: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"Fragile",Move(0,0,1));
        PlaceStartEffect(map,"Fragile");
    }
}

//TODO 100 Fragile, Fragile + remove reset + start Macroblock

class FreeWheel: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"Turbo");
        PlaceStartEffect(map,"Turbo");
        PlaceCPEffect(map,"NoEngine",Move(0,0,1));
        PlaceStartEffect(map,"NoEngine",Move(0,0,3));
    }
}

class Glider: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"Boost",oriented: true);
        PlaceStartEffect(map,"Boost",oriented: true);
    }
}

class NoBrake: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"NoBrake",Move(0,0,1));
        PlaceStartEffect(map,"NoBrake");
    }
}

class NoEffect: EffectAlteration {
    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select(AllEffects)
            .RemoveKeyword(["Boost","Boost2","Turbo","Turbo2","TurboRoulette","Fragile","NoSteering","SlowMotion","NoBrake","Cruise","Reset","NoEngine"]).Replace(map);
        map.Delete(inventory.Select(BlockType.Item).Select(AllEffects));
        map.PlaceStagedBlocks();
    }
    public override void ChangeInventory()
    {
        AddNoCPBlocks();
    }
}

//TODO no-grip

class NoSteer: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"NoSteering",Move(0,0,1));
        PlaceStartEffect(map,"NoSteering");
    }
}

//TODO random-dankness

//TODO random-effects

class Reactor: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"Boost2",oriented: true);
        PlaceStartEffect(map,"Boost2",oriented: true);
    }
}

class ReactorDown: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"Boost2",RotateMid(PI,0,0),true);
        PlaceStartEffect(map,"Boost2",RotateMid(PI,0,0),true);
    }
}

class RedEffects: EffectAlteration {
    public override void Run(Map map){
        inventory.Select(AllEffects).RemoveKeyword(["Boost","Boost2","Turbo","Turbo2","TurboRoulette","Fragile","NoSteering","SlowMotion","NoBrake","Cruise","Reset","NoEngine"]).AddKeyword("Turbo2").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class RngBooster: EffectAlteration {
    public override void Run(Map map){
        inventory.Select(AllEffects).RemoveKeyword(["Boost","Boost2","Turbo","Turbo2","TurboRoulette","Fragile","NoSteering","SlowMotion","NoBrake","Cruise","Reset","NoEngine"]).AddKeyword("TurboRoulette").Replace(map);
        map.PlaceStagedBlocks();
    }
}


class SlowMo: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"SlowMotion",Move(0,0,1));
        PlaceStartEffect(map,"SlowMotion");
    }
}

//TODO WetWheels

//TODO Worn-tires, similair to 100 fragile