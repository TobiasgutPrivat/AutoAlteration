using GBX.NET;
class EffectAlteration: Alteration {
    public override void AddArticles() {
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
        inventory.Select(BlockType.Item).Select("Checkpoint").RemoveKeyword("Checkpoint").RemoveKeyword(new string[] {"Left", "Right", "Center", "v2" }).AddKeyword(Effect).PlaceRelative(map,moveChain);
        map.PlaceRelative(inventory.Select("(CheckpointTrigger|MultilapTrigger)&!Ring&!WithWall"), GateSpecial,Move(-16,-16,-16).AddChain(moveChain));
        map.PlaceRelative(inventory.Select("(CheckpointTrigger|MultilapTrigger)&WithWall&Left"), inventory.GetArticle("GateSpecial32m" + Effect),Move(0,7,0).Rotate(0,0,PI).AddChain(moveChain));
        map.PlaceRelative(inventory.Select("(CheckpointTrigger|MultilapTrigger)&WithWall&Left"), inventory.GetArticle("GateSpecial32m" + Effect),Move(6,12,0).Rotate(0,0,PI*-0.5f).AddChain(moveChain));
        map.PlaceRelative(inventory.Select("(CheckpointTrigger|MultilapTrigger)&WithWall&Right"), inventory.GetArticle("GateSpecial32m" + Effect),Move(0,7,0).Rotate(0,0,PI).AddChain(moveChain));
        map.PlaceRelative(inventory.Select("(CheckpointTrigger|MultilapTrigger)&WithWall&Right"), inventory.GetArticle("GateSpecial32m" + Effect),Move(-6,12,0).Rotate(0,0,PI*0.5f).AddChain(moveChain));
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
        inventory.Select(BlockType.Item).Select("MapStart&Gate").AddKeyword(Effect).RemoveKeyword(new string[] {"MapStart", "Left", "Right", "Center", "v2" }).PlaceRelative(map,Move(0,0,-10).AddChain(moveChain));
        map.PlaceStagedBlocks();
    }
}
class NoBrake: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"NoBrake",Move(0,0,1));
        PlaceStartEffect(map,"NoBrake");
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

class SlowMo: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"SlowMotion",Move(0,0,1));
        PlaceStartEffect(map,"SlowMotion");
    }
}

class NoSteer: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"NoSteering",Move(0,0,1));
        PlaceStartEffect(map,"NoSteering");
    }
}

class Glider: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"Boost",oriented: true);
        PlaceStartEffect(map,"Boost",oriented: true);
    }
}

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

class FreeWheel: EffectAlteration {
    public override void Run(Map map){
        PlaceCPEffect(map,"Turbo");
        PlaceStartEffect(map,"Turbo");
        PlaceCPEffect(map,"NoEngine",Move(0,0,1));
        PlaceStartEffect(map,"NoEngine",Move(0,0,3));
    }
}

class AntiBooster: Alteration {
    public override void Run(Map map){
        inventory.Select("Boost|Boost2|Turbo|Turbo2|TurboRoulette").Edit().Replace(map,RotateMid(PI,0,0));
        //TODO Diagonals
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