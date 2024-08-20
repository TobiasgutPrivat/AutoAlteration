using GBX.NET;
using GBX.NET.Engines.Plug;

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
    public override bool Run(CustomBlock customBlock) {
        CustomBlock LowCube = new CustomBlock(AutoAlteration.ProjectFolder + "data/LowCubeLayer.Item.Gbx");
        CPlugCrystal.GeometryLayer layer = LowCube.Layers[0] as CPlugCrystal.GeometryLayer;
        layer.Crystal.Positions = layer.Crystal.Positions.Select(x => new Vec3(x.X * 0.1f, x.Y * 0.1f - 1000, x.Z * 0.1f)).ToArray();
        customBlock.MeshCrystal.Layers.Add(layer);
        return true;
    }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {
        
        layer.Crystal.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = "Stadium\\Media\\Modifier\\InvisibleDecal\\InvisibleDecal");
        return true;
    }
}