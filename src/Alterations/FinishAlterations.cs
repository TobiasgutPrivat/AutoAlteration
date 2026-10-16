using GBX.NET;
using GBX.NET.Engines.GameData;

public class MoveFinish(MoveChain move): Alteration {
    public override string Description => "Moves the Finish to a new position";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Inventory inventory, Map map){
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

    public override void Run(Inventory inventory, Map map){
        inventory.Select(["MapStart","Multilap","Finish"]).Edit().PlaceRelative(inventory, map,new Rotate(0,0.2f*PI,0));
        map.Delete(inventory.Select("MapStart"),true);
        map.PlaceStagedBlocks();
    }
}

//TODO Manslaughter (custom) finishes

//no gear 5 (manual)

//TODO Podium (custom)finish

//puzzle (manual)

public class Reverse : Alteration {
    public override string Description => "swaps start and finish";
    public override bool Published => true;
    public override bool LikeAN => false;
    public override bool Complete => true;

    public override void Run(Inventory inventory, Map map){
        inventory.Select("MapStart").Edit().RemoveKeyword("MapStart").AddKeyword("Finish").Replace(inventory, map);
        inventory.Select("Finish").Edit().RemoveKeyword("Finish").AddKeyword("MapStart").Replace(inventory, map);
        map.PlaceStagedBlocks();
    }
}

//TODO Roofing, (Macro)block

//short (manual)

//TODO sky is the finish (Macro)block

public class ThereAndBack : Alteration {
    public override string Description => "replaces Start with Multilap and Finish with Checkpoint";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public override void Run(Inventory inventory, Map map){
        inventory.Select("Finish").Edit().RemoveKeyword("Finish").AddKeyword("Checkpoint").Replace(inventory, map);//TODO (Custom)blocks (No removekeyword)
        inventory.Select("MapStart").Edit().RemoveKeyword("MapStart").AddKeyword("Multilap").Replace(inventory, map);//TODO (Custom)blocks
        map.map.IsLapRace = true;
        map.map.NbLaps = 1;
        map.PlaceStagedBlocks();
    }
}

public class Flags : Alteration {
    public override string Description => "removes all finishes, adds finish triggers to all the flags (excluding their poles) on the map";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => true;

    internal override List<ArticleProvider> customBlocks => [new ArticleProvider("Flags\\")];

    public override void Run(Inventory inventory, Map map){
        var sttf = new STTF();
        sttf.Run(inventory, map);

        map.Delete(inventory.Any(["Finish","Multilap"]));

        foreach (string size in new[] { "8m", "16m" }) {
            Article flag = inventory.GetArticle($"Flag{size}");
            Article trigger = inventory.GetArticle($"ClothFinishTriggerFlag{size}.Item.Gbx");
            map.PlaceRelative(flag, trigger); // the trigger uses the exact pose of its flag
        }

        map.stagedBlocks.ForEach(b => {
            b.WaypointSpecialProperty = GoalProperty();
            b.color = DifficultyColor.Default;
            b.BlockUnitCoord = UnitCoord(b.position.coords, map);
        });
        map.PlaceStagedBlocks();
    }

    // The chunks are needed for the property to get serialized (same layout as the editor writes)
    private static CGameWaypointSpecialProperty GoalProperty() {
        CGameWaypointSpecialProperty property = new() { Tag = "Goal", Order = 0 };
        property.Chunks.Create<CGameWaypointSpecialProperty.Chunk2E009000>().Version = 2;
        return property;
    }

    // The editor stores the grid cell of an item, in a flag (255) is unset which the game doesn't count as a finish
    private static Byte3 UnitCoord(Vec3 position, Map map) {
        // y coordinate of the ground, measured on maps made in the editor
        int groundHeight = map.map.Collection.ToString() switch {
            "RedIsland" or "WhiteShore" => 15,
            "BlueBay" or "GreenCoast" => 5,
            _ => map.map.DecoBaseHeightOffset
        };
        return new Byte3(
            (byte)Math.Floor(position.X / 32),
            (byte)(Math.Floor(position.Y / 8) + groundHeight),
            (byte)Math.Floor(position.Z / 32)
        );
    }
}

//yep tree puzzle (manual)
