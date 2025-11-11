using GBX.NET;
using GBX.NET.Engines.Game;
using ManiaAPI.NadeoAPI;
using System.IO.Compression;

public class Map
{
  Gbx<CGameCtnChallenge> gbx;
  public CGameCtnChallenge map;
  public List<Block> stagedBlocks = [];
  public Dictionary<string,BlockType> embeddedBlocks = []; //Format: "Blocks/SomeFolder/BlockName.Block.Gbx" -> "SomeFolder\\BlockName" (like name in Inventory)
  public int FreeBlockHeightOffset = 0;

  private Replay? WRReplay;

  #region loading
  public Map(string mapPath)
  { 
    gbx = Gbx.Parse<CGameCtnChallenge>(mapPath);
    map = gbx.Node;
    map.Chunks.Get<CGameCtnChallenge.Chunk03043040>().Version = 4; //TODO test if causes crashes
    FreeBlockHeightOffset = map.DecoBaseHeightOffset*8;
    map.Comments += "\nAltered using AutoAlteration";
    
    embeddedBlocks = GetEmbeddedBlocks();
  }

  public void Save(string path)
  { 
    NewMapUid();
    RemoveAuthor(); //TODO test if causes crashes
    map.RemovePassword();
    if (!Directory.Exists(Path.GetDirectoryName(path)))
      {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
      }
    gbx.Save(path);
  }

  private void RemoveAuthor(){
    map.AuthorTime = TmEssentials.TimeInt32.MaxValue;
    map.GoldTime = TmEssentials.TimeInt32.MaxValue;
    map.SilverTime = TmEssentials.TimeInt32.MaxValue;
    map.BronzeTime = TmEssentials.TimeInt32.MaxValue;
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
  public void EmbedBlock(string name, string path){
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
        .Where(x => x.Contains(".Block.Gbx", StringComparison.OrdinalIgnoreCase) || x.Contains(".Item.Gbx", StringComparison.OrdinalIgnoreCase))
        .Select(x => {
          BlockType type = x.Contains(".Item.Gbx", StringComparison.OrdinalIgnoreCase) ? BlockType.CustomItem : BlockType.CustomBlock;
          string name = x.Replace("/","\\");
          // assuming that every embedded block has Items\ or Blocks\ at the start which needs to be removed
          int itemPos = name.IndexOf("Items\\");                                                                                                                                                           //assuming every embedded name has Items// or Blocks// at the start which needs to be removed //TODO check if this is correct
          int blockPos = name.IndexOf("Blocks\\");
          if (itemPos != -1) {
            name = name.Substring(itemPos + 6);
          } else if (blockPos != -1) {
            name = name.Substring(blockPos + 7);
          } else {
            throw new Exception("Embedded Block without Items\\ or Blocks\\ in the name");
          }
          return (name, type);
        })
        .ToDictionary();
    } else {
      return [];
    }
  }

  public void ExtractEmbeddedBlocks(string path){
    ZipArchive zipArchive;
    if (map.EmbeddedZipData == null || map.EmbeddedZipData.Length == 0)
    {
      return;
    }
    zipArchive = map.OpenReadEmbeddedZipData();
    
    zipArchive.Entries.ToList().ForEach(x => {
      string name = x.FullName.Split("Blocks\\").Last().Split("Items\\").Last().Split("Blocks/").Last().Split("Items/").Last();
      string filePath = path + "\\" + name; // Extract with Folderstructure (FullName)
      Directory.CreateDirectory(Path.GetDirectoryName(filePath));
      x.ExtractToFile(filePath, true);
    });
    return;
  }
  #endregion

  #region actions
  public void PlaceRelativeWithRandom(Article atBlock, Inventory newInventory,MoveChain ?moveChain = null){
    if (newInventory.Count == 0) return;
    Random rand = new();
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == atBlock.Name)){
      Block block = new Block(ctnBlock, atBlock, FreeBlockHeightOffset);
      block.Move(moveChain);
      block.SetArticle(newInventory.ToList()[rand.Next(newInventory.Count)]);
      stagedBlocks.Add(block);
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == atBlock.Name)){
      Block block = new Block(ctnItem, atBlock);
      block.Move(moveChain);
      block.SetArticle(newInventory.ToList()[rand.Next(newInventory.Count)]);
      stagedBlocks.Add(block);
    }
  }

  public void PlaceRelative(Article atArticle, Article newArticle, MoveChain ?moveChain = null, Predicate<CGameCtnBlock>? blockCondition = null){
    string ArticleName = atArticle.Name;
    if (atArticle.Type == BlockType.CustomBlock) ArticleName += "_CustomBlock";
    ArticleName = ArticleName.Replace("/", "\\");
    
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == ArticleName))
    {
      if (blockCondition != null && !blockCondition(ctnBlock)) continue;
      Block block = new Block(ctnBlock, atArticle, FreeBlockHeightOffset);
      block.Move(moveChain);
      block.SetArticle(newArticle);
      stagedBlocks.Add(block);
    }

    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == ArticleName))
    {
      Block block = new Block(ctnItem, atArticle);
      block.Move(moveChain);
      block.SetArticle(newArticle);
      stagedBlocks.Add(block);
    }
  }

  public void PlaceRelative(Inventory inventory, Article newArticle, MoveChain ?moveChain = null, Predicate<CGameCtnBlock>? blockCondition = null)=> //TODO maybe move to inventory
    inventory.ToList().ForEach(a => PlaceRelative(a, newArticle, moveChain, blockCondition));
  
  public void ReplaceWithRandom(Article oldArticle, Inventory newInventory,MoveChain ?moveChain = null){
    if (newInventory.Count == 0) return;
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
    inventory.ToList().ForEach(a => Move(a, moveChain));

  public void Delete(Article Block, bool includePillars = false){
    string ArticleName = Block.Name;
    if (Block.Type == BlockType.CustomBlock) ArticleName = Block.Name + "_CustomBlock";
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
    foreach(var block in inventory){
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

    switch (block.blockType){
      case BlockType.CustomBlock:
        if(!embeddedBlocks.Any(x => x.Key == block.name && x.Value == block.blockType)){
          EmbedBlock("Blocks/" + block.name,block.article.Path);
          embeddedBlocks.Add(block.name, block.blockType);
        }
        break;
      case BlockType.CustomItem:
        if(!embeddedBlocks.Any(x => x.Key == block.name && x.Value == block.blockType)){
          EmbedBlock("Items/" + block.name,block.article.Path);
          embeddedBlocks.Add(block.name, block.blockType);
        }
        break;
    }

    block.PlaceInMap(map, revertFreeBlock);
  }
  #endregion

  public Replay? GetWRReplay(){
    if (WRReplay != null) return WRReplay;
    WRReplay = new Replay(map);
    return WRReplay;
  }

  public void StageAll(Inventory inventory, MoveChain ?moveChain = null){
    stagedBlocks.AddRange(map.Blocks.Select(x => {
      Article article = inventory.GetArticle(x.BlockModel.Id.Replace("_CustomBlock","", StringComparison.OrdinalIgnoreCase));
      Block block = new Block(x, article, FreeBlockHeightOffset);
      block.Move(moveChain);
      return block;
    }));
    map.Blocks = [];
    stagedBlocks.AddRange(map.AnchoredObjects.Select(x => {
      Article article = inventory.GetArticle(x.ItemModel.Id);
      Block block = new Block(x, article);
      block.Move(moveChain);
      return block;
    }));
    map.AnchoredObjects = [];
  }

  public void SetBuildDimension(Vec2 Origin, Vec2 Target){ //TODO test this
    map.MapCoordOrigin = Origin;
    map.MapCoordTarget = Target;
  }
}
