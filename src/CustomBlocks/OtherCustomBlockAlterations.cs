using GBX.NET;
using GBX.NET.Engines.Plug;

public class ResizeBlock(float factor) : CustomBlockAlteration
{
    private float factor = factor;

    public override bool Run(CustomBlock customBlock)
    {
        if (customBlock.Type == BlockType.Block)
        {
            customBlock.customBlock.DefaultPlacement.GridSnapHStep *= factor;
            customBlock.customBlock.DefaultPlacement.GridSnapVStep *= factor;
            customBlock.customBlock.DefaultPlacement.FlyVStep *= factor;
        }
        else if (customBlock.Type == BlockType.Item)
        {
            //TODO
        }
        foreach (CPlugCrystal.Layer layer in customBlock.MeshCrystals.SelectMany(x => x.Layers))
        {
            if (layer is CPlugCrystal.GeometryLayer geometryLayer)
            {
                geometryLayer.Crystal.Positions = geometryLayer.Crystal.Positions.ToList().Select(x => new Vec3(x.X * factor, x.Y * factor, x.Z * factor)).ToArray();
            }
            if (layer is CPlugCrystal.TriggerLayer triggerLayer)
            {
                triggerLayer.Crystal.Positions = triggerLayer.Crystal.Positions.ToList().Select(x => new Vec3(x.X * factor, x.Y * factor, x.Z * factor)).ToArray();
            }
            if (layer is CPlugCrystal.SpawnPositionLayer spawnPositionLayer)
            {
                    spawnPositionLayer.SpawnPosition = new Vec3(spawnPositionLayer.SpawnPosition.X * factor, spawnPositionLayer.SpawnPosition.Y * factor, spawnPositionLayer.SpawnPosition.Z * factor);
            }
        }
        return true;
    }
}

public class SupersizedBlock(float factor = 2) : ResizeBlock(factor) { }
public class MiniBlock(float factor = 0.5f) : ResizeBlock(factor) { }

public class InvisibleBlock : CustomBlockAlteration {
    public override bool Run(CustomBlock customBlock) {
        foreach (CPlugCrystal MeshCrystal in customBlock.MeshCrystals)
        {
            // make invisible
            foreach (CPlugCrystal.GeometryLayer layer in MeshCrystal.Layers.Where(x => x.GetType() == typeof(CPlugCrystal.GeometryLayer)).Cast<CPlugCrystal.GeometryLayer>())
            {
                layer.Crystal.Faces.ToList().ForEach(x => x.Material.MaterialUserInst.Link = "Stadium\\Media\\Modifier\\InvisibleDecal\\InvisibleDecal");
            }
            // add visible cube for tracking
            CustomBlock LowCube = new CustomBlock(Path.Combine(AlterationConfig.DataFolder, "Templates", "LowCubeLayer.Item.Gbx"));
            CPlugCrystal.GeometryLayer LowCubeLayer = LowCube.MeshCrystals[0].Layers[0] as CPlugCrystal.GeometryLayer;
            LowCubeLayer.Crystal.Positions = LowCubeLayer.Crystal.Positions.Select(x => new Vec3(x.X, x.Y - 1500, x.Z)).ToArray();
            MeshCrystal.Layers.Add(LowCubeLayer); //will not be effected by Altergeometry, because MeshCrystals to Alter get selected before this
        }
        return customBlock.MeshCrystals.Count > 0;
    }
}