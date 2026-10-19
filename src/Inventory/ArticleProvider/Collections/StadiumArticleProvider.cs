using GBX.NET;
using Newtonsoft.Json;

class StadiumArticleProvider() : ArticleProvider("VanillaItems")
{
    private static readonly float PI = (float)Math.PI;
    
    protected override List<Article> GenerateArticles()
    {
        var blockJson = File.ReadAllText(Path.Combine(AlterationConfig.DataFolder, "Inventory", "Collections", "Stadium", "BlockData.json"));
        var itemJson = File.ReadAllText(Path.Combine(AlterationConfig.DataFolder, "Inventory", "Collections", "Stadium", "ItemData.json"));
        
        blockJson = CollectionProviderUtility.NormalizeBlockData(blockJson);
        itemJson = CollectionProviderUtility.NormalizeItemData(itemJson);

        dynamic[]? BlockJsonArray = JsonConvert.DeserializeObject<dynamic[]>(blockJson) ?? [];
        dynamic[]? ItemJsonArray = JsonConvert.DeserializeObject<dynamic[]>(itemJson) ?? [];

        var jsonArray = BlockJsonArray.Concat(ItemJsonArray);
        Dictionary<string, Article> articleDict = [];
        foreach (var item in jsonArray)
        {
            switch ((string)item.type)
            {
                case "block":
                    articleDict[(string)item.name] = new Article((int)item.size.y, (int)item.size.x, (int)item.size.z, BlockType.Block, (string)item.name);
                    foreach (var pillar in item.pillar)
                    {
                        if (articleDict.ContainsKey((string)pillar.name)) continue;
                        //TODO fix sizes of pillars in data, temporarly using item size
                        articleDict[(string)pillar.name] = new Article((int)item.size.y, (int)item.size.x, (int)item.size.z, BlockType.Pillar, (string)pillar.name);
                    }
                    break;
                case "item":
                    articleDict[(string)item.name] = new Article((int)item.size.y, (int)item.size.x, (int)item.size.z, BlockType.Item, (string)item.name);
                    break;
                default:
                    Console.WriteLine("Blocktype missing");
                    break;
            }

        }
        List<Article> articles = [.. articleDict.Values];
        Inventory inventory = [.. articles];
        articles.AddRange(CollectionProviderUtility.CreateCheckpointTriggers(inventory));
        articles.AddRange(CollectionProviderUtility.CreateNonCheckpointBlocks());
        return articles;
    }

     protected override void PerformInventoryChanges(Inventory inventory) =>  CollectionProviderUtility.PerformInventoryChanges(inventory);
}