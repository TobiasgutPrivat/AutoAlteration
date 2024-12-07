﻿using Newtonsoft.Json;
//Initial load

AutoAlteration.devMode = true;
AutoAlteration.devPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..","data","dev");
AutoAlteration.Load();

// ----------- Code for Execution (change for your use) ----------- //
AutoAlteration.RunConfig("CampaignConfig.json");
// AutoAlteration.RunConfig("FileConfig.json");
// File.WriteAllText(Path.Combine(AutoAlteration.devPath, "Materials.json"), JsonConvert.SerializeObject(MaterialInfo.materials));
// AutoAlteration.AlterFolder(new SnowScenery(),"SourcePath","DestinationPath","SnowScenery");

//Customblocks -------------
// string sourceFolder = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/data/CustomBlocks/dev";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Items/dev";
// AutoAlteration.AlterAll(new HeavyDirt(), sourceFolder, destinationFolder, "HeavyDirt");

// DevUtils.CombineItemPrefabs("C:/Users/Tobias/Documents/Trackmania2020/Items/GateSupport.Item.Gbx","C:/Users/Tobias/Documents/Trackmania2020/Items/Support.Prefab.Gbx");
// DevUtils.resaveBlock("C:/Users/Tobias/Documents/Trackmania2020/Blocks/test/PlatformDirtRoadWallStraight4WithHole24m.Block.Gbx");