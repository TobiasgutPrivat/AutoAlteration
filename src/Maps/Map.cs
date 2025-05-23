using GBX.NET;
using GBX.NET.Engines.Game;
using System.IO.Compression;

public class Map
{
  Gbx<CGameCtnChallenge> gbx;
  public CGameCtnChallenge map;
  public List<Block> stagedBlocks = [];
  public Dictionary<string,BlockType> embeddedBlocks = []; //Format: "Blocks/SomeFolder/BlockName.Block.Gbx" -> "SomeFolder\\BlockName" (like name in Inventory)
  public int FreeBlockHeightOffset = 0;

  private Replay WRReplay;

  #region loading
  public Map(string mapPath)
  { 
    gbx = Gbx.Parse<CGameCtnChallenge>(mapPath);
    map = gbx.Node;
    map.Chunks.Get<CGameCtnChallenge.Chunk03043040>().Version = 4;
    FreeBlockHeightOffset = map.DecoBaseHeightOffset*8;
    
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

  public Dictionary<string,BlockType> GetEmbeddedBlocks(){
    if (map.EmbeddedZipData != null && map.EmbeddedZipData.Length > 0) {
      ZipArchive zipArchive = map.OpenReadEmbeddedZipData();
      return zipArchive.Entries.Select(x => x.FullName)
        .Where(x => x.Contains(".Block.Gbx") || x.Contains(".Item.Gbx"))
        .Select(x => {
          BlockType type = x.Contains(".Item.Gbx") ? BlockType.CustomItem : BlockType.CustomBlock;
          string name = x.Replace("/","\\").Replace("Items\\","").Replace("Blocks\\","").Replace(".Item.Gbx","").Replace(".Block.Gbx","");
          return (name,type);})
        .ToDictionary();
    } else {
      return [];
    }
  }

  private void ExtractEmbeddedBlocks(string path){
    ZipArchive zipArchive;
    try { 
      zipArchive = map.OpenReadEmbeddedZipData(); 
    } catch {
      Console.WriteLine("No previously Embedded Blocks"); 
      return; 
    }
    
    zipArchive.Entries.ToList().ForEach(x => {
      string filePath = path + "\\" + x.FullName; // Extract without Folderstructure (FullName)
      Directory.CreateDirectory(Path.GetDirectoryName(filePath));
      x.ExtractToFile(filePath);
    });
    return;
  }

  public void GenerateCustomBlocks(CustomBlockAlteration customBlockAlteration){
    string TempFolder = Path.Join(AlterationConfig.CustomBlocksFolder,"Temp");
    string CustomFolder = Path.Join(TempFolder,customBlockAlteration.GetType().Name);
    string TempExportsFolder = Path.Join(AlterationConfig.CustomBlocksFolder,"Exports");
    if (!Directory.Exists(TempFolder)) { 
      Directory.CreateDirectory(Path.Join(TempFolder, customBlockAlteration.GetType().Name ,"\\Items")); 
      Directory.CreateDirectory(Path.Join(TempFolder, customBlockAlteration.GetType().Name ,"\\Blocks")); 
    }
    if (!Directory.Exists(CustomFolder)) { Directory.CreateDirectory(CustomFolder); }
    if (!Directory.Exists(TempExportsFolder)) { Directory.CreateDirectory(TempExportsFolder); }
    ExtractEmbeddedBlocks(TempExportsFolder);
    AutoAlteration.AlterAll(customBlockAlteration,TempExportsFolder,CustomFolder,customBlockAlteration.GetType().Name);
    new CustomBlockFolder("Temp\\" + customBlockAlteration.GetType().Name + "\\Items").ChangeInventory(Alteration.inventory,true);
    new CustomBlockFolder("Temp\\" + customBlockAlteration.GetType().Name + "\\Blocks").ChangeInventory(Alteration.inventory,true);
  }
  #endregion

  #region actions
  public void PlaceRelativeWithRandom(Article atBlock, Inventory newInventory,MoveChain ?moveChain = null){
    List<Article> newArticles = newInventory.articles;
    if (newArticles.Count == 0) return;
    Random rand = new();
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == atBlock.Name)){
      stagedBlocks.Add(new Block(ctnBlock,atBlock,newArticles[rand.Next(newArticles.Count)],FreeBlockHeightOffset,moveChain));
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == atBlock.Name)){
      stagedBlocks.Add(new Block(ctnItem,atBlock,newArticles[rand.Next(newArticles.Count)],moveChain));
    }
  }

  public void PlaceRelative(Article atArticle, Article newArticle, MoveChain ?moveChain = null, Predicate<CGameCtnBlock>? blockCondition = null){
    string ArticleName = atArticle.Name;
    if (atArticle.Type == BlockType.CustomBlock) ArticleName = atArticle.Name + ".Block.Gbx";
    if (atArticle.Type == BlockType.CustomItem) ArticleName = atArticle.Name + ".Item.Gbx";
    ArticleName = ArticleName.Replace("/","\\");
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == ArticleName)){
      if (blockCondition != null && !blockCondition(ctnBlock)) continue;
      stagedBlocks.Add(new Block(ctnBlock,atArticle,newArticle,FreeBlockHeightOffset,moveChain));
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == ArticleName)){
      stagedBlocks.Add(new Block(ctnItem,atArticle,newArticle,moveChain));
    }
  }

  public void PlaceRelative(Inventory inventory, Article newArticle, MoveChain ?moveChain = null, Predicate<CGameCtnBlock>? blockCondition = null)=> //TODO maybe move to inventory
    inventory.articles.ForEach(a => PlaceRelative(a, newArticle, moveChain, blockCondition));
  
  public void ReplaceWithRandom(Article oldArticle, Inventory newInventory,MoveChain ?moveChain = null){
    if (newInventory.articles.Count == 0) return;
    PlaceRelativeWithRandom(oldArticle, newInventory, moveChain);
    Delete(oldArticle);
  }

  public void Replace(Article oldArticle, Article article, MoveChain ?moveChain = null, Predicate<CGameCtnBlock>? blockCondition = null){
    PlaceRelative(oldArticle, article,moveChain, blockCondition);
    Delete(oldArticle);
  }
  
  public void Replace(Inventory inventory, Article article, MoveChain ?moveChain = null, Predicate<CGameCtnBlock>? blockCondition = null){
    PlaceRelative(inventory, article,moveChain, blockCondition);
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
  public void PlaceStagedBlocks(bool revertFreeBlock = true){
    foreach (var block in stagedBlocks){
      PlaceBlock(block, revertFreeBlock);
    } 
    stagedBlocks = [];
  }

  private void PlaceBlock(Block block, bool revertFreeBlock){
    block.name = block.name.TrimStart('\\');
    string orgName = block.name;

    switch (block.blockType){
        case BlockType.CustomBlock:
          if(!embeddedBlocks.Any(x => x.Key == block.name && x.Value == block.blockType)){
            EmbedBlock("Blocks/" + block.name + ".Block.Gbx",block.Path);
            embeddedBlocks.Add(block.name, block.blockType);
          }
          block.name += ".Block.Gbx_CustomBlock";
          break;
        case BlockType.CustomItem:
          if(!embeddedBlocks.Any(x => x.Key == block.name && x.Value == block.blockType)){
            EmbedBlock("Items/" + block.name + ".Item.Gbx",block.Path);
            embeddedBlocks.Add(block.name, block.blockType);
          }
          block.name += ".Item.Gbx";
          break;
      }

    block.PlaceInMap(map, revertFreeBlock);

    block.name = orgName;
  }
  #endregion

  public Replay GetWRReplay(){
    //AR
    //download the replay

    return new Replay("somePath");
  }

  public void StageAll(MoveChain ?moveChain = null){
    stagedBlocks.AddRange(map.Blocks.Select(x => {
      Article article = Alteration.inventory.GetArticle(x.BlockModel.Id.Replace(".Block.Gbx_CustomBlock",""));
      return new Block(x,article,article,FreeBlockHeightOffset,moveChain);
    }));
    map.Blocks = [];
    stagedBlocks.AddRange(map.AnchoredObjects.Select(x => {
      Article article = Alteration.inventory.GetArticle(x.ItemModel.Id.Replace(".Item.Gbx",""));
      return new Block(x,article,article, moveChain);
    }));
    map.AnchoredObjects = [];
  }

  public void SetBuildDimension(Vec2 Origin, Vec2 Target){ //TODO test this
    map.MapCoordOrigin = Origin;
    map.MapCoordTarget = Target;
  }
}
