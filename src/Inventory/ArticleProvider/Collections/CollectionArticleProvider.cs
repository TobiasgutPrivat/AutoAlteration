
using Newtonsoft.Json;

abstract class CollectionArticleProvider(Collection collection) : ArticleProvider("VanillaItems")
{

    protected override List<Article> GenerateArticles()
    {
        var collectionPath = Path.Combine(
            AlterationConfig.DataFolder,
            "Inventory",
            "Collections",
            collection.ToString()
        );

        var blockJson = File.ReadAllText(Path.Combine(collectionPath, "BlockData.json"));
        var itemJson = File.ReadAllText(Path.Combine(collectionPath, "ItemData.json"));

        blockJson = CollectionProviderUtility.NormalizeBlockData(blockJson);
        itemJson = CollectionProviderUtility.NormalizeItemData(itemJson);

        var blocks = JsonConvert.DeserializeObject<dynamic[]>(blockJson) ?? [];
        var items = JsonConvert.DeserializeObject<dynamic[]>(itemJson) ?? [];

        Dictionary<string, Article> articleDict = [];

        foreach (var item in blocks.Concat(items))
        {
            switch ((string)item.type)
            {
                case "block":
                    articleDict[(string)item.name] = CreateArticle(item, BlockType.Block);

                    foreach (var pillar in item.pillar)
                    {
                        string pillarName = (string)pillar.name;

                        if (articleDict.ContainsKey(pillarName))
                        {
                            continue;
                        }

                        // TODO: Use the actual pillar dimensions from the data.
                        articleDict[pillarName] = CreateArticle(item, BlockType.Pillar, pillarName);
                    }

                    break;

                case "item":
                    articleDict[(string)item.name] = CreateArticle(item, BlockType.Item);
                    break;

                default:
                    Console.WriteLine($"Unknown collection entry type: {item.type}");
                    break;
            }
        }

        List<Article> articles = [.. articleDict.Values];
        Inventory inventory = [.. articles];

        articles.AddRange(CollectionProviderUtility.CreateCheckpointTriggers(inventory));
        articles.AddRange(CollectionProviderUtility.CreateNonCheckpointBlocks());

        return articles;
    }

    protected override void PerformInventoryChanges(Inventory inventory)
    {
        CollectionProviderUtility.PerformInventoryChanges(inventory);
    }

    private static Article CreateArticle(dynamic item, BlockType type, string? name = null)
    {
        return new Article(
            (int)item.size.y,
            (int)item.size.x,
            (int)item.size.z,
            type,
            name ?? (string)item.name
        );
    }
}
