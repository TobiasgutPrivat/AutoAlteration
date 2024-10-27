using GBX.NET;
using Newtonsoft.Json;
public abstract class Alteration: PosUtils {
    public abstract void Run(Map map);

    public virtual string? Description { get; }

    public virtual List<InventoryChange> InventoryChanges { get; } = [];

    public static Inventory inventory = new();
    
    public static Inventory ImportVanillaInventory(string path){
        string json = File.ReadAllText(path);

        json = BlockDataCorrections(json);
        
        var jsonArray = JsonConvert.DeserializeObject<dynamic[]>(json);
        Inventory temp = new();
        foreach (var item in jsonArray)
        {
            temp.articles.Add(new Article((int)item.Height, (int)item.Width, (int)item.Length, (string)item.type, (string)item.Name, (bool)item.Theme, (bool)item.DefaultRotation));
        }
        return temp;
    }

    public static string BlockDataCorrections(string json){
        json = json.Replace("PlatformGrassSlope2UTop", "PlatformGrasssSlope2UTop");
        json = json.Replace("PlatForm", "Platform");
        json = json.Replace("ShowFogger8M", "ShowFogger8m");
        json = json.Replace("ShowFogger16M", "ShowFogger16m");
        json = json.Replace("CheckPoint", "Checkpoint");
        json = json.Replace("DecoHillSlope2curve2Out", "DecoHillSlope2Curve2Out");
        json = json.Replace("RoadIceWithWallDiagLeftStraight", "RoadIceDiagLeftWithWallStraight");
        json = json.Replace("RoadIceWithWallDiagRightStraight", "RoadIceDiagRightWithWallStraight");
        json = json.Replace("\"GateSpecialBoost\"", "\"GateSpecialBoostOriented\"");
        json = json.Replace("\"GateSpecialBoost2\"", "\"GateSpecialBoost2Oriented\"");
        return json[..1] + "{\"Height\":1,\"Width\":1,\"Length\":1,\"type\":\"Block\",\"Name\":\"OpenIceRoadToZoneRight\",\"Theme\":false,\"DefaultRotation\":false}," + json[1..];
    }
    
    public static void CreateInventory() {
        inventory = ImportVanillaInventory(Path.Combine(AutoAlteration.DataFolder, "Inventory","BlockData.json"));
        inventory.articles.ForEach(x => x.cacheFilter.Clear());
        if (AutoAlteration.devMode){
            inventory.Export("Vanilla");
        }
    }

    public static void DefaultInventoryChanges(){
        inventory.Select(BlockType.Block).Select("Gate").EditOriginal().RemoveKeyword("Gate").AddKeyword("Ring");
        inventory.Select("Special").EditOriginal().RemoveKeyword("Special");
        inventory.Select("Start&!(Slope2|Loop|DiagRight|DiagLeft|Slope|Inflatable)").EditOriginal().RemoveKeyword("Start").AddKeyword("MapStart");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Straight").Align().EditOriginal().RemoveKeyword("Straight");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("StraightX2").Align().EditOriginal().RemoveKeyword("StraightX2");
        inventory.Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Base").Align().EditOriginal().RemoveKeyword("Base");
        inventory.RemoveArticles(inventory.Select("v2").RemoveKeyword("v2").Align());
        inventory.Select("v2").EditOriginal().RemoveKeyword("v2");
        inventory.Select("Oriented").EditOriginal().RemoveKeyword("Oriented");
    }
}
