using GBX.NET;
using Newtonsoft.Json;
//Initial load
//"C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/"
//"C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/"
//"blockChange": null -> "blockChange":{"absolutePosition":{"X":0.0,"Y":0.0,"Z":0.0},"pitchYawRoll":{"X":0.0,"Y":0.0,"Z":0.0}}
Alteration.load("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");
// Alteration.createInventory("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");

// testBlock("RoadIceDiagRightToRoadIceWithWallDiagLeft");
// createInventory("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");

//Code for Execution (change for your use)
//Folder Processing
string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2024/";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Nadeo/Spring 2024/";
Alteration.alterFolder(new Surfaceless(), sourcefolder, destinationFolder + "Spring 2024 Surfaceless/", "Surfaceless");

// //Single File Processing
string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test STTF.Map.Gbx";
string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test CPFull.Map.Gbx";
// Alteration.alterFile(new CPFull(), sourceFile, destinationFile, "CPFull");


//Development Section -----------------------------------------------------------------------------------------------------------------------
void testBlock(string Name) {
    Map map = new Map("C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template.Map.Gbx");
    map.map.PlaceBlock(Name, new Int3(10, 10, 10), Direction.North);
    map.map.PlaceAnchoredObject(new Ident(Name, new Id(26), "Nadeo"), new Int3(320, 80, 320), new Vec3(0,0,0));
    map.save("C:/Users/Tobias/Documents/Trackmania2020/Maps/Test.Map.Gbx");
}
void stringToName(string projectFolder) {
    string json = File.ReadAllText(projectFolder + "src/Vanilla/Items.json");
    string[] lines = JsonConvert.DeserializeObject<string[]>(json);
    string[] articles = lines.Select(line => line.Split('/')[line.Split('/').Length-1].Trim()).ToArray();
    json = JsonConvert.SerializeObject(articles);
    File.WriteAllText(projectFolder + "src/Vanilla/ItemNames.json", json);
}