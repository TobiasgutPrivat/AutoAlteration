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
}


class MaterialInfo : CustomBlockAlteration {
    public static Dictionary<string, CPlugSurface.MaterialId> materials = [];
    public static Dictionary<CPlugSurface.MaterialId, string> SurfacePhysicIds = [];
    public static Dictionary<CPlugMaterialUserInst.GameplayId, string> SurfaceGameplayIds = [];

    public override bool Run(CustomBlock customBlock) {
        if (customBlock.Type == BlockType.Block) {
            customBlock.Block.CustomizedVariants[0].Crystal.Materials.ToList().ForEach(x => {
                if (!materials.ContainsKey(x.MaterialUserInst.Link)) {
                    materials.Add(x.MaterialUserInst.Link,x.MaterialUserInst.SurfacePhysicId);
                    Console.WriteLine(x.MaterialUserInst.Link);
                }
                if (!SurfacePhysicIds.ContainsKey(x.MaterialUserInst.SurfacePhysicId)) {
                    SurfacePhysicIds.Add(x.MaterialUserInst.SurfacePhysicId,x.MaterialUserInst.Link);
                    Console.WriteLine(x.MaterialUserInst.SurfacePhysicId);
                }
                if (!SurfaceGameplayIds.ContainsKey(x.MaterialUserInst.SurfaceGameplayId)) {
                    SurfaceGameplayIds.Add(x.MaterialUserInst.SurfaceGameplayId,x.MaterialUserInst.Link);
                    Console.WriteLine(x.MaterialUserInst.SurfaceGameplayId);
                }
            });
        } else {
            customBlock.Item.MeshCrystal.Materials.ToList().ForEach(x => {
                if (!materials.ContainsKey(x.MaterialUserInst.Link)) {
                    materials.Add(x.MaterialUserInst.Link,x.MaterialUserInst.SurfacePhysicId);
                    Console.WriteLine(x.MaterialUserInst.Link);
                }
                if (!SurfacePhysicIds.ContainsKey(x.MaterialUserInst.SurfacePhysicId)) {
                    SurfacePhysicIds.Add(x.MaterialUserInst.SurfacePhysicId,x.MaterialUserInst.Link);
                    Console.WriteLine(x.MaterialUserInst.SurfacePhysicId);
                }
                if (!SurfaceGameplayIds.ContainsKey(x.MaterialUserInst.SurfaceGameplayId)) {
                    SurfaceGameplayIds.Add(x.MaterialUserInst.SurfaceGameplayId,x.MaterialUserInst.Link);
                    Console.WriteLine(x.MaterialUserInst.SurfaceGameplayId);
                }
            });
        }
        return false;
    }
    public override bool AlterGeometry(CustomBlock customBlock, CPlugCrystal.GeometryLayer layer)
    {
        layer.Crystal.Faces.ToList().ForEach(x => {
            if (!materials.ContainsKey(x.Material.MaterialUserInst.Link)) {
                materials.Add(x.Material.MaterialUserInst.Link,x.Material.MaterialUserInst.SurfacePhysicId);
                Console.WriteLine(x.Material.MaterialUserInst.Link);
            }
            if (!SurfacePhysicIds.ContainsKey(x.Material.MaterialUserInst.SurfacePhysicId)) {
                SurfacePhysicIds.Add(x.Material.MaterialUserInst.SurfacePhysicId,x.Material.MaterialUserInst.Link);
                Console.WriteLine(x.Material.MaterialUserInst.SurfacePhysicId);
            }
            if (!SurfaceGameplayIds.ContainsKey(x.Material.MaterialUserInst.SurfaceGameplayId)) {
                SurfaceGameplayIds.Add(x.Material.MaterialUserInst.SurfaceGameplayId,x.Material.MaterialUserInst.Link);
                Console.WriteLine(x.Material.MaterialUserInst.SurfaceGameplayId);
            }
        });
        return false;
    }
}