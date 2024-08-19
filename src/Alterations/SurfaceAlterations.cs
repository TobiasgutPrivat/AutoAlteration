class SurfaceAlteration : Alteration {
    public void AlterLightSurface(Map map, string Surface) {
        inventory.Select("!Light" + Surface + "&(RoadTech|RoadBump|RoadDirt|RoadIce|OpenGrassRoad|OpenDirtRoad|OpenTechRoad|OpenIceRoad|OpenGrassZone|OpenDirtZone|OpenTechZone|OpenIceZone)").AddKeyword("Light" + Surface).PlaceRelative(map);
        inventory.Select("Platform").RemoveKeyword(["Grass","Dirt","Plastic","Ice","Tech"]).AddKeyword(Surface).Replace(map);
    }
}
class Tech : SurfaceAlteration {
    public override void Run(Map map){
        AlterLightSurface(map, "Tech");
        map.PlaceStagedBlocks();
    }

    public override void ChangeInventory()
    {
        AddCustomBlocks("Surface/LightTech");
    }
}

class Dirt : SurfaceAlteration {
    public override void Run(Map map){
        AlterLightSurface(map, "Dirt");
        map.PlaceStagedBlocks();
    }

    public override void ChangeInventory()
    {
        AddCustomBlocks("Surface/LightDirt");
    }

}
class Grass : SurfaceAlteration {
    public override void Run(Map map){
        AlterLightSurface(map, "Grass");
        map.PlaceStagedBlocks();
    }

    public override void ChangeInventory()
    {
        AddCustomBlocks("Surface/LightGrass");
    }
}

class Plastic : SurfaceAlteration {
    public override void Run(Map map){
        AlterLightSurface(map, "Plastic");
        map.PlaceStagedBlocks();
    }

    public override void ChangeInventory()
    {
        AddCustomBlocks("Surface/LightPlastic");
    }
}

class Ice : SurfaceAlteration {
    public override void Run(Map map){
        AlterLightSurface(map, "Ice");
        map.PlaceStagedBlocks();
    }

    public override void ChangeInventory()
    {
        AddCustomBlocks("Surface/LightIce");
    }
}