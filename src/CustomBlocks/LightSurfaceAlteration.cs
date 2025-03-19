using GBX.NET;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;

class LightSurfaceBlock : CustomSurfaceAlteration {
    //doesn't change the surface
    public override bool Run(CustomBlock customBlock) {
        bool changed = false;
        if (customBlock.customBlock.WaypointType != CGameItemModel.EWaypointType.None){
            customBlock.customBlock.WaypointType = CGameItemModel.EWaypointType.None;
            changed = true;
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
        //TODO check why some are unchanged
        layer.Crystal.Faces = layer.Crystal.Faces.Where(x => DrivableMaterials.Contains(GetMaterialLink(x))).ToArray();
        if (layer.Crystal.Faces.Length == 0){
            return false;
        }
        
        // move all positions closer to topmiddle point
        // Will have issues with surfaces looking away from topmiddle point
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
        return true;
    }
}