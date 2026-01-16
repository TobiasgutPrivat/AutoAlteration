class TMNFArticleProvider() : ArticleProvider("TMNF")
{
    protected override List<Article> GenerateArticles() { return []; }

    public override void EmbeddedChanges(Inventory inventory) 
    {
        // Create Checkpoint Triggers
        inventory.AddRange(inventory.Select("Checkpoint").Edit().RemoveKeyword("Checkpoint").AddKeyword("CheckpointTrigger").SetChain([new Offset(16, 0, 16)]).getEdited());
    }
}
