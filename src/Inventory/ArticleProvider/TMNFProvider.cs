using GBX.NET;

class TMNFArticleProvider() : ArticleProvider("TMNF")
{
    protected override List<Article> GenerateArticles() 
    {
        // get Customblocks from TMNF Folder
        List<Article> articles = base.GenerateArticles(); 
        // Create Checkpoint Triggers
        articles.AddRange(new Inventory(articles).Select("Checkpoint").Edit().RemoveKeyword("Checkpoint").AddKeyword("CheckpointTrigger").SetChain([new Offset(0, 2, 0)]).getEdited());

        return articles;
    }
}
