//Initial load

AlterationConfig.devMode = true;
AlterationConfig.devPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..","data","dev");
AlterationConfig.Load();

// ----------- Code for Execution (change for your use) ----------- //
//TODO test edited Alterations
new AlterationScript ("Campaign.json").RunConfig();

//TODO LightSurface Template
// AutoAlteration.AlterAll(new LightSurfaceBlock(), Path.Combine(AltertionConfig.CustomBlocksFolder, "HeavySurface"), Path.Combine(AltertionConfig.CustomBlocksFolder, "LightSurface"), "");
