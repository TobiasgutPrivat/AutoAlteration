using GBX.NET;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
using GBX.NET.LZO;
using GBX.NET.ZLib;
using Newtonsoft.Json;
//Initial load
AutoAlteration.Load(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..")) + "/");
// AutoAlteration.Load(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory) + "/");
// CLI.Run();
// ----------- Code for Execution (change for your use) ----------- //

//Folder Processing -------------
string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Summer 2024/";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto-Altered-Nadeo/Summer 2024/";
AutoAlteration.AlterFolder(new NoEffect(), sourceFolder, destinationFolder + "Summer 2024 NoEffect/", "NoEffect");

//Full Folder Processing -------------
// string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto-Altered-Nadeo/";
// AutoAlteration.AlterAll(new CPBoost(), sourceFolder, destinationFolder, "CP-Boost");

//All Alterations Full Folder Processing -------------
// string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto-Altered-Nadeo/";
// AutoAlteration.AllAlterations(sourceFolder, destinationFolder);

//Single File Processing -------------
// string sourceFile = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/data/CustomBlocks/Vanilla/Roads/RoadBump/Slopes/SlopeU/RoadBumpSlopeUBottomX2/RoadBumpSlopeUBottomX2.Item.Gbx";
// string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/My Maps/Thread of Ariadne.Map.Gbx";
string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template.Map.Gbx";
// AutoAlteration.AlterFile(new Test(), sourceFile, "Test");

//Customblocks -------------
// string sourceFolder = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/data/CustomBlocks/Vanilla";
// string destinationFolder = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/data/CustomBlocks/InvisibleBlock";
// AutoAlteration.AlterAll(new InvisibleBlock(), sourceFolder, destinationFolder, "InvisibleBlock");

// Materials Log using MaterialInfo
// string sourceFolder = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/data/CustomBlocks/Vanilla";
// string destinationFolder = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/data/CustomBlocks/MaterialInfo";
// AutoAlteration.AlterAll(new MaterialInfo(), sourceFolder, destinationFolder, "MaterialInfo");
// File.WriteAllText(AutoAlteration.ProjectFolder + "dev/SurfacePhysicIds.json", JsonConvert.SerializeObject(MaterialInfo.SurfacePhysicIds));
// File.WriteAllText(AutoAlteration.ProjectFolder + "dev/SurfaceGameplayIds.json", JsonConvert.SerializeObject(MaterialInfo.SurfaceGameplayIds));
// File.WriteAllText(AutoAlteration.ProjectFolder + "dev/Materials.json", JsonConvert.SerializeObject(MaterialInfo.materials));

// Unvalidated -------------
// AutoAlteration.AlterFile(new List<Alteration>{}, sourceFile, "(Unvalidated)");

//Development Section -----------------------------------------------------------------------------------------------------------------------
void stringToName(string projectFolder) {
    string json = File.ReadAllText(projectFolder + "data/Vanilla/Items.json");
    string[] lines = JsonConvert.DeserializeObject<string[]>(json);
    string[] articles = lines.Select(line => line.Split('/')[^1].Trim()).ToArray();
    json = JsonConvert.SerializeObject(articles);
    File.WriteAllText(projectFolder + "data/Vanilla/ItemNames.json", json);
}

void sponsorNonColidableFix(string projectFolder) {
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
        map.PlaceRelative("PlatformTechCheckpoint","DiagWaterEnterMiniBlock",Move(0,16,0));
        map.PlaceStagedBlocks();
    }

    public override void ChangeInventory(){
        AddCustomBlocks("dev/Water");
    }
}