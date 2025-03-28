using GBX.NET;
using GBX.NET.Engines.Plug;

public class SupersizedBlock : CustomBlockAlteration {
    private float factor;
    public SupersizedBlock(float factor = 2) { this.factor = factor; }

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
        if (customBlock.Type == BlockType.Block) {
            customBlock.customBlock.DefaultPlacement.GridSnapHStep *= factor;
            customBlock.customBlock.DefaultPlacement.GridSnapVStep *= factor;
            customBlock.customBlock.DefaultPlacement.FlyVStep *= factor;
        } else if (customBlock.Type == BlockType.Item) {
            //TODO
        }
        return false;
    }
}

public class MiniBlock : CustomBlockAlteration {
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
        if (customBlock.Type == BlockType.Block) {
            customBlock.customBlock.DefaultPlacement.GridSnapHStep *= factor;
            customBlock.customBlock.DefaultPlacement.GridSnapVStep *= factor;
            customBlock.customBlock.DefaultPlacement.FlyVStep *= factor;
        } else if (customBlock.Type == BlockType.Item) {
            //TODO
        }
        return false;
    }
}

public class InvisibleBlock : CustomBlockAlteration {
    public override bool AlterMeshCrystal(CustomBlock customBlock, CPlugCrystal MeshCrystal) {
        CustomBlock LowCube = new CustomBlock(Path.Combine(AltertionConfig.DataFolder, "Templates","LowCubeLayer.Item.Gbx"));
        CPlugCrystal.GeometryLayer layer = LowCube.MeshCrystals[0].Layers[0] as CPlugCrystal.GeometryLayer;
        layer.Crystal.Positions = layer.Crystal.Positions.Select(x => new Vec3(x.X, x.Y - 1500, x.Z)).ToArray();
        MeshCrystal.Layers.Add(layer); //will not be effected by Altergeometry, because MeshCrystals to Alter get selected before this
        return true;
    }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        
        layer.Crystal.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = "Stadium\\Media\\Modifier\\InvisibleDecal\\InvisibleDecal");
        return true;
    }
}