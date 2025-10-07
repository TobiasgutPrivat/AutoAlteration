//Initial load

AlterationConfig.devMode = true;
AlterationConfig.devPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..","data","dev");
AlterationConfig.Load();

// ----------- Code for Execution (change for your use) ----------- //
//TODO test edited Alterations
new AlterationScript ("Campaign.json").RunConfig();

AutoAlteration.AlterFile(new Tilted(new GBX.NET.Vec3(0,0,0)),"SourceFile","DestinationFile","Tilted");

// LightSurface Template
// AutoAlteration.AlterAll(new LightSurfaceBlock(), Path.Combine(AlterationConfig.CustomBlocksFolder, "HeavySurface"), Path.Combine("C:/Users/Tobias/Documents/Trackmania2020/Blocks", "LightSurface"), "");
// AutoAlteration.AlterAll(new LightSurfaceBlock(), Path.Combine(AlterationConfig.CustomBlocksFolder, "HeavySurface"), Path.Combine(AlterationConfig.CustomBlocksFolder, "LightSurface"), "");
