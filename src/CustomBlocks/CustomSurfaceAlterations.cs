using GBX.NET.Engines.Plug;

class CustomSurfaceAlteration : CustomBlockAlteration {
    public static List<string> DrivableMaterials = ["Stadium\\Media\\Material\\PlatformTech","Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech","Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech","Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech","Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech","Stadium\\Media\\Material\\RoadBump","Stadium\\Media\\Material\\RoadTech","Stadium\\Media\\Material\\RoadDirt","Stadium\\Media\\Material\\RoadIce","Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic","Stadium\\Media\\Modifier\\PlatformDirt\\OpenTechBorders","Stadium\\Media\\Modifier\\PlatformGrass\\OpenTechBorders","Stadium\\Media\\Modifier\\PlatformIce\\OpenTechBorders","Stadium\\Media\\Material\\OpenTechBorders","Stadium\\Media\\Material\\ThemeSnowRoad","Stadium\\Media\\Material\\ThemeSnowRoadBorder"];//Top of TrackWall: "Stadium\\Media\\Material\\TrackWallClips"

    public static bool HeavySurface(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer, string Surface, string RoadSurface, CPlugSurface.MaterialId SurfacePhysicId){
        string SurfaceToUse = customBlock.Name.Contains("Road") && !customBlock.Name.Contains("Open") ? RoadSurface : Surface;
        List<CPlugCrystal.Face> facesToModify = layer.Crystal.Faces.ToList().Where(x => GetMaterialLink(x) != SurfaceToUse && DrivableMaterials.Contains(GetMaterialLink(x))).ToList();
        facesToModify.ForEach(x => {
            x.Material.MaterialUserInst.Link = SurfaceToUse;
            x.Material.MaterialUserInst.SurfacePhysicId = SurfacePhysicId;
            });
        // if (facesToModify.Count > 0) {
        //     layer.Collidable = true;
        // };
        return facesToModify.Count > 0;
    }
}


class HeavyTech : CustomSurfaceAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Material\\PlatformTech", "Stadium\\Media\\Material\\RoadTech",CPlugSurface.MaterialId.Tech);
    }
}

class HeavyDirt : CustomSurfaceAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech", "Stadium\\Media\\Material\\RoadDirt",CPlugSurface.MaterialId.Dirt);
    }
}

class HeavyGrass : CustomSurfaceAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech",CPlugSurface.MaterialId.Green);
    }
}

class HeavyIce : CustomSurfaceAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech", "Stadium\\Media\\Material\\RoadIce",CPlugSurface.MaterialId.Ice);
    }
}
class HeavyPlastic : CustomSurfaceAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech",CPlugSurface.MaterialId.Plastic);
    }
}
class HeavyMagnet : CustomSurfaceAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic", "Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic",CPlugSurface.MaterialId.TechSuperMagnetic);
    }
}
class HeavyWood : CustomSurfaceAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Material\\ThemeSnowRoad", "Stadium\\Media\\Material\\ThemeSnowRoad",CPlugSurface.MaterialId.Wood);
    }
}

class RouteOnlyBlock : CustomSurfaceAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        // layer.Crystal.Faces = layer.Crystal.Faces.ToList().Where(x => DrivableMaterials.Contains(GetMaterialSurfacePhysicId(x)) || (x.Material.MaterialUserInst.SurfaceGameplayId != CPlugMaterialUserInst.GameplayId.None)).ToArray();
        return true;
    }
}