using Newtonsoft.Json;
//Initial load
//"C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/"
//"C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/"
Alteration.load("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");
// Alteration.inventory.checkDuplicates();//TODO
// Alteration.inventory.analyzeKeywords();

//Code for Execution (change for your use)
//Folder Processing
// string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2024/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/Spring 2024/";
// Alteration.alterFolder(new STTF(), sourcefolder, destinationFolder + "Spring 2024 STTF/", "STTF");

//Full Folder Processing
string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/";
// Alteration.alterAll(new CPBoost(), sourceFolder, destinationFolder, "CP-Boost");

//All Alterations Full Folder Processing
// string sourceFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/";
// AllAlterations(sourceFolder, destinationFolder);

// //Single File Processing
string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template.Map.Gbx";
// string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test CPBoost.Map.Gbx";
// Alteration.alterFile(new FreeWheel(), sourceFile, "FreeWheel");

void AllAlterations(string sourceFolder, string destinationFolder) {
    Alteration.alterAll(new Stadium(), sourceFolder, destinationFolder, "Stadium");
    Alteration.alterAll(new Snow(), sourceFolder, destinationFolder, "Snow");
    Alteration.alterAll(new Rally(), sourceFolder, destinationFolder, "Rally");
    Alteration.alterAll(new Desert(), sourceFolder, destinationFolder, "Desert");

    Alteration.alterAll(new NoBrake(), sourceFolder, destinationFolder, "NoBrake");
    Alteration.alterAll(new Cruise(), sourceFolder, destinationFolder, "Cruise");
    Alteration.alterAll(new Fragile(), sourceFolder, destinationFolder, "Fragile");
    Alteration.alterAll(new SlowMo(), sourceFolder, destinationFolder, "SlowMo");
    Alteration.alterAll(new NoSteer(), sourceFolder, destinationFolder, "NoSteer");
    Alteration.alterAll(new Glider(), sourceFolder, destinationFolder, "Glider");
    Alteration.alterAll(new Reactor(), sourceFolder, destinationFolder, "Reactor");
    Alteration.alterAll(new ReactorDown(), sourceFolder, destinationFolder, "ReactorDown");
    Alteration.alterAll(new FreeWheel(), sourceFolder, destinationFolder, "FreeWheel");
    
    Alteration.alterAll(new CPBoost(), sourceFolder, destinationFolder, "CP-Boost");
    Alteration.alterAll(new STTF(), sourceFolder, destinationFolder, "STTF");
    Alteration.alterAll(new CPFull(), sourceFolder, destinationFolder, "CPFull");
    // Alteration.alterAll(new CPLess(), sourceFolder, destinationFolder, "CPLess");

    Alteration.alterAll(new OneUP(), sourceFolder, destinationFolder, "(1-UP)");
    Alteration.alterAll(new OneDown(), sourceFolder, destinationFolder, "(1-Down)");
    Alteration.alterAll(new OneLeft(), sourceFolder, destinationFolder, "(1-Left)");
    Alteration.alterAll(new OneRight(), sourceFolder, destinationFolder, "(1-Right)");
    Alteration.alterAll(new TwoUP(), sourceFolder, destinationFolder, "(2-UP)");

    Alteration.alterAll(new YepTree(), sourceFolder, destinationFolder, "YepTree");

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