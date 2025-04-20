using GBX.NET;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;

public class LightSurfaceBlock : CustomBlockAlteration {
    //doesn't change the surface
    public override bool Run(CustomBlock customBlock) {
        bool changed = false;
        if (customBlock.customBlock.WaypointType != CGameItemModel.EWaypointType.None){
            customBlock.customBlock.WaypointType = CGameItemModel.EWaypointType.None;
            changed = true;
        }
        // avoid declaration as start/finish/checkpoint
        if (customBlock.Type == BlockType.Block && customBlock.customBlock.EntityModelEdition is not null){
            CGameBlockItem Block = (CGameBlockItem)customBlock.customBlock.EntityModelEdition;
            if (Block.ArchetypeBlockInfoId.Contains("Start") || Block.ArchetypeBlockInfoId.Contains("Finish") || Block.ArchetypeBlockInfoId.Contains("Checkpoint")){
                Block.ArchetypeBlockInfoId = "RoadTechStraight"; //doesn't matter as long as it's in air-mode and not on ground
                changed = true;
            }
        }
        return changed;
    }
    public override bool AlterMeshCrystal(CustomBlock customBlock, CPlugCrystal MeshCrystal) {
        bool changed = false;
        //remove all non geometry layers
        if (MeshCrystal.Layers.Any(x => x is not CPlugCrystal.GeometryLayer)){
            MeshCrystal.Layers.Where(x => x is not CPlugCrystal.GeometryLayer).ToList().ForEach(x => MeshCrystal.Layers.Remove(x));
            changed = true;
        }
        //make single geometry layer visible and collidable, priority: collidable, visible
        MeshCrystal.Layers = [MeshCrystal.Layers.
            Where(x => x is CPlugCrystal.GeometryLayer && (x as CPlugCrystal.GeometryLayer).IsEnabled).
            OrderBy(x => ((x as CPlugCrystal.GeometryLayer).Collidable ? 0 : 2) + ((x as CPlugCrystal.GeometryLayer).IsVisible ? 0 : 1)).First()];
        (MeshCrystal.Layers[0] as CPlugCrystal.GeometryLayer).Collidable = true;
        (MeshCrystal.Layers[0] as CPlugCrystal.GeometryLayer).IsVisible = true;
        return changed;
    }

    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer){
        float offset = 0.04f;
        //TODO check why some are unchanged
        layer.Crystal.Faces = layer.Crystal.Faces.Where(x => CustomSurfaceAlteration.DrivableMaterials.Contains(GetMaterialLink(x))).ToArray();
        if (layer.Crystal.Faces.Length == 0){
            return false;
        }
        
        // move all positions closer to topmiddle point
        // Will have issues with surfaces looking away from topmiddle point
        Vec3 topMiddle = GetTopMiddle(layer.Crystal);
        layer.Crystal.Positions = layer.Crystal.Positions.ToList().Select(x => {
            float X,Y,Z;
            Y = x.Y + offset;
            if (x.X > topMiddle.X){
                X = x.X - offset;
            } else {
                X = x.X + offset;
            }
            if (x.Z > topMiddle.Z){
                Z = x.Z - offset;
            } else {
                Z = x.Z + offset;
            }
            return new Vec3(X, Y, Z);
            }).ToArray();
        return true;
    }
}