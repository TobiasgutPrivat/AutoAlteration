class AutoAlteration {
    public static int mapCount = 0;
    private static Alteration ?lastAlteration;
    public static bool devMode = false;
    public static string ProjectFolder = "";
    public static string CustomBlocksFolder = "";
    public static string[] Keywords = Array.Empty<string>();
    public static string[] shapeKeywords = Array.Empty<string>();
    public static string[] surfaceKeywords = Array.Empty<string>();

    public static void Load(string projectFolder) {
        ProjectFolder = projectFolder;
        CustomBlocksFolder = ProjectFolder + "src/CustomBlocks/";
        shapeKeywords = File.ReadAllLines(ProjectFolder + "src/Inventory/shapeKeywords.txt");
        surfaceKeywords = File.ReadAllLines(ProjectFolder + "src/Inventory/surfaceKeywords.txt");
        Keywords = File.ReadAllLines(ProjectFolder + "src/Inventory/Keywords.txt");
    }

    public static void Alter(List<Alteration> alterations, Map map) {
        foreach (Alteration alteration in alterations) {
            if (lastAlteration == null || (alteration.GetType() != lastAlteration.GetType())) {
                devMode = true;
                Alteration.CreateInventory();
                alteration.ChangeInventory();
                devMode = false;
            }
            alteration.Run(map);
            lastAlteration = alteration;
        }
        mapCount++;
    }
    public static void Alter(Alteration alteration, Map map) {
        alteration.Run(map);
        mapCount++;
    }

    public static void AlterFolder(List<Alteration> alterations, string mapFolder, string destinationFolder, string Name) {
        foreach (string mapFile in Directory.GetFiles(mapFolder, "*.map.gbx", SearchOption.TopDirectoryOnly)){
            AlterFile(alterations,mapFile,destinationFolder + Path.GetFileName(mapFile)[..^8] + " " + Name + ".map.gbx",Name);
        }
    }
    public static void AlterFolder(Alteration alteration, string mapFolder, string destinationFolder, string Name) =>
        AlterFolder(new List<Alteration>{alteration},mapFolder,destinationFolder,Name);
    
    public static void AlterAll(List<Alteration> alterations, string mapFolder, string destinationFolder, string Name) {
        AlterFolder(alterations,mapFolder,destinationFolder + Path.GetFileName(mapFolder) + " - " + Name + "/",Name);
        foreach (string Directory in Directory.GetDirectories(mapFolder, "*", SearchOption.TopDirectoryOnly))
        {
            AlterAll(alterations,Directory,destinationFolder + Directory[mapFolder.Length..] + "/",Name);
        }
    }
    public static void AlterAll(Alteration alteration, string mapFolder, string destinationFolder, string Name) =>
        AlterAll(new List<Alteration>{alteration},mapFolder,destinationFolder,Name);
    
    public static void AlterFolder(List<Alteration> alterations, string mapFolder, string Name) =>
        AlterFolder(alterations,mapFolder,mapFolder,Name);
    
    public static void AlterFolder(Alteration alteration, string mapFolder, string Name) =>
        AlterFolder(alteration,mapFolder,mapFolder,Name);
    
    public static void AlterFile(List<Alteration> alterations, string mapFile, string destinationFile, string Name) {
        Map map = new(mapFile);
        Alter(alterations, map);
        map.map.MapName = map.map.MapName + " " + Name;
        map.Save(destinationFile);
        Console.WriteLine(destinationFile);
    }
    public static void AlterFile(Alteration alteration, string mapFile, string destinationFile, string Name) =>
        AlterFile(new List<Alteration>{alteration},mapFile,destinationFile,Name);
    
    public static void AlterFile(List<Alteration> alterations, string mapFile, string Name) =>
        AlterFile(alterations,mapFile,Path.GetDirectoryName(mapFile)  + "\\" +  Path.GetFileName(mapFile)[..^8] + " " + Name + ".map.gbx",Name);
    
    public static void AlterFile(Alteration alteration, string mapFile, string Name) =>
        AlterFile(alteration,mapFile,Path.GetDirectoryName(mapFile) + "\\" +  Path.GetFileName(mapFile)[..^8] + " " + Name + ".map.gbx",Name);

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