//Initial load

AlterationConfig.devMode = true;
AlterationConfig.devPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..","data","dev");
AlterationConfig.Load();

// ----------- Code for Execution (change for your use) ----------- //
//TODO test edited Alterations
new AlterationScript("Campaign.json").RunConfig();

// direct Alteration
// string from = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Under10";
// string to = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Under10/Tilted";
// AutoAlteration.AlterFolder(new Tilted(new GBX.NET.Vec3(0,0.3926f,0)),from,to+"1","Tilted");

// LightSurface Template
// AutoAlteration.AlterAll(new LightSurfaceBlock(), Path.Combine(AlterationConfig.CustomBlocksFolder, "HeavySurface"), Path.Combine("C:/Users/Tobias/Documents/Trackmania2020/Blocks", "LightSurface"), "");
// AutoAlteration.AlterAll(new LightSurfaceBlock(), Path.Combine(AlterationConfig.CustomBlocksFolder, "HeavySurface"), Path.Combine(AlterationConfig.CustomBlocksFolder, "LightSurface"), "");
