string source = "C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Mapping\\RC Racing.Map.Gbx";
Map map = new Map(source);
    EffectAlterations.FreeWheel(map);
    map.map.MapName = map.map.MapName + " FreeWheel";
    map.save("C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Mapping\\RC Racing FreeWheel.Map.Gbx");
// string sourcefolder = "C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\Nadeo Maps\\Spring 2020 alpha\\";
// string[] files = Directory.GetFiles(sourcefolder);

// foreach (string file in files)
// {
//     Map map = new Map(file);
//     EffectAlterations.FreeWheel(map);
//     map.map.MapName = map.map.MapName + " FreeWheel";
//     map.save("C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Spring 2020 FreeWheel\\" + Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 8) + " FreeWheel.map.gbx");
// }