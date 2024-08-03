using GBX.NET;
using GBX.NET.Engines.Plug;

class CustomBlockAlteration {
    public static string[] DrivableMaterials = new string[] {
        "Stadium\\Media\\Material\\PlatformTech",
        // "Stadium\\Media\\Modifier\\PlatformIce\\DecoHill",
        // "Stadium\\Media\\Modifier\\PlatformDirt\\DecoHill",
        "Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech",
        "Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech",
        "Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech",
        // "Stadium\\Media\\Modifier\\PlatformIce\\OpenTechBorders",
        "Stadium\\Media\\Modifier\\PlatformPlastic\\PlatformTech",
        // "Stadium\\Media\\Modifier\\PlatformPlastic\\DecalPlatform",
        // "Stadium\\Media\\Modifier\\PlatformGrass\\OpenTechBorders",
        // "Stadium\\Media\\Modifier\\PlatformDirt\\OpenTechBorders",
        // "Stadium\\Media\\Modifier\\PlatformGrass\\DecalPlatform",
        // "Stadium\\Media\\Modifier\\PlatformDirt\\DecalPlatform",
        // "Stadium\\Media\\Modifier\\PlatformIce\\DecalPlatform",
        // "Stadium\\Media\\Modifier\\PlatformDirt\\DecoHill2",
        // "Stadium\\Media\\Modifier\\PlatformIce\\DecoHill2",
        "Stadium\\Media\\Material\\RoadBump",
        "Stadium\\Media\\Material\\RoadTech",
        "Stadium\\Media\\Material\\RoadDirt",
        "Stadium\\Media\\Material\\RoadIce",
        // "Stadium\\Media\\Material\\RoadTechSubIce",
    };
    public virtual void AlterMesh(CustomBlock customBlock, CPlugCrystal.Crystal mesh) {}
    public virtual void Run(CustomBlock customBlock) {}
    public virtual void AlterBlock(CustomBlock customBlock) {}
    public virtual void AlterItem(CustomBlock customBlock) {}

    public static Vec3 GetTopMiddle(CPlugCrystal.Crystal mesh) {
        float top = mesh.Positions.Max(x => x.Y);
        float Right = mesh.Positions.Min(x => x.X);
        float Left = mesh.Positions.Max(x => x.X);
        float Front = mesh.Positions.Max(x => x.Z);
        float Back = mesh.Positions.Min(x => x.Z);
        return new Vec3((Right + Left) / 2, top, (Front + Back) / 2);
    }
}

class MaterialInfo : CustomBlockAlteration {
    public static string[] materials = Array.Empty<string>();
    public override void AlterBlock(CustomBlock customBlock) {
        customBlock.Block.CustomizedVariants[0].Crystal.Materials.ToList().ForEach(x => {
            if (!materials.Contains(x.MaterialUserInst.Link)) {
                materials = materials.Append(x.MaterialUserInst.Link).ToArray();
                Console.WriteLine(x.MaterialUserInst.Link);
            }
        });
    }
    public override void AlterItem(CustomBlock customBlock) {
        customBlock.Item.MeshCrystal.Materials.ToList().ForEach(x => {
            if (!materials.Contains(x.MaterialUserInst.Link)) {
                materials = materials.Append(x.MaterialUserInst.Link).ToArray();
                Console.WriteLine(x.MaterialUserInst.Link);
            }
        });
    }
}

class HeavyDirt : CustomBlockAlteration {
    public override void AlterMesh(CustomBlock customBlock, CPlugCrystal.Crystal mesh) {
        if (customBlock.Name.Contains("Road")) {
            mesh.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = "Stadium\\Media\\Material\\RoadDirt");
        } else {
            mesh.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = "Stadium\\Media\\Modifier\\PlatformDirt\\PlatformTech");
        }
    }
}

class HeavyGrass : CustomBlockAlteration {
    public override void AlterMesh(CustomBlock customBlock, CPlugCrystal.Crystal mesh) {
        mesh.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = "Stadium\\Media\\Modifier\\PlatformGrass\\PlatformTech");
    }
}

class HeavyTech : CustomBlockAlteration {
    public override void AlterMesh(CustomBlock customBlock, CPlugCrystal.Crystal mesh) {
        if (customBlock.Name.Contains("Road")) {
            mesh.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = "Stadium\\Media\\Material\\RoadTech");
        } else if (customBlock.Name.Contains("RoadBump")){
            mesh.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = "Stadium\\Media\\Material\\RoadBump");
        } else {
            mesh.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = "Stadium\\Media\\Material\\PlatformTech");
        }    
    }
}

class LightTech : CustomBlockAlteration {
    public override void AlterMesh(CustomBlock customBlock, CPlugCrystal.Crystal mesh) {
        mesh.Faces = mesh.Faces.ToList().Where(x => DrivableMaterials.Contains(x.Material.MaterialUserInst.Link)).ToArray();
        if (customBlock.Name.Contains("RoadBump")) {
            mesh.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = "Stadium\\Media\\Material\\RoadBump");
        } else if (customBlock.Name.Contains("Road")){
            mesh.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = "Stadium\\Media\\Material\\RoadTech");
        } else {
            mesh.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = "Stadium\\Media\\Material\\PlatformTech");
        }
        Vec3 topMiddle = GetTopMiddle(mesh);
        mesh.Positions = mesh.Positions.ToList().Select(x => new Vec3(x.X,x.Y + 0.01f,x.Z) * 0.99f + topMiddle * 0.01f).ToArray();
        //Will have issues with surfaces looking away from topmiddle point
    }
}

class HeavyIce : CustomBlockAlteration {
    public override void AlterMesh(CustomBlock customBlock, CPlugCrystal.Crystal mesh) {
        if (customBlock.Name.Contains("Road")) {
            mesh.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = "Stadium\\Media\\Material\\RoadIce");
        } else {
            mesh.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = "Stadium\\Media\\Modifier\\PlatformIce\\PlatformTech");
        }
    }
}

class SupersizedBlock : CustomBlockAlteration {
    public override void AlterMesh(CustomBlock customBlock, CPlugCrystal.Crystal mesh) {
        mesh.Positions = mesh.Positions.ToList().Select(x => new Vec3(x.X * 2, x.Y * 2, x.Z * 2)).ToArray();
    }
}

class MiniBlock : CustomBlockAlteration {
    public override void AlterMesh(CustomBlock customBlock, CPlugCrystal.Crystal mesh) {
        mesh.Positions = mesh.Positions.ToList().Select(x => new Vec3(x.X / 2, x.Y / 2, x.Z / 2)).ToArray();
    }
}