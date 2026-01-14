class EmbeddedProvider(Map map) : ArticleProvider()
{
    protected override string GetOrigin() { return Path.Join(Path.GetTempPath(), "AutoAlteration"); }

    public override List<Article> GetAlteredArticles(CustomBlockAlteration customBlockAlteration) {
        // cleanup previous
        string Name = customBlockAlteration.GetType().Name;
        string folder = Path.Combine(AlterationConfig.CacheFolder, Name);
        if (Directory.Exists(folder)) Directory.Delete(folder, true);
        if (Directory.Exists(GetOrigin())) Directory.Delete(GetOrigin(), true);
        //extrect embedded blocks before alteration
        map.ExtractEmbeddedBlocks(GetOrigin());
        return base.GetAlteredArticles(customBlockAlteration);
    }
    protected override List<Article> GenerateArticles()
    {
        return map.embeddedBlocks.Select(x => new Article(x.Key, x.Value, "")).ToList();
    }
}
