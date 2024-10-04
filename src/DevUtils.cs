using GBX.NET;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
using GBX.NET.LZO;
using GBX.NET.ZLib;
using Newtonsoft.Json;

class DevUtils{
    public static void CombineItemPrefabs(string ItemPath, string PrefabPath) {
        // TODO
        // Ents[0].Model.Shape.Surf.Vertices/Triangles
        // To
        // entitymodeledition.MeshCrystal.Layers[0].Crystal.Positions/Faces
    }

    public static void resaveBlock(string BlockPath) {
        Gbx.LZO = new MiniLZO();
        Gbx.ZLib = new ZLib();
        CGameItemModel customBlock = Gbx.Parse<CGameItemModel>(BlockPath);
        customBlock.Save(BlockPath + "resaved.block.gbx");
    }

    public static void LogMaterialInfo() {
        string sourceFolder = Path.Combine(AutoAlteration.DataFolder, "CustomBlocks/Vanilla");
        string destinationFolder = Path.Combine(AutoAlteration.DataFolder, "CustomBlocks/MaterialInfo");
        AutoAlteration.AlterAll(new MaterialInfo(), sourceFolder, destinationFolder, "MaterialInfo");
        File.WriteAllText(Path.Combine(AutoAlteration.devPath, "SurfacePhysicIds.json"), JsonConvert.SerializeObject(MaterialInfo.SurfacePhysicIds));
        File.WriteAllText(Path.Combine(AutoAlteration.devPath, "SurfaceGameplayIds.json"), JsonConvert.SerializeObject(MaterialInfo.SurfaceGameplayIds));
        File.WriteAllText(Path.Combine(AutoAlteration.devPath, "Materials.json"), JsonConvert.SerializeObject(MaterialInfo.materials));
    }

    public static void stringToName(string projectFolder) {
        string json = File.ReadAllText(projectFolder + "data/Vanilla/Items.json");
        string[] lines = JsonConvert.DeserializeObject<string[]>(json);
        string[] articles = lines.Select(line => line.Split('/')[^1].Trim()).ToArray();
        json = JsonConvert.SerializeObject(articles);
        File.WriteAllText(projectFolder + "data/Vanilla/ItemNames.json", json);
    }

    public static void sponsorNonColidableFix(string projectFolder) {
        foreach (string file in Directory.GetFiles(projectFolder + "data/CustomBlocks/Vanilla", "*", SearchOption.AllDirectories))
        {
            if (!file.Contains("Sponsors")){
                continue;
            }
            Gbx.LZO = new MiniLZO();
            Gbx.ZLib = new ZLib();
            CGameItemModel customBlock = Gbx.Parse<CGameItemModel>(file);
            CGameCommonItemEntityModelEdition Item = (CGameCommonItemEntityModelEdition)customBlock.EntityModelEdition;
            CPlugCrystal.GeometryLayer layer = (CPlugCrystal.GeometryLayer)Item.MeshCrystal.Layers.Where(x => x is CPlugCrystal.GeometryLayer layer && !layer.Collidable && layer.IsVisible).First();
            Console.WriteLine(layer.LayerName);
            layer.Collidable = true;
            customBlock.Save(file);
        }
    }
}

class Replace : Alteration {
    public override void Run(Map map)
    {
        inventory.Edit().Replace(map);
        map.PlaceStagedBlocks();
    }
}

class Test : Alteration {
    public override void Run(Map map)
    {
        map.PlaceStagedBlocks();
    }

    public override void ChangeInventory(){
        AddCustomBlocks("dev/Water");
    }
}