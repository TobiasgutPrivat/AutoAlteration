//Initial load
AutoAlteration.devMode = true;
AutoAlteration.devPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..","dev");
AutoAlteration.Load();

// ----------- Code for Execution (change for your use) ----------- //
AutoAlteration.RunConfig("Summer2024Config.json");
// AutoAlteration.RunConfig("C:/Users/Tobias/Documents/Programmieren/AutoAlteration/config/TestTemplateConfig.json");
// AutoAlteration.GenerateBlockSet(new HeavyDirt(), "HeavyDirt");

//Customblocks -------------
// string sourceFolder = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/data/CustomBlocks/dev";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Items/dev";
// AutoAlteration.AlterAll(new HeavyDirt(), sourceFolder, destinationFolder, "HeavyDirt");

// DevUtils.CombineItemPrefabs("C:/Users/Tobias/Documents/Trackmania2020/Items/GateSupport.Item.Gbx","C:/Users/Tobias/Documents/Trackmania2020/Items/Support.Prefab.Gbx");
// DevUtils.resaveBlock("C:/Users/Tobias/Documents/Trackmania2020/Blocks/test/PlatformDirtRoadWallStraight4WithHole24m.Block.Gbx");