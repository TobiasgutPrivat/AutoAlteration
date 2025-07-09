using Newtonsoft.Json;

class ArticleImport {

    public static List<Article> ImportVanillaInventory(){
        string json = File.ReadAllText(AlterationConfig.BlockPropertiesPath);
        // expected json Format:
        // [
        //     {
        //         "size": {
        //             "x": int,
        //             "y": int,
        //             "z": int
        //         },
        //         "type": "block"/"item"/"pillar"
        //         "name": string,
        //     },
        //     ...
        // ]

        json = BlockPropertiesCorrections(json);
        
        var jsonArray = JsonConvert.DeserializeObject<dynamic[]>(json);
        Dictionary<string,Article> articles = [];
        foreach (var item in jsonArray)
        {
            BlockType blockType = BlockType.Block;
            switch ((string)item.type)
            {
                case "block":
                    articles[(string)item.name] = new Article((int)item.size.z, (int)item.size.y, (int)item.size.x, BlockType.Block, (string)item.name);
                    foreach (var pillar in item.pillar)
                    {
                        if (articles.ContainsKey((string)pillar.name)) continue;
                        articles[(string)pillar.name] = new Article((int)pillar.size.z, (int)pillar.size.y, (int)pillar.size.x, BlockType.Pillar, (string)pillar.name);
                    }
                    break;
                case "item":
                    articles[(string)item.name] = new Article((int)item.size.z, (int)item.size.y, (int)item.size.x, BlockType.Item, (string)item.name);
                    break;
                default:
                    Console.WriteLine("Blocktype missing");
                    break;
            }
            
        }
        return articles.Values.ToList();
    }
    
    public static string BlockPropertiesCorrections(string json) { //Hardcoded corrections, depends on imported Data
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
        return json;
    }
}