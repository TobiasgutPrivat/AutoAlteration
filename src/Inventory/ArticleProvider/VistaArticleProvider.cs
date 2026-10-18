/// <summary>
/// The vista environments' terrain-integrated block variants (RedIsland, BlueBay, GreenCoast, WhiteShore):
/// "RoadTechStraightOnDirtCliff4" is one block that carries a 4-cell dirt cliff AND the Stadium RoadTechStraight on top.
/// The game builds every such block by embedding the Stadium base block's own prefab inside the cliff/hill prefab, so the
/// deck IS the base block's geometry, at a fixed pose inside the variant's frame. These articles expose each variant under
/// its real block name (so the placed blocks are found), with the BASE block's keywords plus "VistaVariant", and a MoveChain
/// that moves from the variant's origin to the deck — i.e. a PlaceRelative from a variant lands the new piece exactly where
/// the base block's surface item belongs. Measured on the vista packs (2026-10-18); the 50 variants used by the Fall 2026
/// campaign. Offsets in metres in the variant's local frame (dir 0), yaw in degrees; sizes in cells (x, y, z).
/// </summary>
class VistaArticleProvider() : ArticleProvider("VistaVariants")
{
    private const float PI = (float)Math.PI;

    private static readonly (string Variant, string Base, float Dx, float Dy, float Dz, float YawDeg, int SizeX, int SizeY, int SizeZ)[] Variants = [
        ("PlatformTechBaseOnGrassCliff4", "PlatformTechBase", 0f, 32f, 0f, 0f, 1, 5, 1),  // used 8×
        ("PlatformTechBaseOnGrassCliff8", "PlatformTechBase", 0f, 64f, 0f, 0f, 1, 9, 1),  // used 8×
        ("PlatformPlasticBaseOnLandHill1", "PlatformPlasticBase", 0f, 8f, 0f, 0f, 1, 2, 1),  // used 7×
        ("PlatformIceBaseOnLandHill1", "PlatformIceBase", 0f, 8f, 0f, 0f, 1, 2, 1),  // used 6×
        ("OpenTechRoadOnGrassHill1SlopeBase", "OpenTechRoadSlopeBase", 32f, 0f, 32f, -180f, 1, 2, 1),  // used 5×
        ("PlatformIceBaseOnLandHill2", "PlatformIceBase", 0f, 16f, 0f, 0f, 1, 3, 1),  // used 5×
        ("PlatformPlasticOnLandHillSlopeBase", "PlatformPlasticSlopeBase", 32f, 0f, 32f, -180f, 1, 2, 1),  // used 5×
        ("PlatformTechBaseOnLandCliff12", "PlatformTechBase", 0f, 96f, 0f, 0f, 1, 13, 1),  // used 5×
        ("RoadTechStraightOnLandHill3", "RoadTechStraight", 0f, 24f, 0f, 0f, 1, 4, 1),  // used 5×
        ("PlatformTechBaseOnDirtCliff8", "PlatformTechBase", 0f, 64f, 0f, 0f, 1, 9, 1),  // used 4×
        ("PlatformTechBaseOnGrassHill1", "PlatformTechBase", 0f, 8f, 0f, 0f, 1, 2, 1),  // used 4×
        ("PlatformTechOnGrassHill1SlopeBase", "PlatformTechSlopeBase", 32f, 0f, 32f, -180f, 1, 2, 1),  // used 4×
        ("OpenTechRoadOnGrassCliff2SlopeBase2", "OpenTechRoadSlope2Base", 32f, 0f, 32f, -180f, 1, 3, 1),  // used 3×
        ("PlatformGrassOnGrassHill1SlopeBase", "PlatformGrassSlopeBase", 32f, 0f, 32f, -180f, 1, 2, 1),  // used 3×
        ("PlatformTechBaseOnLandHill1", "PlatformTechBase", 0f, 8f, 0f, 0f, 1, 2, 1),  // used 3×
        ("PlatformTechBaseOnWaterHillStraight", "PlatformTechBase", 0f, 8f, 0f, 0f, 1, 2, 1),  // used 3×
        ("RoadTechOnLandHillSlopeBase", "RoadTechSlopeBase", 32f, 0f, 32f, 180f, 1, 2, 1),  // used 3×
        ("RoadTechSlope2BaseCurve2RightOnLandHill2", "RoadTechSlope2BaseCurve2Right", 0f, 0f, 0f, 0f, 2, 3, 2),  // used 3×
        ("RoadTechSlopeBaseCurve2LeftOnLandHill1", "RoadTechSlopeBaseCurve2Left", 0f, 0f, 0f, 0f, 2, 2, 2),  // used 3×
        ("RoadTechStraightOnGrassCliff4", "RoadTechStraight", 0f, 32f, 0f, 0f, 1, 5, 1),  // used 3×
        ("RoadTechStraightOnGrassCliff8", "RoadTechStraight", 0f, 64f, 0f, 0f, 1, 9, 1),  // used 3×
        ("RoadTechStraightOnGrassHill1", "RoadTechStraight", 0f, 8f, 0f, 0f, 1, 2, 1),  // used 3×
        ("OpenDirtRoadOnLandHillSlopeBase", "OpenDirtRoadSlopeBase", 32f, 0f, 32f, -180f, 1, 2, 1),  // used 2×
        ("PlatformDirtOnLandHillSlopeBase", "PlatformDirtSlopeBase", 32f, 0f, 32f, -180f, 1, 2, 1),  // used 2×
        ("PlatformPlasticBaseOnLandHill2", "PlatformPlasticBase", 0f, 16f, 0f, 0f, 1, 3, 1),  // used 2×
        ("RoadTechOnLandHillSlopeBase2x1", "RoadTechSlopeBase2x1", 32f, 0f, 64f, -180f, 1, 2, 2),  // used 2×
        ("RoadTechSlope2BaseCurve2LeftOnLandHill2", "RoadTechSlope2BaseCurve2Left", 0f, 0f, 0f, 0f, 2, 3, 2),  // used 2×
        ("RoadTechSlopeBaseCurve2RightOnGrassHill1", "RoadTechSlopeBaseCurve2Right", 0f, 0f, 0f, 0f, 2, 2, 2),  // used 2×
        ("RoadTechStraightOnDirtCliff8", "RoadTechStraight", 0f, 64f, 0f, 0f, 1, 9, 1),  // used 2×
        ("RoadTechStraightOnLandHill", "RoadTechStraight", 0f, 8f, 0f, 0f, 1, 2, 1),  // used 2×
        ("OpenTechRoadOnLandHill2SlopeBase2", "OpenTechRoadSlope2Base", 32f, 0f, 32f, -180f, 1, 3, 1),  // used 1×
        ("OpenTechRoadStraightOnLandHill2", "OpenTechRoadStraight", 0f, 16f, 0f, 0f, 1, 3, 1),  // used 1×
        ("PlatformDirtBaseOnLandHill3", "PlatformDirtBase", 0f, 24f, 0f, 0f, 1, 4, 1),  // used 1×
        ("PlatformGrassBaseOnLandHill1", "PlatformGrassBase", 0f, 8f, 0f, 0f, 1, 2, 1),  // used 1×
        ("PlatformTechBaseOnDirtCliff4", "PlatformTechBase", 0f, 32f, 0f, 0f, 1, 5, 1),  // used 1×
        ("PlatformTechOnLandHillSlopeBase", "PlatformTechSlopeBase", 32f, 0f, 32f, -180f, 1, 2, 1),  // used 1×
        ("RoadBumpOnGrassHill1SlopeBase2x1", "RoadBumpSlopeBase2x1", 32f, 0f, 64f, 180f, 1, 2, 2),  // used 1×
        ("RoadBumpSlopeBaseCurve2LeftOnGrassHill1", "RoadBumpSlopeBaseCurve2Left", 0f, 0f, 0f, 0f, 2, 2, 2),  // used 1×
        ("RoadBumpSlopeBaseCurve2RightOnGrassHill1", "RoadBumpSlopeBaseCurve2Right", 0f, 0f, 0f, 0f, 2, 2, 2),  // used 1×
        ("RoadDirtOnLandHillSlopeBase2x1", "RoadDirtSlopeBase2x1", 32f, 0f, 64f, -180f, 1, 2, 2),  // used 1×
        ("RoadDirtSlopeBaseCurve2RightOnLandHill", "RoadDirtSlopeBaseCurve2Right", 0f, 0f, 0f, 0f, 2, 2, 2),  // used 1×
        ("RoadDirtStraightOnLandHill6", "RoadDirtStraight", 0f, 48f, 0f, 0f, 1, 7, 1),  // used 1×
        ("RoadTechOnLandHill2SlopeBase2", "RoadTechSlopeBase2", 32f, 0f, 32f, -180f, 1, 3, 1),  // used 1×
        ("RoadTechSlopeBaseCurve2LeftOnGrassHill1", "RoadTechSlopeBaseCurve2Left", 0f, 0f, 0f, 0f, 2, 2, 2),  // used 1×
        ("RoadTechSlopeBaseCurve2RightOnLandHill1", "RoadTechSlopeBaseCurve2Right", 0f, 0f, 0f, 0f, 2, 2, 2),  // used 1×
        ("RoadTechStraightOnDirtCliff4", "RoadTechStraight", 0f, 32f, 0f, 0f, 1, 5, 1),  // used 1×
        ("RoadTechStraightOnDirtHill2", "RoadTechStraight", 0f, 16f, 0f, 0f, 1, 3, 1),  // used 1×
        ("RoadTechStraightOnLandHill1", "RoadTechStraight", 0f, 8f, 0f, 0f, 1, 2, 1),  // used 1×
        ("RoadTechStraightOnLandHill2", "RoadTechStraight", 0f, 16f, 0f, 0f, 1, 3, 1),  // used 1×
        ("RoadTechStraightOnLandHill6", "RoadTechStraight", 0f, 48f, 0f, 0f, 1, 7, 1),  // used 1×
    ];

    public static readonly IReadOnlyDictionary<string, string> BaseOf = Variants.ToDictionary(v => v.Variant, v => v.Base);

    protected override List<Article> GetAlteredArticles(CustomBlockAlteration customBlockAlteration) => [];

    public override List<string> GetAdditionalKeywords() => ["VistaVariant"];

    protected override List<Article> GenerateArticles()
    {
        List<Article> articles = [];
        foreach (var (variant, baseName, dx, dy, dz, yawDeg, sizeX, sizeY, sizeZ) in Variants)
        {
            var baseArticle = new Article(sizeY, sizeX, sizeZ, BlockType.Block, baseName);
            MoveChain toDeck = yawDeg == 0
                ? [new Offset(dx, dy, dz)]
                : [new Offset(dx, dy, dz), new Rotate(yawDeg * PI / 180f, 0, 0)];
            articles.Add(new Article(variant, BlockType.Block, [.. baseArticle.Keywords, "VistaVariant"], baseArticle.ToShapes, toDeck, length: sizeZ, width: sizeX, height: sizeY));
        }
        return articles;
    }
}