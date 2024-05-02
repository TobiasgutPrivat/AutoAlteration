string sourcefolder = "C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Mapping\\";
string[] files = Directory.GetFiles(sourcefolder);

foreach (string file in files)
{
    EffectMapHelper map = new EffectMapHelper(file);
    EffectAlterations.NoBrakes(map);
    map.map.MapName = map.map.MapName + " NoBrakes";
    map.save("C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Spring 2020 NoBrake\\" + Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 8) + " NoBrake.map.gbx");
}

// foreach (string file in files)
// {
//     Map map = new Map(file);
//     EffectAlterations.NoBrakes(map);
//     map.map.MapName = map.map.MapName + " NoBrakes";
//     map.save("C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Spring 2020 NoSteer\\" + Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 8) + " NoSteer.map.gbx");
// }