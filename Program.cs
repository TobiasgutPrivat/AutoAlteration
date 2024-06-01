using GBX.NET;
using Newtonsoft.Json;
//Initial load
//"C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/"
//"C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/"
//"blockChange": null -> "blockChange":{"absolutePosition":{"X":0.0,"Y":0.0,"Z":0.0},"pitchYawRoll":{"X":0.0,"Y":0.0,"Z":0.0}}
Alteration.load("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");
createInventory("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");

// testBlock("RoadIceDiagRightToRoadIceWithWallDiagLeft");
// createInventory("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");

//Code for Execution (change for your use)
//Folder Processing
string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2024/";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Nadeo/Spring 2024/";
// Alteration.alterFolder(new STTF(), sourcefolder, destinationFolder + "Spring 2024 STTF/", "STTF");

// //Single File Processing
string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template.Map.Gbx";
string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test STTF.Map.Gbx";
Alteration.alterFile(new STTF(), sourceFile, destinationFile, "STTF");


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

void createInventory(string projectFolder) {
    Alteration.devMode = true;
    Inventory items = Alteration.importArrayInventory(projectFolder + "src/Vanilla/ItemNames.json");
    items.articles.ForEach(x => x.Type = BlockType.Item);
    Inventory blocks = Alteration.importArrayInventory(projectFolder + "src/Vanilla/BlockNames.json");
    blocks.articles.ForEach(x => x.Type = BlockType.Block);
    blocks.select("Gate").editOriginal().remove("Gate").add("Ring");

    Inventory inventory = Alteration.inventory;
    inventory.articles.Clear();
    inventory.articles.AddRange(items.articles);
    inventory.articles.AddRange(blocks.articles);

    inventory.select("Checkpoint").remove("Checkpoint").remove("Left").remove("Right").remove("Up").remove("Down").add("Straight").align().editOriginal().remove("Straight");
    inventory.select("Checkpoint").remove("Checkpoint").remove("Up").add("Straight4").align().editOriginal().remove("Straight4");
    inventory.select("Checkpoint").remove("Checkpoint").add("StraightX2").align().editOriginal().remove("StraightX2");
    inventory.select("Checkpoint").remove("Checkpoint").remove("Left").remove("Right").remove("Up").remove("Down").add("Base").align().editOriginal().remove("Base").print();
    inventory.select("Special").editOriginal().remove("Special");
    // inventory.select("Start&!(Slope2|Loop|DiagRight|DiagLeft|Slope|Inflatable)").editOriginal().remove("Start").add("MapStart");
    
    // inventory.checkDuplicates();

    inventory.articles.ForEach(x => x.cacheFilter.Clear());
    string json = JsonConvert.SerializeObject(inventory.articles);
    File.WriteAllText(projectFolder + "src/Inventory.json", json);
}