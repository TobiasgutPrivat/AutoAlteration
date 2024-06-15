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
  public List<string> embeddedBlocks = new List<string>();
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

  private void embedBlock(string name){//TODO overthink getting path
    List<string> customBlocks = Directory.GetFiles(Alteration.CustomBlocksFolder, "*.Block.Gbx", SearchOption.AllDirectories).Where(x => Path.GetFileName(x) == name).ToList();
    if (customBlocks.Count != 1){
      Console.WriteLine("Found " + customBlocks.Count + " custom blocks for " + name);
      return;
    }
    string path = customBlocks.First();
    map.UpdateEmbeddedZipData((ZipArchive zipArchive) =>
    {
      ZipArchiveEntry entry = zipArchive.CreateEntry("Blocks\\" + name);
        using Stream entryStream = entry.Open();
        using FileStream fileStream = File.OpenRead(path);
        fileStream.CopyTo(entryStream);
    });
    Console.WriteLine(string.Join(",", map.OpenReadEmbeddedZipData().Entries.Select(x => x.Name)));
  }
  private void embedItem(string name){//TODO overthink getting path
    List<string> customBlocks = Directory.GetFiles(Alteration.CustomBlocksFolder, "*.Item.Gbx", SearchOption.AllDirectories).Where(x => Path.GetFileName(x) == name).ToList();
    if (customBlocks.Count != 1){
      Console.WriteLine("Found " + customBlocks.Count + " custom blocks for " + name);
      return;
    }
    string path = customBlocks.First();
    map.UpdateEmbeddedZipData((ZipArchive zipArchive) =>
    {
      ZipArchiveEntry entry = zipArchive.CreateEntry("Items\\" + name);
        using Stream entryStream = entry.Open();
        using FileStream fileStream = File.OpenRead(path);
        fileStream.CopyTo(entryStream);
    });
    Console.WriteLine(string.Join(",", map.OpenReadEmbeddedZipData().Entries.Select(x => x.Name)));
  }

  public void placeRelative(string atBlock, string newBlock,Position positionChange = null){
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == atBlock)){
      stagedBlocks.Add(new Block(ctnBlock,getArticle(atBlock),getArticle(newBlock),positionChange));
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == atBlock)){
      stagedBlocks.Add(new Block(ctnItem,getArticle(atBlock),getArticle(newBlock),positionChange));
    }
  }

  public void placeRelative(string[] atBlocks, string newBlock,Position positionChange = null){
    foreach(var atBlock in atBlocks){
      placeRelative(atBlock, newBlock, positionChange);
    }
  }
  public void placeRelative(Inventory inventory, string newBlock,Position positionChange = null){
    placeRelative(inventory.names(), newBlock, positionChange);
  }

  public void placeRelative(Article atArticle, Article newArticle, Position positionChange = null){
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == atArticle.name)){
      stagedBlocks.Add(new Block(ctnBlock,atArticle,newArticle,positionChange));
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == atArticle.name)){
      stagedBlocks.Add(new Block(ctnItem,atArticle,newArticle,positionChange));
    }
  }

  public void placeRelative(Inventory inventory, Article newArticle, Position positionChange = null){
    inventory.articles.ForEach(a => placeRelative(a, newArticle, positionChange));
  }

  public void replace(string oldBlock, string newBlock,Position positionChange = null){
    placeRelative(oldBlock, newBlock, positionChange);
    delete(oldBlock);
  }

  public void replace(string[] oldBlocks, string newBlock,Position positionChange = null){
    placeRelative(oldBlocks, newBlock, positionChange);
    delete(oldBlocks);
  }

  public void replace(Article oldArticle, Article article, Position positionChange = null){
    placeRelative(oldArticle, article,positionChange);
    delete(oldArticle.name);
  }

  public void replace(Inventory inventory, Article article, Position positionChange = null){
    placeRelative(inventory, article,positionChange);
    delete(inventory);
  }

  public void move(string block, Vec3 offset, Vec3 rotation)
  {
    foreach (CGameCtnBlock ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == block)){
      ctnBlock.AbsolutePositionInMap += offset;
      ctnBlock.Coord += new Int3((int)offset.X/32, (int)offset.Y/8, (int)offset.Z/32);
      ctnBlock.PitchYawRoll += rotation;
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == block)){
      ctnItem.AbsolutePositionInMap += offset;
      ctnItem.PitchYawRoll += rotation;
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
      switch (block.blockType){
        case BlockType.Block:
          placeBlock(block);
          break;
        case BlockType.Item:
          map.PlaceAnchoredObject(new Ident(block.name, new Id(26), "Nadeo"),block.position.coords,block.position.pitchYawRoll);
          break;
        case BlockType.CustomBlock:
          if(!embeddedBlocks.Any(x => x == block.name)){
            embedBlock(block.name);
            embeddedBlocks.Add(block.name);
          }
          block.name += "_CustomBlock";
          placeBlock(block);
          break;
        case BlockType.CustomItem:
          if(!embeddedBlocks.Any(x => x == block.name)){
            embedItem(block.name);
            embeddedBlocks.Add(block.name);
          }
          map.PlaceAnchoredObject(new Ident(block.name, new Id(26), "Nadeo"),block.position.coords,block.position.pitchYawRoll);
          break;
      }
    } 
    stagedBlocks = new List<Block>();
  }

  private void placeBlock(Block block){
    CGameCtnBlock newBlock = map.PlaceBlock(block.name,new(0,0,0),Direction.North);
    if (!block.IsFree && block.isInGrid()){//TODO untested
      newBlock.IsFree = false;
      // newBlock.Coord = new Int3 ((int)(block.position.coords.X/32), (int)(block.position.coords.Y/8) + 8, (int)(block.position.coords.Z/32));
      newBlock.IsGround = block.IsGround;
      switch (Block.round(block.position.pitchYawRoll.X / ((float)Math.PI/2)) % 4){
        case 0:
          newBlock.Direction = Direction.North;
          // newBlock.Coord += new Int3(1,0,1);
          break;
        case 1:
          newBlock.Direction = Direction.East;
          // newBlock.Coord += new Int3(0,0,1);
          // newBlock.Coord += new Int3((block.article.length - 1) * -1,0,0); 
          break;
        case 2:
          newBlock.Direction = Direction.South;
          // newBlock.Coord += new Int3(0,0,0);
          // newBlock.Coord += new Int3((block.article.width - 1)*-1,0,(block.article.length - 1)*-1); 
          break;
        case 3:
          newBlock.Direction = Direction.West;
          // newBlock.Coord += new Int3(1,0,0);
          // newBlock.Coord += new Int3(0,0,block.article.width - 1);
          break;
        default:
          Console.WriteLine("Unknown Direction");
          break;
      }
      Vec3 offset;
      Block.getDirectionOffset(block.article,newBlock.Direction,out offset,out _);
      Vec3 coords = block.position.coords - offset;
      newBlock.Coord += new Int3((int)coords.X/32, (int)coords.Y/8 + 8, (int)coords.Z/32);
    } else{
      newBlock.IsFree = true;
      newBlock.AbsolutePositionInMap = block.position.coords;
      newBlock.PitchYawRoll = block.position.pitchYawRoll;
    }
    newBlock.IsGhost = block.IsGhost;
    newBlock.IsClip = block.IsClip;
    newBlock.Color = block.color;
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

  public static Article getArticle(string name){
    Article article = Alteration.inventory.articles.FirstOrDefault(x => x.name == name);
    if (article == null){
      throw new Exception("Article not found: " + name);
    }
    return article;
  }
}