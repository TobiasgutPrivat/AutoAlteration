using GBX.NET;
using Newtonsoft.Json;
//Initial load
Alteration.Load(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../..\..")) + "/");
// CLI.Run();
// Alteration.TestInventory();
//Code for Execution (change for your use)
//Folder Processing
string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Summer 2024/";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/Summer 2024/";
// AutoAlteration.AlterFolder(new Flipped(), sourcefolder, destinationFolder + "Summer 2024 Flipped/", "Flipped");

//Full Folder Processing
// string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/";
// AutoAlteration.AlterAll(new CPBoost(), sourceFolder, destinationFolder, "CP-Boost");

//All Alterations Full Folder Processing
// string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/";
// AutoAlteration.AllAlterations(sourceFolder, destinationFolder);

//Single File Processing
string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/My Maps/Deep_Dip_2r1.Map.Gbx";
// string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template FreeWheel.Map.Gbx";
// string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Summer 2024/Summer 2024 - 18.Map.Gbx";
// AutoAlteration.AlterFile(new List<Alteration>{new FreeWheel()}, sourceFile, "FreeWheel");
AutoAlteration.AlterFile(new List<Alteration>{new Test()}, sourceFile, "Yeet");

// Unvalidated
// AutoAlteration.AlterFile(new List<Alteration>{}, sourceFile, "(Unvalidated)");

//CustomBlock
// new CustomBlock("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/src/CustomBlocks/RoadTechToThemeSnowRoadX2Magnet.Block.Gbx");

// Console.WriteLine(Directory.GetFiles(destinationFolder, "*.map.gbx", SearchOption.AllDirectories).Count());

//Development Section -----------------------------------------------------------------------------------------------------------------------
void stringToName(string projectFolder) {
    string json = File.ReadAllText(projectFolder + "src/Vanilla/Items.json");
    string[] lines = JsonConvert.DeserializeObject<string[]>(json);
    string[] articles = lines.Select(line => line.Split('/')[^1].Trim()).ToArray();
    json = JsonConvert.SerializeObject(articles);
    File.WriteAllText(projectFolder + "src/Vanilla/ItemNames.json", json);
}

class Test : Alteration {
    public override void Run(Map map)
    {
        // inventory.Edit().PlaceRelative(map);
        inventory.Select("!Ring").Select("Boost|NoEngine|Turbo|Turbo2|TurboRoulette|Fragile|NoSteering|SlowMotion|NoBrake|Cruise|Reset").RemoveKeyword(new string[] { "Boost","NoEngine","Turbo","Turbo2","TurboRoulette","Fragile","NoSteering","SlowMotion","NoBrake","Cruise","Reset","Right","Left","Down","Up" }).AddKeyword("Boost2").Replace(map);
        inventory.Select("Ring").Select("Boost|NoEngine|Turbo|Turbo2|TurboRoulette|Fragile|NoSteering|SlowMotion|NoBrake|Cruise|Reset").RemoveKeyword(new string[] { "Boost","NoEngine","Turbo","Turbo2","TurboRoulette","Fragile","NoSteering","SlowMotion","NoBrake","Cruise","Reset","Right","Left","Down","Up" }).AddKeyword("Boost2").AddKeyword("Oriented").Replace(map);
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