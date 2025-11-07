using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;

public class CustomSurfaceAlteration(string Surface, string RoadSurface, CPlugSurface.MaterialId SurfacePhysicId) : CustomBlockAlteration {
    public static List<string> DrivableMaterials = ["Stadium\\Media\\Material\\ThemeSnowRoad","Stadium\\Media\\Material\\ThemeSnowRoadBorder","Stadium\\Media\\Material\\PlatformTech","Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech","Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech","Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech","Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech","Stadium\\Media\\Material\\RoadBump","Stadium\\Media\\Material\\RoadTech","Stadium\\Media\\Material\\RoadDirt","Stadium\\Media\\Material\\RoadIce","Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic","Stadium\\Media\\Modifier\\PlatformDirt\\OpenTechBorders","Stadium\\Media\\Modifier\\PlatformGrass\\OpenTechBorders","Stadium\\Media\\Modifier\\PlatformIce\\OpenTechBorders","Stadium\\Media\\Material\\OpenTechBorders"];// Top of TrackWall: "Stadium\\Media\\Material\\TrackWallClips"

    public override bool Run(CustomBlock customBlock)
    {
        bool altered = false;
        List<CPlugCrystal.Face> facesToModify = [];
        string SurfaceToUse = customBlock.Name.Contains("Road") && !customBlock.Name.Contains("Open") ? RoadSurface : Surface;
        foreach (CPlugCrystal.GeometryLayer layer in customBlock.MeshCrystals.SelectMany(x => x.Layers).Where(x => x.GetType() == typeof(CPlugCrystal.GeometryLayer)).Cast<CPlugCrystal.GeometryLayer>()) {
            facesToModify.AddRange(layer.Crystal.Faces.ToList().Where(x => GetMaterialLink(x) != SurfaceToUse && DrivableMaterials.Contains(GetMaterialLink(x))));
        }
        facesToModify?.ForEach(x =>
        {
            x.Material!.MaterialUserInst!.Link = SurfaceToUse;
            x.Material.MaterialUserInst.SurfacePhysicId = SurfacePhysicId;
        });
        altered |= facesToModify is not null && facesToModify.Count > 0;

        foreach (CPlugSolid2Model model in customBlock.Models) {
            if (model.CustomMaterials is null) return false;
                model.CustomMaterials.ToList().ForEach(x => {
                if (DrivableMaterials.Contains(GetMaterialLink(x)) && GetMaterialLink(x) != Surface) {
                    x.MaterialUserInst!.Link = Surface;
                    x.MaterialUserInst.SurfacePhysicId = SurfacePhysicId;
                    altered = true;
                }
            });
        }
        return altered;
    }
}


public class TechSurface : CustomSurfaceAlteration{
    public TechSurface() : base("Stadium\\Media\\Material\\PlatformTech", "Stadium\\Media\\Material\\RoadTech",CPlugSurface.MaterialId.Tech) { }
}

public class DirtSurface : CustomSurfaceAlteration {
    public DirtSurface() : base("Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech", "Stadium\\Media\\Material\\RoadDirt",CPlugSurface.MaterialId.Dirt) { }
}

public class GrassSurface : CustomSurfaceAlteration {
    public GrassSurface() : base("Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech",CPlugSurface.MaterialId.Green) { }
}

public class IceSurface : CustomSurfaceAlteration {
    public IceSurface() : base("Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech", "Stadium\\Media\\Material\\RoadIce",CPlugSurface.MaterialId.Ice) { }
}
public class PlasticSurface : CustomSurfaceAlteration {
    public PlasticSurface() : base("Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech",CPlugSurface.MaterialId.Plastic) { }
}
public class MagnetSurface : CustomSurfaceAlteration {
    public MagnetSurface() : base("Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic", "Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic",CPlugSurface.MaterialId.TechSuperMagnetic) { }
}
public class WoodSurface : CustomSurfaceAlteration {
    public WoodSurface() : base("Stadium\\Media\\Material\\ThemeSnowRoad", "Stadium\\Media\\Material\\ThemeSnowRoad", CPlugSurface.MaterialId.Wood) { }
}

// public class RouteOnlyBlock : CustomBlockAlteration{
//     public override bool Run(CustomBlock customBlock) {
//         foreach(CPlugCrystal.GeometryLayer layer in customBlock.MeshCrystals.SelectMany(x => x.Layers).Where(x => x.GetType() == typeof(CPlugCrystal.GeometryLayer)).Cast<CPlugCrystal.GeometryLayer>())
//         {
//             layer.Crystal.Faces = layer.Crystal.Faces.ToList().Where(x => DrivableMaterials.Contains(GetMaterialSurfacePhysicId(x)) || (x.Material.MaterialUserInst.SurfaceGameplayId != CPlugMaterialUserInst.GameplayId.None)).ToArray();
//         }
//         return true;
//     }
// }