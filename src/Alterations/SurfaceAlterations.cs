// class LightSurface : Alteration {
//     public void AlterLightSurface(Map map, string Surface) {
//         inventory.Select("!Light" + Surface + "&(RoadTech|RoadBump|RoadDirt|RoadIce|OpenGrassRoad|OpenDirtRoad|OpenTechRoad|OpenIceRoad|OpenGrassZone|OpenDirtZone|OpenTechZone|OpenIceZone)").AddKeyword("Light" + Surface).PlaceRelative(map);
//         inventory.Select("Platform").RemoveKeyword(["Grass","Dirt","Plastic","Ice","Tech"]).AddKeyword(Surface).Replace(map);
//     }
// }

class Penalty : Alteration {
    public override void Run(Map map){
        inventory.Select("Platform&(Ice|Dirt)").RemoveKeyword("Platform").AddKeyword("DecoPlatform").Replace(map);
        inventory.Select("Platform&(Tech|Grass)").Print().RemoveKeyword(["Platform","Grass","Tech"]).AddKeyword("DecoPlatform").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class Dirt : Alteration {
    public override List<InventoryChange> InventoryChanges => [new CustomBlockSet(new HeavyDirt())];
    public override void Run(Map map){
        // map.PlaceRelative(inventory.Select("MapStart"),"RoadTechToThemeSnowRoadMagnet");
        inventory.Select("!Dirt&!OpenDirtRoad&!OpenDirtZone&!RoadDirt").AddKeyword("HeavyDirt").Replace(map);
        map.PlaceStagedBlocks();
    }
}

//TODO Fast-Magnet

//flooded manual

class Grass : Alteration {
    public override List<InventoryChange> InventoryChanges => [new CustomBlockSet(new HeavyGrass())];
    public override void Run(Map map){
        inventory.Select("!Grass&!OpenGrassRoad&!OpenGrassZone").AddKeyword("HeavyGrass").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class Ice : Alteration {
    public override List<InventoryChange> InventoryChanges => [new CustomBlockSet(new HeavyIce())];
    public override void Run(Map map){
        inventory.Select("!Ice&!OpenIceRoad&!OpenIceZone&!RoadIce").AddKeyword("HeavyIce").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class Magnet : Alteration {
    public override List<InventoryChange> InventoryChanges => [new CustomBlockSet(new HeavyMagnet())];
    public override void Run(Map map){
        inventory.AddKeyword("HeavyMagnet").Replace(map);
        map.PlaceStagedBlocks();
    }
}

//mixed manual

//TODO penalty

class Plastic : Alteration {
    public override List<InventoryChange> InventoryChanges => [new CustomBlockSet(new HeavyPlastic())];
    public override void Run(Map map){
        inventory.Select("!Plastic&!OpenPlasticRoad&!OpenPlasticZone&!RoadPlastic").AddKeyword("HeavyPlastic").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class Road : Alteration {
    public override List<InventoryChange> InventoryChanges => [new CustomBlockSet(new HeavyTech())];
    public override void Run(Map map){
        inventory.Select("!Tech&!OpenTechRoad&!OpenTechZone&!RoadTech").AddKeyword("HeavyTech").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class Wood : Alteration {
    public override List<InventoryChange> InventoryChanges => [new CustomBlockSet(new HeavyWood())];
    public override void Run(Map map){
        inventory.AddKeyword("HeavyWood").Replace(map);
        //TODO Light variant with only partial Customblocks
        map.PlaceStagedBlocks();
    }
}

class Bobsleigh : Alteration { //half manual
    public override void Run(Map map){
        inventory.Select("RoadBump|RoadDirt|RoadTech").RemoveKeyword(["RoadBump","RoadDirt","RoadTech"]).AddKeyword("RoadIce").Replace(map);
        map.PlaceStagedBlocks();
    }
}

//pipe manual

class Sausage : Alteration { //half manual
    public override void Run(Map map){
        inventory.Select("RoadIce|RoadDirt|RoadTech").RemoveKeyword(["RoadIce","RoadDirt","RoadTech"]).AddKeyword("RoadBump").Replace(map);
        map.PlaceStagedBlocks();
    }
}

//slot-track manual

class Surfaceless: Alteration {
    public override void Run(Map map){    
        Inventory Blocks = inventory.Select(BlockType.Block);
        map.PlaceRelative(Blocks.Select("MapStart"),inventory.GetArticle("GateStartCenter32m"));
        map.PlaceRelative(Blocks.Select("Checkpoint"),inventory.GetArticle("GateCheckpointCenter32m"));
        map.PlaceRelative(Blocks.Select("Finish"),inventory.GetArticle("GateFinishCenter32m"));
        inventory.Select(BlockType.Item).Select("MapStart&Finish&Checkpoint").Edit().PlaceRelative(map);
        map.Delete(inventory.Sub(inventory.Select(BlockType.Pillar)));
        map.PlaceStagedBlocks();
    }
}

//TODO underwater (Macroblock)


class RouteOnly: Alteration {
    public override List<InventoryChange> InventoryChanges => [new CustomBlockSet(new RouteOnlyBlock())];
    public override void Run(Map map){
        inventory.AddKeyword("RouteOnlyBlock").Replace(map);
        map.Delete(inventory.Sub(inventory.Select(BlockType.Item).Select("MapStart|Finish|Checkpoint")));
        map.PlaceStagedBlocks();
    }
}