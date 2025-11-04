using GBX.NET;
using GBX.NET.LZO;
using GBX.NET.ZLib;

public class AlterationConfig {
    public static string devPath = "";
    public static bool devMode = false;
    public static string DataFolder = "";
    public static string BlockDataPath = "";
    public static string ItemDataPath = "";
    public static string CustomBlocksFolder = "";
    public static string ApplicationDataFolder = "";
    public static string CacheFolder = "";
    public static List<string> Keywords = [];
    public static List<string> ToKeywords = [];
    public static List<string> CustomBlockSets = [];
    internal static readonly VanillaArticleProvider VanillaArticles = new();
    public static int mapCount = 0;

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
        CacheFolder = Path.Combine(ApplicationDataFolder, "Cache");
        BlockDataPath = Path.Combine(DataFolder, "Inventory","BlockData.json");
        ItemDataPath = Path.Combine(DataFolder, "Inventory","ItemData.json");

        ToKeywords = LoadKeywordsFile(Path.Combine(DataFolder, "Inventory","ToKeywords.txt")).ToList();
        ToKeywords = ToKeywords.Concat(LoadKeywordsFile(Path.Combine(ApplicationDataFolder, "ToKeywords.txt"))).ToList();

        // CustomBlockSets = loadKeywordsFile(Path.Combine(DataFolder, "Inventory","CustomBlockSets.txt")).ToList();
        // CustomBlockSets = CustomBlockSets.Concat(loadKeywordsFile(Path.Combine(ApplicationDataFolder, "CustomBlockSets.txt"))).ToList();
        
        Keywords = LoadKeywordsFile(Path.Combine(DataFolder,"Inventory","Keywords.txt")).ToList();
        Keywords = Keywords.Concat(LoadKeywordsFile(Path.Combine(DataFolder,"Inventory","OldGameKeywords.txt"))).ToList();
        Keywords = Keywords.Concat(LoadKeywordsFile(Path.Combine(ApplicationDataFolder, "Keywords.txt"))).ToList();
        Keywords = Keywords.OrderBy(x => x.Length).Reverse().ToList();
        Keywords = LoadKeywordsFile(Path.Combine(DataFolder,"Inventory","KeywordsStart.txt")).Concat(Keywords).ToList();
        Keywords = LoadKeywordsFile(Path.Combine(ApplicationDataFolder, "KeywordsStart.txt")).Concat(Keywords).ToList();
    }

    private static List<string> LoadKeywordsFile(string path){
        if (!File.Exists(path)){
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.Create(path).Close();
        }
        return File.ReadAllLines(path).ToList();
    }
}