using Newtonsoft.Json;
//Initial load
Alteration.Load(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..")) + "/");

//Code for Execution (change for your use)
//Folder Processing
string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Summer 2024/";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/Summer 2024/";
// Alteration.AlterFolder(new FreeWheel(), sourcefolder, destinationFolder + "Summer 2024 FreeWheel/", "FreeWheel");

//Full Folder Processing
// string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/";
// Alteration.AlterAll(new CPBoost(), sourceFolder, destinationFolder, "CP-Boost");

//All Alterations Full Folder Processing
// string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/";
// AllAlterations(sourceFolder, destinationFolder);

// //Single File Processing
string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template.Map.Gbx";
// string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test CPBoost.Map.Gbx";
Alteration.AlterFile(new Test(), sourceFile, "Test");

void AllAlterations(string sourceFolder, string destinationFolder) {
    Alteration.AlterAll(new Stadium(), sourceFolder, destinationFolder, "Stadium");
    Alteration.AlterAll(new Snow(), sourceFolder, destinationFolder, "Snow");
    Alteration.AlterAll(new Rally(), sourceFolder, destinationFolder, "Rally");
    Alteration.AlterAll(new Desert(), sourceFolder, destinationFolder, "Desert");

    Alteration.AlterAll(new NoBrake(), sourceFolder, destinationFolder, "NoBrake");
    Alteration.AlterAll(new Cruise(), sourceFolder, destinationFolder, "Cruise");
    Alteration.AlterAll(new Fragile(), sourceFolder, destinationFolder, "Fragile");
    Alteration.AlterAll(new SlowMo(), sourceFolder, destinationFolder, "SlowMo");
    Alteration.AlterAll(new NoSteer(), sourceFolder, destinationFolder, "NoSteer");
    Alteration.AlterAll(new Glider(), sourceFolder, destinationFolder, "Glider");
    Alteration.AlterAll(new Reactor(), sourceFolder, destinationFolder, "Reactor");
    Alteration.AlterAll(new ReactorDown(), sourceFolder, destinationFolder, "ReactorDown");
    Alteration.AlterAll(new FreeWheel(), sourceFolder, destinationFolder, "FreeWheel");
    Alteration.AlterAll(new AntiBooster(), sourceFolder, destinationFolder, "AntiBooster");
    
    Alteration.AlterAll(new CPBoost(), sourceFolder, destinationFolder, "CP-Boost");
    Alteration.AlterAll(new STTF(), sourceFolder, destinationFolder, "STTF");
    Alteration.AlterAll(new CPFull(), sourceFolder, destinationFolder, "CPFull");
    // Alteration.alterAll(new CPLess(), sourceFolder, destinationFolder, "CPLess");

    Alteration.AlterAll(new OneUP(), sourceFolder, destinationFolder, "(1-UP)");
    Alteration.AlterAll(new OneDown(), sourceFolder, destinationFolder, "(1-Down)");
    Alteration.AlterAll(new OneLeft(), sourceFolder, destinationFolder, "(1-Left)");
    Alteration.AlterAll(new OneRight(), sourceFolder, destinationFolder, "(1-Right)");
    Alteration.AlterAll(new TwoUP(), sourceFolder, destinationFolder, "(2-UP)");

    Alteration.AlterAll(new YepTree(), sourceFolder, destinationFolder, "YepTree");

    Console.WriteLine("Done!");
    Console.WriteLine("Map Count: " + Alteration.mapCount);
}
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
        // inventory.Edit().PlaceRelative(map);
        map.Delete(inventory);
        map.PlaceStagedBlocks();
    }
}