using System.IO.Compression;
using System.Reflection;
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
using GBX.NET.LZO;
using GBX.NET.ZLib;
using ManiaAPI.NadeoAPI;
using Newtonsoft.Json;

class DevUtils{
    public static void transferMapUids(string sourceFolder, string destinationFolder) {
        List<string> sourcefiles = Directory.GetFiles(sourceFolder, "*.map.gbx", SearchOption.TopDirectoryOnly).ToList();
        List<string> destinationFiles = Directory.GetFiles(destinationFolder, "*.map.gbx", SearchOption.TopDirectoryOnly).ToList();
        foreach (string sourceFile in sourcefiles) {
            string? match = destinationFiles.Find(file => Path.GetFileNameWithoutExtension(file) == Path.GetFileNameWithoutExtension(sourceFile));
            if (match != null) {
                Map sourceMap = new Map(sourceFile);
                Map destinationMap = new Map(match);
                destinationMap.map.MapUid = sourceMap.map.MapUid;
                // destinationMap.map.MapInfo.Id = sourceMap.map.MapInfo.Id;
                destinationMap.Save(match);
            }
        }
    }

    public static void ResaveBlock(string BlockPath) {
        Gbx.LZO = new MiniLZO();
        Gbx.ZLib = new ZLib();
        CGameItemModel customBlock = Gbx.Parse<CGameItemModel>(BlockPath);
        customBlock.Save(BlockPath + "resaved.block.gbx");
    }

    public static void LogMaterialInfo(string Folder) {
        // string sourceFolder = Path.Combine(AltertionConfig.DataFolder, "CustomBlocks/Vanilla");
        string destinationFolder = Path.Combine(AlterationConfig.DataFolder, "CustomBlocks","MaterialInfo");
        AutoAlteration.AlterAll([new MaterialInfo()], Folder, destinationFolder, "MaterialInfo");
        File.WriteAllText(Path.Combine(AlterationConfig.devPath, "SurfacePhysicIds.json"), JsonConvert.SerializeObject(MaterialInfo.SurfacePhysicIds));
        File.WriteAllText(Path.Combine(AlterationConfig.devPath, "SurfaceGameplayIds.json"), JsonConvert.SerializeObject(MaterialInfo.SurfaceGameplayIds));
        File.WriteAllText(Path.Combine(AlterationConfig.devPath, "Materials.json"), JsonConvert.SerializeObject(MaterialInfo.materials));
    }

    public static void NonColidableFix() {
        foreach (string file in Directory.GetFiles(Path.Join(AlterationConfig.CustomBlocksFolder, "Vanilla"), "*", SearchOption.AllDirectories))
        {
            Gbx.LZO = new MiniLZO();
            // Gbx.ZLib = new ZLib();
            CustomBlock customBlock = new(file);
            bool changed = false;
            customBlock.MeshCrystals.ForEach(x => {
                //if there are no collidable layers, make first one collidable
                if (!x.Layers.Any(y => y.GetType() == typeof(CPlugCrystal.GeometryLayer) && ((CPlugCrystal.GeometryLayer)y).Collidable)){ 
                    CPlugCrystal.GeometryLayer layer = (CPlugCrystal.GeometryLayer)x.Layers.Where(y => y.GetType() == typeof(CPlugCrystal.GeometryLayer)).First();
                    layer.Collidable = true;
                    changed = true;
                }
            });
            if (changed){
                customBlock.Save(file);
            }
        }
    }

    public static void GenerateAlterationList()
    {
        Dictionary<string, List<Alteration>> AlterationCategories = new() {
            {"Effect Alterations", [new Cruise(),new Fragile(),new FreeWheel(),new Glider(),new NoBrake(),new NoEffect(),new NoSteer(),new RandomDankness(),new RandomEffects(),new Reactor(),new ReactorDown(),new RedEffects(),new RngBooster(),new SlowMo()]},
            {"Environment Alterations", [new Stadium(),new Snow(), new Rally(), new Desert(), new SnowCarswitchToDesert(), new SnowCarswitchToRally()]},
            {"Finish Alterations", [new OneBack(), new OneForward(), new OneDown(), new OneLeft(), new OneRight(), new TwoUP(), new Inclined(), new ThereAndBack()]},
            {"Gamemode Alterations", [new Race(), new Stunt(), new Platform()]},
            {"Surface Alterations", [new Dirt(), new Grass(), new Ice(), new Magnet(), new Penalty(), new Plastic(), new Road(), new Wood(), new Bobsleigh(), new Sausage(), new Surfaceless(), new RouteOnly()]},
        };

        Assembly assembly = Assembly.GetExecutingAssembly();
        List<Type> types = assembly.GetTypes().ToList();
        List<Alteration> alterations = types.Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Alteration))).Select(t => Activator.CreateInstance(t) as Alteration ?? throw new Exception("Alteration couldnt be instantiated")).Where(a => a.Published).ToList();
        Dictionary<string, List<string>> CategoryTexts = [];
        foreach (Alteration alteration in alterations)
        {
            string? category = null;
            foreach (KeyValuePair<string, List<Alteration>> pair in AlterationCategories)
            {
                if (pair.Value.Select(x => x.GetType()).Contains(alteration.GetType())){
                    category = pair.Key;
                    break;
                }
            }
            category ??= "Other Alterations";
            if (!CategoryTexts.TryGetValue(category, out List<string>? value))
            {
                value = [];
                CategoryTexts.Add(category, value);
            }

            value.Add(
                "- <span title=\"" + alteration.Description + "\">"+alteration.GetType().Name + (!alteration.LikeAN ? "*" : "") + (!alteration.Complete ? "/": "") +"</span>"
            );
        }
        string text = "";
        foreach (KeyValuePair<string, List<string>> category in CategoryTexts)
        {
            text += "<strong>" + category.Key + "</strong>\n";
            text += string.Join("\n", category.Value) + "\n\n";
        }
        File.WriteAllText(Path.Combine(AlterationConfig.devPath, "AlterationList.md"), text);
    }

    public static void generateLightSurfaceBlocks() {
        AutoAlteration.AlterAll(new LightSurfaceBlock(), Path.Join(AlterationConfig.CustomBlocksFolder, "HeavySurface"), Path.Join(AlterationConfig.CustomBlocksFolder, "LightSurface"), "");
    }

    public static void renameMaps(string folder,string remove,string add) {
        List<string> sourcefiles = Directory.GetFiles(folder, "*.map.gbx", SearchOption.TopDirectoryOnly).ToList();
        foreach (string sourceFile in sourcefiles) {
            Map map = new Map(sourceFile);
            string name = map.map.MapName;
            name = name.Replace(remove, add);
            map.map.MapName = name;
            map.Save(sourceFile.Replace(remove, add));
        }
        
    }
}

class CustomBlockAirTest : Alteration {
    // int id = 1000000; 
    //known options: 1000, 1000000, 1001000, 1002000
    // 100100 is good for most to have correct model, (Exception ex. dirtroad curve2)
    // 1002001 bad for all, 
    // 1000 bad for all
    // 1000000 good for all if air-mode false, (only known exception: RoadTechTiltTransition2Up1LeftChicane)
    // last digit doesn't really matter
    internal override List<CustomBlockAlteration> customBlockAlts => [new WoodSurface()];
    protected override void Run(Inventory inventory, Map map)
    {
        Inventory platform = inventory.Select("Platform");
        platform.Edit().RemoveKeyword(["Grass","Dirt","Plastic","Ice","Tech"]).AddKeyword(["Plastic","WoodSurfaceHeavy"]).PlaceRelative(inventory,map,new Offset(new Vec3(0,100,0)));
        (inventory/platform).Edit().AddKeyword(["WoodSurfaceHeavy"]).PlaceRelative(inventory,map,new Offset(new Vec3(0,100,0)));
        map.stagedBlocks.ForEach(block => block.IsAir = false);
        map.PlaceStagedBlocks(false);
        // turns out to be not in air mode and not wood
        platform.Edit().RemoveKeyword(["Grass","Dirt","Plastic","Ice","Tech"]).AddKeyword(["Plastic","WoodSurfaceHeavy"]).Replace(inventory,map);
        (inventory/platform).Edit().AddKeyword(["WoodSurfaceHeavy"]).Replace(inventory,map);
        map.stagedBlocks.ForEach(block => block.IsAir = true);
        // map.stagedBlocks.ForEach(block => block. = false);
        map.PlaceStagedBlocks(false);
        // turns out to be in airmode and as wood

        // seams like IsAir = true + id = 1001000 is working
    }
}

class Replace : Alteration {
    protected override void Run(Inventory inventory, Map map)
    {
        map.StageAll(inventory);
        map.PlaceStagedBlocks();
    }
}

class Test : Alteration {
    protected override void Run(Inventory inventory, Map map)
    {
        inventory.Edit().Replace(inventory,map,new Offset(16,0,0));//Test if Rotation is before or after Move
        map.PlaceStagedBlocks();
    }
}

class Nothing : Alteration {
    protected override void Run(Inventory inventory, Map map)
    {
    }
}

class NothingBlock : CustomBlockAlteration {
    public override bool Run(CustomBlock customBlock) { return true; }
}

class TestReEmbed()
{
    public static void testReEmbed()
    {
        // //reembeding: works
        // Map map = new Map("C:/Users/Tobias/Documents/Trackmania2020/Maps/Test/TestEmbed.map.gbx");
        // map.ExtractEmbeddedBlocks("C:/Users/Tobias/Documents/Programmieren/AutoAlteration/data/CustomBlocks/dev/TestEmbed");
        // string path = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/data/CustomBlocks/dev/TestEmbed/Test/RoadTechStraightWoodEdit.Block.Gbx";
        // map.map.EmbeddedZipData = [];
        // map.EmbedBlock("Test/RoadTechStraightWoodEdit.Block.Gbx",path);
        // map.Save("C:/Users/Tobias/Documents/Trackmania2020/Maps/Test/A01-Race Test.map.gbx");

        // resaving block: resaved block cant be used anymore
        Map map = new Map("C:/Users/Tobias/Documents/Trackmania2020/Maps/Test/TestEmbed.map.gbx");
        map.ExtractEmbeddedBlocks("C:/Users/Tobias/Documents/Programmieren/AutoAlteration/data/CustomBlocks/dev/TestEmbed");
        string path = "C:/Users/Tobias/Documents/Programmieren/AutoAlteration/data/CustomBlocks/dev/TestEmbed/Test/RoadTechStraightWoodEdit.Block.Gbx";
        Gbx gbx = Gbx.Parse<CGameItemModel>(path);
        gbx.Save(path + "resaved.block.gbx");
    }
}