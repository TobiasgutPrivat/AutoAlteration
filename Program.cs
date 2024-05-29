using GBX.NET;
using Newtonsoft.Json;
//Initial load
//"C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/"
//"C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/"
//"blockChange": null -> "blockChange":{"absolutePosition":{"X":0.0,"Y":0.0,"Z":0.0},"pitchYawRoll":{"X":0.0,"Y":0.0,"Z":0.0}}

createInventory("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");
// testBlock("DecoHillIceDeadendSlope2Curve1OutLeft");
// Alteration.load("C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/");
// Alteration.inventory.checkInventory();

//Code for Execution (change for your use)
//Folder Processing
// string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2024/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Nadeo/Spring 2024/";
// AutoAlteration.alterFolder(new FreeWheel(), sourcefolder, destinationFolder + "Spring 2024 FreeWheel temp/", "FreeWheel");

// //Single File Processing
// string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2020 alpha/Spring 2020 - S12.map.gbx";
// string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/My-Maps/Altered Nadeo/Spring 2020 CPFull/t01 Spring 2020 CPFull.map.gbx";
// AutoAlteration.alterFile(new FreeWheel(), sourceFile, destinationFile, "FreeWheel");


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
    string[] Keywords = File.ReadAllLines(projectFolder + "src/Vanilla/Keywords.txt");
    Array.Sort(Keywords, (a, b) => b.Length.CompareTo(a.Length));
    Inventory items = Alteration.importArrayInventory(projectFolder + "src/Vanilla/ItemNames.json",Keywords);
    items.articles.ForEach(x => x.Type = BlockType.Item);
    Inventory blocks = Alteration.importArrayInventory(projectFolder + "src/Vanilla/BlockNames.json",Keywords);
    blocks.articles.ForEach(x => x.Type = BlockType.Block);
    Inventory inventory = new Inventory();
    inventory.articles.AddRange(items.articles);
    inventory.articles.AddRange(blocks.articles);

    inventory.select("Chicane&X2").changeKeywords(new string[] { "X2" }, new string[] { "Scale2" });
    inventory.select("Chicane&X3").changeKeywords(new string[] { "X3" }, new string[] { "Scale3" });
    inventory.select("Deco&Hill&!Cliff").changeKeywords(new string[] { "Slope","2" }, new string[] { "Scale3" });
    inventory.checkDuplicates();

    string json = JsonConvert.SerializeObject(inventory.articles);
    File.WriteAllText(projectFolder + "src/Inventory.json", json);
}