using GBX.NET;
using GBX.NET.Engines.GameData;

using GBX.NET;

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

public class STTC : Alteration
{
    public override string Description => "sttf + moves center to absolute center of the environment";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override List<Alteration> AlterationsBefore => [new STTF()];

    public override void Run(Inventory inventory, Map map){
        var finishArticles = inventory.Any(["Finish"]);
        var finishes = finishArticles.SelectMany(map.GetBlocks).ToList();
        if (finishes.Count == 0) return;
        var offset = MapCenter(map) - BoundsCenter(finishes);
        map.Move(finishArticles, new Translate(offset));
        map.PlaceStagedBlocks();
    }

    // The point shared by the 4 middle blocks (24 / 32 blocks of 32 units), y is ignored
    
    
    private static Vec3 MapCenter(Map map) {
        var isStadium = map.map.Collection?.ToString().StartsWith("Stadium", StringComparison.OrdinalIgnoreCase) ?? true;
        var center = (isStadium ? 24 : 32) * 32;
        return new Vec3(center, 0, center);
    }

    // Center of the x/z bounding box around all finishes, y is ignored
    private static Vec3 BoundsCenter(List<Block> blocks) {
        var points = blocks.SelectMany(Footprint).ToList();
        return new Vec3(
            (points.Min(p => p.X) + points.Max(p => p.X)) / 2,
            0,
            (points.Min(p => p.Z) + points.Max(p => p.Z)) / 2
        );
    }

    // Corners of a block's box (rotation included, so a block laid flat uses its height), items only have their origin
    private static IEnumerable<Vec3> Footprint(Block block) {
        if (block.blockType is BlockType.Item or BlockType.CustomItem) {
            return [block.position.coords];
        }
        var width = block.article.Width * 32;
        var height = block.article.Height * 8;
        var length = block.article.Length * 32;
        return new[] { 0f, width }
            .SelectMany(x => new[] { 0f, height }.SelectMany(y => new[] { 0f, length }.Select(z => new Vec3(x, y, z))))
            .Select(corner => {
                Position position = new(block.position.coords, block.position.YawPitchRoll);
                position.Move(corner);
                return position.coords;
            });
    }
}

public class Flags : Alteration {
    public override string Description => "removes all finishes, adds finish triggers to all the flags (excluding their poles) on the map";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override List<Alteration> AlterationsBefore => [new STTF()];

    internal override List<ArticleProvider> customBlocks => [new ArticleProvider("Flags\\")];

    public override void Run(Inventory inventory, Map map){
        map.Delete(inventory.Any(["Finish"]));
        inventory
            .Select("Flag")
            .Any(["8m", "16m"])
            .Edit()
            .AddKeyword("ClothFinishTrigger")
            .PlaceRelative(inventory, map);
        map.stagedBlocks.ForEach(b => {
            b.WaypointSpecialProperty = GoalProperty();
            b.color = DifficultyColor.Default;
        });
        map.PlaceStagedBlocks();
    }

    private static CGameWaypointSpecialProperty GoalProperty() {
        var property = new CGameWaypointSpecialProperty() { Tag = "Goal", Order = 0 };
        property.Chunks.Create<CGameWaypointSpecialProperty.Chunk2E009000>().Version = 2;
        return property;
    }
}

//yep tree puzzle (manual)
