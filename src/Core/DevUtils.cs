using GBX.NET;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
using GBX.NET.LZO;
using GBX.NET.ZLib;
using Newtonsoft.Json;

class DevUtils{
    public static void ResaveBlock(string BlockPath) {
        Gbx.LZO = new MiniLZO();
        Gbx.ZLib = new ZLib();
        CGameItemModel customBlock = Gbx.Parse<CGameItemModel>(BlockPath);
        customBlock.Save(BlockPath + "resaved.block.gbx");
    }

    public static void LogMaterialInfo() {
        string sourceFolder = Path.Combine(AltertionConfig.DataFolder, "CustomBlocks/Vanilla");
        string destinationFolder = Path.Combine(AltertionConfig.DataFolder, "CustomBlocks/MaterialInfo");
        AutoAlteration.AlterAll([new MaterialInfo()], sourceFolder, destinationFolder, "MaterialInfo");
        File.WriteAllText(Path.Combine(AltertionConfig.devPath, "SurfacePhysicIds.json"), JsonConvert.SerializeObject(MaterialInfo.SurfacePhysicIds));
        File.WriteAllText(Path.Combine(AltertionConfig.devPath, "SurfaceGameplayIds.json"), JsonConvert.SerializeObject(MaterialInfo.SurfaceGameplayIds));
        File.WriteAllText(Path.Combine(AltertionConfig.devPath, "Materials.json"), JsonConvert.SerializeObject(MaterialInfo.materials));
    }

    public static void StringToName(string projectFolder) {
        string json = File.ReadAllText(projectFolder + "data/Vanilla/Items.json");
        SList<string> lines = JsonConvert.DeserializeObject<SList<string>>(json);
        SList<string> articles = lines.Select(line => line.Split('/')[^1].Trim()).ToList();
        json = JsonConvert.SerializeObject(articles);
        File.WriteAllText(projectFolder + "data/Vanilla/ItemNames.json", json);
    }

    public static void NonColidableFix() {
        foreach (string file in Directory.GetFiles(Path.Join(AltertionConfig.CustomBlocksFolder, "Vanilla"), "*", SearchOption.AllDirectories))
        {
            Gbx.LZO = new MiniLZO();
            // Gbx.ZLib = new ZLib();
            CustomBlock customBlock = new(file);
            bool changed = false;
            customBlock.MeshCrystals.ForEach(x => {
                //if there are no collidable layers, make first one collidable
                if (!x.Layers.Any(y => y.GetType() == typeof(CPlugCrystal.GeometryLayer) && ((CPlugCrystal.GeometryLayer)y).Collidable)){ 
                    CPlugCrystal.GeometryLayer layer = (CPlugCrystal.GeometryLayer)x.Layers.Where(y => y.GetType() == typeof(CPlugCrystal.GeometryLayer)).First();
                    layer.Collidable = true;
                    changed = true;
                }
            });
            if (changed){
                customBlock.Save(file);
            }
        }
    }

    public static void TestInventory(){
        Alteration.CreateInventory();
        new CustomBlockFolder("").ChangeInventory(Alteration.inventory);
        new NoCPBlocks().ChangeInventory(Alteration.inventory);
        new CheckpointTrigger().ChangeInventory(Alteration.inventory);
        Alteration.inventory.CheckDuplicates();
        Alteration.inventory.articles.ForEach(x => {
            if (x.Keywords.Any(y => y == "")){Console.WriteLine("Empty Keyword found in " + x.Name);}
            if (x.ToShapes.Any(y => y == "")){Console.WriteLine("Empty ToShape found in " + x.Name);}
        });
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
        inventory.Edit().Replace(map,Move(16,0,0));//Test if Rotation is before or after Move
        map.PlaceStagedBlocks();
    }
}

class Nothing : Alteration {
    public override void Run(Map map)
    {
    }
}