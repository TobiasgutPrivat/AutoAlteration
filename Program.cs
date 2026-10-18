//Initial load

AlterationConfig.devMode = true;
AlterationConfig.devPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..","data","dev");
AlterationConfig.Load();

// ----------- Code for Execution (change for your use) ----------- //

string from = "C:\\Users\\hugok\\Documents\\Trackmania\\Maps\\My Maps\\gate-test.Map.Gbx";
string to = "C:\\Users\\hugok\\Documents\\Trackmania\\Maps\\My Maps\\gate-test Free Wheel.Gbx";
AutoAlteration.AlterFile(new FreeWheel(), from, to, "Free Wheel");
    