﻿//Initial load

AltertionConfig.devMode = true;
AltertionConfig.devPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..","data","dev");
AltertionConfig.Load();

// ----------- Code for Execution (change for your use) ----------- //
new AlterationScript ("TMNF.json").RunConfig();

//TODO LightSurface Template
// AutoAlteration.AlterAll(new LightWood(), Path.Combine(AltertionConfig.CustomBlocksFolder, "HeavySurface"), Path.Combine(AltertionConfig.CustomBlocksFolder, "LightSurface"), "LightWood");

// DevUtils.LogMaterialInfo("C:/Users/Tobias/AppData/Roaming/AutoAlteration/HeavyPlastic");
