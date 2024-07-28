class SnowScenery : Alteration
{
    public override void Run(Map map)
    {
        // Scenery blocks: 
        inventory.Select("Deco|Water|DecoWall|DecoPlatform").RemoveKeyword(new string[] {"Grass","Dirt"}).AddKeyword("Ice").Replace(map);
        // Actual blocks: 
        inventory.Select("PenaltyDirt|Penalty").RemoveKeyword(new string[] {"PenaltyDirt","Penalty"}).AddKeyword("PenaltyIce").Replace(map);
        // Open Road/Zone
        // inventory.Select("OpenDirtRoad|OpenTechRoad").RemoveKeyword(new string[] {"OpenDirtRoad","OpenTechRoad"}).AddKeyword("OpenIceRoad").Replace(map);
        // inventory.Select("OpenDirtZone|OpenTechZone").RemoveKeyword(new string[] {"OpenDirtZone","OpenTechZone"}).AddKeyword("OpenIceZone").Replace(map);
        // Trees
        map.Replace(new string[] { "FirTall", "CypressTall", "PalmTreeMedium", "SpringPalmTree" }, "FirSnowTall");
        map.Replace(new string[] { "FirMedium", "PalmTreeSmall" }, "FirSnowMedium");
        map.Replace(new string[] { "FallTreeTall", "CypressDirtTall", "SpringTreeTall" }, "FrozenTreeTall");
        map.Replace(new string[] { "FallTreeBig", "PalmTreeDirtMedium", "SpringTreeBig" }, "FrozenTreeBig");
        map.Replace(new string[] { "FallTreeMedium", "PalmTreeDirtSmall", "SpringTreeMedium", "CherryTreeMedium" }, "FrozenTreeMedium");
        map.Replace(new string[] { "FallTreeSmall", "CactusMedium", "SpringTreeSmall" }, "FrozenTreeSmall");
        map.Replace(new string[] { "FallTreeVerySmall", "CactusVerySmall", "SpringTreeVerySmall" }, "FrozenTreeVerySmall");
 
        map.Replace(new string[] { "Fall" }, "WinterFrozenTree");
        map.Replace(new string[] { "SummerPalmTree" }, "WinterFrozenTree");
        map.Replace(new string[] { "Spring" }, "WinterFrozenTree");
        map.Replace(new string[] { "SpringCherryTree" }, "WinterFrozenTree");
        map.Replace(new string[] { "Summer" }, "Winter");
        map.PlaceStagedBlocks();
    }
}