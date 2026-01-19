using GBX.NET.Engines.Game;

public class ChangeEnvironment(string GamePlay, string? Car = null): Alteration {
    public override string Description => $"adds {GamePlay} Carswitch at Start and replaces all Carswitchgates";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => true;

    public override void Run(Inventory inventory, Map map){
        if(Car is not null)
            // reverts it when opened without editor++
            map.map.PlayerModel = new GBX.NET.Ident(Car, 10003, "Nadeo");
        else
        {
            inventory.Select("Gameplay").Edit().RemoveKeyword("Snow").RemoveKeyword("Desert").RemoveKeyword("Rally").RemoveKeyword("Stadium").AddKeyword(GamePlay).Replace(inventory, map);
            Inventory start = inventory.Select(BlockType.Block).Select("MapStart");
            Article GateSpecial = inventory.GetArticle("GateGameplay" + GamePlay);
            map.PlaceRelative(start.Not("Water").Not("RoadIce"), GateSpecial,[new Offset(0,-16,0)]);
            map.PlaceRelative(start.Select("RoadIce"), GateSpecial,[new Offset(0,-8,0)]);
            map.PlaceRelative(inventory.GetArticle("RoadWaterStart"), GateSpecial,[new Offset(0,-16,-2)]);
            inventory.Select(["MapStart","Gate"]).Edit().AddKeyword("Gameplay").AddKeyword(GamePlay).RemoveKeyword(["MapStart", "Left", "Right", "Center", "v2"]).PlaceRelative(inventory, map,[new Offset(0,0,-10)]);
            Inventory Multilap = inventory.Select(BlockType.Block).Select("Multilap");
            map.PlaceRelative(Multilap, GateSpecial,[new Offset(0,-16,-10)]);
            map.PlaceStagedBlocks();
        }
        map.Delete(inventory.Select("Gameplay").Not(GamePlay));
    }
}

public class Stadium: ChangeEnvironment {
    public Stadium(): base("Stadium", "CarSport") { }
}

public class Snow: ChangeEnvironment {
    public Snow(): base("Snow", "CarSnow") { }
}

public class Rally: ChangeEnvironment {
    public Rally(): base("Rally", "CarRally") { }
}

public class Desert: ChangeEnvironment {
    public Desert(): base("Desert","CarDesert") { }
}

//Snow Carswitch manual

public class ChangeSnowCarswitch(string GamePlay): Alteration {
    public override string Description => $"changes Snow Carswitch to {GamePlay} Carswitch";
    public override bool Published => false;
    public override bool LikeAN => false;
    public override bool Complete => false;

    public override void Run(Inventory inventory, Map map){
        inventory.Select(["Gameplay","Snow"]).Edit().RemoveKeyword("Snow").AddKeyword(GamePlay).Replace(inventory, map);
        map.PlaceStagedBlocks();
    }
}
public class SnowCarswitchToRally: ChangeSnowCarswitch {
    public SnowCarswitchToRally(): base("Rally") { }
}
public class SnowCarswitchToDesert: ChangeSnowCarswitch {
    public SnowCarswitchToDesert(): base("Desert") { }
}
