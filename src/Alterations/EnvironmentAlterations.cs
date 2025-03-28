abstract public class EnvironmentAlterations: Alteration {
    public static void SetGamePlay(Map map, string GamePlay){
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

public class Stadium: EnvironmentAlterations {
    public override string Description => "adds Stadium Carswitch at Start and replaces all Carswitchgates";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        SetGamePlay(map,"Stadium");
    }
}

public class Snow: EnvironmentAlterations {
    public override string Description => "adds Snow Carswitch at Start and replaces all Carswitchgates";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        SetGamePlay(map,"Snow");
    }
}

public class Rally: EnvironmentAlterations {
    public override string Description => "adds Rally Carswitch at Start and replaces all Carswitchgates";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        SetGamePlay(map,"Rally");
    }
}

public class Desert: EnvironmentAlterations {
    public override string Description => "adds Desert Carswitch at Start and replaces all Carswitchgates";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Map map){
        SetGamePlay(map,"Desert");
    }
}

//Snow Carswitch manual

public class SnowCarswitchToDesert: Alteration {
    public override string Description => "changes Snow Carswitch to Desert Carswitch";
    public override bool Published => false;
    public override bool LikeAN => false;
    public override bool Complete => false;

    public override void Run(Map map){
        inventory.Select("Gameplay&Snow").RemoveKeyword("Snow").AddKeyword("Desert").Replace(map);
        map.PlaceStagedBlocks();
    }
}
public class SnowCarswitchToRally: Alteration {
    public override string Description => "changes Snow Carswitch to Rally Carswitch";
    public override bool Published => false;
    public override bool LikeAN => false;
    public override bool Complete => false;

    public override void Run(Map map){
        inventory.Select("Gameplay&Snow").RemoveKeyword("Snow").AddKeyword("Rally").Replace(map);
        map.PlaceStagedBlocks();
    }
}
