public class OneBack: Alteration {
    public override string Description => "moves the Finish one Tile back";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,0,32));
        map.PlaceStagedBlocks();
    }
}

public class OneForward: Alteration {
    public override string Description => "moves the Finish one Tile forward";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,0,-32));
        map.PlaceStagedBlocks();
    }
}

public class OneDown: Alteration {
    public override string Description => "moves the Finish one Tile down";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select("Finish|Multilap").Edit().PlaceRelative(map,Move(0,-8,0));
        map.Delete(inventory.Select("Finish|Multilap"),true);
        map.PlaceStagedBlocks();
    }
}

public class OneLeft: Alteration {
    public override string Description => "moves the Finish one Tile to the left";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(32,0,0));
        map.PlaceStagedBlocks();
    }
}

public class OneRight: Alteration {
    public override string Description => "moves the Finish one Tile to the right";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(-32,0,0));
        map.PlaceStagedBlocks();
    }
}

public class OneUP: Alteration {
    public override string Description => "moves the Finish one Tile up";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,8,0));
        map.PlaceStagedBlocks();
    }
}

public class TwoUP: Alteration {
    public override string Description => "moves the Finish two Tiles up";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,16,0));
        map.PlaceStagedBlocks();
    }
}

//better reverse (manual)

//cp1 is end (manual)

//TODO floor-fin (Macro)block

//TODO Ground-Clippers, Pillars at y=0 get (custom) finishblock

public class Inclined : Alteration {
    public override string Description => "tilt's the start and finish down";
    public override bool Published => true;
    public override bool LikeAN => false;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select("MapStart|Multilap|Finish").Edit().PlaceRelative(map,Rotate(0,0.2f*PI,0));
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

public class ThereAndBack : Alteration {
    public override string Description => "replaces Start with Multilap and Finish with Checkpoint";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public override void Run(Map map){
        inventory.Select("Finish").RemoveKeyword("Finish").AddKeyword("Checkpoint").Replace(map);//TODO (Custom)blocks (No removekeyword)
        inventory.Select("MapStart").RemoveKeyword("MapStart").AddKeyword("Multilap").Replace(map);//TODO (Custom)blocks
        map.map.IsLapRace = true;
        map.map.NbLaps = 1;
        map.PlaceStagedBlocks();
    }
}

//yep tree puzzle (manual)
