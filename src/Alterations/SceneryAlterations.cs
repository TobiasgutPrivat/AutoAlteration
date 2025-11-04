using GBX.NET;
using GBX.NET.Engines.Game;

public class SnowScenery : Alteration
{
    public override string Description => "turns scenery into snow";
    public override bool Published => true;
    public override bool LikeAN => false;
    public override bool Complete => false;

    protected override void Run(Inventory inventory, Map map)
    {
        // Scenery blocks: 
        inventory.Any(["Deco","Water","DecoWall","DecoPlatform"]).Edit().RemoveKeyword(["Grass","Dirt"]).AddKeyword("Ice").Replace(inventory, map);
        // Actual blocks: 
        inventory.Any(["PenaltyDirt","Penalty"]).Edit().RemoveKeyword(["PenaltyDirt","Penalty"]).AddKeyword("PenaltyIce").Replace(inventory, map);
        
        Inventory WaterWall = inventory.Select("Water").Select("Wall");
        WaterWall.Edit().RemoveKeyword([ "Dirt", "Ice", "Wood" ]).AddKeyword("Ice").Replace(inventory, map);
        
        // Open Road/Zone
        // inventory.Select("OpenDirtRoad|OpenTechRoad").RemoveKeyword(["OpenDirtRoad","OpenTechRoad"]).AddKeyword("OpenIceRoad").Replace(inventory, map);
        // inventory.Select("OpenDirtZone|OpenTechZone").RemoveKeyword(["OpenDirtZone","OpenTechZone"]).AddKeyword("OpenIceZone").Replace(inventory, map);
        // Trees

        map.Replace(inventory.GetArticles([ "SpringTreeTall", "FallTreeTall", "PalmTreeMedium", "PalmTreeDirtMedium" ]), inventory.GetArticle("FrozenTreeTall"));
        map.Replace(inventory.GetArticles([ "SpringTreeBig", "FallTreeBig" ]), inventory.GetArticle("FrozenTreeBig"));
        map.Replace(inventory.GetArticles([ "SpringTreeMedium", "FallTreeMedium" ]), inventory.GetArticle("FrozenTreeMedium"));
        map.Replace(inventory.GetArticles([ "SpringTreeSmall", "FallTreeSmall", "CactusMedium" ]), inventory.GetArticle("FrozenTreeSmall"));
        map.Replace(inventory.GetArticles([ "SpringTreeVerySmall", "FallTreeVerySmall", "CactusVerySmall" ]), inventory.GetArticle("FrozenTreeVerySmall"));
        map.Replace(inventory.GetArticles([ "FirTall", "CypressTall", "CypressDirtTall" ]), inventory.GetArticle("FirSnowTall"));
        map.Replace(inventory.GetArticles([ "FirMedium", "CypressTall", "PalmTreeSmall", "PalmTreeDirtSmall" ]), inventory.GetArticle("FirSnowMedium"));
        // map.Replace(inventory.GetArticles([ "CherryTreeMedium" ]), inventory.GetArticle("CherryTreeMedium")); // tbh, cherry trees look nice so they can stay xdd

        //
        map.Replace(inventory.GetArticles([ "Spring", "Fall" ]), inventory.GetArticle("WinterFrozenTree"));
        map.Replace(inventory.GetArticles([ "SpringPalmTree", "SummerPalmTree", "Summer" ]), inventory.GetArticle("Winter"));

        map.PlaceStagedBlocks();
        
        var skinPath = Path.Combine(AlterationConfig.DataFolder, "Templates", "IceSkin.Gbx");
        CGameCtnBlockSkin IceSkin = Gbx.Parse<CGameCtnBlockSkin>(skinPath);

        map.map.Blocks.ToList().ForEach(x => x.Skin = IceSkin);
        map.map.BakedBlocks.ToList().ForEach(x => x.Skin = IceSkin);
        map.PlaceStagedBlocks();
    }
}

public class SandScenery : Alteration
{
    public override string Description => "turns scenery into dirt";
    public override bool Published => true;
    public override bool LikeAN => false;
    public override bool Complete => false;

    protected override void Run(Inventory inventory, Map map)
    {
        // Scenery blocks: 
        inventory.Any(["Deco","Water","DecoWall","DecoPlatform"]).Edit().RemoveKeyword(["Grass","Ice"]).AddKeyword("Dirt").Replace(inventory, map);
        // Actual blocks: 
        inventory.Any(["PenaltyIce","Penalty"]).Edit().RemoveKeyword(["PenaltyIce","Penalty"]).AddKeyword("PenaltyDirt").Replace(inventory, map);
        
        Inventory WaterWall = inventory.Select("Water").Select("Wall");
        WaterWall.Edit().RemoveKeyword([ "Dirt", "Ice", "Wood" ]).AddKeyword("Dirt").Replace(inventory, map);
        
        // Open Road/Zone
        // inventory.Select("OpenIceRoad|OpenTechRoad").RemoveKeyword(["OpenIceRoad","OpenTechRoad"]).AddKeyword("OpenDirtRoad").Replace(inventory, map);
        // inventory.Select("OpenIceZone|OpenTechZone").RemoveKeyword(["OpenIceZone","OpenTechZone"]).AddKeyword("OpenDirtZone").Replace(inventory, map);
        
        // Trees
        map.Replace(inventory.GetArticles([ "SpringTreeTall" ]), inventory.GetArticle("FallTreeTall"));
        map.Replace(inventory.GetArticles([ "FrozenTreeTall" ]), inventory.GetArticle("FirTall"));
        map.Replace(inventory.GetArticles([ "SpringTreeBig", "FrozenTreeBig" ]), inventory.GetArticle("FallTreeBig"));
        map.Replace(inventory.GetArticles([ "SpringTreeMedium" ]), inventory.GetArticle("FallTreeMedium"));
        map.Replace(inventory.GetArticles([ "FrozenTreeMedium" ]), inventory.GetArticle("FirMedium"));
        map.Replace(inventory.GetArticles([ "SpringTreeSmall" ]), inventory.GetArticle("FallTreeSmall"));
        map.Replace(inventory.GetArticles([ "FrozenTreeSmall" ]), inventory.GetArticle("CactusMedium"));
        map.Replace(inventory.GetArticles([ "SpringTreeVerySmall" ]), inventory.GetArticle("FallTreeVerySmall"));
        map.Replace(inventory.GetArticles([ "FrozenTreeVerySmall" ]), inventory.GetArticle("CactusVerySmall"));
        map.Replace(inventory.GetArticles([ "CypressTall" ]), inventory.GetArticle("PalmTreeMedium"));
        // map.Replace(inventory.GetArticles([ "CherryTreeMedium" ]), inventory.GetArticle("")); // tbh, cherry trees look nice so they can stay xdd
        map.Replace(inventory.GetArticles([ "PalmTreeMedium" ]), inventory.GetArticle("PalmTreeDirtMedium"));
        map.Replace(inventory.GetArticles([ "PalmTreeSmall" ]), inventory.GetArticle("PalmTreeDirtSmall"));

        map.Replace(inventory.GetArticles([ "FirSnowTall" ]), inventory.GetArticle("FirTall"));
        map.Replace(inventory.GetArticles([ "FirSnowMedium" ]), inventory.GetArticle("FirMedium"));

        //
        // map.Replace(inventory.GetArticles([ "Spring", "WinterFrozenTree" ]), inventory.GetArticle("Fall"));
        // map.Replace(inventory.GetArticles([ "Winter" ]), inventory.GetArticle("SummerPalmTree"));

        
        var skinPath = Path.Combine(AlterationConfig.DataFolder, "Templates", "DirtSkin.Gbx");
        CGameCtnBlockSkin SandSkin = Gbx.Parse<CGameCtnBlockSkin>(skinPath);

        map.map.Blocks.ToList().ForEach(x => x.Skin = SandSkin);
        map.map.BakedBlocks.ToList().ForEach(x => x.Skin = SandSkin);
        map.PlaceStagedBlocks();
    }
}

public class GrassScenery : Alteration
{
    public override string Description => "turns scenery into grass";
    public override bool Published => true;
    public override bool LikeAN => false;
    public override bool Complete => false;

    protected override void Run(Inventory inventory, Map map)
    {
        // Scenery blocks: 
        Inventory DecoWater = inventory.Any(["Deco", "Water", "DecoWall", "DecoPlatform"]);
        DecoWater.Edit().RemoveKeyword(["Dirt","Ice"]).Replace(inventory, map);
        map.PlaceStagedBlocks();
        DecoWater.Edit().AddKeyword("Grass").Replace(inventory, map);
        map.PlaceStagedBlocks();

        // Actual blocks: 
        inventory.Any(["PenaltyDirt","PenaltyIce"]).Edit().RemoveKeyword(["PenaltyDirt","PenaltyIce"]).Replace(inventory, map);

        Inventory WaterWall = inventory.Select("Water").Select("Wall");
        WaterWall.Edit().RemoveKeyword([ "Dirt", "Ice", "Wood" ]).AddKeyword("Grass").Replace(inventory, map);

        // Trees
        map.Replace(inventory.GetArticles([ "FirSnowTall" ]), inventory.GetArticle("FirTall"));
        map.Replace(inventory.GetArticles([ "FirSnowMedium" ]), inventory.GetArticle("FirMedium"));

        map.Replace(inventory.GetArticles([ "CypressDirtTall" ]), inventory.GetArticle("CypressTall"));
        
        map.Replace(inventory.GetArticles([ "FrozenTreeTall", "FallTreeTall" ]), inventory.GetArticle("SpringTreeTall"));
        map.Replace(inventory.GetArticles([ "FrozenTreeBig", "FallTreeBig" ]), inventory.GetArticle("SpringTreeBig"));
        map.Replace(inventory.GetArticles([ "FrozenTreeMedium", "FallTreeMedium" ]), inventory.GetArticle("SpringTreeMedium"));
        map.Replace(inventory.GetArticles([ "FrozenTreeSmall", "FallTreeSmall" ]), inventory.GetArticle("SpringTreeSmall"));
        map.Replace(inventory.GetArticles([ "FrozenTreeVerySmall", "FallTreeVerySmall" ]), inventory.GetArticle("SpringTreeVerySmall"));

        map.Replace(inventory.GetArticles([ "CactusMedium" ]), inventory.GetArticle("SpringTreeSmall"));
        map.Replace(inventory.GetArticles([ "CactusVerySmall" ]), inventory.GetArticle("SpringTreeVerySmall"));

        map.Replace(inventory.GetArticles([ "PalmTreeDirtMedium", "PalmTreeDirtSmall", "PalmTreeMedium", "PalmTreeSmall" ]), inventory.GetArticle("CherryTreeMedium"));

        //
        // map.Replace(inventory.GetArticles([ "Fall", "WinterFrozenTree" ]), inventory.GetArticle("Spring"));
        // map.Replace(inventory.GetArticles([ "SpringPalmTree", "SummerPalmTree", "Summer" ]), inventory.GetArticle("SpringCherryTree"));


        var skinPath = Path.Combine(AlterationConfig.DataFolder, "Templates", "GrassSkin.Gbx");
        CGameCtnBlockSkin GrassSkin = Gbx.Parse<CGameCtnBlockSkin>(skinPath);

        map.map.Blocks.ToList().ForEach(x => x.Skin = GrassSkin);
        map.map.BakedBlocks.ToList().ForEach(x => x.Skin = GrassSkin);
        map.PlaceStagedBlocks();
    }
}

// Grass Scenery Trees:
// SpringTreeTall, SpringTreeBig, SpringTreeMedium, SpringTreeSmall, SpringTreeVerySmall, CypressTall, CherryTreeMedium, FirTall, FirMedium, CypressDirtTall

// Sand Scenery Trees:
// PalmTreeMedium, PalmTreeSmall, PalmTreeDirtMedium, PalmTreeDirtSmall, CactusMedium, CactusVerySmall, FallTreeTall, FallTreeBig, FallTreeMedium, FallTreeSmall, FallTreeVerySmall

// Snow Scenery Trees:
// FirSnowTall, FirSnowMedium, FrozenTreeTall, FrozenTreeBig, FrozenTreeMedium, FrozenTreeSmall, FrozenTreeVerySmall
