using Newtonsoft.Json;
//Initial load
Alteration.Load(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../..\..")) + "/");
// CLI.Run();

//Code for Execution (change for your use)
//Folder Processing
string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Summer 2024/";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/Summer 2024/";
// AutoAlteration.AlterAll(new NoItems(), sourcefolder, destinationFolder + "Summer 2024 NoItems/", "NoItems");

//Full Folder Processing
// string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/";
// AutoAlteration.AlterAll(new CPBoost(), sourceFolder, destinationFolder, "CP-Boost");

//All Alterations Full Folder Processing
// string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/";
// AutoAlteration.AllAlterations(sourceFolder, destinationFolder);

//Single File Processing
string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template.Map.Gbx";
// string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test CPBoost.Map.Gbx";
AutoAlteration.AlterFile(new Test(), sourceFile, "Test");

// Unvalidated
// AutoAlteration.AlterFile(new List<Alteration>{}, sourceFile, "(Unvalidated)");

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
        map.PlaceRelative(new string[] { "DecoHillSlope2ChicaneX2Left", "DecoHillDirtSlope2ChicaneX2Left" }, "DecoHillIceSlope2ChicaneX2Left", new(new(0,30,0)));
        map.Replace(new string[] { "DecoHillSlope2ChicaneX2Left", "DecoHillDirtSlope2ChicaneX2Left" }, "DecoHillIceSlope2ChicaneX2Left");
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