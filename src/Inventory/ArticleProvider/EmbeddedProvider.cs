class EmbeddedProvider(Map map) : ArticleProvider()
{
    private string TempFolder = Path.Join(Path.GetTempPath(), "AutoAlteration");

    protected override string GetOrigin() { return Path.Combine(); }

    public override List<Article> GetAlteredArticles(CustomBlockAlteration customBlockAlteration) {
        string Name = customBlockAlteration.GetType().Name;
        string folder = Path.Combine(AlterationConfig.CacheFolder, Name);
        if (Directory.Exists(folder)) Directory.Delete(folder, true);
        map.ExtractEmbeddedBlocks(GetOrigin());
        return base.GetAlteredArticles(customBlockAlteration);
    }
    protected override List<Article> GenerateArticles()
    {
        return map.embeddedBlocks.Select(x => new Article(x.Key, x.Value, "")).ToList();
    }
}
