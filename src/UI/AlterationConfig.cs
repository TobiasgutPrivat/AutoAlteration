class AlterationConfig {
    public List<Alteration> alterations = new();
    public string source = "";
    public string destination = "";
    public string name = "";
    public static int mapCount;

    public AlterationConfig(List<Alteration> alterations, string source, string destination, string name) {
        this.alterations = alterations;
        this.source = source;
        this.destination = destination;
        this.name = name;
    }
    public AlterationConfig(Alteration alterations, string source, string destination, string name) {
        this.alterations = new List<Alteration>(){alterations};
        this.source = source;
        this.destination = destination;
        this.name = name;
    }

    public void AlterFile() {
        Map map = new(source);
        Alter(alterations, map);
        map.map.MapName = Path.GetFileName(source).Substring(0, Path.GetFileName(source).Length - 8) + " " + name;
        map.Save(destination);
        Console.WriteLine(destination);
    }

    public static void Alter(List<Alteration> alterations, Map map) {
        foreach (Alteration alteration in alterations) {
            alteration.Run(map);
        }
        mapCount++;
    }

    public void AlterFolder() {
        foreach (string mapFile in Directory.GetFiles(source, "*.map.gbx", SearchOption.TopDirectoryOnly)){
            new AlterationConfig(alterations,mapFile,destination + Path.GetFileName(mapFile).Substring(0, Path.GetFileName(mapFile).Length - 8) + " " + name + ".map.gbx",name).AlterFile();
        }
    }

    public void AlterAll() {
        new AlterationConfig(alterations,source,destination + Path.GetFileName(source) + " - " + name + "/",name).AlterFolder();
        foreach (string Directory in Directory.GetDirectories(source, "*", SearchOption.TopDirectoryOnly))
        {
            new AlterationConfig(alterations,Directory,destination + Directory[source.Length..] + "/",name).AlterAll();
        }
    }
}

enum AlterationConfigType
{
    File,
    Folder,
    All
}