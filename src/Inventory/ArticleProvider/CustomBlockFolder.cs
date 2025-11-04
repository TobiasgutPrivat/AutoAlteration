class CustomBlockFolder(string subFolder) : ArticleProvider
{
    public readonly string folder = Path.Combine(AlterationConfig.CustomBlocksFolder, subFolder);
    protected override List<Article> GenerateArticles()
    {
        List<Article> articles = [];
        articles.AddRange(Directory.GetFiles(folder, "*.Block.Gbx", SearchOption.AllDirectories).ToList().Select(x =>
            new Article(x.Replace(folder, ""), BlockType.CustomBlock, x)));

        articles.AddRange(Directory.GetFiles(folder, "*.Item.Gbx", SearchOption.AllDirectories).ToList().Select(x =>
            new Article(x.Replace(folder, ""), BlockType.CustomItem, x)));

        return articles;
    }
}
