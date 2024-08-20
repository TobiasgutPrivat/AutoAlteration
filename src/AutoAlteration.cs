using GBX.NET.Engines.Plug;

public class AutoAlteration {
    public static int mapCount = 0;
    private static Alteration ?lastAlteration;
    public static bool devMode = false;
    public static string ProjectFolder = "";
    public static string CustomBlocksFolder = "";
    public static string[] Keywords = Array.Empty<string>();
    public static string[] shapeKeywords = Array.Empty<string>();
    public static string[] surfaceKeywords = Array.Empty<string>();
    public static string[] specialKeywords = Array.Empty<string>();

    public static void Load(string projectFolder) {
        ProjectFolder = projectFolder;
        CustomBlocksFolder = ProjectFolder + "src/CustomBlocks/";
        shapeKeywords = File.ReadAllLines(ProjectFolder + "src/Inventory/shapeKeywords.txt");
        surfaceKeywords = File.ReadAllLines(ProjectFolder + "src/Inventory/surfaceKeywords.txt");
        Keywords = File.ReadAllLines(ProjectFolder + "src/Inventory/Keywords.txt");
        specialKeywords = File.ReadAllLines(ProjectFolder + "src/Inventory/SpecialKeywords.txt");
    }

    public static void Alter(List<Alteration> alterations, Map map) {
        foreach (Alteration alteration in alterations) {
            if (lastAlteration == null || (alteration.GetType() != lastAlteration.GetType())) {
                devMode = true;
                Alteration.CreateInventory();
                alteration.ChangeInventory();
                Alteration.InventoryChanges();
                Alteration.inventory.Export(alteration.GetType().Name);
                devMode = false;
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
            customBlock.Layers.Where(x => x.GetType() == typeof(CPlugCrystal.GeometryLayer)).ToList().ForEach(x => {
                CPlugCrystal.GeometryLayer layer = (CPlugCrystal.GeometryLayer)x;
                changed = alteration.AlterGeometry(customBlock, layer) || changed;
            });
            customBlock.Layers.Where(x => x.GetType() == typeof(CPlugCrystal.TriggerLayer)).ToList().ForEach(x => {
                CPlugCrystal.TriggerLayer layer = (CPlugCrystal.TriggerLayer)x;
                changed = alteration.AlterTrigger(customBlock, layer) || changed;
            });
            customBlock.Layers.Where(x => x.GetType() == typeof(CPlugCrystal.SpawnPositionLayer)).ToList().ForEach(x => {
                CPlugCrystal.SpawnPositionLayer layer = (CPlugCrystal.SpawnPositionLayer)x;
                changed = alteration.AlterSpawn(customBlock, layer) || changed;
            });
        }
        mapCount++;
        return changed;
    }

    public static void AlterFolder(List<Alteration> alterations, string mapFolder, string destinationFolder, string Name) {
        foreach (string mapFile in Directory.GetFiles(mapFolder, "*.map.gbx", SearchOption.TopDirectoryOnly)){
            AlterFile(alterations,mapFile,destinationFolder + Path.GetFileName(mapFile)[..^8] + " " + Name + ".map.gbx",Name);
        }
    }
    public static void AlterFolder(List<CustomBlockAlteration> alterations, string mapFolder, string destinationFolder, string Name) {
        foreach (string mapFile in Directory.GetFiles(mapFolder, "*.item.gbx", SearchOption.TopDirectoryOnly)){
            AlterFile(alterations,mapFile,GetNewCustomBlockName(destinationFolder + Path.GetFileName(mapFile),Name),Name);
        }
        foreach (string mapFile in Directory.GetFiles(mapFolder, "*.block.gbx", SearchOption.TopDirectoryOnly)){
            AlterFile(alterations,mapFile,GetNewCustomBlockName(destinationFolder + Path.GetFileName(mapFile),Name),Name);
        }
    }
    public static void AlterFolder(Alteration alteration, string mapFolder, string destinationFolder, string Name) =>
        AlterFolder(new List<Alteration>{alteration},mapFolder,destinationFolder,Name);
    public static void AlterFolder(CustomBlockAlteration alteration, string mapFolder, string destinationFolder, string Name) =>
        AlterFolder(new List<CustomBlockAlteration>{alteration},mapFolder,destinationFolder,Name);
    
    public static void AlterAll(List<Alteration> alterations, string mapFolder, string destinationFolder, string Name) {
        AlterFolder(alterations,mapFolder,destinationFolder + Path.GetFileName(mapFolder) + " - " + Name + "/",Name);
        foreach (string Directory in Directory.GetDirectories(mapFolder, "*", SearchOption.TopDirectoryOnly))
        {
            AlterAll(alterations,Directory,destinationFolder + Directory[mapFolder.Length..] + "/",Name);
        }
    }
    public static void AlterAll(List<CustomBlockAlteration> alterations, string mapFolder, string destinationFolder, string Name) {
        AlterFolder(alterations,mapFolder,destinationFolder,Name);
        foreach (string Directory in Directory.GetDirectories(mapFolder, "*", SearchOption.TopDirectoryOnly))
        {
            AlterAll(alterations,Directory,destinationFolder + Directory[mapFolder.Length..] + "/",Name);
        }
    }
    public static void AlterAll(Alteration alteration, string mapFolder, string destinationFolder, string Name) =>
        AlterAll(new List<Alteration>{alteration},mapFolder,destinationFolder,Name);
    public static void AlterAll(CustomBlockAlteration alteration, string mapFolder, string destinationFolder, string Name) =>
        AlterAll(new List<CustomBlockAlteration>{alteration},mapFolder,destinationFolder,Name);
    
    public static void AlterFolder(List<Alteration> alterations, string mapFolder, string Name) =>
        AlterFolder(alterations,mapFolder,mapFolder,Name);
    public static void AlterFolder(List<CustomBlockAlteration> alterations, string mapFolder, string Name) =>
        AlterFolder(alterations,mapFolder,mapFolder,Name);
    
    public static void AlterFolder(Alteration alteration, string mapFolder, string Name) =>
        AlterFolder(alteration,mapFolder,mapFolder,Name);
    public static void AlterFolder(CustomBlockAlteration alteration, string mapFolder, string Name) =>
        AlterFolder(alteration,mapFolder,mapFolder,Name);
    
    public static void AlterFile(List<Alteration> alterations, string mapFile, string destinationFile, string Name) {
        Map map = new(mapFile);
        Alter(alterations, map);
        map.map.MapName = map.map.MapName + " " + Name;
        map.Save(destinationFile);
        Console.WriteLine(destinationFile);
    }
    public static void AlterFile(List<CustomBlockAlteration> alterations, string blockFile, string destinationFile, string Name) {
        CustomBlock customBlock;
        try {
            customBlock = new(blockFile);
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
    public static void AlterFile(Alteration alteration, string mapFile, string destinationFile, string Name) =>
        AlterFile(new List<Alteration>{alteration},mapFile,destinationFile,Name);
    public static void AlterFile(CustomBlockAlteration alteration, string blockFile, string destinationFile, string Name) =>
        AlterFile(new List<CustomBlockAlteration>{alteration},blockFile,destinationFile,Name);
    
    public static void AlterFile(List<Alteration> alterations, string mapFile, string Name) =>
        AlterFile(alterations,mapFile,Path.GetDirectoryName(mapFile)  + "\\" +  Path.GetFileName(mapFile)[..^8] + " " + Name + ".map.gbx",Name);
    public static void AlterFile(List<CustomBlockAlteration> alterations, string blockFile, string Name) =>
        AlterFile(alterations,blockFile,GetNewCustomBlockName(blockFile,Name),Name);
    
    public static void AlterFile(Alteration alteration, string mapFile, string Name) =>
        AlterFile(alteration,mapFile,Path.GetDirectoryName(mapFile) + "\\" +  Path.GetFileName(mapFile)[..^8] + " " + Name + ".map.gbx",Name);
    public static void AlterFile(CustomBlockAlteration alteration, string blockFile, string Name) =>
        AlterFile(alteration,blockFile,GetNewCustomBlockName(blockFile,Name),Name);

    private static string GetNewCustomBlockName(string path, string name){
        if (path.Contains(".item.gbx", StringComparison.OrdinalIgnoreCase)){
            return Path.GetDirectoryName(path) + "\\" +  Path.GetFileName(path)[..^9] + name + path[^9..];
        } else if (path.Contains(".block.gbx", StringComparison.OrdinalIgnoreCase)){
            return Path.GetDirectoryName(path) + "\\" +  Path.GetFileName(path)[..^10] + name + path[^10..];
        } else {
            Console.WriteLine("Invalid Filetype");
            return "";
        }
    }

    public static void AllAlterations(string sourceFolder, string destinationFolder) {
        AlterAll(new Stadium(), sourceFolder, destinationFolder, "Stadium");
        AlterAll(new Snow(), sourceFolder, destinationFolder, "Snow");
        AlterAll(new Rally(), sourceFolder, destinationFolder, "Rally");
        AlterAll(new Desert(), sourceFolder, destinationFolder, "Desert");

        AlterAll(new NoBrake(), sourceFolder, destinationFolder, "NoBrake");
        AlterAll(new Cruise(), sourceFolder, destinationFolder, "Cruise");
        AlterAll(new Fragile(), sourceFolder, destinationFolder, "Fragile");
        AlterAll(new SlowMo(), sourceFolder, destinationFolder, "SlowMo");
        AlterAll(new NoSteer(), sourceFolder, destinationFolder, "NoSteer");
        AlterAll(new Glider(), sourceFolder, destinationFolder, "Glider");
        AlterAll(new Reactor(), sourceFolder, destinationFolder, "Reactor");
        AlterAll(new ReactorDown(), sourceFolder, destinationFolder, "ReactorDown");
        AlterAll(new FreeWheel(), sourceFolder, destinationFolder, "FreeWheel");
        AlterAll(new AntiBooster(), sourceFolder, destinationFolder, "AntiBooster");
        
        AlterAll(new CPBoost(), sourceFolder, destinationFolder, "CP-Boost");
        AlterAll(new STTF(), sourceFolder, destinationFolder, "STTF");
        AlterAll(new CPFull(), sourceFolder, destinationFolder, "CPFull");
        // alterAll(new CPLess(), sourceFolder, destinationFolder, "CPLess");

        AlterAll(new OneUP(), sourceFolder, destinationFolder, "(1-UP)");
        AlterAll(new OneDown(), sourceFolder, destinationFolder, "(1-Down)");
        AlterAll(new OneLeft(), sourceFolder, destinationFolder, "(1-Left)");
        AlterAll(new OneRight(), sourceFolder, destinationFolder, "(1-Right)");
        AlterAll(new TwoUP(), sourceFolder, destinationFolder, "(2-UP)");

        AlterAll(new YepTree(), sourceFolder, destinationFolder, "YepTree");

        Console.WriteLine("Done!");
        Console.WriteLine("Map Count: " + mapCount);
    }
}

enum AlterationConfigType
{
    File,
    Folder,
    All
}