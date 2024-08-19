using GBX.NET;
using GBX.NET.Engines.Plug;

class CustomBlockAlteration {
    public static string[] DrivableMaterials = ["Stadium\\Media\\Material\\PlatformTech","Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech","Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech","Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech","Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech","Stadium\\Media\\Material\\RoadBump","Stadium\\Media\\Material\\RoadTech","Stadium\\Media\\Material\\RoadDirt","Stadium\\Media\\Material\\RoadIce","Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic",];
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

    // public static void MakeSingleLayer(CustomBlock customBlock){
    //     List<CPlugCrystal.GeometryLayer> GeometryLayers = customBlock.Layers.Where(x => x is CPlugCrystal.GeometryLayer layer && layer.Collidable).Select(x => x as CPlugCrystal.GeometryLayer).ToList();
    //     if (GeometryLayers.Count == 0){
    //         return;
    //     }
    //     customBlock.Layers.Where(x => x is CPlugCrystal.GeometryLayer layer).OrderByDescending(x => x is CPlugCrystal.GeometryLayer layer ? layer.Crystal.Faces.Length : 0).Skip(1).ToList().ForEach(x => customBlock.Layers.Remove(x));
    //     GeometryLayers.First().IsVisible = true;
    //     GeometryLayers.First().Collidable = true;
    // }

    public static bool LightSurface(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer, string Surface, string RoadSurface){
        layer.Crystal.Faces = layer.Crystal.Faces.ToList().Where(x => DrivableMaterials.Contains(x.Material.MaterialUserInst.Link)).ToArray();
        if (layer.Crystal.Faces.Length == 0){
            return false;
        }
        if (customBlock.Name.Contains("Road") && !customBlock.Name.Contains("Open")){
            layer.Crystal.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = RoadSurface);
        } else {
            layer.Crystal.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = Surface);
        }
        Vec3 topMiddle = GetTopMiddle(layer.Crystal);
        layer.Crystal.Positions = layer.Crystal.Positions.ToList().Select(x => new Vec3(x.X,x.Y + 0.01f,x.Z) * 0.99f + topMiddle * 0.01f).ToArray();
        //Will have issues with surfaces looking away from topmiddle point
        return true;
    }

    public static bool HeavySurface(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer, string Surface, string RoadSurface){
        // MakeSingleLayer(customBlock);
        if (customBlock.Name.Contains("Road") && !customBlock.Name.Contains("Open")) {
            layer.Crystal.Faces.ToList().ForEach(x => {if (DrivableMaterials.Contains(x.Material.MaterialUserInst.Link)) {x.Material.MaterialUserInst.Link = RoadSurface;}});
            if (layer.Crystal.Faces.ToList().Any(x => x.Material.MaterialUserInst.Link == RoadSurface)){return true;};
        } else {
            layer.Crystal.Faces.ToList().ForEach(x => {if (DrivableMaterials.Contains(x.Material.MaterialUserInst.Link)) {x.Material.MaterialUserInst.Link = Surface;}});
            if (layer.Crystal.Faces.ToList().Any(x => x.Material.MaterialUserInst.Link == Surface)){return true;};
        }
        return false;
    }
}

// class SingleLayer : CustomBlockAlteration {
//     public override bool Run(CustomBlock customBlock) {
//         MakeSingleLayer(customBlock);
//         return true;
//     }
// }

class MaterialInfo : CustomBlockAlteration {
    public static string[] materials = [];
    public override bool Run(CustomBlock customBlock) {
        if (customBlock.Type == BlockType.Block) {
            customBlock.Block.CustomizedVariants[0].Crystal.Materials.ToList().ForEach(x => {
                if (!materials.Contains(x.MaterialUserInst.Link)) {
                    materials = materials.Append(x.MaterialUserInst.Link).ToArray();
                    Console.WriteLine(x.MaterialUserInst.Link);
                }
            });
        } else {
            customBlock.Item.MeshCrystal.Materials.ToList().ForEach(x => {
                if (!materials.Contains(x.MaterialUserInst.Link)) {
                    materials = materials.Append(x.MaterialUserInst.Link).ToArray();
                    Console.WriteLine(x.MaterialUserInst.Link);
                }
            });
        }
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
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Material\\PlatformTech", "Stadium\\Media\\Material\\RoadTech");
    }
}

class HeavyDirt : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech", "Stadium\\Media\\Material\\RoadDirt");
    }
}

class HeavyGrass : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech");
    }
}

class HeavyIce : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech", "Stadium\\Media\\Material\\RoadIce");
    }
}
class HeavyPlastic : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech");
    }
}
class HeavyMagnet : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic", "Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic");
    }
}

class LightTech : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Material\\PlatformTech", "Stadium\\Media\\Material\\RoadTech");
    }
}
class LightDirt : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech", "Stadium\\Media\\Material\\RoadDirt");
    }
}
class LightGrass : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech");
    }
}
class LightIce : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech", "Stadium\\Media\\Material\\RoadIce");
    }
}
class LightPlastic : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech");
    }
}
class LightMagnet : CustomBlockAlteration {
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic", "Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic");
    }
}