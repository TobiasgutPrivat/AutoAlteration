using GBX.NET;

class Dirt : Alteration {
    public override void Run(Map map){
        inventory.Select("!HeavyDirt").AddKeyword("HeavyDirt").Replace(map);
        map.PlaceStagedBlocks();
    }

    public override void ChangeInventory()
    {
        AddCustomBlocks("Surface/HeavyDirt");
    }
}
class Tech : Alteration {
    public override void Run(Map map){
        inventory.Select("!LightTech&(RoadDirt|RoadIce|OpenDirtRoad|OpenTechRoad|OpenIceRoad|OpenDirtZone|OpenTechZone|OpenIceZone)").AddKeyword("LightTech").PlaceRelative(map);
        inventory.Select("Platform").RemoveKeyword(["Grass","Dirt","Plastic","Ice"]).AddKeyword("Tech").Replace(map);
        map.PlaceStagedBlocks();
    }

    public override void ChangeInventory()
    {
        AddCustomBlocks("Surface/LightTech");
    }
}