
using GBX.NET;
using GBX.NET.Engines.Plug;

class CustomSurfaceAlteration : CustomBlockAlteration {
    public static bool LightSurface(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer, string Surface, string RoadSurface, CPlugSurface.MaterialId SurfacePhysicId){
        layer.Crystal.Faces = layer.Crystal.Faces.ToList().Where(x => DrivableMaterials.Contains(x.Material.MaterialUserInst.Link) || (x.Material.MaterialUserInst.SurfaceGameplayId != CPlugMaterialUserInst.GameplayId.None)).ToArray();
        if (layer.Crystal.Faces.Length == 0){
            return false;
        }
        if (customBlock.Name.Contains("Road") && !customBlock.Name.Contains("Open")){
            layer.Crystal.Faces.Where(x => x.Material.MaterialUserInst.SurfaceGameplayId == CPlugMaterialUserInst.GameplayId.None).ToList().ForEach(x => x.Material.MaterialUserInst.Link = RoadSurface);
        } else {
            layer.Crystal.Faces.Where(x => x.Material.MaterialUserInst.SurfaceGameplayId == CPlugMaterialUserInst.GameplayId.None).ToList().ForEach(x => x.Material.MaterialUserInst.Link = Surface);
        }
        layer.Crystal.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.SurfacePhysicId = SurfacePhysicId);
        Vec3 topMiddle = GetTopMiddle(layer.Crystal);
        layer.Crystal.Positions = layer.Crystal.Positions.ToList().Select(x => {
            float X,Y,Z;
            Y = x.Y + .02f;
            if (x.X > topMiddle.X){
                X = x.X - .02f;
            } else {
                X = x.X + .02f;
            }
            if (x.Z > topMiddle.Z){
                Z = x.Z - .02f;
            } else {
                Z = x.Z + .02f;
            }
            return new Vec3(X, Y, Z);
            }).ToArray();
        //Will have issues with surfaces looking away from topmiddle point
        return true;
    }

    public static bool HeavySurface(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer, string Surface, string RoadSurface, CPlugSurface.MaterialId SurfacePhysicId){
        // MakeSingleLayer(customBlock);
        if (customBlock.Name.Contains("Road") && !customBlock.Name.Contains("Open")) {
            layer.Crystal.Faces.ToList().ForEach(x => {if (DrivableMaterials.Contains(x.Material.MaterialUserInst.Link)) {x.Material.MaterialUserInst.Link = RoadSurface;}});
            if (layer.Crystal.Faces.ToList().Any(x => x.Material.MaterialUserInst.Link == RoadSurface)){return true;};
        } else {
            layer.Crystal.Faces.ToList().ForEach(x => {if (DrivableMaterials.Contains(x.Material.MaterialUserInst.Link)) {x.Material.MaterialUserInst.Link = Surface;}});
            if (layer.Crystal.Faces.ToList().Any(x => x.Material.MaterialUserInst.Link == Surface)){return true;};
        }
        layer.Crystal.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.SurfacePhysicId = SurfacePhysicId);
        return false;
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

class LightTech : CustomSurfaceAlteration {
    public override bool Run(CustomBlock customBlock) {
        return MakeGeometryOnly(customBlock);
    }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Material\\PlatformTech", "Stadium\\Media\\Material\\RoadTech",CPlugSurface.MaterialId.Tech);
    }
}
class LightDirt : CustomSurfaceAlteration {
    public override bool Run(CustomBlock customBlock) {
        return MakeGeometryOnly(customBlock);
    }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech", "Stadium\\Media\\Material\\RoadDirt",CPlugSurface.MaterialId.Dirt);
    }
}
class LightGrass : CustomSurfaceAlteration {
    public override bool Run(CustomBlock customBlock) {
        return MakeGeometryOnly(customBlock);
    }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech","Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech",CPlugSurface.MaterialId.Green);
    }
}
class LightIce : CustomSurfaceAlteration {
    public override bool Run(CustomBlock customBlock) {
        return MakeGeometryOnly(customBlock);
    }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech", "Stadium\\Media\\Material\\RoadIce",CPlugSurface.MaterialId.Ice);
    }
}
class LightPlastic : CustomSurfaceAlteration {
    public override bool Run(CustomBlock customBlock) {
        return MakeGeometryOnly(customBlock);
    }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech",CPlugSurface.MaterialId.Plastic);
    }
}
class LightMagnet : CustomSurfaceAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic", "Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic",CPlugSurface.MaterialId.TechSuperMagnetic);
    }
}