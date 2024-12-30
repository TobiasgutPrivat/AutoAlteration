using System.Text.Json;
using System.Reflection;

public class AlterationScript {
    public string filePath = "";

    public AlterationScript(string filePath){
        this.filePath = filePath;
    }

    public void RunConfig(){
        if (!filePath.Contains(':')){
            filePath = Path.Combine(AltertionConfig.ApplicationDataFolder,"scripts", filePath);
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

    private Alteration GetAlteration(string name){
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
                AutoAlteration.AlterFile(alterations.ToList(),source,Path.Combine(destination, Path.GetFileName(source)[..^8] + " " + name + ".Map.Gbx"),name);
                break;
            case AlterType.Folder:
                AutoAlteration.AlterFolder(alterations.ToList(),source,destination,name);
                break;
            case AlterType.FullFolder:
                AutoAlteration.AlterAll(alterations.ToList(),source,destination,name);
                break;
        }
    }

    public static void RunAlteration(AlterType type, string source, string destination, string name, List<CustomBlockAlteration> alterations)
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
                if (source.Contains(".item.gbx", StringComparison.OrdinalIgnoreCase)){
                    AutoAlteration.AlterFile(alterations.ToList(),source,Path.Combine(destination, Path.GetFileName(source)[..^9] + " " + name + ".Item.Gbx"),name);
                } else if (source.Contains(".block.gbx", StringComparison.OrdinalIgnoreCase)){
                    AutoAlteration.AlterFile(alterations.ToList(),source,Path.Combine(destination, Path.GetFileName(source)[..^10] + " " + name + ".Item.Gbx"),name);
                } else {
                    throw new Exception("Invalid Filetype");
                }
                break;
            case AlterType.Folder:
                AutoAlteration.AlterFolder(alterations.ToList(),source,destination,name);
                break;
            case AlterType.FullFolder:
                AutoAlteration.AlterAll(alterations.ToList(),source,destination,name);
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
}

public enum AlterType
{
    File,
    Folder,
    FullFolder
}