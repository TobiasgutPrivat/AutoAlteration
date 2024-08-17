using GBX.NET;
using GBX.NET.Engines.Plug;

class CustomBlockAlteration {
    public static string[] DrivableMaterials = [
        "Stadium\\Media\\Material\\PlatformTech",
        "Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech",
        "Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech",
        "Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech",
        "Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech",
        "Stadium\\Media\\Material\\RoadBump",
        "Stadium\\Media\\Material\\RoadTech",
        "Stadium\\Media\\Material\\RoadDirt",
        "Stadium\\Media\\Material\\RoadIce",
        "Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic",
    ];
    public virtual bool AlterLayer(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {return false;}
    public virtual bool Run(CustomBlock customBlock) {return false;}
    public virtual bool AlterBlock(CustomBlock customBlock) {return false;}
    public virtual bool AlterItem(CustomBlock customBlock) {return false;}

    public static Vec3 GetTopMiddle(CPlugCrystal.Crystal mesh) {
        float top = mesh.Positions.Max(x => x.Y);
        float Right = mesh.Positions.Min(x => x.X);
        float Left = mesh.Positions.Max(x => x.X);
        float Front = mesh.Positions.Max(x => x.Z);
        float Back = mesh.Positions.Min(x => x.Z);
        return new Vec3((Right + Left) / 2, top, (Front + Back) / 2);
    }

    public static void MakeSingleLayer(CustomBlock customBlock){
        if (customBlock.Name == "GateExpandableGameplayRally"){
            Console.WriteLine("Skipping GateExpandableGameplayRally");
        }
        customBlock.Layers.Where(x => x is CPlugCrystal.GeometryLayer layer && layer.Collidable).Skip(1).ToList().ForEach(x => customBlock.Layers.Remove(x));
        customBlock.Layers.Where(x => x is CPlugCrystal.GeometryLayer layer && !layer.Collidable).ToList().ForEach(x => customBlock.Layers.Remove(x));
        List<CPlugCrystal.GeometryLayer>collidables = customBlock.Layers.Where(x => x is CPlugCrystal.GeometryLayer layer && layer.Collidable).Select(x => x as CPlugCrystal.GeometryLayer).ToList();
        if (collidables.Count == 0) {
            Console.WriteLine("No collidables");
        } else {
            collidables.First().IsVisible = true;
        }
    }

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
        MakeSingleLayer(customBlock);
        if (customBlock.Name.Contains("Road") && !customBlock.Name.Contains("Open")) {
            layer.Crystal.Faces.ToList().ForEach(x => {if (DrivableMaterials.Contains(x.Material.MaterialUserInst.Link)) {x.Material.MaterialUserInst.Link = RoadSurface;}});
            if (layer.Crystal.Faces.ToList().Any(x => x.Material.MaterialUserInst.Link == RoadSurface)){return true;};
        } else {
            layer.Crystal.Faces.ToList().ForEach(x => {if (DrivableMaterials.Contains(x.Material.MaterialUserInst.Link)) {x.Material.MaterialUserInst.Link = Surface;}});
            if (layer.Crystal.Faces.ToList().Any(x => x.Material.MaterialUserInst.Link == Surface)){return true;};
        }
        return false;
    }

    public static void Resize(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer, float factor)
    {
        layer.Crystal.Positions = layer.Crystal.Positions.ToList().Select(x => new Vec3(x.X * factor, x.Y * factor, x.Z * factor)).ToArray();
        if (customBlock.Layers.Where(x => x is CPlugCrystal.SpawnPositionLayer).FirstOrDefault() is CPlugCrystal.SpawnPositionLayer spawn) {
            spawn.SpawnPosition = new Vec3(spawn.SpawnPosition.X * factor, spawn.SpawnPosition.Y * factor, spawn.SpawnPosition.Z * factor);
        }
        if (customBlock.Layers.Where(x => x is CPlugCrystal.TriggerLayer).FirstOrDefault() is CPlugCrystal.TriggerLayer trigger) {
            trigger.Crystal.Positions = trigger.Crystal.Positions.ToList().Select(x => new Vec3(x.X * factor, x.Y * factor, x.Z * factor)).ToArray();
        }
        //TODO resize snapping steps
    }
}

class MaterialInfo : CustomBlockAlteration {
    public static string[] materials = [];
    public override bool AlterBlock(CustomBlock customBlock) {
        customBlock.Block.CustomizedVariants[0].Crystal.Materials.ToList().ForEach(x => {
            if (!materials.Contains(x.MaterialUserInst.Link)) {
                materials = materials.Append(x.MaterialUserInst.Link).ToArray();
                Console.WriteLine(x.MaterialUserInst.Link);
            }
        });
        return false;
    }
    public override bool AlterItem(CustomBlock customBlock) {
        customBlock.Item.MeshCrystal.Materials.ToList().ForEach(x => {
            if (!materials.Contains(x.MaterialUserInst.Link)) {
                materials = materials.Append(x.MaterialUserInst.Link).ToArray();
                Console.WriteLine(x.MaterialUserInst.Link);
            }
        });
        return false;
    }
}

class SupersizedBlock : CustomBlockAlteration {
    public override bool AlterLayer(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        MakeSingleLayer(customBlock);
        Resize(customBlock,layer,2);
        return true;
    }
}

class MiniBlock : CustomBlockAlteration {
    public override bool AlterLayer(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        MakeSingleLayer(customBlock);
        Resize(customBlock,layer,0.5f);
        return true;
    }
}

class InvisibleBlock : CustomBlockAlteration {
    public override bool Run(CustomBlock customBlock) {
        MakeSingleLayer(customBlock);
        CPlugCrystal.GeometryLayer invisible = (CPlugCrystal.GeometryLayer)customBlock.Layers[0];
        invisible.Collidable = true;
        invisible.IsVisible = false;
        CPlugCrystal.GeometryLayer visible = new(){
            Collidable = false,
            IsVisible = true,
            Crystal = new(){IsEmbeddedCrystal = true},
        };
        customBlock.Layers.Add(visible);
        return true;
    }
}

// ------------ Surface Alterations ------------
class HeavyTech : CustomBlockAlteration {
    public override bool AlterLayer(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Material\\PlatformTech", "Stadium\\Media\\Material\\RoadTech");
    }
}

class HeavyDirt : CustomBlockAlteration {
    public override bool AlterLayer(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech", "Stadium\\Media\\Material\\RoadDirt");
    }
}

class HeavyGrass : CustomBlockAlteration {
    public override bool AlterLayer(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech");
    }
}

class HeavyIce : CustomBlockAlteration {
    public override bool AlterLayer(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech", "Stadium\\Media\\Material\\RoadIce");
    }
}
class HeavyPlastic : CustomBlockAlteration {
    public override bool AlterLayer(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech");
    }
}
class HeavyMagnet : CustomBlockAlteration {
    public override bool AlterLayer(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return HeavySurface(customBlock, layer,"Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic", "Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic");
    }
}

class LightTech : CustomBlockAlteration {
    public override bool AlterLayer(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Material\\PlatformTech", "Stadium\\Media\\Material\\RoadTech");
    }
}
class LightDirt : CustomBlockAlteration {
    public override bool AlterLayer(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech", "Stadium\\Media\\Material\\RoadDirt");
    }
}
class LightGrass : CustomBlockAlteration {
    public override bool AlterLayer(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech");
    }
}
class LightIce : CustomBlockAlteration {
    public override bool AlterLayer(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech", "Stadium\\Media\\Material\\RoadIce");
    }
}
class LightPlastic : CustomBlockAlteration {
    public override bool AlterLayer(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech", "Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech");
    }
}
class LightMagnet : CustomBlockAlteration {
    public override bool AlterLayer(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        return LightSurface(customBlock, layer,"Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic", "Editors\\MeshEditorMedia\\Materials\\TechSuperMagnetic");
    }
}