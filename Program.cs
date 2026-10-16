//Initial load

AlterationConfig.devMode = true;
AlterationConfig.devPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..","data","dev");
AlterationConfig.Load();

// ----------- Code for Execution (change for your use) ----------- //

string from = "C:\\Users\\hugok\\Documents\\Trackmania\\Maps\\Campaigns\\Summer 2026\\Summer 2026 - 15_(334566).Map.Gbx";
string to = "C:\\Users\\hugok\\Documents\\Trackmania\\Maps\\Campaigns\\Summer 2026\\Summer 2026 - 15_(334566) Flags.Map.Gbx";
AutoAlteration.AlterFile(new Flags(), from, to, "flags");
    