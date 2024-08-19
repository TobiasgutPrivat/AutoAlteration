using Newtonsoft.Json;
//Initial load
AutoAlteration.Load(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..")) + "/");
// CLI.Run();

// ----------- Code for Execution (change for your use) ----------- //

//Folder Processing -------------
string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Summer 2024/";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto-Altered-Nadeo/Summer 2024/";
AutoAlteration.AlterFolder(new Grass(), sourceFolder, destinationFolder + "Summer 2024 Grass/", "Grass");

//Full Folder Processing -------------
// string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto-Altered-Nadeo/";
// AutoAlteration.AlterAll(new CPBoost(), sourceFolder, destinationFolder, "CP-Boost");

//All Alterations Full Folder Processing -------------
// string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto-Altered-Nadeo/";
// AutoAlteration.AllAlterations(sourceFolder, destinationFolder);

//Single File Processing -------------
// string sourceFile = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/src/CustomBlocks/Vanilla/Roads/RoadBump/Slopes/SlopeU/RoadBumpSlopeUBottomX2/RoadBumpSlopeUBottomX2.Item.Gbx";
// string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/My Maps/Thread of Ariadne.Map.Gbx";
string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template.Map.Gbx";
// AutoAlteration.AlterFile(new Test(), sourceFile, "Test");

//Customblocks -------------
// string sourceFolder = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/src/CustomBlocks/Vanilla";
// string destinationFolder = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/src/CustomBlocks/Surface/LightGrass";
// AutoAlteration.AlterAll(new LightGrass(), sourceFolder, destinationFolder, "LightGrass");

// Materials Log using MaterialInfo
// string sourceFolder = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/src/CustomBlocks/Vanilla";
// string destinationFolder = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/src/CustomBlocks/MaterialInfo";
// AutoAlteration.AlterAll(new MaterialInfo(), sourceFolder, destinationFolder, "MaterialInfo");
// File.WriteAllText(AutoAlteration.ProjectFolder + "src/Inventory/SurfacePhysicIds.json", JsonConvert.SerializeObject(MaterialInfo.SurfacePhysicIds));
// File.WriteAllText(AutoAlteration.ProjectFolder + "src/Inventory/SurfaceGameplayIds.json", JsonConvert.SerializeObject(MaterialInfo.SurfaceGameplayIds));
// File.WriteAllText(AutoAlteration.ProjectFolder + "src/Inventory/Materials.json", JsonConvert.SerializeObject(MaterialInfo.materials));

// Unvalidated -------------
// AutoAlteration.AlterFile(new List<Alteration>{}, sourceFile, "(Unvalidated)");

//Development Section -----------------------------------------------------------------------------------------------------------------------
void stringToName(string projectFolder) {
    string json = File.ReadAllText(projectFolder + "src/Vanilla/Items.json");
    string[] lines = JsonConvert.DeserializeObject<string[]>(json);
    string[] articles = lines.Select(line => line.Split('/')[^1].Trim()).ToArray();
    json = JsonConvert.SerializeObject(articles);
    File.WriteAllText(projectFolder + "src/Vanilla/ItemNames.json", json);
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