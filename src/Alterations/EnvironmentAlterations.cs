class EnvironmentAlterations: Alteration {
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

class Stadium: EnvironmentAlterations {
    public override void Run(Map map){
        SetGamePlay(map,"Stadium");
    }
}

class Snow: EnvironmentAlterations {
    public override void Run(Map map){
        SetGamePlay(map,"Snow");
    }
}

class Rally: EnvironmentAlterations {
    public override void Run(Map map){
        SetGamePlay(map,"Rally");
    }
}

class Desert: EnvironmentAlterations {
    public override void Run(Map map){
        SetGamePlay(map,"Desert");
    }
}


//Snow Carswitch manual

class SnowCarswitchToDesert: Alteration {
    public override void Run(Map map){
        inventory.Select("Gameplay&Snow").RemoveKeyword("Snow").AddKeyword("Desert").Replace(map);
        map.PlaceStagedBlocks();
    }
}
class SnowCarswitchToRally: Alteration {
    public override void Run(Map map){
        inventory.Select("Gameplay&Snow").RemoveKeyword("Snow").AddKeyword("Rally").Replace(map);
        map.PlaceStagedBlocks();
    }
}
