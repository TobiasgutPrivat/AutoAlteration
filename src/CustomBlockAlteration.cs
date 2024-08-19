using GBX.NET;
using GBX.NET.Engines.Plug;

class CustomBlockAlteration {
    public static string[] DrivableMaterials = ["Stadium\\Media\\Material\\PlatformTech","Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech","Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech","Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech","Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech","Stadium\\Media\\Material\\RoadBump","Stadium\\Media\\Material\\RoadTech","Stadium\\Media\\Material\\RoadDirt","Stadium\\Media\\Material\\RoadIce","Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic",];
    //TODO OpenRoad Edges
    public virtual bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {return false;}
    public virtual bool AlterTrigger(CustomBlock customBlock, CPlugCrystal.TriggerLayer layer) {return false;}
    public virtual bool AlterSpawn(CustomBlock customBlock, CPlugCrystal.SpawnPositionLayer layer) {return false;}
    public virtual bool Run(CustomBlock customBlock) {return false;}

    public static Vec3 GetTopMiddle(CPlugCrystal.Crystal mesh) {
        float top = mesh.Positions.Max(x => x.Y);
        float Right = mesh.Positions.Min(x => x.X);
        float Left = mesh.Positions.Max(x => x.X);
        float Front = mesh.Positions.Max(x => x.Z);
        float Back = mesh.Positions.Min(x => x.Z);
        return new Vec3((Right + Left) / 2, top, (Front + Back) / 2);
    }

    public static bool MakeGeometryOnly(CustomBlock customBlock){
        bool changed = false;
        if (customBlock.customBlock.WaypointType != GBX.NET.Engines.GameData.CGameItemModel.EWaypointType.None){
            customBlock.customBlock.WaypointType = GBX.NET.Engines.GameData.CGameItemModel.EWaypointType.None;
            changed = true;
        }
        if (customBlock.Layers.Any(x => x is not CPlugCrystal.GeometryLayer)){
            customBlock.Layers.Where(x => x is not CPlugCrystal.GeometryLayer).ToList().ForEach(x => customBlock.Layers.Remove(x));
            changed = true;
        }
        return changed;
    }

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


class SupersizedBlock : CustomBlockAlteration {
    private static readonly float factor = 2;
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        layer.Crystal.Positions = layer.Crystal.Positions.ToList().Select(x => new Vec3(x.X * factor, x.Y * factor, x.Z * factor)).ToArray();
        return true;
    }
    public override bool AlterTrigger(CustomBlock customBlock, CPlugCrystal.TriggerLayer layer) {
        layer.Crystal.Positions = layer.Crystal.Positions.ToList().Select(x => new Vec3(x.X * factor, x.Y * factor, x.Z * factor)).ToArray();
        return true;
    }
    public override bool AlterSpawn(CustomBlock customBlock, CPlugCrystal.SpawnPositionLayer layer) {
        layer.SpawnPosition = new Vec3(layer.SpawnPosition.X * factor, layer.SpawnPosition.Y * factor, layer.SpawnPosition.Z * factor);
        return true;
    }
    public override bool Run(CustomBlock customBlock) {
        customBlock.customBlock.DefaultPlacement.GridSnapHStep *= factor;
        customBlock.customBlock.DefaultPlacement.GridSnapVStep *= factor;
        customBlock.customBlock.DefaultPlacement.FlyVStep *= factor;
        return false;
    }
}

class MiniBlock : CustomBlockAlteration {
    private static readonly float factor = 0.5f;
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        layer.Crystal.Positions = layer.Crystal.Positions.ToList().Select(x => new Vec3(x.X * factor, x.Y * factor, x.Z * factor)).ToArray();
        return true;
    }
    public override bool AlterTrigger(CustomBlock customBlock, CPlugCrystal.TriggerLayer layer) {
        layer.Crystal.Positions = layer.Crystal.Positions.ToList().Select(x => new Vec3(x.X * factor, x.Y * factor, x.Z * factor)).ToArray();
        return true;
    }
    public override bool AlterSpawn(CustomBlock customBlock, CPlugCrystal.SpawnPositionLayer layer) {
        layer.SpawnPosition = new Vec3(layer.SpawnPosition.X * factor, layer.SpawnPosition.Y * factor, layer.SpawnPosition.Z * factor);
        return true;
    }
    public override bool Run(CustomBlock customBlock) {
        customBlock.customBlock.DefaultPlacement.GridSnapHStep *= factor;
        customBlock.customBlock.DefaultPlacement.GridSnapVStep *= factor;
        customBlock.customBlock.DefaultPlacement.FlyVStep *= factor;
        return false;
    }
}

class InvisibleBlock : CustomBlockAlteration {
    // public override bool Run(CustomBlock customBlock) {
    //     MakeSingleLayer(customBlock);
    //     CPlugCrystal.GeometryLayer invisible = (CPlugCrystal.GeometryLayer)customBlock.Layers[0];
    //     invisible.Collidable = true;
    //     invisible.IsVisible = false;
    //     CPlugCrystal.GeometryLayer visible = new(){
    //         Collidable = false,
    //         IsVisible = true,
    //         Crystal = new(){IsEmbeddedCrystal = true},
    //     };
    //     customBlock.Layers.Add(visible);
    //     return true;
    // }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        layer.Crystal.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = "Stadium\\Media\\Modifier\\InvisibleDecal\\InvisibleDecal");
        return true;
    }
}

// ------------ Surface Alterations ------------
class HeavyTech : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Material\\PlatformTech", "Stadium\\Media\\Material\\RoadTech",CPlugSurface.MaterialId.Tech);
    }
}

class HeavyDirt : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech", "Stadium\\Media\\Material\\RoadDirt",CPlugSurface.MaterialId.Dirt);
    }
}

class HeavyGrass : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech",CPlugSurface.MaterialId.Green);
    }
}

class HeavyIce : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech", "Stadium\\Media\\Material\\RoadIce",CPlugSurface.MaterialId.Ice);
    }
}
class HeavyPlastic : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech",CPlugSurface.MaterialId.Plastic);
    }
}
class HeavyMagnet : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic", "Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic",CPlugSurface.MaterialId.TechSuperMagnetic);
    }
}

class LightTech : CustomBlockAlteration {
    public override bool Run(CustomBlock customBlock) {
        return MakeGeometryOnly(customBlock);
    }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Material\\PlatformTech", "Stadium\\Media\\Material\\RoadTech",CPlugSurface.MaterialId.Tech);
    }
}
class LightDirt : CustomBlockAlteration {
    public override bool Run(CustomBlock customBlock) {
        return MakeGeometryOnly(customBlock);
    }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech", "Stadium\\Media\\Material\\RoadDirt",CPlugSurface.MaterialId.Dirt);
    }
}
class LightGrass : CustomBlockAlteration {
    public override bool Run(CustomBlock customBlock) {
        return MakeGeometryOnly(customBlock);
    }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech","Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech",CPlugSurface.MaterialId.Green);
    }
}
class LightIce : CustomBlockAlteration {
    public override bool Run(CustomBlock customBlock) {
        return MakeGeometryOnly(customBlock);
    }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech", "Stadium\\Media\\Material\\RoadIce",CPlugSurface.MaterialId.Ice);
    }
}
class LightPlastic : CustomBlockAlteration {
    public override bool Run(CustomBlock customBlock) {
        return MakeGeometryOnly(customBlock);
    }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech",CPlugSurface.MaterialId.Plastic);
    }
}
class LightMagnet : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic", "Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic",CPlugSurface.MaterialId.TechSuperMagnetic);
    }
}

class MaterialInfo : CustomBlockAlteration {
    public static Dictionary<string, CPlugSurface.MaterialId> materials = [];
    public static Dictionary<CPlugSurface.MaterialId, string> SurfacePhysicIds = [];
    public static Dictionary<CPlugMaterialUserInst.GameplayId, string> SurfaceGameplayIds = [];

    public override bool Run(CustomBlock customBlock) {
        if (customBlock.Type == BlockType.Block) {
            customBlock.Block.CustomizedVariants[0].Crystal.Materials.ToList().ForEach(x => {
                if (!materials.ContainsKey(x.MaterialUserInst.Link)) {
                    materials.Add(x.MaterialUserInst.Link,x.MaterialUserInst.SurfacePhysicId);
                    Console.WriteLine(x.MaterialUserInst.Link);
                }
                if (!SurfacePhysicIds.ContainsKey(x.MaterialUserInst.SurfacePhysicId)) {
                    SurfacePhysicIds.Add(x.MaterialUserInst.SurfacePhysicId,x.MaterialUserInst.Link);
                    Console.WriteLine(x.MaterialUserInst.SurfacePhysicId);
                }
                if (!SurfaceGameplayIds.ContainsKey(x.MaterialUserInst.SurfaceGameplayId)) {
                    SurfaceGameplayIds.Add(x.MaterialUserInst.SurfaceGameplayId,x.MaterialUserInst.Link);
                    Console.WriteLine(x.MaterialUserInst.SurfaceGameplayId);
                }
            });
        } else {
            customBlock.Item.MeshCrystal.Materials.ToList().ForEach(x => {
                if (!materials.ContainsKey(x.MaterialUserInst.Link)) {
                    materials.Add(x.MaterialUserInst.Link,x.MaterialUserInst.SurfacePhysicId);
                    Console.WriteLine(x.MaterialUserInst.Link);
                }
                if (!SurfacePhysicIds.ContainsKey(x.MaterialUserInst.SurfacePhysicId)) {
                    SurfacePhysicIds.Add(x.MaterialUserInst.SurfacePhysicId,x.MaterialUserInst.Link);
                    Console.WriteLine(x.MaterialUserInst.SurfacePhysicId);
                }
                if (!SurfaceGameplayIds.ContainsKey(x.MaterialUserInst.SurfaceGameplayId)) {
                    SurfaceGameplayIds.Add(x.MaterialUserInst.SurfaceGameplayId,x.MaterialUserInst.Link);
                    Console.WriteLine(x.MaterialUserInst.SurfaceGameplayId);
                }
            });
        }
        return false;
    }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer)
    {
        layer.Crystal.Faces.ToList().ForEach(x => {
            if (!materials.ContainsKey(x.Material.MaterialUserInst.Link)) {
                materials.Add(x.Material.MaterialUserInst.Link,x.Material.MaterialUserInst.SurfacePhysicId);
                Console.WriteLine(x.Material.MaterialUserInst.Link);
            }
            if (!SurfacePhysicIds.ContainsKey(x.Material.MaterialUserInst.SurfacePhysicId)) {
                SurfacePhysicIds.Add(x.Material.MaterialUserInst.SurfacePhysicId,x.Material.MaterialUserInst.Link);
                Console.WriteLine(x.Material.MaterialUserInst.SurfacePhysicId);
            }
            if (!SurfaceGameplayIds.ContainsKey(x.Material.MaterialUserInst.SurfaceGameplayId)) {
                SurfaceGameplayIds.Add(x.Material.MaterialUserInst.SurfaceGameplayId,x.Material.MaterialUserInst.Link);
                Console.WriteLine(x.Material.MaterialUserInst.SurfaceGameplayId);
            }
        });
        return false;
    }
}