string sourcefolder = "C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\Nadeo Maps\\Spring 2020 alpha\\";
string[] files = Directory.GetFiles(sourcefolder);

foreach (string file in files)
{
    Map map = new Map(file);
    Alterations.CPBoost(map);
    map.map.MapName = map.map.MapName + " CP/Boost";
    map.save("C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Altered Nadeo\\Spring 2020 CPBoost\\" + Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 8) + " CPBoost.map.gbx");
}