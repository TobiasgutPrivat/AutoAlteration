//Code for Execution (change for your use)

string sourcefolder = "C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\Nadeo Maps\\Spring 2020 alpha\\";
string destinationFolder = "C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Altered Nadeo\\Spring 2020 NoSteer\\";
string[] files = Directory.GetFiles(sourcefolder);
foreach (string file in files)
{
    Map map = new Map(file);
    EffectAlterations.NoSteer(map);
    map.map.MapName = map.map.MapName + " NoSteer";
    map.save(destinationFolder + Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 8) + " NoSteer.map.gbx");
}