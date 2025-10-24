public class ChangeEnvironment(string GamePlay): Alteration {
    public override string Description => $"adds {GamePlay} Carswitch at Start and replaces all Carswitchgates";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        inventory.Select("Gameplay").RemoveKeyword("Snow").RemoveKeyword("Desert").RemoveKeyword("Rally").RemoveKeyword("Stadium").AddKeyword(GamePlay).Replace(map);
        Inventory start = inventory.Select(BlockType.Block).Select("MapStart");
        Article GateSpecial = inventory.GetArticle("GateGameplay" + GamePlay);
        map.PlaceRelative(start.Select("!Water&!RoadIce"), GateSpecial,Move(0,-16,0));
        map.PlaceRelative(start.Select("RoadIce"), GateSpecial,Move(0,-8,0));
        map.PlaceRelative(inventory.GetArticle("RoadWaterStart"), GateSpecial,Move(0,-16,-2));
        inventory.Select("MapStart&Gate").AddKeyword("Gameplay").AddKeyword(GamePlay).RemoveKeyword(["MapStart", "Left", "Right", "Center", "v2"]).PlaceRelative(map,Move(0,0,-10));
        map.PlaceStagedBlocks();
        map.Delete(inventory.Select("Gameplay&!" + GamePlay));
    }
}

public class Stadium: ChangeEnvironment {
    public Stadium(): base("Stadium") { }
}

public class Snow: ChangeEnvironment {
    public Snow(): base("Snow") { }
}

public class Rally: ChangeEnvironment {
    public Rally(): base("Rally") { }
}

public class Desert: ChangeEnvironment {
    public Desert(): base("Desert") { }
}

//Snow Carswitch manual

public class ChangeSnowCarswitch(string GamePlay): Alteration {
    public override string Description => $"changes Snow Carswitch to {GamePlay} Carswitch";
    public override bool Published => false;
    public override bool LikeAN => false;
    public override bool Complete => false;

    public override void Run(Map map){
        inventory.Select("Gameplay&Snow").RemoveKeyword("Snow").AddKeyword(GamePlay).Replace(map);
        map.PlaceStagedBlocks();
    }
}
public class SnowCarswitchToRally: ChangeSnowCarswitch {
    public SnowCarswitchToRally(): base("Rally") { }
}
public class SnowCarswitchToDesert: ChangeSnowCarswitch {
    public SnowCarswitchToDesert(): base("Desert") { }
}
