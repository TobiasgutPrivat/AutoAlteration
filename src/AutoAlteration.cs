using System.Reflection;
using System.Text.Json;
using GBX.NET.Engines.Plug;

public class AutoAlteration {
    public static int mapCount = 0;
    private static Alteration ?lastAlteration;
    public static string devPath = "";
    public static bool devMode = false;
    public static string DataFolder = "";
    public static string CustomBlocksFolder = "";
    public static string[] Keywords = [];
    public static string[] shapeKeywords = [];
    public static string[] surfaceKeywords = [];
    public static string[] specialKeywords = [];

    public static void Load() {
        DataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "AutoAlteration", "data");
        CustomBlocksFolder = Path.Combine(DataFolder, "CustomBlocks");
        shapeKeywords = File.ReadAllLines(Path.Combine(DataFolder, "Inventory","shapeKeywords.txt"));
        surfaceKeywords = File.ReadAllLines(Path.Combine(DataFolder,"Inventory","surfaceKeywords.txt"));
        Keywords = File.ReadAllLines(Path.Combine(DataFolder,"Inventory","Keywords.txt"));
        specialKeywords = File.ReadAllLines(Path.Combine(DataFolder,"Inventory","SpecialKeywords.txt"));
    }

    public static void Alter(List<Alteration> alterations, Map map) {
        foreach (Alteration alteration in alterations) {
            if (lastAlteration == null || (alteration.GetType() != lastAlteration.GetType())) {
                Alteration.CreateInventory();
                alteration.ChangeInventory();
                Alteration.InventoryChanges();
                if (devMode){
                    Alteration.inventory.Export(alteration.GetType().Name);
                }
            }
            alteration.Run(map);
            lastAlteration = alteration;
        }
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

    public static void AlterFolder(List<Alteration> alterations, string sourceFolder, string destinationFolder, string Name) {
        foreach (string mapFile in Directory.GetFiles(sourceFolder, "*.map.gbx", SearchOption.TopDirectoryOnly)){
            AlterFile(alterations,mapFile,Path.Combine(destinationFolder,Path.GetFileName(mapFile)[..^8] + " " + Name + ".map.gbx"),Name);
        }
    }
    public static void AlterFolder(List<CustomBlockAlteration> alterations, string sourceFolder, string destinationFolder, string Name) {
        foreach (string mapFile in Directory.GetFiles(sourceFolder, "*.item.gbx", SearchOption.TopDirectoryOnly)){
            AlterFile(alterations,mapFile,GetNewCustomBlockName(Path.Combine(destinationFolder, Path.GetFileName(mapFile)),Name),Name);
        }
        foreach (string mapFile in Directory.GetFiles(sourceFolder, "*.block.gbx", SearchOption.TopDirectoryOnly)){
            AlterFile(alterations,mapFile,GetNewCustomBlockName(Path.Combine(destinationFolder, Path.GetFileName(mapFile)),Name),Name);
        }
    }
    public static void AlterFolder(Alteration alteration, string sourceFolder, string destinationFolder, string Name) =>
        AlterFolder(new List<Alteration>{alteration},sourceFolder,destinationFolder,Name);
    public static void AlterFolder(CustomBlockAlteration alteration, string sourceFolder, string destinationFolder, string Name) =>
        AlterFolder(new List<CustomBlockAlteration>{alteration},sourceFolder,destinationFolder,Name);
    
    public static void AlterAll(List<Alteration> alterations, string sourceFolder, string destinationFolder, string Name) {
        AlterFolder(alterations,sourceFolder,Path.Combine(destinationFolder, Path.GetFileName(sourceFolder) + " - " + Name),Name);
        foreach (string Directory in Directory.GetDirectories(sourceFolder, "*", SearchOption.TopDirectoryOnly))
        {
            AlterAll(alterations,Directory,Path.Combine(destinationFolder, Directory[sourceFolder.Length..]),Name);
        }
    }
    public static void AlterAll(List<CustomBlockAlteration> alterations, string sourceFolder, string destinationFolder, string Name) {
        AlterFolder(alterations,sourceFolder,destinationFolder,Name);
        foreach (string Directory in Directory.GetDirectories(sourceFolder, "*", SearchOption.TopDirectoryOnly))
        {
            AlterAll(alterations,Directory,Path.Combine(destinationFolder, Directory[sourceFolder.Length..]),Name);
        }
    }
    public static void AlterAll(Alteration alteration, string sourceFolder, string destinationFolder, string Name) =>
        AlterAll(new List<Alteration>{alteration},sourceFolder,destinationFolder,Name);
    public static void AlterAll(CustomBlockAlteration alteration, string sourceFolder, string destinationFolder, string Name) =>
        AlterAll(new List<CustomBlockAlteration>{alteration},sourceFolder,destinationFolder,Name);
    
    public static void AlterFile(List<Alteration> alterations, string sourceFile, string destinationFile, string Name) {
        Map map = new(sourceFile);
        Alter(alterations, map);
        map.map.MapName = map.map.MapName + " " + Name;
        map.Save(destinationFile);
        Console.WriteLine(destinationFile);
    }
    public static void AlterFile(List<CustomBlockAlteration> alterations, string sourceFile, string destinationFile, string Name) {
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
    public static void AlterFile(Alteration alteration, string sourceFile, string destinationFile, string Name) =>
        AlterFile(new List<Alteration>{alteration},sourceFile,destinationFile,Name);
    public static void AlterFile(CustomBlockAlteration alteration, string sourceFile, string destinationFile, string Name) =>
        AlterFile(new List<CustomBlockAlteration>{alteration},sourceFile,destinationFile,Name);
    
    public static void AlterFile(List<Alteration> alterations, string sourceFile, string Name) =>
        AlterFile(alterations,sourceFile,Path.Combine(Path.GetDirectoryName(sourceFile),  Path.GetFileName(sourceFile)[..^8] + " " + Name + ".map.gbx"),Name);
    public static void AlterFile(List<CustomBlockAlteration> alterations, string sourceFile, string Name) =>
        AlterFile(alterations,sourceFile,GetNewCustomBlockName(sourceFile,Name),Name);
    
    public static void AlterFile(Alteration alteration, string sourceFile, string Name) =>
        AlterFile(alteration,sourceFile,Path.Combine(Path.GetDirectoryName(sourceFile),  Path.GetFileName(sourceFile)[..^8] + " " + Name + ".map.gbx"),Name);
    public static void AlterFile(CustomBlockAlteration alteration, string sourceFile, string Name) =>
        AlterFile(alteration,sourceFile,GetNewCustomBlockName(sourceFile,Name),Name);

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

    public static void RunConfig(string filePath){
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
        if (path.Contains(".")) {
            return "Not a Folder Path";
        }       
        return "";     
    }

    public static List<Alteration> GetImplementedAlterations() {
        return [
            new AntiBooster(),
            new Boosterless(),
            new Broken(),
            new CPFull(),
            new CPBoost(),
            new Cruise(),
            new Desert(),
            new Fast(),
            new Fragile(),
            new FreeWheel(),
            new Glider(),
            new NoBrake(),
            new NoEffect(),
            new NoItems(),
            new NoSteer(),
            new OneBack(),
            new OneDown(),
            new OneLeft(),
            new OneRight(),
            new OneUP(),
            new Rally(),
            new Reactor(),
            new ReactorDown(),
            new RedEffects(),
            new RngBooster(),
            new SlowMo(),
            new Stadium(),
            new STTF(),
            new Snow(),
            new TwoUP(),
            new YepTree()
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