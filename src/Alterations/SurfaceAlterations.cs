// class LightSurface : Alteration {
//     public void AlterLightSurface(Map map, string Surface) {
//         inventory.Select("!Light" + Surface + "&(RoadTech|RoadBump|RoadDirt|RoadIce|OpenGrassRoad|OpenDirtRoad|OpenTechRoad|OpenIceRoad|OpenGrassZone|OpenDirtZone|OpenTechZone|OpenIceZone)").AddKeyword("Light" + Surface).PlaceRelative(map);
//         inventory.Select("Platform").RemoveKeyword(["Grass","Dirt","Plastic","Ice","Tech"]).AddKeyword(Surface).Replace(map);
//     }
// }

class Dirt : Alteration {
    public override List<InventoryChange> InventoryChanges => [new LightSurface(new DirtSurface())];
    public override void Run(Map map){
        // map.PlaceRelative(inventory.Select("MapStart"),"RoadTechToThemeSnowRoadMagnet");
        inventory.Select("!Dirt&!OpenDirtRoad&!OpenDirtZone&!RoadDirt").AddKeyword("DirtSurface").Replace(map);
        map.PlaceStagedBlocks();
    }
}

//TODO Fast-Magnet

//flooded manual

class Grass : Alteration {
    public override List<InventoryChange> InventoryChanges => [new LightSurface(new GrassSurface())];
    public override void Run(Map map){
        inventory.Select("!Grass&!OpenGrassRoad&!OpenGrassZone").AddKeyword("GrassSurface").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class Ice : Alteration {
    public override List<InventoryChange> InventoryChanges => [new LightSurface(new IceSurface())];
    public override void Run(Map map){
        inventory.Select("!Ice&!OpenIceRoad&!OpenIceZone&!RoadIce").AddKeyword("IceSurface").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class Magnet : Alteration {
    public override List<InventoryChange> InventoryChanges => [new LightSurface(new MagnetSurface())];
    public override void Run(Map map){
        inventory.AddKeyword("MagnetSurface").Replace(map);
        map.PlaceStagedBlocks();
    }
}

//mixed manual

class Penalty : Alteration {

    public override void Run(Map map){
        inventory.Select("Platform&(Ice|Dirt)").RemoveKeyword("Platform").AddKeyword("DecoPlatform").Replace(map);
        inventory.Select("Platform&(Tech|Grass)").RemoveKeyword(["Platform","Grass","Tech"]).AddKeyword("DecoPlatform").Replace(map);
        Inventory notCurve2 = inventory.Select("!Curve2");
        Inventory Slope2 = notCurve2.Select("Slope2");
        Inventory notSlope2 = notCurve2.Select("!Slope2");
        Slope2.Select("OpenTechZone").RemoveKeyword(["OpenTechZone","EndLargeRight","EndLargeLeft","EndRight","EndLeft","Straight","Straight","Left","Right","Down","Up","Curve1","In","Out"]).AddKeyword("DecoPlatform").Replace(map);
        Slope2.Select("OpenGrassZone").RemoveKeyword(["OpenGrassZone","EndLargeRight","EndLargeLeft","EndRight","EndLeft","Straight","Straight","Left","Right","Down","Up","Curve1","In","Out"]).AddKeyword("DecoPlatform").Replace(map);
        Slope2.Select("OpenDirtZone").RemoveKeyword(["OpenDirtZone","EndLargeRight","EndLargeLeft","EndRight","EndLeft","Straight","Straight","Left","Right","Down","Up","Curve1","In","Out"]).AddKeyword(["DecoPlatform", "Dirt"]).Replace(map);
        Slope2.Select("OpenIceZone").RemoveKeyword(["OpenIceZone","EndLargeRight","EndLargeLeft","EndRight","EndLeft","Straight","Straight","Left","Right","Down","Up","Curve1","In","Out"]).AddKeyword(["DecoPlatform", "Ice"]).Replace(map);
        Slope2.Select("OpenTechZone").RemoveKeyword(["OpenTechZone","EndLargeRight","EndLargeLeft","EndRight","EndLeft","Straight","Straight","Left","Right","Down","Up","Curve1","In","Out"]).AddKeyword(["DecoPlatform","Straight"]).Replace(map);
        Slope2.Select("OpenGrassZone").RemoveKeyword(["OpenGrassZone","EndLargeRight","EndLargeLeft","EndRight","EndLeft","Straight","Straight","Left","Right","Down","Up","Curve1","In","Out"]).AddKeyword(["DecoPlatform","Straight"]).Replace(map);
        Slope2.Select("OpenDirtZone").RemoveKeyword(["OpenDirtZone","EndLargeRight","EndLargeLeft","EndRight","EndLeft","Straight","Straight","Left","Right","Down","Up","Curve1","In","Out"]).AddKeyword(["DecoPlatform","Straight", "Dirt"]).Replace(map);
        Slope2.Select("OpenIceZone").RemoveKeyword(["OpenIceZone","EndLargeRight","EndLargeLeft","EndRight","EndLeft","Straight","Straight","Left","Right","Down","Up","Curve1","In","Out"]).AddKeyword(["DecoPlatform","Straight", "Ice"]).Replace(map);
        notSlope2.Select("OpenTechZone").RemoveKeyword(["OpenTechZone","EndLargeRight","EndLargeLeft","EndRight","EndLeft","Straight","Straight","Left","Right","Down","Up","Curve1","In","Out"]).AddKeyword(["DecoPlatform","Base"]).Replace(map);
        notSlope2.Select("OpenGrassZone").RemoveKeyword(["OpenGrassZone","EndLargeRight","EndLargeLeft","EndRight","EndLeft","Straight","Straight","Left","Right","Down","Up","Curve1","In","Out"]).AddKeyword(["DecoPlatform","Base"]).Replace(map);
        notSlope2.Select("OpenDirtZone").RemoveKeyword(["OpenDirtZone","EndLargeRight","EndLargeLeft","EndRight","EndLeft","Straight","Straight","Left","Right","Down","Up","Curve1","In","Out"]).AddKeyword(["DecoPlatform","Base", "Dirt"]).Replace(map);
        notSlope2.Select("OpenIceZone").RemoveKeyword(["OpenIceZone","EndLargeRight","EndLargeLeft","EndRight","EndLeft","Straight","Straight","Left","Right","Down","Up","Curve1","In","Out"]).AddKeyword(["DecoPlatform","Base", "Ice"]).Replace(map);
        Inventory notCheckpoints = notCurve2.Select("!Checkpoint");
        notCheckpoints.Select("OpenTechRoad").RemoveKeyword(["OpenTechRoad","Curve1"]).AddKeyword(["DecoPlatform"]).Replace(map);
        notCheckpoints.Select("OpenGrassRoad").RemoveKeyword(["OpenGrassRoad","Curve1"]).AddKeyword(["DecoPlatform"]).Replace(map);
        notCheckpoints.Select("OpenDirtRoad").RemoveKeyword(["OpenDirtRoad","Curve1"]).AddKeyword(["DecoPlatform", "Dirt"]).Replace(map);
        notCheckpoints.Select("OpenIceRoad").RemoveKeyword(["OpenIceRoad","Curve1"]).AddKeyword(["DecoPlatform", "Ice"]).Replace(map);
        //TODO OpenRoads
        map.PlaceStagedBlocks();
    }
}

class Plastic : Alteration {
    public override List<InventoryChange> InventoryChanges => [new LightSurface(new PlasticSurface())];
    public override void Run(Map map){
        inventory.Select("!Plastic&!OpenPlasticRoad&!OpenPlasticZone&!RoadPlastic").AddKeyword("PlasticSurface").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class Road : Alteration {
    public override List<InventoryChange> InventoryChanges => [new LightSurface(new TechSurface())];
    public override void Run(Map map){
        inventory.Select("!Tech&!OpenTechRoad&!OpenTechZone&!RoadTech").AddKeyword("TechSurface").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class Wood : Alteration {
    public override List<InventoryChange> InventoryChanges => [new LightSurface(new WoodSurface())];
    public override void Run(Map map){
        inventory.AddKeyword("WoodSurface").Replace(map);
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