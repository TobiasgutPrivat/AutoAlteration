using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using System.IO.Compression;
using GBX.NET.ZLib;
class Map
{
  Gbx<CGameCtnChallenge> gbx;
  public CGameCtnChallenge map;
  public List<Block> stagedBlocks = new List<Block>();
  public List<Article> embeddedBlocks = new List<Article>();
  static string BlocksFolder = "C:\\Users\\Tobias\\Documents\\Programmieren\\GBX Test\\Alteration\\Blocks\\";
  public Map(string mapPath)
  { 
    Gbx.LZO = new MiniLZO();
    Gbx.ZLib = new ZLib();
    gbx = Gbx.Parse<CGameCtnChallenge>(mapPath);
    map = gbx.Node;
    map.Chunks.Get<CGameCtnChallenge.Chunk03043040>().Version = 4;
  }

  public void save(string path)
  { 
    cleanDupes();
    newMapUid();
    removeAuthor();
    map.RemovePassword();
    if (!Directory.Exists(Path.GetDirectoryName(path)))
      {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
      }
    gbx.Save(path);
  }

  private void removeAuthor(){
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

  private void cleanDupes()
  {
    if (map.Blocks is List<CGameCtnBlock> blocks){
      blocks.RemoveAll(x =>
      {
        if (x.Name == "PlatformTechBase")
          foreach (var block in map.Blocks)
            if (block.Coord == x.Coord && block.Name == "DecoWallBasePillar")
              return true;
        return false;
      });
    }
  }

  private void newMapUid()
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

  private void embedBlock(string path){
    map.UpdateEmbeddedZipData((ZipArchive zipArchive) =>
    {
      ZipArchiveEntry entry = zipArchive.CreateEntry(path);
      using (Stream entryStream = entry.Open())
      {
        using (FileStream fileStream = File.OpenRead(BlocksFolder + path))
        {
          fileStream.CopyTo(entryStream);
        }
      }
    });
  }

  public void placeRelative(string atBlock, string newBlock,BlockType blockType, BlockChange blockChange = null){
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == atBlock)){//blocks
      stagedBlocks.Add(new Block(ctnBlock,newBlock,blockType,blockChange));
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == atBlock)){//items
      stagedBlocks.Add(new Block(ctnItem,newBlock,blockType,blockChange));
    }
  }

  public void placeRelative(string[] atBlocks, string newBlock,BlockType blockType, BlockChange blockChange = null){
    foreach(var atBlock in atBlocks){
      placeRelative(atBlock, newBlock, blockType, blockChange);
    }
  }
  public void placeRelative(Inventory inventory, string newBlock,BlockType blockType, BlockChange blockChange = null){
    placeRelative(inventory.names(), newBlock, blockType, blockChange);
  }

  public void placeRelative(string atBlock, Article newArticle, BlockChange blockChange = null){
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == atBlock)){//blocks
      stagedBlocks.Add(new Block(ctnBlock,newArticle,blockChange));
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == atBlock)){//items
      stagedBlocks.Add(new Block(ctnItem,newArticle,blockChange));
    }
  }

  public void placeRelative(string[] atBlocks, Article newArticle, BlockChange blockChange = null){
    foreach(var atBlock in atBlocks){
      placeRelative(atBlock, newArticle, blockChange);
    }
  }
  public void placeRelative(Inventory inventory, Article newArticle, BlockChange blockChange = null){
    placeRelative(inventory, newArticle, blockChange);
  }

  public void replace(string oldBlock, string newBlock,BlockType blockType, BlockChange blockChange = null){
    placeRelative(oldBlock, newBlock, blockType,blockChange);
    delete(oldBlock);
  }

  public void replace(string[] oldBlocks, string newBlock,BlockType blockType, BlockChange blockChange = null){
    placeRelative(oldBlocks, newBlock, blockType,blockChange);
    delete(oldBlocks);
  }
  public void replace(Inventory inventory, string newBlock,BlockType blockType, BlockChange blockChange = null){
    placeRelative(inventory, newBlock, blockType,blockChange);
    delete(inventory);
  }

  public void replace(string oldBlock, Article article, BlockChange blockChange = null){
    placeRelative(oldBlock, article,blockChange);
    delete(oldBlock);
  }

  public void replace(string[] oldBlocks, Article article, BlockChange blockChange = null){
    placeRelative(oldBlocks, article,blockChange);
    delete(oldBlocks);
  }
  public void replace(Inventory inventory, Article article, BlockChange blockChange = null){
    placeRelative(inventory, article,blockChange);
    delete(inventory);
  }
  // public void replace(Inventory inventory, Article newArticle, BlockChange blockChange = null){
  //   replace(inventory.names(), newArticle, blockChange);
  // }

  public void move(string block, Vec3 offset, Vec3 rotation)
  {
    foreach (CGameCtnBlock ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == block)){
      ctnBlock.AbsolutePositionInMap = ctnBlock.AbsolutePositionInMap + offset;
      ctnBlock.Coord = ctnBlock.Coord + new Int3((int)offset.X/32, (int)offset.Y/8, (int)offset.Z/32);
      ctnBlock.PitchYawRoll = ctnBlock.PitchYawRoll + rotation;
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == block)){
      ctnItem.AbsolutePositionInMap = ctnItem.AbsolutePositionInMap + offset;
      ctnItem.PitchYawRoll = ctnItem.PitchYawRoll + rotation;
    }
  }

  public void move(string[] blocks, Vec3 offset, Vec3 rotation)
  {
    foreach(var block in blocks){
      move(block, offset, rotation);
    }
  }
  public void move(Inventory inventory, Vec3 offset, Vec3 rotation)
  {
    move(inventory.names(), offset, rotation);
  }

  public void move(string block, Vec3 offset)
  {
    move(block, offset, Vec3.Zero);
  }

  public void move(string[] blocks, Vec3 offset)
  {
    move(blocks, offset, Vec3.Zero);
  }
  public void move(Inventory inventory, Vec3 offset)
  {
    move(inventory.names(), offset, Vec3.Zero);
  }

  public void placeStagedBlocks(){
    foreach (var block in stagedBlocks){
      //TODO Embedded Blocks
      // if(block.name.Contains("_CustomBlock")){
      //   if(embeddedBlocks.Any(x => x.Name == block.name)){
      //     embedBlock(block.name.Replace("_CustomBlock",""));
      //   }
      // }
      switch (block.blockType){
        case BlockType.Block:
          CGameCtnBlock newBlock = map.PlaceBlock(block.name,new(0,0,0),Direction.North);
          newBlock.IsFree = true;
          newBlock.AbsolutePositionInMap = block.absolutePosition;
          newBlock.PitchYawRoll = block.pitchYawRoll;
          break;
        case BlockType.Item:
          map.PlaceAnchoredObject(new Ident(block.name, new Id(26), "Nadeo"),block.absolutePosition,block.pitchYawRoll);
          break;
      }
    } 
    stagedBlocks = new List<Block>();
  }

  public void delete(string Block){
    List<int> indexes;
    List<CGameCtnBlock> modifiedblocks = map.Blocks.ToList();
    indexes = modifiedblocks.FindAll(block => block.BlockModel.Id == Block).Select(block => modifiedblocks.IndexOf(block)).ToList();
    indexes.Reverse();
    foreach(int index in indexes){
      modifiedblocks.RemoveAt(index);
    }
    map.Blocks = modifiedblocks;

    List<CGameCtnAnchoredObject> modifiedItems = map.AnchoredObjects.ToList();
    indexes = modifiedItems.FindAll(block => block.ItemModel.Id == Block).Select(block => modifiedItems.IndexOf(block)).ToList();
    indexes.Reverse();
    foreach(int index in indexes){
      modifiedItems.RemoveAt(index);
    }
    map.AnchoredObjects = modifiedItems;
  }
  public void delete(string[] blocks){
    foreach(var block in blocks){
      delete(block);
    }
  }
  public void delete(Inventory inventory){
    delete(inventory.names());
  }
}