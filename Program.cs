using Newtonsoft.Json;
//Initial load
AutoAlteration.Load(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..")) + "/");
// CLI.Run();

// ----------- Code for Execution (change for your use) ----------- //

//Folder Processing -------------
string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Summer 2024/";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto-Altered-Nadeo/Summer 2024/";
AutoAlteration.AlterFolder(new Tech(), sourceFolder, destinationFolder + "Summer 2024 Tech/", "Tech");

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
// AutoAlteration.AlterFile(new Dirt(), sourceFile, "Dirt");

//Customblocks -------------
// string sourceFolder = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/src/CustomBlocks/Vanilla";
// string destinationFolder = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/src/CustomBlocks/Surface/LightTech";
// AutoAlteration.AlterAll(new LightTech(), sourceFolder, destinationFolder, "LightTech");

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

class Test : Alteration {
    public override void Run(Map map)
    {
        // inventory.Edit().PlaceRelative(map);
        // inventory.Select("!Ring").Select("Boost|NoEngine|Turbo|Turbo2|TurboRoulette|Fragile|NoSteering|SlowMotion|NoBrake|Cruise|Reset").RemoveKeyword(new string[] { "Boost","NoEngine","Turbo","Turbo2","TurboRoulette","Fragile","NoSteering","SlowMotion","NoBrake","Cruise","Reset","Right","Left","Down","Up" }).AddKeyword("Boost2").Replace(map);
        // inventory.Select("Ring").Select("Boost|NoEngine|Turbo|Turbo2|TurboRoulette|Fragile|NoSteering|SlowMotion|NoBrake|Cruise|Reset").RemoveKeyword(new string[] { "Boost","NoEngine","Turbo","Turbo2","TurboRoulette","Fragile","NoSteering","SlowMotion","NoBrake","Cruise","Reset","Right","Left","Down","Up" }).AddKeyword("Boost2").AddKeyword("Oriented").Replace(map);
        // map.PlaceStagedBlocks();
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