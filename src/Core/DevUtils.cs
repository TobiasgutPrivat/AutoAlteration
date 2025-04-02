using System.Reflection;
using GBX.NET;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
using GBX.NET.LZO;
using GBX.NET.ZLib;
using Newtonsoft.Json;

class DevUtils{
    public static void ResaveBlock(string BlockPath) {
        Gbx.LZO = new MiniLZO();
        Gbx.ZLib = new ZLib();
        CGameItemModel customBlock = Gbx.Parse<CGameItemModel>(BlockPath);
        customBlock.Save(BlockPath + "resaved.block.gbx");
    }

    public static void LogMaterialInfo(string Folder) {
        // string sourceFolder = Path.Combine(AltertionConfig.DataFolder, "CustomBlocks/Vanilla");
        string destinationFolder = Path.Combine(AltertionConfig.DataFolder, "CustomBlocks","MaterialInfo");
        AutoAlteration.AlterAll([new MaterialInfo()], Folder, destinationFolder, "MaterialInfo");
        File.WriteAllText(Path.Combine(AltertionConfig.devPath, "SurfacePhysicIds.json"), JsonConvert.SerializeObject(MaterialInfo.SurfacePhysicIds));
        File.WriteAllText(Path.Combine(AltertionConfig.devPath, "SurfaceGameplayIds.json"), JsonConvert.SerializeObject(MaterialInfo.SurfaceGameplayIds));
        File.WriteAllText(Path.Combine(AltertionConfig.devPath, "Materials.json"), JsonConvert.SerializeObject(MaterialInfo.materials));
    }

    public static void StringToName(string projectFolder) {
        string json = File.ReadAllText(projectFolder + "data/Vanilla/Items.json");
        SList<string> lines = JsonConvert.DeserializeObject<SList<string>>(json);
        SList<string> articles = lines.Select(line => line.Split('/')[^1].Trim()).ToList();
        json = JsonConvert.SerializeObject(articles);
        File.WriteAllText(projectFolder + "data/Vanilla/ItemNames.json", json);
    }

    public static void NonColidableFix() {
        foreach (string file in Directory.GetFiles(Path.Join(AltertionConfig.CustomBlocksFolder, "Vanilla"), "*", SearchOption.AllDirectories))
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

    public static void TestInventory(){
        Alteration.CreateInventory();
        new CustomBlockFolder("").ChangeInventory(Alteration.inventory);
        new NoCPBlocks().ChangeInventory(Alteration.inventory);
        new CheckpointTrigger().ChangeInventory(Alteration.inventory);
        Alteration.inventory.CheckDuplicates();
        Alteration.inventory.articles.ForEach(x => {
            if (x.Keywords.Any(y => y == "")){Console.WriteLine("Empty Keyword found in " + x.Name);}
            if (x.ToShapes.Any(y => y == "")){Console.WriteLine("Empty ToShape found in " + x.Name);}
        });
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
        List<Alteration> alterations = types.Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Alteration))).Select(t => Activator.CreateInstance(t) as Alteration).Where(a => a.Published).ToList();
        Dictionary<string, List<string>> CategoryTexts = new();
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
        File.WriteAllText(Path.Combine(AltertionConfig.devPath, "AlterationList.md"), text);
    }

    public static void generateLightSurfaceBlocks() {
        AutoAlteration.AlterAll(new LightSurfaceBlock(), Path.Join(AltertionConfig.CustomBlocksFolder, "HeavySurface"), Path.Join(AltertionConfig.CustomBlocksFolder, "LightSurface"), "");
    }
}

class CustomBlockAirTest : Alteration {
    int id = 1000000; 
    //known options: 1000, 1000000, 1001000, 1002000
    // 100100 is good for most to have correct model, (Exception ex. dirtroad curve2)
    // 1002001 bad for all, 
    // 1000 bad for all
    // 1000000 good for all if air-mode false, (only known exception: RoadTechTiltTransition2Up1LeftChicane)
    // last digit doesn't really matter
    public override List<InventoryChange> InventoryChanges => [new HeavySurface(new WoodSurface())];
    public override void Run(Map map)
    {
        Inventory platform = inventory.Select("Platform");
        platform.RemoveKeyword(["Grass","Dirt","Plastic","Ice","Tech"]).AddKeyword(["Plastic","WoodSurfaceHeavy"]).PlaceRelative(map,Move(new Vec3(0,100,0)));
        (!platform).AddKeyword(["WoodSurfaceHeavy"]).PlaceRelative(map,Move(new Vec3(0,100,0)));
        map.stagedBlocks.ForEach(block => block.IsAir = false);
        map.PlaceStagedBlocks(false);
        // turns out to be not in air mode and not wood
        platform.RemoveKeyword(["Grass","Dirt","Plastic","Ice","Tech"]).AddKeyword(["Plastic","WoodSurfaceHeavy"]).Replace(map);
        (!platform).AddKeyword(["WoodSurfaceHeavy"]).Replace(map);
        map.stagedBlocks.ForEach(block => block.IsAir = true);
        // map.stagedBlocks.ForEach(block => block. = false);
        map.PlaceStagedBlocks(false);
        // turns out to be in airmode and as wood

        // seams like IsAir = true + id = 1001000 is working
    }
}

class Replace : Alteration {
    public override void Run(Map map)
    {
        map.StageAll();
        map.PlaceStagedBlocks();
    }
}

class Test : Alteration {
    public override void Run(Map map)
    {
        inventory.Edit().Replace(map,Move(16,0,0));//Test if Rotation is before or after Move
        map.PlaceStagedBlocks();
    }
}

class Nothing : Alteration {
    public override void Run(Map map)
    {
    }
}

class FixAutoRotation : Alteration {
    public override void Run(Map map)
    {
        Inventory DecoWall = inventory.Select("DecoWall");
        map.Move(DecoWall.Select("LoopEnd&!Center&!Side"), RotateMid(PI*0.5f,0,0));
        map.Move(DecoWall.Select("Arch&Slope2&(UTop|End|Straight)"), RotateMid(PI*0.5f,0,0));//not sure if Straight is correct
        map.PlaceStagedBlocks();
    }
}

class NothingBlock : CustomBlockAlteration {
    public override bool Run(CustomBlock customBlock) { return true; }
}

class EmbedTest : Alteration {
    public override List<InventoryChange> InventoryChanges => [new CustomBlockSet(new NothingBlock())];
    public override void Run(Map map){
        Inventory specific = inventory.Select(article => article.MapSpecific);
        (!specific).Select("Platform").RemoveKeyword(["Grass","Dirt","Plastic","Ice","Tech"]).AddKeyword(["Plastic","NothingBlockHeavy"]).Replace(map);
        (!specific).AddKeyword(["NothingBlockHeavy"]).Replace(map);
        specific.AddKeyword(["NothingBlock"]).Replace(map);
        map.stagedBlocks.ForEach(x => x.IsAir = false);
        map.PlaceStagedBlocks(false);
    }
}

class AirPillars : Alteration {
    public override void Run(Map map){
        inventory.Select(BlockType.Pillar).Edit().Replace(map);
        map.PlaceStagedBlocks(false);
    }    
}