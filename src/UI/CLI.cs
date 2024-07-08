using System.Reflection;

class CLI {
        
    public static List<Alteration> alterations = GetAllImplementations<Alteration>();
    public static List<T> GetAllImplementations<T>() where T : class
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type baseType = typeof(T);

            List<T> implementations = assembly.GetTypes()
                .Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract)
                .Select(t => Activator.CreateInstance(t) as T)
                .ToList();

            return implementations;
        }
    public static void Run(){
        try{
            
        AlterationConfigType? type = CLISelectType();
        if (type == null){
            return;
        }
        Configuration((AlterationConfigType)type);
        
        Console.WriteLine("Press Enter to exit");
        Console.ReadLine();
        return;
        } catch (Exception e){
            Console.WriteLine(e);
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
            return;
        }
    }

    static void Configuration(AlterationConfigType type){
        string source = SelectSourcePath(type);
        string destination = SelectDestinationPath(type);

        List<Alteration> Alterations = SelectAlterations();
        Console.WriteLine("Enter Name added to Filenames: ");
        string name = Console.ReadLine() ?? "";

        Console.WriteLine("Do you want to run Alteration? (y/n)");
        if (Console.ReadLine() != "y"){
            Console.WriteLine("Exiting");
            return;
        }
        AlterationConfig config = new(Alterations, source, destination, name);
        switch (type){
            case AlterationConfigType.File:
                config.AlterFile();
                break;
            case AlterationConfigType.Folder:
                config.AlterFolder();
                break;
            case AlterationConfigType.All:
                config.AlterAll();
                break;
        }
        Console.WriteLine("Alteration Complete");
        Console.WriteLine("Altered maps:" + AlterationConfig.mapCount);
        AlterationConfig.mapCount = 0;
    }

    static List<Alteration> SelectAlterations(){
        List<Alteration> Alterations = new();
        Alteration selectedAlteration;
        do {
            selectedAlteration = SelectAlteration();
            if (selectedAlteration != null){
                Alterations.Add(selectedAlteration);
            } else if (Alterations.Count == 0){
                Console.WriteLine("No Alterations Selected. Continue anyways? (y/n)");
                if (Console.ReadLine() != "y"){
                    return SelectAlterations();
                }
            }
        } while (selectedAlteration != null);
        return Alterations;
    }

    static Alteration? SelectAlteration(){
        Console.WriteLine("Input Alteration Name: ");
        Console.WriteLine("Enter to Exit: ");
        string name = Console.ReadLine() ?? "";
        if (name == ""){return null;}
        List<Alteration> selection = alterations.Where(alteration => alteration.GetType().Name == name).ToList();
        if (selection.Count == 0){
            Console.WriteLine("No Alteration with name: " + name);
            return SelectAlteration();
        }
        return selection.First();
    }

    static string SelectSourcePath(AlterationConfigType type){
        if (type == AlterationConfigType.File){
            Console.WriteLine("Enter Source File: ");
        } else {
            Console.WriteLine("Enter Source Folder: ");
        }
        string ?source = Console.ReadLine();
        if (source == null){
            Console.WriteLine("Invalid Path");
            return SelectSourcePath(type);
        } else {
            source = source.Replace("\"", "");
            if (ValidPath(type, source)){
                if (type == AlterationConfigType.File && !File.Exists(source)){
                    Console.WriteLine("File not found");
                    return SelectSourcePath(type);
                }
                return source;
            }else{
                Console.WriteLine("Invalid Path");
                return SelectSourcePath(type);
            }
        }
    }
    static string SelectDestinationPath(AlterationConfigType type){
        if (type == AlterationConfigType.File){
            Console.WriteLine("Enter Destination File: ");
        } else {
            Console.WriteLine("Enter Destination Folder: ");
        }
        string ?destination = Console.ReadLine();
        if (destination == null){
            Console.WriteLine("Invalid Path");
            return SelectDestinationPath(type);
        } else {
            destination = destination.Replace("\"", "");
            if (ValidPath(type, destination)){
                return destination;
            }else{
                Console.WriteLine("Invalid Path");
                return SelectDestinationPath(type);
            }
        }
    }

    static bool ValidPath(AlterationConfigType type, string path){
        switch (type){
            case AlterationConfigType.File:
                if (!Directory.Exists(Path.GetDirectoryName(path))){
                    Console.WriteLine("Folder not found: " + Path.GetDirectoryName(path));
                    return false;
                }
                return true;
            case AlterationConfigType.Folder:
            case AlterationConfigType.All:
                if (!Directory.Exists(path)){
                    Console.WriteLine("Folder not found: " + path);
                    return false;
                }
                return true;
            default:
                Console.WriteLine("Invalid Type");
                return false;
        }
    }

    static AlterationConfigType? CLISelectType(){
        Console.WriteLine("Do you want to");
        Console.WriteLine("1. Alter a File");
        Console.WriteLine("2. Alter a Folder");
        Console.WriteLine("3. Alter a Folder with subfolders");
        Console.WriteLine("Input according Number: ");
        string ?type = Console.ReadLine();
        switch (type){
            case "1":
                return AlterationConfigType.File;
            case "2":
                return AlterationConfigType.Folder;
            case "3":
                return AlterationConfigType.All;
            default:
                Console.WriteLine("Invalid Input");
                return CLISelectType();
        }
    }
}