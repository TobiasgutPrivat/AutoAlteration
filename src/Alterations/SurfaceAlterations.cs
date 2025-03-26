// class LightSurface : Alteration {
//     public void AlterLightSurface(Map map, string Surface) {
//         inventory.Select("!Light" + Surface + "&(RoadTech|RoadBump|RoadDirt|RoadIce|OpenGrassRoad|OpenDirtRoad|OpenTechRoad|OpenIceRoad|OpenGrassZone|OpenDirtZone|OpenTechZone|OpenIceZone)").AddKeyword("Light" + Surface).PlaceRelative(map);
//         inventory.Select("Platform").RemoveKeyword(["Grass","Dirt","Plastic","Ice","Tech"]).AddKeyword(Surface).Replace(map);
//     }
// }

class Dirt : Alteration {
    public override string Description => "replaces all drivable surfaces with Dirt";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;

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
    public override string Description => "replaces all drivable surfaces with Grass";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public override List<InventoryChange> InventoryChanges => [new LightSurface(new GrassSurface())];
    public override void Run(Map map){
        Inventory Platform = inventory.Select("Platform");
        Platform.RemoveKeyword(["Grass","Dirt","Plastic","Ice","Tech"]).AddKeyword("Grass").Replace(map);
        //TODO think about Opens
        map.PlaceStagedBlocks();
        (!Platform).Select("!Platform&!Grass&!OpenGrassRoad&!OpenGrassZone").AddKeyword("GrassSurfaceLight").PlaceRelative(map);
        map.stagedBlocks.ForEach(x => x.IsAir = false);
        map.PlaceStagedBlocks(false);
    }
}

class Ice : Alteration {
    public override string Description => "replaces all drivable surfaces with Ice";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;
    
    public override List<InventoryChange> InventoryChanges => [new LightSurface(new IceSurface())];
    public override void Run(Map map){
        inventory.Select("!Ice&!OpenIceRoad&!OpenIceZone&!RoadIce").AddKeyword("IceSurface").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class Magnet : Alteration {
    public override string Description => "replaces all drivable surfaces with Magnet";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;
    
    public override List<InventoryChange> InventoryChanges => [new LightSurface(new MagnetSurface())];
    public override void Run(Map map){
        inventory.AddKeyword("MagnetSurface").Replace(map);
        map.PlaceStagedBlocks();
    }
}

//mixed manual

class Penalty : Alteration {
    public override string Description => "replaces all drivable surfaces with according Penalty surface";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

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
        
        map.PlaceStagedBlocks();
    }
}

class Plastic : Alteration {
    public override string Description => "replaces all drivable surfaces with Plastic";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;
    
    public override List<InventoryChange> InventoryChanges => [new LightSurface(new PlasticSurface())];
    public override void Run(Map map){
        inventory.Select("!Plastic&!OpenPlasticRoad&!OpenPlasticZone&!RoadPlastic").AddKeyword("PlasticSurface").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class Road : Alteration {
    public override string Description => "replaces all drivable surfaces with Tech";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;
    
    public override List<InventoryChange> InventoryChanges => [new HeavySurface(new TechSurface())];
    public override void Run(Map map){
        inventory.Select("!Tech&!OpenTechRoad&!OpenTechZone&!RoadTech").AddKeyword("TechSurface").Replace(map);
        map.PlaceStagedBlocks();
    }
}

class Wood : Alteration {
    public override string Description => "replaces all drivable surfaces with Wood";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;
    
    public override List<InventoryChange> InventoryChanges => [new HeavySurface(new WoodSurface(),false)];
    public override void Run(Map map){
        //TODO get full block set
        Inventory specific = inventory.Select(article => article.MapSpecific); //handle mapSpecifics seperately

        // 1. find blocks which should be replaced by Heavy
        KeywordEdit HeavyReplace = specific.Edit().AddKeyword("WoodSurfaceHeavy");
        HeavyReplace.Add((!specific).AddKeyword(["WoodSurfaceHeavy","Middle"]));
        HeavyReplace.Add((!specific).Select("Platform").RemoveKeyword(["Grass","Dirt","Plastic","Ice","Tech"]).AddKeyword(["Plastic","WoodSurfaceHeavy"]));
        //TODO some more blocks not yet handled
        Inventory replaced = HeavyReplace.Align(); //blocks which will be replaced by Heavy later

        // 2. place Pillars on blocks which will be replaced by Heavy and were not air
        KeywordEdit Pillar = replaced.RemoveKeyword([""]); //TODO align to pillars

        Pillar.PlaceRelative(map,blockCondition: block => !block.Bit21); //place Pillars for blocks which were not in AirMode
        map.PlaceStagedBlocks();

        // 3. replace blocks
        HeavyReplace.Replace(map);
        specific.AddKeyword(["WoodSurface"]).Replace(map);
        map.stagedBlocks.ForEach(x => x.IsAir = false);
        map.PlaceStagedBlocks(false);
    }
}

class Bobsleigh : Alteration { //half manual
    public override string Description => "replaces all roads with Bobsleigh";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;
    
    public override void Run(Map map){
        inventory.Select("RoadBump|RoadDirt|RoadTech").RemoveKeyword(["RoadBump","RoadDirt","RoadTech"]).AddKeyword("RoadIce").Replace(map);
        map.PlaceStagedBlocks();
    }
}

//pipe manual

class Sausage : Alteration { //half manual
    public override string Description => "replaces all roads with Sausage";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;
    
    public override void Run(Map map){
        inventory.Select("RoadIce|RoadDirt|RoadTech").RemoveKeyword(["RoadIce","RoadDirt","RoadTech"]).AddKeyword("RoadBump").Replace(map);
        map.PlaceStagedBlocks();
    }
}

//slot-track manual

class Surfaceless: Alteration {
    public override string Description => "removes all non-Pillar blocks, exccept for Start, Finish and Checkpoints";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;
    
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
    public override string Description => "removes all non-drivable surfaces";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;
    
    public override List<InventoryChange> InventoryChanges => [new CustomBlockSet(new RouteOnlyBlock())];
    public override void Run(Map map){
        inventory.AddKeyword("RouteOnlyBlock").Replace(map);
        map.Delete(inventory.Sub(inventory.Select(BlockType.Item).Select("MapStart|Finish|Checkpoint")));
        map.PlaceStagedBlocks();
    }
}