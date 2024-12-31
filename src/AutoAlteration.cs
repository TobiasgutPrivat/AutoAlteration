using System.Reflection;

public class AutoAlteration {
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
        Map map = new Map(sourceFile);
        AlterationLogic.Alter(alterations, map);
        map.map.MapName = map.map.MapName + " " + Name;
        map.Save(destinationFile);
        Console.WriteLine(destinationFile);
    }

    public static void AlterFile(SList<CustomBlockAlteration> alterations, string sourceFile, string destinationFile, string Name) {
        CustomBlock customBlock;
        try {
            customBlock = new CustomBlock(sourceFile);
        } catch {
            Console.WriteLine("Load Error");
            return;
        }
        if (AlterationLogic.Alter(alterations, customBlock)){
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

    public static List<Alteration> GetImplementedAlterations() {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Alteration)))
            .Select(t => Activator.CreateInstance(t) as Alteration)
            .OfType<Alteration>()
            .Where(x => x.Published)
            .OrderBy(x => x.GetType().Name)
            .ToList();
    }
    public static List<CustomBlockAlteration> GetImplementedBlockAlterations() {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(CustomBlockAlteration)))
            .Select(t => Activator.CreateInstance(t) as CustomBlockAlteration)
            .OfType<CustomBlockAlteration>()
            .Where(x => x.Published)
            .OrderBy(x => x.GetType().Name)
            .ToList();
    }
}