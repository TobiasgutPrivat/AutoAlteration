//Initial load

AlterationConfig.devMode = true;
AlterationConfig.devPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..","data","dev");
AlterationConfig.Load();

// ----------- Code for Execution (change for your use) ----------- //

string from = "C:\\Users\\hugok\\Documents\\Trackmania\\Maps\\Campaigns\\Fall 2026\\Fall 2026 - 01.Map.Gbx";
string to = "C:\\Users\\hugok\\Documents\\Trackmania\\Maps\\Campaigns\\Fall 2026\\Fall 2026 - 01 FreeWheel Map.Gbx";
AutoAlteration.AlterFile(new FreeWheel(), from, to, "FreeWheel");
    