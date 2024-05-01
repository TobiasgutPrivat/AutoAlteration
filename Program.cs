﻿string sourcefolder = "C:\\Users\\Tobias\\Documents\\Programmieren\\GBX Test\\AutoAlteration\\Maps\\MX-175_spring-2020-alpha (1)\\";
string[] files = Directory.GetFiles(sourcefolder);

foreach (string file in files)
{
    Map map = new Map(file);
    EffectAlterations.NoBrakes(map);
    map.map.MapName = map.map.MapName + " NoBrakes";
    map.save("C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Spring 2020 NoBrake\\" + Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 8) + " NoBrake.map.gbx");
}