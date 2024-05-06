using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using System.IO.Compression;
using System.Linq.Expressions;
class Map
{
  Gbx<CGameCtnChallenge> gbx;
  public CGameCtnChallenge map;
  public List<Block> stagedBlocks = new List<Block>();
  public string[] embeddedBlocks = Array.Empty<string>();
  static string BlocksFolder = "C:\\Users\\Tobias\\Documents\\Programmieren\\GBX Test\\AutoAlteration\\Blocks\\";
  public Map(string mapPath)
  { 
    Gbx.LZO = new MiniLZO();
    gbx = Gbx.Parse<CGameCtnChallenge>(mapPath);
    map = gbx.Node;
    map.Chunks.Get<CGameCtnChallenge.Chunk03043040>().Version = 4;
  }

  public void save(string Path)
  { 
    cleanDupes();
    newMapUid();
    removeAuthor();
    map.RemovePassword();
    gbx.Save(Path);
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

  public void embedBlock(string path){
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

  public void placeRelative(string atBlock, BlockChange blockChange){
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == atBlock)){//blocks
      Block block = new Block(ctnBlock);
      blockChange.changeBlock(ctnBlock,block);
      stagedBlocks.Add(block);
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == atBlock)){//items
      Block block = new Block(ctnItem);
      blockChange.changeItem(ctnItem,block);
      stagedBlocks.Add(block);
    }
  }

  public void placeRelative(string[] atBlocks, BlockChange blockChange){
    foreach(var atBlock in atBlocks){
      placeRelative(atBlock,blockChange);
    }
  }

  public void placeEmbeddedRelative(string atBlock, BlockChange blockChange){
    try{
      if (!map.OpenReadEmbeddedZipData().Entries.Any(x => x.FullName == blockChange.model)){
        embedBlock(blockChange.model);
      }
    } catch {
      embedBlock(blockChange.model);
    }
    blockChange.model += "_CustomBlock";
    placeRelative(atBlock,blockChange);
  }

  public void placeEmbeddedRelative(string[] atBlocks, BlockChange blockChange){
    if (!map.OpenReadEmbeddedZipData().Entries.Any(x => x.FullName ==  blockChange.model)){
      embedBlock(blockChange.model);
    }
    blockChange.model += "_CustomBlock";
    placeRelative(atBlocks,blockChange);
  }

  public void replace(string oldModel, BlockChange blockChange)
  {
    placeRelative(oldModel,blockChange);
    delete(oldModel);
  }

  public void placeStagedBlocks(){
    foreach (var block in stagedBlocks){
      switch (block.blockType)
      {
        case BlockType.Block:
          CGameCtnBlock newBlock = map.PlaceBlock(block.model,new(0,0,0),Direction.North);
          newBlock.IsFree = true;
          newBlock.AbsolutePositionInMap = block.absolutePosition;
          newBlock.PitchYawRoll = block.pitchYawRoll;
          break;
        case BlockType.Item:
          map.PlaceAnchoredObject(new Ident(block.model, new Id(26), "Nadeo"),block.absolutePosition,block.pitchYawRoll);
          break;
        default:
          Console.WriteLine("Invalid Blocktype");
          break;
      }
    } 
    stagedBlocks = new List<Block>();
  }

  public void delete(string modelId){
    List<int> indexes;
    List<CGameCtnBlock> modifiedblocks = map.Blocks.ToList();
    indexes = modifiedblocks.FindAll(block => block.BlockModel.Id == modelId).Select(block => modifiedblocks.IndexOf(block)).ToList();
    indexes.Reverse();
    foreach(int index in indexes){
      if (map.BakedBlocks is List<CGameCtnBlock> bakedBlocks){
        bakedBlocks.Where(x => x.Coord.X == modifiedblocks[index].Coord.X && x.Coord.Z == modifiedblocks[index].Coord.Z && x.Name != "Grass").ToList().ForEach(x =>  Console.WriteLine(x.Name));
        bakedBlocks.RemoveAll(x => x.Coord.X == modifiedblocks[index].Coord.X && x.Coord.Z == modifiedblocks[index].Coord.Z && x.Name != "Grass");
      }
      if (map.BakedBlocks is List<CGameCtnBlock> newbakedBlocks){
        newbakedBlocks.Where(x => x.Coord.X == modifiedblocks[index].Coord.X && x.Coord.Z == modifiedblocks[index].Coord.Z && x.Name != "Grass").ToList().ForEach(x =>  Console.WriteLine("new" + x.Name));
      }
      modifiedblocks.RemoveAt(index);
    }
    map.Blocks = modifiedblocks;

    List<CGameCtnAnchoredObject> modifiedItems = map.AnchoredObjects.ToList();
    indexes = modifiedItems.FindAll(block => block.ItemModel.Id == modelId).Select(block => modifiedItems.IndexOf(block)).ToList();
    indexes.Reverse();
    foreach(int index in indexes){
      modifiedItems.RemoveAt(index);
    }
    map.AnchoredObjects = modifiedItems;

  }
}