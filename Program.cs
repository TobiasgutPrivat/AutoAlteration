//Code for Execution (change for your use)

//Test Readembedded
// string source = "C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Altered Nadeo\\Spring 2020 Test\\s01 Spring 2020 Test.map.gbx";
// Map map = new Map(source);
// foreach(System.IO.Compression.ZipArchiveEntry entry in map.map.OpenReadEmbeddedZipData().Entries){
// Console.WriteLine(entry.FullName);  
// }

//CPLess
// string sourcefolder = "C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\Nadeo Maps\\Spring 2020 alpha\\";
// string destinationFolder = "C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Altered Nadeo\\Spring 2020 CPLess\\";
// string[] files = Directory.GetFiles(sourcefolder);
// foreach (string file in files)
// {
//     Map map = new Map(file);
//     CPAlterations.CPLess(map);
//     map.map.MapName = map.map.MapName + " CPLess";
//     map.save(destinationFolder + Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 8) + " CPLess.map.gbx");
// }

//Test BakedBlocks
// string source = "C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\Nadeo Maps\\Spring 2020 alpha\\s14 Spring 2020.map.gbx";
// string destination = "C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Altered Nadeo\\Spring 2020 CPLess\\s14 Spring 2020 CPLess.map.gbx";
// Map map = new Map(source);
// CPAlterations.CPLess(map);
// map.map.MapName = map.map.MapName + " CPLess";
// map.save(destinationFolder + Path.GetFileName(source).Substring(0, Path.GetFileName(source).Length - 8) + " CPLess.map.gbx");
