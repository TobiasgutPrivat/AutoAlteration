//Code for Execution (change for your use)
Inventory inventory = new Inventory("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/src/Inventory/Vanilla Inventory.txt","C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/src/Inventory/Keywords.txt");
string[] keywords = {"Road","Tech"};
inventory.GetArticlesWithAllKeywords(keywords).Select(a => a.Name).ToList().ForEach(Console.WriteLine);

//Folder Processing
// string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2024/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Nadeo/Spring 2024/Spring 2020 FreeWheel temp/";
// string[] files = Directory.GetFiles(sourcefolder);
// foreach (string file in files)
// {
//     Map map = new Map(file);
//     EffectAlterations.FreeWheel(map);
//     map.map.MapName = Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 8) + " FreeWheel";
//     map.save(destinationFolder + Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 8) + " FreeWheel.map.gbx");
// }

// Test Single Map
// string source = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2020 alpha/Spring 2020 - S12.map.gbx";
// string destination = "C:/Users/Tobias/Documents/Trackmania2020/Maps/My-Maps/Altered Nadeo/Spring 2020 CPFull/t01 Spring 2020 CPFull.map.gbx";
// Map map = new Map(source);
// CPAlterations.CPFull(map);
// map.map.MapName = map.map.MapName + " CPFull";
// map.save(destination + Path.GetFileName(source).Substring(0, Path.GetFileName(source).Length - 8) + " CPFull.map.gbx");