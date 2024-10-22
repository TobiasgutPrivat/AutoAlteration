using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using System.IO.Compression;
using GBX.NET.ZLib;
public class Map
{
  Gbx<CGameCtnChallenge> gbx;
  public CGameCtnChallenge map;
  public List<Block> stagedBlocks = [];
  public List<string> embeddedBlocks = [];
  public Map(string mapPath)
  { 
    Gbx.LZO = new MiniLZO();
    Gbx.ZLib = new ZLib();
    gbx = Gbx.Parse<CGameCtnChallenge>(mapPath);
    map = gbx.Node;
    map.Chunks.Get<CGameCtnChallenge.Chunk03043040>().Version = 4;
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

  private void EmbedBlock(string name, string path){
    map.UpdateEmbeddedZipData((ZipArchive zipArchive) =>
    {
      ZipArchiveEntry entry = zipArchive.CreateEntry("Blocks\\" + name + ".Block.Gbx");
        using Stream entryStream = entry.Open();
        using FileStream fileStream = File.OpenRead(path);
        fileStream.CopyTo(entryStream);
    });
    // Console.WriteLine(string.Join(",", map.OpenReadEmbeddedZipData().Entries.Select(x => x.Name)));
  }
  private void EmbedItem(string name, string path){
    map.UpdateEmbeddedZipData((ZipArchive zipArchive) =>
    {
      ZipArchiveEntry entry = zipArchive.CreateEntry("Items\\" + name + ".Item.Gbx");
        using Stream entryStream = entry.Open();
        using FileStream fileStream = File.OpenRead(path);
        fileStream.CopyTo(entryStream);
    });
    // Console.WriteLine(string.Join(",", map.OpenReadEmbeddedZipData().Entries.Select(x => x.Name)));
  }

  public void PlaceRelativeWithRandom(Article atBlock, Inventory newInventory,MoveChain ?moveChain = null){
    List<Article> newArticles = newInventory.articles;
    if (newArticles.Count == 0) return;
    Random rand = new Random();
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == atBlock.Name)){
      stagedBlocks.Add(new Block(ctnBlock,atBlock,newArticles[rand.Next(newArticles.Count)],moveChain));
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == atBlock.Name)){
      stagedBlocks.Add(new Block(ctnItem,atBlock,newArticles[rand.Next(newArticles.Count)],moveChain));
    }
  }

  public void PlaceRelative(Article atArticle, Article newArticle, MoveChain ?moveChain = null){
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == atArticle.Name)){
      stagedBlocks.Add(new Block(ctnBlock,atArticle,newArticle,moveChain));
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == atArticle.Name)){
      stagedBlocks.Add(new Block(ctnItem,atArticle,newArticle,moveChain));
    }
  }

  public void PlaceRelative(Inventory inventory, Article newArticle, MoveChain ?moveChain = null)=>
    inventory.articles.ForEach(a => PlaceRelative(a, newArticle, moveChain));
  
  public void PlaceRelative(KeywordEdit keywordEdit, MoveChain ?moveChain = null) =>
    keywordEdit.PlaceRelative(this,moveChain);


  public void ReplaceWithRandom(Article oldArticle, Inventory newInventory,MoveChain ?moveChain = null){
    if (newInventory.articles.Count == 0) return;
    PlaceRelativeWithRandom(oldArticle, newInventory, moveChain);
    Delete(oldArticle);
  }

  public void Replace(Article oldArticle, Article article, MoveChain ?moveChain = null){
    PlaceRelative(oldArticle, article,moveChain);
    Delete(oldArticle);
  }

  public void Replace(Inventory inventory, string newBlock, MoveChain ?moveChain = null){
    PlaceRelative(inventory, GetArticle(newBlock),moveChain);
    Delete(inventory);
  }
  
  public void Replace(Inventory inventory, Article article, MoveChain ?moveChain = null){
    PlaceRelative(inventory, article,moveChain);
    Delete(inventory);
  }

  public void Replace(KeywordEdit keywordEdit, MoveChain ?moveChain = null) =>
    keywordEdit.Replace(this,moveChain);

  public void Move(Article article, MoveChain moveChain) =>
    Replace(article, article, moveChain);

  public void Move(Inventory inventory, MoveChain moveChain) =>
    inventory.articles.ForEach(a => Move(a, moveChain));

  public void PlaceStagedBlocks(){
    foreach (var block in stagedBlocks){
      PlaceBlock(block);
    } 
    stagedBlocks = [];
  }

  public void PlaceBlock(Block block){
    switch (block.blockType){
        case BlockType.Block:
        case BlockType.Pillar:
          PlaceTypeBlock(block);
          break;
        case BlockType.Item:
          PlaceTypeItem(block);
          break;
        case BlockType.CustomBlock:
          if(!embeddedBlocks.Any(x => x == block.name)){
            EmbedBlock(block.name,block.Path);
            embeddedBlocks.Add(block.name);
          }
          block.name += ".Block.Gbx_CustomBlock";
          PlaceTypeBlock(block);
          break;
        case BlockType.CustomItem:
          if(!embeddedBlocks.Any(x => x == block.name)){
            EmbedItem(block.name,block.Path);
            embeddedBlocks.Add(block.name);
          }
          PlaceTypeItem(block);
          break;
      }
  }

  private void PlaceTypeBlock(Block block){
    CGameCtnBlock newBlock = map.PlaceBlock(block.name,new(0,0,0),Direction.North);
    newBlock.IsFree = true;
    newBlock.AbsolutePositionInMap = block.position.coords;
    newBlock.PitchYawRoll = block.position.pitchYawRoll;
    newBlock.IsGhost = false;
    newBlock.IsClip = block.IsClip;
    newBlock.Color = block.color;
    newBlock.Skin = block.Skin;
    newBlock.Bit21 = block.IsAir;
  }
  private void PlaceTypeItem(Block block){
    CGameCtnAnchoredObject item = map.PlaceAnchoredObject(new Ident(block.name + ".Item.Gbx", new Id(26), "Nadeo"),block.position.coords,block.position.pitchYawRoll);
    item.SnappedOnItem = block.SnappedOnItem;
    item.SnappedOnBlock = block.SnappedOnBlock;
    item.PlacedOnItem = block.PlacedOnItem;
    item.PivotPosition = block.PivotPosition;
    item.BlockUnitCoord = block.BlockUnitCoord;
    item.Color = block.color;
    item.Scale = 1;
  }

  public void Delete(Article Block, bool includePillars = false){
    List<CGameCtnBlock> deleted = map.Blocks.Where(block => block.BlockModel.Id == Block.Name).ToList();
    map.Blocks = map.Blocks.Where(block => block.BlockModel.Id != Block.Name).ToList();
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

    map.AnchoredObjects = map.AnchoredObjects.Where(block => block.ItemModel.Id != Block.Name).ToList();
  }
  public void Delete(Inventory inventory, bool includePillars = false){
    foreach(var block in inventory.articles){
      Delete(block,includePillars);
    }
  }

  public static Article GetArticle(string name) =>
    Alteration.inventory.GetArticle(name);
}