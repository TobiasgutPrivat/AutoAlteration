/// <summary>
/// The vista environments' terrain-integrated block variants (RedIsland, BlueBay, GreenCoast, WhiteShore): one block that
/// carries a cliff/hill AND a Stadium base block on top ("RoadTechStraightOnDirtCliff4"). The game builds each by embedding
/// the base block's prefab inside the terrain prefab, so the deck IS the base block's geometry at a fixed pose in the
/// variant's frame. Data only: (variant, base block, dx, dy, dz, yawDeg, sizeX, sizeY, sizeZ) — metres in the variant's
/// local frame at dir 0, yaw in degrees, size in cells. Measured on the vista packs 2026-10-18; the 50 variants the
/// Fall 2026 campaign uses (VISTA-ALL.tsv has all 491).
/// </summary>
public static class VistaVariants
{
    public static readonly (string Variant, string Base, float Dx, float Dy, float Dz, float YawDeg, int SizeX, int SizeY, int SizeZ)[] Table = [
        ("PlatformTechBaseOnGrassCliff4", "PlatformTechBase", 0f, 32f, 0f, 0f, 1, 5, 1),
        ("PlatformTechBaseOnGrassCliff8", "PlatformTechBase", 0f, 64f, 0f, 0f, 1, 9, 1),
        ("PlatformPlasticBaseOnLandHill1", "PlatformPlasticBase", 0f, 8f, 0f, 0f, 1, 2, 1),
        ("PlatformIceBaseOnLandHill1", "PlatformIceBase", 0f, 8f, 0f, 0f, 1, 2, 1),
        ("OpenTechRoadOnGrassHill1SlopeBase", "OpenTechRoadSlopeBase", 32f, 0f, 32f, -180f, 1, 2, 1),
        ("PlatformIceBaseOnLandHill2", "PlatformIceBase", 0f, 16f, 0f, 0f, 1, 3, 1),
        ("PlatformPlasticOnLandHillSlopeBase", "PlatformPlasticSlopeBase", 32f, 0f, 32f, -180f, 1, 2, 1),
        ("PlatformTechBaseOnLandCliff12", "PlatformTechBase", 0f, 96f, 0f, 0f, 1, 13, 1),
        ("RoadTechStraightOnLandHill3", "RoadTechStraight", 0f, 24f, 0f, 0f, 1, 4, 1),
        ("PlatformTechBaseOnDirtCliff8", "PlatformTechBase", 0f, 64f, 0f, 0f, 1, 9, 1),
        ("PlatformTechBaseOnGrassHill1", "PlatformTechBase", 0f, 8f, 0f, 0f, 1, 2, 1),
        ("PlatformTechOnGrassHill1SlopeBase", "PlatformTechSlopeBase", 32f, 0f, 32f, -180f, 1, 2, 1),
        ("OpenTechRoadOnGrassCliff2SlopeBase2", "OpenTechRoadSlope2Base", 32f, 0f, 32f, -180f, 1, 3, 1),
        ("PlatformGrassOnGrassHill1SlopeBase", "PlatformGrassSlopeBase", 32f, 0f, 32f, -180f, 1, 2, 1),
        ("PlatformTechBaseOnLandHill1", "PlatformTechBase", 0f, 8f, 0f, 0f, 1, 2, 1),
        ("PlatformTechBaseOnWaterHillStraight", "PlatformTechBase", 0f, 8f, 0f, 0f, 1, 2, 1),
        ("RoadTechOnLandHillSlopeBase", "RoadTechSlopeBase", 32f, 0f, 32f, 180f, 1, 2, 1),
        ("RoadTechSlope2BaseCurve2RightOnLandHill2", "RoadTechSlope2BaseCurve2Right", 0f, 0f, 0f, 0f, 2, 3, 2),
        ("RoadTechSlopeBaseCurve2LeftOnLandHill1", "RoadTechSlopeBaseCurve2Left", 0f, 0f, 0f, 0f, 2, 2, 2),
        ("RoadTechStraightOnGrassCliff4", "RoadTechStraight", 0f, 32f, 0f, 0f, 1, 5, 1),
        ("RoadTechStraightOnGrassCliff8", "RoadTechStraight", 0f, 64f, 0f, 0f, 1, 9, 1),
        ("RoadTechStraightOnGrassHill1", "RoadTechStraight", 0f, 8f, 0f, 0f, 1, 2, 1),
        ("OpenDirtRoadOnLandHillSlopeBase", "OpenDirtRoadSlopeBase", 32f, 0f, 32f, -180f, 1, 2, 1),
        ("PlatformDirtOnLandHillSlopeBase", "PlatformDirtSlopeBase", 32f, 0f, 32f, -180f, 1, 2, 1),
        ("PlatformPlasticBaseOnLandHill2", "PlatformPlasticBase", 0f, 16f, 0f, 0f, 1, 3, 1),
        ("RoadTechOnLandHillSlopeBase2x1", "RoadTechSlopeBase2x1", 32f, 0f, 64f, -180f, 1, 2, 2),
        ("RoadTechSlope2BaseCurve2LeftOnLandHill2", "RoadTechSlope2BaseCurve2Left", 0f, 0f, 0f, 0f, 2, 3, 2),
        ("RoadTechSlopeBaseCurve2RightOnGrassHill1", "RoadTechSlopeBaseCurve2Right", 0f, 0f, 0f, 0f, 2, 2, 2),
        ("RoadTechStraightOnDirtCliff8", "RoadTechStraight", 0f, 64f, 0f, 0f, 1, 9, 1),
        ("RoadTechStraightOnLandHill", "RoadTechStraight", 0f, 8f, 0f, 0f, 1, 2, 1),
        ("OpenTechRoadOnLandHill2SlopeBase2", "OpenTechRoadSlope2Base", 32f, 0f, 32f, -180f, 1, 3, 1),
        ("OpenTechRoadStraightOnLandHill2", "OpenTechRoadStraight", 0f, 16f, 0f, 0f, 1, 3, 1),
        ("PlatformDirtBaseOnLandHill3", "PlatformDirtBase", 0f, 24f, 0f, 0f, 1, 4, 1),
        ("PlatformGrassBaseOnLandHill1", "PlatformGrassBase", 0f, 8f, 0f, 0f, 1, 2, 1),
        ("PlatformTechBaseOnDirtCliff4", "PlatformTechBase", 0f, 32f, 0f, 0f, 1, 5, 1),
        ("PlatformTechOnLandHillSlopeBase", "PlatformTechSlopeBase", 32f, 0f, 32f, -180f, 1, 2, 1),
        ("RoadBumpOnGrassHill1SlopeBase2x1", "RoadBumpSlopeBase2x1", 32f, 0f, 64f, 180f, 1, 2, 2),
        ("RoadBumpSlopeBaseCurve2LeftOnGrassHill1", "RoadBumpSlopeBaseCurve2Left", 0f, 0f, 0f, 0f, 2, 2, 2),
        ("RoadBumpSlopeBaseCurve2RightOnGrassHill1", "RoadBumpSlopeBaseCurve2Right", 0f, 0f, 0f, 0f, 2, 2, 2),
        ("RoadDirtOnLandHillSlopeBase2x1", "RoadDirtSlopeBase2x1", 32f, 0f, 64f, -180f, 1, 2, 2),
        ("RoadDirtSlopeBaseCurve2RightOnLandHill", "RoadDirtSlopeBaseCurve2Right", 0f, 0f, 0f, 0f, 2, 2, 2),
        ("RoadDirtStraightOnLandHill6", "RoadDirtStraight", 0f, 48f, 0f, 0f, 1, 7, 1),
        ("RoadTechOnLandHill2SlopeBase2", "RoadTechSlopeBase2", 32f, 0f, 32f, -180f, 1, 3, 1),
        ("RoadTechSlopeBaseCurve2LeftOnGrassHill1", "RoadTechSlopeBaseCurve2Left", 0f, 0f, 0f, 0f, 2, 2, 2),
        ("RoadTechSlopeBaseCurve2RightOnLandHill1", "RoadTechSlopeBaseCurve2Right", 0f, 0f, 0f, 0f, 2, 2, 2),
        ("RoadTechStraightOnDirtCliff4", "RoadTechStraight", 0f, 32f, 0f, 0f, 1, 5, 1),
        ("RoadTechStraightOnDirtHill2", "RoadTechStraight", 0f, 16f, 0f, 0f, 1, 3, 1),
        ("RoadTechStraightOnLandHill1", "RoadTechStraight", 0f, 8f, 0f, 0f, 1, 2, 1),
        ("RoadTechStraightOnLandHill2", "RoadTechStraight", 0f, 16f, 0f, 0f, 1, 3, 1),
        ("RoadTechStraightOnLandHill6", "RoadTechStraight", 0f, 48f, 0f, 0f, 1, 7, 1),
    ];
}