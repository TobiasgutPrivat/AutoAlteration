using System.Reflection;
using System.Text.Json;
using GBX.NET.Engines.Plug;

public class AutoAlteration {
    public static int mapCount = 0;
    private static List<Alteration> ?lastAlterations;
    public static string devPath = "";
    public static bool devMode = false;
    public static string DataFolder = "";
    public static string CustomBlocksFolder = "";
    public static string CustomBlockSetsFolder = "";
    public static List<string> Keywords = [];
    public static List<string> ToKeywords = [];
    public static List<string> customBlockAltNames = [];

    public static void Load() {
        if (devMode) {
            DataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..","data");
        }else {
            DataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "AutoAlteration", "data");
        }
        CustomBlocksFolder = Path.Combine(DataFolder, "CustomBlocks");
        CustomBlockSetsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AutoAlteration");
        ToKeywords = File.ReadAllLines(Path.Combine(DataFolder, "Inventory","ToKeywords.txt")).ToList();
        ToKeywords = ToKeywords.Concat(loadKeywordsFile(Path.Combine(CustomBlockSetsFolder, "ToKeywords.txt"))).ToList();
        Keywords = File.ReadAllLines(Path.Combine(DataFolder,"Inventory","Keywords.txt")).ToList();
        Keywords = Keywords.Concat(loadKeywordsFile(Path.Combine(CustomBlockSetsFolder, "Keywords.txt"))).ToList();
        Keywords = Keywords.OrderBy(x => x.Length).Reverse().ToList();
        Keywords = File.ReadAllLines(Path.Combine(DataFolder,"Inventory","KeywordsStart.txt")).Concat(Keywords).ToList();
        Keywords = loadKeywordsFile(Path.Combine(CustomBlockSetsFolder, "KeywordsStart.txt")).Concat(Keywords).ToList();
        customBlockAltNames = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(CustomBlockAlteration))).Select(x => x.Name).ToList();
    }

    private static List<string> loadKeywordsFile(string path){
        if (!File.Exists(path)){
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.Create(path).Close();
        }
        return File.ReadAllLines(path).ToList();
    }

    #region Altering Logic
    public static void Alter(List<Alteration> alterations, Map map) {
        //cleanup
        if (Directory.Exists(Path.Join(CustomBlocksFolder,"Temp"))){
            Directory.Delete(Path.Join(CustomBlocksFolder,"Temp"),true);
        }
        Alteration.inventory.ClearSpecific();

        //create inventory for Alteration
        if (lastAlterations == null || alterations.Any(a => !lastAlterations.Select(lAs => lAs.GetType()).Contains(a.GetType())) || (alterations.Count != lastAlterations.Count)) {
            // needs Inventory recreation
            foreach (Alteration alteration in alterations) {
                Alteration.CreateInventory();
                alteration.InventoryChanges.ForEach(x => x.ChangeInventory(Alteration.inventory));
                Alteration.DefaultInventoryChanges();
            }
            if (devMode){
                Alteration.inventory.Export(string.Join("",alterations.Select(x => x.GetType().Name)));
            }
        }

        //Map specific custom blocks
        Alteration.inventory.AddArticles(map.embeddedBlocks.Select(x => {
            if (x.Contains(".Item.Gbx")){
                return new Article(x.Substring(x.IndexOf('/') + 1)[..^9], BlockType.CustomItem,"",true);
            } else if (x.Contains(".Block.Gbx")){
                return new Article(x.Substring(x.IndexOf('/') + 1)[..^10], BlockType.CustomBlock,"",true);
            } else {
                throw new Exception("Unknown file type: " + x);
            }
        }).ToList());

        alterations
            .SelectMany(alteration => alteration.InventoryChanges)
            .Where(change => change is CustomBlockSet)
            .Cast<CustomBlockSet>().ToList().ForEach(
                x => map.GenerateCustomBlocks(x.customBlockAlteration)); //includes updating inventory

        //alteration
        foreach (Alteration alteration in alterations) {
            alteration.Run(map);
        }
        map.map.OpenReadEmbeddedZipData().Entries.Select(x => x.FullName).ToList().ForEach(Console.WriteLine);

        lastAlterations = alterations;
        mapCount++;
    }
    
    public static bool Alter(List<CustomBlockAlteration> alterations, CustomBlock customBlock) {
        bool changed = false;
        foreach (CustomBlockAlteration alteration in alterations) {
            changed = alteration.Run(customBlock);
            if (customBlock.Type == BlockType.Block) {
                customBlock.Block.CustomizedVariants.ToList().ForEach(x => {
                    if (x.Crystal != null) {
                        changed = AlterMeshCrystal(alteration, customBlock, x.Crystal) || changed;
                    }
                });
            } else {
                changed = AlterMeshCrystal(alteration, customBlock, customBlock.Item.MeshCrystal) || changed;
            }
        }
        mapCount++;
        return changed;
    }

    private static bool AlterMeshCrystal(CustomBlockAlteration alteration, CustomBlock customBlock, CPlugCrystal MeshCrystal) {
        bool changed = false;
        alteration.AlterMeshCrystal(customBlock, MeshCrystal);
        MeshCrystal.Layers.Where(x => x.GetType() == typeof(CPlugCrystal.GeometryLayer)).ToList().ForEach(x => {
            CPlugCrystal.GeometryLayer layer = (CPlugCrystal.GeometryLayer)x;
            changed = alteration.AlterGeometry(customBlock, layer) || changed;
        });
        MeshCrystal.Layers.Where(x => x.GetType() == typeof(CPlugCrystal.TriggerLayer)).ToList().ForEach(x => {
            CPlugCrystal.TriggerLayer layer = (CPlugCrystal.TriggerLayer)x;
            changed = alteration.AlterTrigger(customBlock, layer) || changed;
        });
        MeshCrystal.Layers.Where(x => x.GetType() == typeof(CPlugCrystal.SpawnPositionLayer)).ToList().ForEach(x => {
            CPlugCrystal.SpawnPositionLayer layer = (CPlugCrystal.SpawnPositionLayer)x;
            changed = alteration.AlterSpawn(customBlock, layer) || changed;
        });
        return changed;
    }
    #endregion

    #region Alter Functions
    public static void AlterFolder(SList<Alteration> alterations, string sourceFolder, string destinationFolder, string Name) {
        foreach (string mapFile in Directory.GetFiles(sourceFolder, "*.map.gbx", SearchOption.TopDirectoryOnly)){
            AlterFile(alterations,mapFile,Path.Combine(destinationFolder,Path.GetFileName(mapFile)[..^8] + " " + Name + ".map.gbx"),Name);
        }
    }
    public static void AlterFolder(SList<CustomBlockAlteration> alterations, string sourceFolder, string destinationFolder, string Name) {
        foreach (string mapFile in Directory.GetFiles(sourceFolder, "*.item.gbx", SearchOption.TopDirectoryOnly)){
            AlterFile(alterations,mapFile,GetNewCustomBlockName(Path.Combine(destinationFolder, Path.GetFileName(mapFile)),Name),Name);
        }
        foreach (string mapFile in Directory.GetFiles(sourceFolder, "*.block.gbx", SearchOption.TopDirectoryOnly)){
            AlterFile(alterations,mapFile,GetNewCustomBlockName(Path.Combine(destinationFolder, Path.GetFileName(mapFile)),Name),Name);
        }
    }

    public static void AlterAll(SList<Alteration> alterations, string sourceFolder, string destinationFolder, string Name) {
        AlterFolder(alterations,sourceFolder,Path.Combine(destinationFolder, Path.GetFileName(sourceFolder) + " - " + Name),Name);
        foreach (string Directory in Directory.GetDirectories(sourceFolder, "*", SearchOption.TopDirectoryOnly))
        {
            AlterAll(alterations,Directory,Path.Combine(destinationFolder, Path.GetFileName(Directory)),Name);
        }
    }
    public static void AlterAll(SList<CustomBlockAlteration> alterations, string sourceFolder, string destinationFolder, string Name) {
        AlterFolder(alterations,sourceFolder,destinationFolder,Name);
        foreach (string Directory in Directory.GetDirectories(sourceFolder, "*", SearchOption.TopDirectoryOnly))
        {
            AlterAll(alterations,Directory,Path.Combine(destinationFolder, Path.GetFileName(Directory)),Name);
        }
    }
    
    public static void AlterFile(SList<Alteration> alterations, string sourceFile, string destinationFile, string Name) {
        Map map = new(sourceFile);
        Alter(alterations, map);
        map.map.MapName = map.map.MapName + " " + Name;
        map.Save(destinationFile);
        Console.WriteLine(destinationFile);
    }
    public static void AlterFile(SList<CustomBlockAlteration> alterations, string sourceFile, string destinationFile, string Name) {
        CustomBlock customBlock;
        try {
            customBlock = new(sourceFile);
        } catch {
            Console.WriteLine("Load Error");
            return;
        }
        if (Alter(alterations, customBlock)){
            customBlock.customBlock.Name = customBlock.customBlock.Name + " " + Name;
            customBlock.Save(destinationFile);
            Console.WriteLine(destinationFile);
        } else {
            Console.WriteLine(customBlock.Name + " unchanged");
        };
    }

    private static string GetNewCustomBlockName(string path, string name){
        if (path.Contains(".item.gbx", StringComparison.OrdinalIgnoreCase)){
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(path)[..^9] + name + path[^9..]);
        } else if (path.Contains(".block.gbx", StringComparison.OrdinalIgnoreCase)){
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(path)[..^10] + name + path[^10..]);
        } else {
            Console.WriteLine("Invalid Filetype");
            return "";
        }
    }
    #endregion

    #region ConfigScript
    public static void RunConfig(string filePath){
        if (!filePath.Contains(':')){
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..","data","config", filePath);
        }
        foreach (JsonElement item in JsonDocument.Parse(File.ReadAllText(filePath)).RootElement.EnumerateArray())
        {
            RunAlteration(
                (AlterType)Enum.Parse(typeof(AlterType), item.GetProperty("Type").GetString()),
                item.GetProperty("Source").GetString(),
                item.GetProperty("Destination").GetString(),
                item.GetProperty("Name").GetString(),
                item.GetProperty("Alterations").EnumerateArray().Select(x => 
                    GetAlteration(x.GetString())
                    ).ToList()
            );
        }
    }

    public static Alteration GetAlteration(string name){
        List<Type> types = Assembly.GetExecutingAssembly().GetTypes().ToList();
        List<Type> alterations = types.Where(t => t.Name == name).ToList();
        if (alterations.Count == 0){
            throw new Exception("Alteration " + name + " not found");
        }
        return (Alteration) Activator.CreateInstance(alterations.First());
    }

    public static void RunAlteration(AlterType type, string source, string destination, string name, List<Alteration> alterations)
    {
        string warning = ValidateSource(type, source);
        if (warning != ""){
            throw new Exception("Source " + warning);
        }
        warning = ValidateDestination(destination);
        if (warning != ""){
            throw new Exception("Destination " + warning);
        }
        Console.WriteLine("Alter " + type.ToString() + ": " + source);
        Console.WriteLine("To: " + destination);
        Console.WriteLine("As " + name);
        Console.WriteLine("Alterations:");
        alterations.ToList().ForEach(x => Console.Write(" " + x.GetType().ToString()));
        switch (type){
            case AlterType.File: 
                AlterFile(alterations.ToList(),source,Path.Combine(destination, Path.GetFileName(source)[..^8] + " " + name + ".Map.Gbx"),name);
                break;
            case AlterType.Folder:
                AlterFolder(alterations.ToList(),source,destination,name);
                break;
            case AlterType.FullFolder:
                AlterAll(alterations.ToList(),source,destination,name);
                break;
        }
    }
    
    private static string ValidateSource(AlterType Type, string path)
    {
		if (path is null || path == ""){
            return "Path missing";
		}
        if (Type != AlterType.File) {
            if (!Directory.Exists(Path.GetFullPath(path))) {
                return "Folder doesn't Exist";
            }
        } else {
            try {
                if (!File.Exists(Path.GetFullPath(path))) {
                    return "File doesn't Exist";
                };
            } catch{
                return "File doesn't Exist";
            }
        }
        return "";     
    }
    private static string ValidateDestination(string path)
    {
		if (path is null || path == ""){
            return "Path missing";
		}
        if (!Path.IsPathFullyQualified(Path.GetFullPath(path))){
            return "Invalid Path";
        }
        if (path.Contains('.')) {
            return "Not a Folder Path";
        }       
        return "";     
    }
    #endregion

    public static List<Alteration> GetImplementedAlterations() {
        return [
            new AntiBooster(),
            new Boosterless(),
            new Broken(),
            new Checkpointnt(),
            new CPBoost(),
            new CPFull(),
            new CPLess(),
            new CPLink(),
            new CPsRotated(),
            new Cruise(),
            new Desert(),
            new Earthquake(),
            new Fast(),
            new Flipped(),
            new Fragile(),
            new FreeWheel(),
            new Glider(),
            new Holes(),
            // new Mirrored(),
            new NoBrake(),
            new NoEffect(),
            new NoItems(),
            new NoSteer(),
            new OneBack(),
            new OneDown(),
            new OneLeft(),
            new OneRight(),
            new OneUP(),
            // new Platform(),
            new RandomBlocks(),
            new Race(),
            new Rally(),
            new RandomDankness(),
            new RandomEffects(),
            new Reactor(),
            new ReactorDown(),
            new RedEffects(),
            new RingCP(),
            new RngBooster(),
            new SnowScenery(),
            new SlowMo(),
            new SpeedLimit(),
            new Stadium(),
            new StartOneDown(),
            new STTF(),
            new Stunt(),
            new Snow(),
            new Tilted(),
            new TwoUP(),
            new Yeet(),
            new YeetDown(),
            new YeetMaxUp(),
            new YepTree(),
        ];
    }
    public static List<CustomBlockAlteration> GetImplementedBlockAlterations() {
        return [
        ];
    }
}

public enum AlterType
{
    File,
    Folder,
    FullFolder
}