//Code for Execution (change for your use)

//Folder Processing
string sourcefolder = "C:/Users\\Tobias\\Documents\\Trackmania2020\\Maps\\Nadeo Maps\\Spring 2020 alpha\\";
string destinationFolder = "C:/Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Altered Nadeo\\Spring 2020 FreeWheel\\";
string[] files = Directory.GetFiles(sourcefolder);
foreach (string file in files)
{
    Map map = new Map(file);
    EffectAlterations.FreeWheel(map);
    map.map.MapName = Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 8) + " FreeWheel";
    map.save(destinationFolder + Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 8) + " FreeWheel.map.gbx");
}

// Test BakedBlocks
// string source = "C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\Nadeo Maps\\Spring 2020 alpha\\s14 Spring 2020.map.gbx";
// string destination = "C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Altered Nadeo\\Spring 2020 CPLess\\s14 Spring 2020 CPLess.map.gbx";
// Map map = new Map(source);
// CPAlterations.CPLess(map);
// map.map.MapName = map.map.MapName + " CPLess";
// map.save(destination + Path.GetFileName(source).Substring(0, Path.GetFileName(source).Length - 8) + " CPLess.map.gbx");
