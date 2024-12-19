using Newtonsoft.Json;

class ArticleImport {

    public static List<Article> ImportVanillaInventory(){
        string json = File.ReadAllText(AltertionConfig.BlockDataPath);
        // expected json Format:
        // [
        //     {
        //         "Height": int,
        //         "Width": int,
        //         "Length": int,
        //         "type": "Block"/"Item"/"Pillar"
        //         "Name": string,
        //         "Theme": bool,
        //         "DefaultRotation": bool //TODO could be cause for additional rotation of some blocks
        //     },
        //     ...
        // ]

        json = BlockDataCorrections(json);
        
        var jsonArray = JsonConvert.DeserializeObject<dynamic[]>(json);
        List<Article> articles = [];
        foreach (var item in jsonArray)
        {
            switch (item.type) {
                case "Block":
                    articles.Add(new BlockArticle((int)item.Height, (int)item.Width, (int)item.Length, (string)item.Name, (bool)item.Theme, (bool)item.DefaultRotation));
                    break;
                case "Item":
                    articles.Add(new ItemArticle((int)item.Height, (int)item.Width, (int)item.Length, (string)item.Name, (bool)item.Theme, (bool)item.DefaultRotation));
                    break;
                case "Pillar":
                    articles.Add(new PillarArticle((int)item.Height, (int)item.Width, (int)item.Length, (string)item.Name, (bool)item.Theme, (bool)item.DefaultRotation));
                    break;
            }
        }
        return articles;
    }

    public static string BlockDataCorrections(string json){ //Hardcoded corrections, depends on imported Data
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
}