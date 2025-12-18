public class Surface(CustomSurfaceAlteration SurfaceAlt, string Surface, bool light = false,Alteration? sceneryAlteration = null) : Alteration {
    // light and Heavy SurfaceAlterations base blocks from HeavyWood (in CustomBlocks/HeavySurface/)
    public override string Description => "replaces all drivable surfaces with " + Surface;
    public override bool Published => false;
    public override bool LikeAN => false;
    public override bool Complete => false;

    internal override List<CustomBlockAlteration> customBlockAlts => [SurfaceAlt];
    public override List<Alteration> AlterationsBefore => sceneryAlteration != null ? [new AirMode(), sceneryAlteration] : [new AirMode(), ];
    public List<string> VanillaSurfaces => ["Grass","Dirt","Plastic","Ice","Tech"];

    protected override void Run(Inventory inventory, Map map) {
        if (light) {
            if (!VanillaSurfaces.Contains(Surface)) {
                throw new Exception("Light SurfaceAlterations requires a VanillaSurface type to be specified.");
            }
            //working apart from light CP/Start still noted as CP/Start
            Inventory Platform = inventory.Select("Platform").Any(VanillaSurfaces);
            Platform.Edit().RemoveKeyword(VanillaSurfaces).AddKeyword(Surface).Replace(inventory, map);
            inventory.Any(["OpenTechRoad", "OpenDirtRoad", "OpenGrassRoad", "OpenIceRoad"]).Edit().RemoveKeyword(["OpenTechRoad","OpenDirtRoad","OpenGrassRoad","OpenIceRoad"]).AddKeyword($"Open{Surface}Road").Replace(inventory, map);
            inventory.Any(["OpenTechZone","OpenDirtZone","OpenGrassZone","OpenIceZone"]).Edit().RemoveKeyword(["OpenTechZone","OpenDirtZone","OpenGrassZone","OpenIceZone"]).AddKeyword($"Open{Surface}Zone").Replace(inventory, map);
            map.PlaceStagedBlocks();
            (inventory/Platform).Not([$"{Surface}",$"Open{Surface}Road",$"Open{Surface}Zone"]).Edit().AddKeyword($"{Surface}Surface").PlaceRelative(inventory, map);
            // map.stagedBlocks.ForEach(x => x.IsAir = false);
            map.PlaceStagedBlocks(false);
        } else {
            inventory.Edit().AddKeyword($"{Surface}Surface").Replace(inventory, map);
            // inventory.Edit().AddKeyword([$"{Surface}Surface","Middle"]));
            map.PlaceStagedBlocks(false);
        }
    }
}

public class Dirt : Surface {
    public override string Description => "replaces all drivable surfaces with Dirt";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public Dirt() : base(new DirtSurface(), "Dirt", true, new SandScenery()) { }
}

//TODO Fast-Magnet

//flooded manual

public class Grass : Surface {
    public override string Description => "replaces all drivable surfaces with Grass";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;
    public Grass() : base(new GrassSurface(), "Grass", true, new GrassScenery()) { }
}

public class Ice : Surface {
    public override string Description => "replaces all drivable surfaces with Ice";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;
    public Ice() : base(new IceSurface(), "Ice", true) { }
}

public class Magnet : Surface
{
    public override string Description => "replaces all drivable surfaces with Magnet";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public Magnet() : base(new MagnetSurface(), "Magnet", false) { }
}

//mixed manual

public class Penalty : Alteration {
    public override string Description => "replaces all drivable surfaces with according Penalty surface";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;

    protected override void Run(Inventory inventory, Map map){
        Inventory Platform = inventory.Select("Platform");
        Platform.Any(["Ice","Dirt"]).Edit().RemoveKeyword("Platform").AddKeyword("DecoPlatform").Replace(inventory, map);
        Platform.Any(["Grass","Tech"]).Edit().RemoveKeyword(["Platform","Grass","Tech"]).AddKeyword("DecoPlatform").Replace(inventory, map);
        Inventory notCurve2 = inventory.Not("Curve2");
        Inventory Slope2 = notCurve2.Select("Slope2");
        Inventory notSlope2 = notCurve2.Not("Slope2");
        List<string> removeKeywords = ["EndLargeRight", "EndLargeLeft", "EndRight", "EndLeft", "Straight", "Straight", "Left", "Right", "Down", "Up", "Curve1", "In", "Out"];
        Slope2.Select("OpenTechZone").Edit().RemoveKeyword(["OpenTechZone",..removeKeywords]).AddKeyword("DecoPlatform").Replace(inventory, map);
        Slope2.Select("OpenGrassZone").Edit().RemoveKeyword(["OpenGrassZone",..removeKeywords]).AddKeyword("DecoPlatform").Replace(inventory, map);
        Slope2.Select("OpenDirtZone").Edit().RemoveKeyword(["OpenDirtZone",..removeKeywords]).AddKeyword(["DecoPlatform", "Dirt"]).Replace(inventory, map);
        Slope2.Select("OpenIceZone").Edit().RemoveKeyword(["OpenIceZone",..removeKeywords]).AddKeyword(["DecoPlatform", "Ice"]).Replace(inventory, map);
        Slope2.Select("OpenTechZone").Edit().RemoveKeyword(["OpenTechZone",..removeKeywords]).AddKeyword(["DecoPlatform","Straight"]).Replace(inventory, map);
        Slope2.Select("OpenGrassZone").Edit().RemoveKeyword(["OpenGrassZone",..removeKeywords]).AddKeyword(["DecoPlatform","Straight"]).Replace(inventory, map);
        Slope2.Select("OpenDirtZone").Edit().RemoveKeyword(["OpenDirtZone",..removeKeywords]).AddKeyword(["DecoPlatform","Straight", "Dirt"]).Replace(inventory, map);
        Slope2.Select("OpenIceZone").Edit().RemoveKeyword(["OpenIceZone",..removeKeywords]).AddKeyword(["DecoPlatform","Straight", "Ice"]).Replace(inventory, map);
        notSlope2.Select("OpenTechZone").Edit().RemoveKeyword(["OpenTechZone",..removeKeywords]).AddKeyword(["DecoPlatform","Base"]).Replace(inventory, map);
        notSlope2.Select("OpenGrassZone").Edit().RemoveKeyword(["OpenGrassZone",..removeKeywords]).AddKeyword(["DecoPlatform","Base"]).Replace(inventory, map);
        notSlope2.Select("OpenDirtZone").Edit().RemoveKeyword(["OpenDirtZone",..removeKeywords]).AddKeyword(["DecoPlatform","Base", "Dirt"]).Replace(inventory, map);
        notSlope2.Select("OpenIceZone").Edit().RemoveKeyword(["OpenIceZone",..removeKeywords]).AddKeyword(["DecoPlatform","Base", "Ice"]).Replace(inventory, map);
        Inventory notCheckpoints = notCurve2.Select("!Checkpoint");
        notCheckpoints.Select("OpenTechRoad").Edit().RemoveKeyword(["OpenTechRoad","Curve1"]).AddKeyword(["DecoPlatform"]).Replace(inventory, map);
        notCheckpoints.Select("OpenGrassRoad").Edit().RemoveKeyword(["OpenGrassRoad","Curve1"]).AddKeyword(["DecoPlatform"]).Replace(inventory, map);
        notCheckpoints.Select("OpenDirtRoad").Edit().RemoveKeyword(["OpenDirtRoad","Curve1"]).AddKeyword(["DecoPlatform", "Dirt"]).Replace(inventory, map);
        notCheckpoints.Select("OpenIceRoad").Edit().RemoveKeyword(["OpenIceRoad","Curve1"]).AddKeyword(["DecoPlatform", "Ice"]).Replace(inventory, map);
        
        map.PlaceStagedBlocks();
    }
}

public class Plastic : Surface {
    public override string Description => "replaces all drivable surfaces with Plastic";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;
    
    public Plastic() : base(new PlasticSurface(), "Plastic", true) { }

}

public class Road : Surface {
    public override string Description => "replaces all drivable surfaces with Tech";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;

    public Road() : base(new TechSurface(), "Tech", true) { }
}

public class Wood : Surface {
    public override string Description => "replaces all drivable surfaces with Wood";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;
    public Wood() : base(new WoodSurface(), "Wood") { }

}

public class Bobsleigh : Alteration { //half manual
    public override string Description => "replaces all roads with Bobsleigh";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;
    
    protected override void Run(Inventory inventory, Map map){
        inventory.Any(["RoadBump","RoadDirt","RoadTech"]).Edit().RemoveKeyword(["RoadBump","RoadDirt","RoadTech"]).AddKeyword("RoadIce").Replace(inventory, map);
        map.PlaceStagedBlocks();
    }
}

//pipe manual

public class Sausage : Alteration { //half manual
    public override string Description => "replaces all roads with Sausage";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;
    
    protected override void Run(Inventory inventory, Map map){
        List<string> roadKeywords = ["RoadIce","RoadDirt","RoadTech"];
        inventory.Any(roadKeywords).Edit().RemoveKeyword(roadKeywords).AddKeyword("RoadBump").Replace(inventory, map);
        map.PlaceStagedBlocks();
    }
}

//slot-track manual

public class Surfaceless: Alteration {
    public override string Description => "removes all non-Pillar blocks, exccept for Start, Finish and Checkpoints";
    public override bool Published => true;
    public override bool LikeAN => true;
    public override bool Complete => false;
    
    protected override void Run(Inventory inventory, Map map){    
        Inventory Blocks = inventory.Select(BlockType.Block);
        map.PlaceRelative(Blocks.Select("MapStart"),inventory.GetArticle("GateStartCenter32m"));
        map.PlaceRelative(Blocks.Select("Checkpoint"),inventory.GetArticle("GateCheckpointCenter32m"));
        map.PlaceRelative(Blocks.Select("Finish"),inventory.GetArticle("GateFinishCenter32m"));
        inventory.Select(BlockType.Item).Select(["MapStart","Finish","Checkpoint"]).Edit().PlaceRelative(inventory, map);
        map.Delete(inventory/inventory.Select(BlockType.Pillar));
        map.PlaceStagedBlocks();
    }
}

//TODO underwater (Macroblock)

public class RouteOnly: Alteration {
    public override string Description => "removes all non-drivable surfaces";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false;
    
    // public override List<InventoryChange> customBlockAlts => [new CustomBlockSet(new RouteOnlyBlock())];
    protected override void Run(Inventory inventory, Map map){
        inventory.Edit().AddKeyword("RouteOnlyBlock").Replace(inventory, map);
        map.Delete(inventory/inventory.Select(BlockType.Item).Any(["MapStart","Finish","Checkpoint"]));
        map.PlaceStagedBlocks();
    }
}