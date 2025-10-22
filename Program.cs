//Initial load

AlterationConfig.devMode = true;
AlterationConfig.devPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..","data","dev");
AlterationConfig.Load();

// ----------- Code for Execution (change for your use) ----------- //
//TODO test edited Alterations
new AlterationScript("Campaign.json").RunConfig();
// TestReEmbed.testReEmbed();

// direct Alteration
// string from = "C:/Users/Tobias/Documents/Trackmania2020/Blocks/PlatformPlasticBase.Block.Gbx";
// string to = "C:/Users/Tobias/Documents/Trackmania2020/Blocks/PlatformPlasticBaseSuperSized.Block.Gbx";
// AutoAlteration.AlterFile(new Wood(), from, to, "SuperSized");

// LightSurface Template
// AutoAlteration.AlterAll(new LightSurfaceBlock(), Path.Combine(AlterationConfig.CustomBlocksFolder, "HeavySurface"), Path.Combine("C:/Users/Tobias/Documents/Trackmania2020/Blocks", "LightSurface"), "");
// AutoAlteration.AlterAll(new LightSurfaceBlock(), Path.Combine(AlterationConfig.CustomBlocksFolder, "HeavySurface"), Path.Combine(AlterationConfig.CustomBlocksFolder, "LightSurface"), "");
