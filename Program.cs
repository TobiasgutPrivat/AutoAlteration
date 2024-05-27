using Newtonsoft.Json;
//Initial load
// Alteration.load("C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/");
Alteration.load("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/"); //Path to this project folder

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
void stringToArticles(string projectFolder, ArticleType type) {
    string[] Keywords = File.ReadAllLines("src/Configuration/Keywords.json");
    Array.Sort(Keywords, (a, b) => b.Length.CompareTo(a.Length));
    Inventory inventory = new Inventory(projectFolder + "src/Configuration/Source.json",Keywords);
    inventory.articles.ForEach(x => x.Type = type);
    string json = JsonConvert.SerializeObject(inventory.articles);
    File.WriteAllText(projectFolder + "src/Configuration/Destination.json", json);
}