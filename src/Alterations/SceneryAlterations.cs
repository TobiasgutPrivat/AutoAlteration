using GBX.NET;
using GBX.NET.Engines.Game;

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
        map.Replace(inventory.GetArticles([ "FirTall", "CypressTall", "PalmTreeMedium", "SpringPalmTree" ]), inventory.GetArticle("FirSnowTall"));
        map.Replace(inventory.GetArticles([ "FirMedium", "PalmTreeSmall" ]), inventory.GetArticle("FirSnowMedium"));
        map.Replace(inventory.GetArticles([ "FallTreeTall", "CypressDirtTall", "SpringTreeTall" ]), inventory.GetArticle("FrozenTreeTall"));
        map.Replace(inventory.GetArticles([ "FallTreeBig", "PalmTreeDirtMedium", "SpringTreeBig" ]), inventory.GetArticle("FrozenTreeBig"));
        map.Replace(inventory.GetArticles([ "FallTreeMedium", "PalmTreeDirtSmall", "SpringTreeMedium", "CherryTreeMedium" ]), inventory.GetArticle("FrozenTreeMedium"));
        map.Replace(inventory.GetArticles([ "FallTreeSmall", "CactusMedium", "SpringTreeSmall" ]), inventory.GetArticle("FrozenTreeSmall"));
        map.Replace(inventory.GetArticles([ "FallTreeVerySmall", "CactusVerySmall", "SpringTreeVerySmall" ]), inventory.GetArticle("FrozenTreeVerySmall"));
 
        map.Replace(inventory.GetArticles([ "Fall" ]), inventory.GetArticle("WinterFrozenTree"));
        map.Replace(inventory.GetArticles([ "SummerPalmTree" ]), inventory.GetArticle("WinterFrozenTree"));
        map.Replace(inventory.GetArticles([ "Spring" ]), inventory.GetArticle("WinterFrozenTree"));
        map.Replace(inventory.GetArticles([ "SpringCherryTree" ]), inventory.GetArticle("WinterFrozenTree"));
        map.Replace(inventory.GetArticles([ "Summer" ]), inventory.GetArticle("Winter"));
        map.PlaceStagedBlocks();
        // map.map.Blocks.ToList().Where(x => x.Skin != null).Where(x => x.Skin.Text == "PlatformIce\\").ToList().ForEach(x => x.Skin.Save("C:/Users/Tobias/Documents/Programmieren/AutoAlteration/IceSkin.gbx"));
        CGameCtnBlockSkin GrassSkin = Gbx.Parse<CGameCtnBlockSkin>("C:/Users/Tobias/Documents/Programmieren/AutoAlteration/data/IceSkin.gbx");
        map.map.Blocks.ToList().ForEach(x => x.Skin = GrassSkin);
    }
}