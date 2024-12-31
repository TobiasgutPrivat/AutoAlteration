using GBX.NET;
using GBX.NET.Engines.Plug;

public class CustomBlockAlteration {
    public virtual string Description { get{ return "No description given";} }
    public virtual bool Published { get{ return false;} }
    public virtual bool HasFlaws { get{ return true;} }
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
        customBlock.MeshCrystals.ForEach(x => {
            x.Materials.ToList().ForEach(y => {
                if (!materials.ContainsKey(GetMaterialLink(y))) {
                    materials.Add(GetMaterialLink(y),GetMaterialUserInst(y).SurfacePhysicId);
                    Console.WriteLine(GetMaterialLink(y));
                }
                if (!SurfacePhysicIds.ContainsKey(GetMaterialUserInst(y).SurfacePhysicId)) {
                    SurfacePhysicIds.Add(GetMaterialUserInst(y).SurfacePhysicId,GetMaterialLink(y));
                    Console.WriteLine(GetMaterialUserInst(y).SurfacePhysicId);
                }
                if (!SurfaceGameplayIds.ContainsKey(GetMaterialUserInst(y).SurfaceGameplayId)) {
                    SurfaceGameplayIds.Add(GetMaterialUserInst(y).SurfaceGameplayId,GetMaterialLink(y));
                    Console.WriteLine(GetMaterialUserInst(y).SurfaceGameplayId);
                }
            });
        });
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