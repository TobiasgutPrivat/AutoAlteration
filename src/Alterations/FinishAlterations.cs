public class MoveFinish(MoveChain move): Alteration {
    public override string Description => "Moves the Finish to a new position";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        map.Move(inventory.Any(["Finish","Multilap"]), move);
        map.PlaceStagedBlocks();
    }
}
public class OneBack: MoveFinish {
    public override string Description => "moves the Finish one Tile back";

    public OneBack(): base(new Offset(0,0,32)) { }
}

public class OneForward: MoveFinish {
    public override string Description => "moves the Finish one Tile forward";
    public OneForward(): base(new Offset(0,0,-32)) { }
}

public class OneDown: MoveFinish {
    public override string Description => "moves the Finish one Tile down";
    public OneDown(): base(new Offset(0,-8,0)) { }
}

public class OneLeft: MoveFinish {
    public override string Description => "moves the Finish one Tile to the left";
    public OneLeft(): base(new Offset(32,0,0)) { }
}

public class OneRight: MoveFinish {
    public override string Description => "moves the Finish one Tile to the right";
    public OneRight(): base(new Offset(-32,0,0)) { }
}

public class OneUP: MoveFinish {
    public override string Description => "moves the Finish one Tile up";
    public OneUP(): base(new Offset(0,8,0)) { }
}

public class TwoUP: MoveFinish {
    public override string Description => "moves the Finish two Tiles up";
    public TwoUP(): base(new Offset(0,16,0)) { }
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
        inventory.Select(["MapStart","Multilap","Finish"]).Edit().PlaceRelative(map,new Rotate(0,0.2f*PI,0));
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
        inventory.Select("Finish").Edit().RemoveKeyword("Finish").AddKeyword("Checkpoint").Replace(map);//TODO (Custom)blocks (No removekeyword)
        inventory.Select("MapStart").Edit().RemoveKeyword("MapStart").AddKeyword("Multilap").Replace(map);//TODO (Custom)blocks
        map.map.IsLapRace = true;
        map.map.NbLaps = 1;
        map.PlaceStagedBlocks();
    }
}

//yep tree puzzle (manual)
