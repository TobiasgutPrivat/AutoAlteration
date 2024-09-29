class OneBack: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,0,32));
        map.PlaceStagedBlocks();
    }
}

class OneForward: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,0,-32));
        map.PlaceStagedBlocks();
    }
}

class OneDown: Alteration {
    public override void Run(Map map){
        inventory.Select("Finish|Multilap").Edit().PlaceRelative(map,Move(0,-8,0));
        map.Delete(inventory.Select("Finish|Multilap"),true);
        map.PlaceStagedBlocks();
    }
}

class OneLeft: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(32,0,0));
        map.PlaceStagedBlocks();
    }
}

class OneRight: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(-32,0,0));
        map.PlaceStagedBlocks();
    }
}

class OneUP: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,8,0));
        map.PlaceStagedBlocks();
    }
}

class TwoUP: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,16,0));
        map.PlaceStagedBlocks();
    }
}

//better reverse (manual)

//cp1 is end (manual)

//TODO floor-fin (Macro)block

//TODO Ground-Clippers, Pillars at y=0 get (custom) finishblock

class Inclined : Alteration {
    public override void Run(Map map){
        inventory.Select("MapStart|Multilap").Edit().PlaceRelative(map,Rotate(0,0.2f*PI,0));
        map.Delete(inventory.Select("MapStart"),true);
        map.PlaceStagedBlocks();
    }
}

//TODO Manslaughter (custom) finishes

//no gear 5 (manual)

//TODO Podium (custom)finish

//puzzle (manual)

//TODO reverse, (custom)blocks

//TODO Roofing, (Macro)block

//short (manual)

//TODO sky is the finish (Macro)block

class ThereAndBack : Alteration {
    public override void Run(Map map){
        inventory.Select("Finish").RemoveKeyword("Finish").AddKeyword("Checkpoint").Replace(map);//TODO (Custom)blocks (No removekeyword)
        inventory.Select("MapStart").RemoveKeyword("MapStart").AddKeyword("Multilap").Replace(map);//TODO (Custom)blocks
        map.map.IsLapRace = true;
        map.map.NbLaps = 1;
        map.PlaceStagedBlocks();
    }
}

//yep tree puzzle (manual)
