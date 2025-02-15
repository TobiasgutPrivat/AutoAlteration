//Initial load

AltertionConfig.devMode = true;
AltertionConfig.devPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..","data","dev");
AltertionConfig.Load();

// ----------- Code for Execution (change for your use) ----------- //
new AlterationScript ("Test.json").RunConfig();

// DevUtils.LogMaterialInfo("C:/Users/Tobias/AppData/Roaming/AutoAlteration/HeavyPlastic");
