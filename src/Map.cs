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

  public void placeRelative(string atBlock, string newBlock, BlockChange blockChange = null){
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == atBlock)){//blocks
      stagedBlocks.Add(new Block(ctnBlock,newBlock,blockChange));
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == atBlock)){//items
      stagedBlocks.Add(new Block(ctnItem,newBlock,blockChange));
    }
  }

  public void placeRelative(string[] atBlocks, string newBlock, BlockChange blockChange = null){
    foreach(var atBlock in atBlocks){
      placeRelative(atBlock, newBlock, blockChange);
    }
  }

  public void placeRelative(Inventory inventory, string newBlock, BlockChange blockChange = null){
    placeRelative(inventory.names(), newBlock, blockChange);
  }

  public void placeRelativeKeyword(string oldKeyword, string newKeyword, BlockChange blockChange = null){
    foreach (Article block in Alteration.Blocks.GetArticles(new string[] {oldKeyword})) {
      string newBlock = Alteration.Blocks.ArticleReplaceKeyword(block, oldKeyword, newKeyword).Name;
      placeRelative(block.Name, newBlock, blockChange);
    }
    foreach (Article block in Alteration.Items.GetArticles(new string[] {oldKeyword})) {
      string newBlock = Alteration.Items.ArticleReplaceKeyword(block, oldKeyword, newKeyword).Name;
      placeRelative(block.Name, newBlock, blockChange);
    }
  }

  public void replace(string oldBlock, string newBlock, BlockChange blockChange = null){
    placeRelative(oldBlock, newBlock, blockChange);
    delete(oldBlock);
  }

  public void replace(string[] oldBlocks, string newBlock, BlockChange blockChange = null){
    placeRelative(oldBlocks, newBlock, blockChange);
    delete(oldBlocks);
  }
  public void replace(Inventory inventory, string newBlock, BlockChange blockChange = null){
    replace(inventory.names(), newBlock, blockChange);
  }

  public void replaceKeyword(string oldKeyword, string newKeyword, BlockChange blockChange = null){
    replaceKeyword(Alteration.Blocks, oldKeyword, newKeyword, blockChange);
    replaceKeyword(Alteration.Items, oldKeyword, newKeyword, blockChange);
  }
  public void replaceKeyword(Inventory inventory, string oldKeyword, string newKeyword, BlockChange blockChange = null){
    foreach (Article article in inventory.GetArticles(new string[] {oldKeyword})) {
      replace(article.Name, inventory.ArticleReplaceKeyword(article, oldKeyword, newKeyword).Name, blockChange);
    }
  }
  public void editKeyword(Inventory inventory, string[] addKeywords = null, string[] removeKeywords = null, BlockChange blockChange = null){//TODO use seperator instead of Array
    foreach (Article article in inventory.GetArticles(addKeywords)){
      replace(article.Name, inventory.ArticleReplaceKeyword(article, addKeywords,removeKeywords).Name, blockChange);
    }
  }

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
      
      if (Alteration.Blocks.hasArticle(block.name)){
        CGameCtnBlock newBlock = map.PlaceBlock(block.name,new(0,0,0),Direction.North);
        newBlock.IsFree = true;
        newBlock.AbsolutePositionInMap = block.absolutePosition;
        newBlock.PitchYawRoll = block.pitchYawRoll;
      } else
      if (Alteration.Items.hasArticle(block.name)){
        map.PlaceAnchoredObject(new Ident(block.name, new Id(26), "Nadeo"),block.absolutePosition,block.pitchYawRoll);
      } else {
        Console.WriteLine("Block not found: " + block.name);
      }
    } 
    stagedBlocks = new List<Block>();
  }

  public void delete(string block){
    List<int> indexes;
    if (Alteration.Blocks.hasArticle(block)){
      List<CGameCtnBlock> modifiedblocks = map.Blocks.ToList();
      indexes = modifiedblocks.FindAll(b => b.BlockModel.Id == block).Select(b => modifiedblocks.IndexOf(b)).ToList();
      foreach(int index in indexes){
        //TODO delete Platform underneath
        modifiedblocks.RemoveAt(index);
      }
      map.Blocks = modifiedblocks;
    }

    if (Alteration.Items.hasArticle(block)){
      List<CGameCtnAnchoredObject> modifiedItems = map.AnchoredObjects.ToList();
      indexes = modifiedItems.FindAll(b => b.ItemModel.Id == block).Select(b => modifiedItems.IndexOf(b)).ToList();
      foreach(int index in indexes){
        modifiedItems.RemoveAt(index);
      }
      map.AnchoredObjects = modifiedItems;
    }
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