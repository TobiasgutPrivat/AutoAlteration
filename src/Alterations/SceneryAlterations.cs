class SnowScenery : Alteration
{
    public override void Run(Map map)
    {
        // Scenery blocks: 
        inventory.Select("Deco|Water|DecoWall|DecoPlatform").RemoveKeyword(["Grass","Dirt"]).AddKeyword("Ice").Replace(map);
        // Actual blocks: 
        inventory.Select("PenaltyDirt|Penalty").RemoveKeyword(["PenaltyDirt","Penalty"]).AddKeyword("PenaltyIce").Replace(map);
        // Open Road/Zone
        // inventory.Select("OpenDirtRoad|OpenTechRoad").RemoveKeyword(["OpenDirtRoad","OpenTechRoad"]).AddKeyword("OpenIceRoad").Replace(map);
        // inventory.Select("OpenDirtZone|OpenTechZone").RemoveKeyword(["OpenDirtZone","OpenTechZone"]).AddKeyword("OpenIceZone").Replace(map);
        // Trees
        // map.Replace([ "FirTall", "CypressTall", "PalmTreeMedium", "SpringPalmTree" ], "FirSnowTall");
        // map.Replace([ "FirMedium", "PalmTreeSmall" ], "FirSnowMedium");
        // map.Replace([ "FallTreeTall", "CypressDirtTall", "SpringTreeTall" ], "FrozenTreeTall");
        // map.Replace([ "FallTreeBig", "PalmTreeDirtMedium", "SpringTreeBig" ], "FrozenTreeBig");
        // map.Replace([ "FallTreeMedium", "PalmTreeDirtSmall", "SpringTreeMedium", "CherryTreeMedium" ], "FrozenTreeMedium");
        // map.Replace([ "FallTreeSmall", "CactusMedium", "SpringTreeSmall" ], "FrozenTreeSmall");
        // map.Replace([ "FallTreeVerySmall", "CactusVerySmall", "SpringTreeVerySmall" ], "FrozenTreeVerySmall");
 
        // map.Replace([ "Fall" ], "WinterFrozenTree");
        // map.Replace([ "SummerPalmTree" ], "WinterFrozenTree");
        // map.Replace([ "Spring" ], "WinterFrozenTree");
        // map.Replace([ "SpringCherryTree" ], "WinterFrozenTree");
        // map.Replace([ "Summer" ], "Winter");
        map.PlaceStagedBlocks();
    }
}