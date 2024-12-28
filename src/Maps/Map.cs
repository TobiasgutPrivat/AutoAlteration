using GBX.NET;
using GBX.NET.Engines.Game;
using System.IO.Compression;

public class Map
{
  Gbx<CGameCtnChallenge> gbx;
  public CGameCtnChallenge map;
  public List<Block> stagedBlocks = [];
  public List<string> embeddedBlocks = [];

  private Replay WRReplay;

  #region loading
  public Map(string mapPath)
  { 
    gbx = Gbx.Parse<CGameCtnChallenge>(mapPath);
    map = gbx.Node;
    map.Chunks.Get<CGameCtnChallenge.Chunk03043040>().Version = 4;
    
    embeddedBlocks = GetEmbeddedBlocks();
  }

  public void Save(string path)
  { 
    NewMapUid();
    RemoveAuthor();
    map.RemovePassword();
    if (!Directory.Exists(Path.GetDirectoryName(path)))
      {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
      }
    gbx.Save(path);
  }

  private void RemoveAuthor(){
    map.AuthorTime = null;
    map.GoldTime = null;
    map.SilverTime = null;
    map.BronzeTime = null;
    map.AuthorScore = 0;
    map.AuthorExtraInfo = null;
    map.AuthorLogin = null;
    map.AuthorNickname = null;
    map.AuthorVersion = 0;
    map.AuthorZone = null;
  }

  private void NewMapUid()
  {
    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
    var stringChars = new char[27];
    var random = new Random();

    for (int i = 0; i < stringChars.Length; i++)
    {
      stringChars[i] = chars[random.Next(chars.Length)];
    }
    map.MapUid = new string(stringChars);
  }
  #endregion

  #region embedding
  private void EmbedBlock(string name, string path){
    map.UpdateEmbeddedZipData((ZipArchive zipArchive) =>
    {
      ZipArchiveEntry entry = zipArchive.CreateEntry(name);
        using Stream entryStream = entry.Open();
        using FileStream fileStream = File.OpenRead(path);
        fileStream.CopyTo(entryStream);
    });
  }

  public List<string> GetEmbeddedBlocks(){
    if (map.EmbeddedZipData != null && map.EmbeddedZipData.Length > 0) {
      ZipArchive zipArchive = map.OpenReadEmbeddedZipData();
      return zipArchive.Entries.Select(x => x.FullName).Where(x => x.Contains(".Block.Gbx") || x.Contains(".Item.Gbx")).ToList();
    } else {
      return [];
    }
  }

  private void ExtractEmbeddedBlocks(string path){
    ZipArchive zipArchive = map.OpenReadEmbeddedZipData();
    zipArchive.Entries.ToList().ForEach(x => {
      string filePath = path + "\\" + x.Name; // Extract without Folderstructure (FullName)
      Directory.CreateDirectory(Path.GetDirectoryName(filePath));
      x.ExtractToFile(filePath);
    });
  }

  // private void FlattenEmbeddedBlocks(){
  //   ZipArchive zipArchive = map.OpenReadEmbeddedZipData();
  //   zipArchive.Entries.ToList().ForEach(x => {
  //     //Flatten customblocks
  //     if (x.FullName.Contains("Blocks")) x.ExtractToFile(Path.Join(AltertionConfig.CustomBlocksFolder,x.FullName.Replace("Blocks\\","").Replace("Blocks/","")));
  //   });
  //   //Flatten references to those blocks
  //   map.Blocks.ToList().Where(x => x.BlockModel.Id.Contains('\\') || x.BlockModel.Id.Contains('/')).ToList().ForEach(x => 
  //     x.BlockModel = new (){Id = x.BlockModel.Id.Split('\\','/').Last(), Author = x.BlockModel.Author, Collection = x.BlockModel.Collection}
  //     );
  //   map.AnchoredObjects.ToList().Where(x => x.ItemModel.Id.Contains('\\') || x.ItemModel.Id.Contains('/')).ToList().ForEach(x => 
  //     x.ItemModel = new (){Id = x.ItemModel.Id.Split('\\','/').Last(), Author = x.ItemModel.Author, Collection = x.ItemModel.Collection}
  //     );
  // }

  public void GenerateCustomBlocks(CustomBlockAlteration customBlockAlteration){
    string TempFolder = Path.Join(AltertionConfig.CustomBlocksFolder,"Temp");
    string CustomFolder = Path.Join(TempFolder,customBlockAlteration.GetType().Name);
    string TempExportsFolder = Path.Join(AltertionConfig.CustomBlocksFolder,"Exports");
    if (!Directory.Exists(TempFolder)) { Directory.CreateDirectory(TempFolder); }
    if (!Directory.Exists(CustomFolder)) { Directory.CreateDirectory(CustomFolder); }
    if (!Directory.Exists(TempExportsFolder)) { 
      Directory.CreateDirectory(TempExportsFolder);
      ExtractEmbeddedBlocks(TempFolder);
    }
    AutoAlteration.AlterAll(customBlockAlteration,TempExportsFolder,CustomFolder,customBlockAlteration.GetType().Name);
    new CustomBlockFolder("Temp\\" + customBlockAlteration.GetType().Name).ChangeInventory(Alteration.inventory,true);
  }
  #endregion

  #region actions
  public void PlaceRelativeWithRandom(Article atBlock, Inventory newInventory,MoveChain ?moveChain = null){
    List<Article> newArticles = newInventory.articles;
    if (newArticles.Count == 0) return;
    Random rand = new();
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == atBlock.Name)){
      stagedBlocks.Add(new Block(ctnBlock,atBlock,newArticles[rand.Next(newArticles.Count)],moveChain));
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == atBlock.Name)){
      stagedBlocks.Add(new Block(ctnItem,atBlock,newArticles[rand.Next(newArticles.Count)],moveChain));
    }
  }

  public void PlaceRelative(Article atArticle, Article newArticle, MoveChain ?moveChain = null){
    string ArticleName = atArticle.Name;
    if (atArticle.Type == BlockType.CustomBlock) ArticleName = atArticle.Name + ".Block.Gbx";
    if (atArticle.Type == BlockType.CustomItem) ArticleName = atArticle.Name + ".Item.Gbx";
    ArticleName = ArticleName.Replace("/","\\");
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == ArticleName)){//TODO issue with customblocks having blockmodel.id as path
      stagedBlocks.Add(new Block(ctnBlock,atArticle,newArticle,moveChain));
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == ArticleName)){
      stagedBlocks.Add(new Block(ctnItem,atArticle,newArticle,moveChain));
    }
  }

  public void PlaceRelative(Inventory inventory, Article newArticle, MoveChain ?moveChain = null)=>
    inventory.articles.ForEach(a => PlaceRelative(a, newArticle, moveChain));
  
  public void ReplaceWithRandom(Article oldArticle, Inventory newInventory,MoveChain ?moveChain = null){
    if (newInventory.articles.Count == 0) return;
    PlaceRelativeWithRandom(oldArticle, newInventory, moveChain);
    Delete(oldArticle);
  }

  public void Replace(Article oldArticle, Article article, MoveChain ?moveChain = null){
    PlaceRelative(oldArticle, article,moveChain);
    Delete(oldArticle);
  }
  
  public void Replace(Inventory inventory, Article article, MoveChain ?moveChain = null){
    PlaceRelative(inventory, article,moveChain);
    Delete(inventory);
  }

  public void Move(Article article, MoveChain moveChain) =>
    Replace(article, article, moveChain);

  public void Move(Inventory inventory, MoveChain moveChain) =>
    inventory.articles.ForEach(a => Move(a, moveChain));

  public void Delete(Article Block, bool includePillars = false){
    string ArticleName = Block.Name;
    if (Block.Type == BlockType.CustomBlock) ArticleName = Block.Name + ".Block.Gbx";
    if (Block.Type == BlockType.CustomItem) ArticleName = Block.Name + ".Item.Gbx";
    ArticleName = ArticleName.Replace("/","\\");
    List<CGameCtnBlock> deleted = map.Blocks.Where(block => block.BlockModel.Id == ArticleName).ToList();
    map.Blocks = map.Blocks.Where(block => block.BlockModel.Id != ArticleName).ToList();
    if (includePillars){
      deleted.ForEach(x => {
        if (!x.IsFree && !x.IsGround){
          map.Blocks = map.Blocks.Where(block => 
            !(block.BlockModel.Id.Contains("Pillar") &&
              block.Coord.X == x.Coord.X &&
              block.Coord.Z == x.Coord.Z &&
              block.Coord.Y < x.Coord.Y
            )
          ).ToList();
        }
      });
    };

    map.AnchoredObjects = map.AnchoredObjects.Where(block => block.ItemModel.Id != ArticleName).ToList();
  }
  public void Delete(Inventory inventory, bool includePillars = false){
    foreach(var block in inventory.articles){
      Delete(block,includePillars);
    }
  }
  #endregion

  #region placement
  public void PlaceStagedBlocks(){
    foreach (var block in stagedBlocks){
      PlaceBlock(block);
    } 
    stagedBlocks = [];
  }

  private void PlaceBlock(Block block){ // expects the block to be deleted after placing -> could cause nameing issues if not
    block.name = block.name.TrimStart('\\');
    switch (block.blockType){
        case BlockType.CustomBlock:
          if(!embeddedBlocks.Any(x => x == block.name)){
            EmbedBlock("Blocks\\" + block.name + ".Block.Gbx",block.Path);
            embeddedBlocks.Add(block.name);
          }
          block.name += ".Block.Gbx_CustomBlock";
          // PlaceTypeBlock(block);
          break;
        case BlockType.CustomItem:
          block.name = (block.name.Split('\\').Last() + ".Item.Gbx").Replace("\\","/");
          if(!embeddedBlocks.Any(x => x == "Items/" + block.name)){
            EmbedBlock("Items/" + block.name,block.Path);
            embeddedBlocks.Add("Items/" + block.name);
          }
          // block.name += ".Item.Gbx";
          break;
      }

    block.PlaceInMap(map);
  }
  #endregion

  public Replay GetWRReplay(){
    //AR
    //download the replay

    return new Replay("somePath");
  }
}