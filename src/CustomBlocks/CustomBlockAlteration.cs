using GBX.NET;
using GBX.NET.Engines.Plug;

public class CustomBlockAlteration {
    public virtual bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer) {return false;}
    public virtual bool AlterTrigger(CustomBlock customBlock, CPlugCrystal.TriggerLayer layer) {return false;}
    public virtual bool AlterSpawn(CustomBlock customBlock, CPlugCrystal.SpawnPositionLayer layer) {return false;}
    public virtual bool AlterMeshCrystal(CustomBlock customBlock, CPlugCrystal MeshCrystal) {return false;}
    public virtual bool Run(CustomBlock customBlock) {return false;}

    public static Vec3 GetTopMiddle(CPlugCrystal.Crystal mesh) {
        float top = mesh.Positions.Max(x => x.Y);
        float Right = mesh.Positions.Min(x => x.X);
        float Left = mesh.Positions.Max(x => x.X);
        float Front = mesh.Positions.Max(x => x.Z);
        float Back = mesh.Positions.Min(x => x.Z);
        return new Vec3((Right + Left) / 2, top, (Front + Back) / 2);
    }

    public static bool MakeGeometryOnly(CustomBlock customBlock, CPlugCrystal MeshCrystal){
        bool changed = false;
        if (customBlock.customBlock.WaypointType != GBX.NET.Engines.GameData.CGameItemModel.EWaypointType.None){
            customBlock.customBlock.WaypointType = GBX.NET.Engines.GameData.CGameItemModel.EWaypointType.None;
            changed = true;
        }
        if (MeshCrystal.Layers.Any(x => x is not CPlugCrystal.GeometryLayer)){
            MeshCrystal.Layers.Where(x => x is not CPlugCrystal.GeometryLayer).ToList().ForEach(x => MeshCrystal.Layers.Remove(x));
            changed = true;
        }
        return changed;
    }

    public static string GetMaterialLink(CPlugCrystal.Face face) =>
        face.Material?.MaterialUserInst?.Link ?? "";
    public static string GetMaterialLink(CPlugCrystal.Material face) =>
        face.MaterialUserInst?.Link ?? "";
    public static CPlugSurface.MaterialId GetMaterialSurfacePhysicId(CPlugCrystal.Face face) =>
        face.Material?.MaterialUserInst?.SurfacePhysicId ?? CPlugSurface.MaterialId.XXX_Null;
    public static CPlugSurface.MaterialId GetMaterialSurfacePhysicId(CPlugCrystal.Material face) =>
        face.MaterialUserInst?.SurfacePhysicId ?? new CPlugSurface.MaterialId();
    public static CPlugMaterialUserInst GetMaterialUserInst(CPlugCrystal.Face face) =>
        face.Material?.MaterialUserInst ?? new CPlugMaterialUserInst();
    public static CPlugMaterialUserInst GetMaterialUserInst(CPlugCrystal.Material face) =>
        face.MaterialUserInst ?? new CPlugMaterialUserInst();
}


class MaterialInfo : CustomBlockAlteration {
    public static Dictionary<string, CPlugSurface.MaterialId> materials = [];
    public static Dictionary<CPlugSurface.MaterialId, string> SurfacePhysicIds = [];
    public static Dictionary<CPlugMaterialUserInst.GameplayId, string> SurfaceGameplayIds = [];

    public override bool Run(CustomBlock customBlock) {
        if (customBlock.Type == BlockType.Block) {
            customBlock.Block.CustomizedVariants[0].Crystal.Materials.ToList().ForEach(x => {
                if (!materials.ContainsKey(GetMaterialLink(x))) {
                    materials.Add(GetMaterialLink(x),GetMaterialUserInst(x).SurfacePhysicId);
                    Console.WriteLine(GetMaterialLink(x));
                }
                if (!SurfacePhysicIds.ContainsKey(GetMaterialUserInst(x).SurfacePhysicId)) {
                    SurfacePhysicIds.Add(GetMaterialUserInst(x).SurfacePhysicId,GetMaterialLink(x));
                    Console.WriteLine(GetMaterialUserInst(x).SurfacePhysicId);
                }
                if (!SurfaceGameplayIds.ContainsKey(GetMaterialUserInst(x).SurfaceGameplayId)) {
                    SurfaceGameplayIds.Add(GetMaterialUserInst(x).SurfaceGameplayId,GetMaterialLink(x));
                    Console.WriteLine(GetMaterialUserInst(x).SurfaceGameplayId);
                }
            });
        } else {
            customBlock.Item.MeshCrystal.Materials.ToList().ForEach(x => {
                if (!materials.ContainsKey(GetMaterialLink(x))) {
                    materials.Add(GetMaterialLink(x),GetMaterialUserInst(x).SurfacePhysicId);
                    Console.WriteLine(GetMaterialLink(x));
                }
                if (!SurfacePhysicIds.ContainsKey(GetMaterialUserInst(x).SurfacePhysicId)) {
                    SurfacePhysicIds.Add(GetMaterialUserInst(x).SurfacePhysicId,GetMaterialLink(x));
                    Console.WriteLine(GetMaterialUserInst(x).SurfacePhysicId);
                }
                if (!SurfaceGameplayIds.ContainsKey(GetMaterialUserInst(x).SurfaceGameplayId)) {
                    SurfaceGameplayIds.Add(GetMaterialUserInst(x).SurfaceGameplayId,GetMaterialLink(x));
                    Console.WriteLine(GetMaterialUserInst(x).SurfaceGameplayId);
                }
            });
        }
        return false;
    }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer)
    {
        layer.Crystal.Faces.ToList().ForEach(x => {
            if (!materials.ContainsKey(GetMaterialLink(x))) {
                materials.Add(GetMaterialLink(x),GetMaterialUserInst(x).SurfacePhysicId);
                Console.WriteLine(GetMaterialLink(x));
            }
            if (!SurfacePhysicIds.ContainsKey(GetMaterialUserInst(x).SurfacePhysicId)) {
                SurfacePhysicIds.Add(GetMaterialUserInst(x).SurfacePhysicId,GetMaterialLink(x));
                Console.WriteLine(GetMaterialUserInst(x).SurfacePhysicId);
            }
            if (!SurfaceGameplayIds.ContainsKey(GetMaterialUserInst(x).SurfaceGameplayId)) {
                SurfaceGameplayIds.Add(GetMaterialUserInst(x).SurfaceGameplayId,GetMaterialLink(x));
                Console.WriteLine(GetMaterialUserInst(x).SurfaceGameplayId);
            }
        });
        return false;
    }
}