using System.Reflection;
using GBX.NET;
using GBX.NET.LZO;
using GBX.NET.ZLib;

class AltertionConfig {
    public static string devPath = "";
    public static bool devMode = false;
    public static string DataFolder = "";
    public static string BlockDataPath = "";
    public static string CustomBlocksFolder = "";
    public static string ApplicationDataFolder = "";
    public static List<string> Keywords = [];
    public static List<string> ToKeywords = [];
    public static List<string> customBlockAltNames = [];

    public static void Load() {
        Gbx.LZO = new MiniLZO();
        // Gbx.ZLib = new ZLib();
        if (devMode) {
            DataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..","data");
        }else {
            DataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "AutoAlteration", "data");
        }

        CustomBlocksFolder = Path.Combine(DataFolder, "CustomBlocks");
        ApplicationDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AutoAlteration");
        BlockDataPath = Path.Combine(DataFolder, "Inventory","BlockData.json");

        ToKeywords = File.ReadAllLines(Path.Combine(DataFolder, "Inventory","ToKeywords.txt")).ToList();
        ToKeywords = ToKeywords.Concat(loadKeywordsFile(Path.Combine(ApplicationDataFolder, "ToKeywords.txt"))).ToList();
        Keywords = File.ReadAllLines(Path.Combine(DataFolder,"Inventory","Keywords.txt")).ToList();
        Keywords = Keywords.Concat(loadKeywordsFile(Path.Combine(ApplicationDataFolder, "Keywords.txt"))).ToList();
        Keywords = Keywords.OrderBy(x => x.Length).Reverse().ToList();
        Keywords = File.ReadAllLines(Path.Combine(DataFolder,"Inventory","KeywordsStart.txt")).Concat(Keywords).ToList();
        Keywords = loadKeywordsFile(Path.Combine(ApplicationDataFolder, "KeywordsStart.txt")).Concat(Keywords).ToList();

        customBlockAltNames = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(CustomBlockAlteration))).Select(x => x.Name).ToList();
    }

    private static List<string> loadKeywordsFile(string path){
        if (!File.Exists(path)){
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.Create(path).Close();
        }
        return File.ReadAllLines(path).ToList();
    }
}