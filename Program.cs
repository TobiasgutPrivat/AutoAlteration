using Newtonsoft.Json;
//Initial load
//"C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/"
//"C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/"
//"blockChange": null -> "blockChange":{"absolutePosition":{"X":0.0,"Y":0.0,"Z":0.0},"pitchYawRoll":{"X":0.0,"Y":0.0,"Z":0.0}}
// Alteration.load("C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/");
// Alteration.inventory.checkInventory();

stringToArticles("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");
//Code for Execution (change for your use)

//Folder Processing
// string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2024/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Nadeo/Spring 2024/";
// AutoAlteration.alterFolder(new FreeWheel(), sourcefolder, destinationFolder + "Spring 2024 FreeWheel temp/", "FreeWheel");

// //Single File Processing
// string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2020 alpha/Spring 2020 - S12.map.gbx";
// string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/My-Maps/Altered Nadeo/Spring 2020 CPFull/t01 Spring 2020 CPFull.map.gbx";
// AutoAlteration.alterFile(new FreeWheel(), sourceFile, destinationFile, "FreeWheel");


//Function to convert list of block names to Serializable Articles
void stringToArticles(string projectFolder) {
    string[] Keywords = File.ReadAllLines(projectFolder + "src/Vanilla/Keywords.txt");
    Array.Sort(Keywords, (a, b) => b.Length.CompareTo(a.Length));
    Inventory inventory = new Inventory(projectFolder + "src/Vanilla/Items.json",Keywords);
    inventory.articles.ForEach(x => x.Type = BlockType.Item);
    string json = JsonConvert.SerializeObject(inventory.articles);
    File.WriteAllText(projectFolder + "src/Vanilla/Destination.json", json);
}
void stringToName(string projectFolder) {
    string json = File.ReadAllText(projectFolder + "src/Vanilla/Items.json");
    string[] lines = JsonConvert.DeserializeObject<string[]>(json);
    string[] articles = lines.Select(line => line.Split('/')[line.Split('/').Length-1].Trim()).ToArray();
    json = JsonConvert.SerializeObject(articles);
    File.WriteAllText(projectFolder + "src/Vanilla/ItemNames.json", json);
}