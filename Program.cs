using GBX.NET;
using Newtonsoft.Json;
//Initial load
Alteration.Load(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../..\..")) + "/");
CLI.Run();
// Alteration.TestInventory();
//Code for Execution (change for your use)
//Folder Processing
// string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Summer 2024/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/Summer 2024/";
// AutoAlteration.AlterFolder(new OneUP(), sourcefolder, destinationFolder + "Summer 2024 OneUP/", "OneUP");

//Full Folder Processing
// string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/";
// AutoAlteration.AlterAll(new CPBoost(), sourceFolder, destinationFolder, "CP-Boost");

//All Alterations Full Folder Processing
// string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/";
// AutoAlteration.AllAlterations(sourceFolder, destinationFolder);

//Single File Processing
// string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template.Map.Gbx";
// string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test CPBoost.Map.Gbx";
// AutoAlteration.AlterFile(new STTF(), sourceFile, "STTF");

// Unvalidated
// AutoAlteration.AlterFile(new List<Alteration>{}, sourceFile, "(Unvalidated)");

//CustomBlock
// new CustomBlock("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/src/CustomBlocks/RoadTechToThemeSnowRoadX2Magnet.Block.Gbx");

// Console.WriteLine(Directory.GetFiles(destinationFolder, "*.map.gbx", SearchOption.AllDirectories).Count());

//Development Section -----------------------------------------------------------------------------------------------------------------------
void stringToName(string projectFolder) {
    string json = File.ReadAllText(projectFolder + "src/Vanilla/Items.json");
    string[] lines = JsonConvert.DeserializeObject<string[]>(json);
    string[] articles = lines.Select(line => line.Split('/')[line.Split('/').Length-1].Trim()).ToArray();
    json = JsonConvert.SerializeObject(articles);
    File.WriteAllText(projectFolder + "src/Vanilla/ItemNames.json", json);
}

class Test : Alteration {
    public override void Run(Map map)
    {
        // map.PlaceRelative(new string[] { "DecoHillIceSlope2ChicaneX2Left" }, "PlatFormTechLoopEndCurve3In", new(new(0,30,0)));
        // map.Replace(new string[] { "DecoHillIceSlope2ChicaneX2Left" }, "PlatformGrassSlope2UTop");
        // map.map.PlaceBlock("DecoHillSlope2curve2Out", new Int3(20,10,20),Direction.North);
        // map.map.PlaceBlock("GateFinish16m", new Int3(20,12,20),Direction.North);
        map.map.PlaceAnchoredObject(new Ident("GateFinish16m", new Id(26), "Nadeo"), new Vec3(800,220,800),Vec3.Zero);
        map.map.PlaceAnchoredObject(new Ident("GateCheckpointCenter16m", new Id(26), "Nadeo"), new Vec3(800,220,800),Vec3.Zero);
        map.PlaceStagedBlocks();
    }
}

class Replace : Alteration {
    public override void Run(Map map)
    {
        inventory.Edit().Replace(map);
        map.PlaceStagedBlocks();
    }
}
class Delete : Alteration {
    public override void Run(Map map)
    {
        map.Delete(inventory);
        map.PlaceStagedBlocks();
    }
}