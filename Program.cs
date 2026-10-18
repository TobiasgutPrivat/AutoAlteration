//Initial load

AlterationConfig.devMode = true;
AlterationConfig.devPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..","data","dev");
AlterationConfig.Load();

// ----------- Code for Execution (change for your use) ----------- //

string from = "C:\\Users\\hugok\\Documents\\Trackmania\\Maps\\Campaigns\\Fall 2026\\Fall 2026 - 01_(361913).Map.Gbx";
string to = "C:\\Users\\hugok\\Documents\\Trackmania\\Maps\\Campaigns\\Fall 2026\\Fall 2026 - 01_(361913).Map Flags.Gbx";
AutoAlteration.AlterFile(new Flags(), from, to, "flags");
    