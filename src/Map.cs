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
  static string BlocksFolder = "C:\\Users\\Tobias\\Documents\\Programmieren\\GBX Test\\AutoAlteration\\Blocks\\";
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

  public void placeRelative(string atBlock, string newBlock, BlockChange blockChange){
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == atBlock)){//blocks
      stagedBlocks.Add(new Block(ctnBlock,newBlock,blockChange));
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == atBlock)){//items
      stagedBlocks.Add(new Block(ctnItem,newBlock,blockChange));
    }
  }

  public void placeRelative(string[] atBlocks, string newBlock, BlockChange blockChange){
    foreach(var atBlock in atBlocks){
      placeRelative(atBlock, newBlock, blockChange);
    }
  }

  public void placeRelative(string[] atBlocks, string newBlock){
    placeRelative(atBlocks, newBlock, null);
  }

  public void placeRelative(string atBlock, string newBlock){
    placeRelative(atBlock, newBlock, null);
  }

  public void placeRelativeKeyword(string oldKeyword, string newKeyword, BlockChange blockChange){
    foreach (Article block in AutoAlteration.Blocks.GetArticlesWithKeywords(new string[] {oldKeyword})) {
      string newBlock = AutoAlteration.Blocks.ArticleReplaceKeyword(block, oldKeyword, newKeyword).Name;
      placeRelative(block.Name, newBlock, blockChange);
    }
    foreach (Article block in AutoAlteration.Items.GetArticlesWithKeywords(new string[] {oldKeyword})) {
      string newBlock = AutoAlteration.Items.ArticleReplaceKeyword(block, oldKeyword, newKeyword).Name;
      placeRelative(block.Name, newBlock, blockChange);
    }
  }

  public void placeRelativeKeyword(string oldKeyword, string newKeyword){
    placeRelativeKeyword(oldKeyword, newKeyword, null);
  }

  public void placeRelativeGroup(string[] keywords, string block, BlockChange blockChange){
    AutoAlteration.Blocks.GetArticlesWithKeywords(keywords).ForEach(article => placeRelative(article.Name,block,blockChange));
    AutoAlteration.Items.GetArticlesWithKeywords(keywords).ForEach(article => placeRelative(article.Name,block,blockChange));
  }
  public void placeRelativeGroup(string[] keywords, string block){
    placeRelativeGroup(keywords, block, null);
  }
  public void placeRelativeGroup(string keyword, string block, BlockChange blockChange){
    AutoAlteration.Blocks.GetArticlesWithKeywords(new string[] {keyword}).ForEach(article => placeRelative(article.Name,block,blockChange));
    AutoAlteration.Items.GetArticlesWithKeywords(new string[] {keyword}).ForEach(article => placeRelative(article.Name,block,blockChange));
  }
  public void placeRelativeGroup(string keyword, string block){
    placeRelativeGroup(keyword, block, null);
  }
  public void replace(string oldBlock, string newBlock, BlockChange blockChange){
    placeRelative(oldBlock, newBlock, blockChange);
    delete(oldBlock);
  }

  public void replace(string[] oldBlocks, string newBlock, BlockChange blockChange){
    placeRelative(oldBlocks, newBlock, blockChange);
    delete(oldBlocks);
  }

  public void replace(string oldBlock, string newBlock){
    placeRelative(oldBlock, newBlock);
    delete(oldBlock);
  }

  public void replace(string[] oldBlocks, string newBlock){
    placeRelative(oldBlocks, newBlock);
    delete(oldBlocks);
  }

  public void replaceKeyword(string oldKeyword, string newKeyword, BlockChange blockChange){
    foreach (Article block in AutoAlteration.Blocks.GetArticlesWithKeywords(new string[] {oldKeyword})) {
      string newBlock = AutoAlteration.Blocks.ArticleReplaceKeyword(block, oldKeyword, newKeyword).Name;
      replace(block.Name, newBlock, blockChange);
    }
    foreach (Article block in AutoAlteration.Items.GetArticlesWithKeywords(new string[] {oldKeyword})) {
      string newBlock = AutoAlteration.Items.ArticleReplaceKeyword(block, oldKeyword, newKeyword).Name;
      replace(block.Name, newBlock, blockChange);
    }
  }

  public void replaceKeyword(string oldKeyword, string newKeyword){
    replaceKeyword(oldKeyword, newKeyword, null);
  }

  public void replaceGroup(string[] keywords, string block, BlockChange blockChange){
    AutoAlteration.Blocks.GetArticlesWithKeywords(keywords).ForEach(article => placeRelative(article.Name,block,blockChange));
    AutoAlteration.Items.GetArticlesWithKeywords(keywords).ForEach(article => placeRelative(article.Name,block,blockChange));
  }

  public void replaceGroup(string[] keywords, string block){
    AutoAlteration.Blocks.GetArticlesWithKeywords(keywords).ForEach(article => placeRelative(article.Name,block));
    AutoAlteration.Items.GetArticlesWithKeywords(keywords).ForEach(article => placeRelative(article.Name,block));
  }

  public void replaceGroup(string keyword, string block, BlockChange blockChange){
    AutoAlteration.Blocks.GetArticlesWithKeywords(new string[] {keyword}).ForEach(article => placeRelative(article.Name,block,blockChange));
    AutoAlteration.Items.GetArticlesWithKeywords(new string[] {keyword}).ForEach(article => placeRelative(article.Name,block,blockChange));
  }

  public void replaceGroup(string keyword, string block){
    AutoAlteration.Blocks.GetArticlesWithKeywords(new string[] {keyword}).ForEach(article => placeRelative(article.Name,block));
    AutoAlteration.Items.GetArticlesWithKeywords(new string[] {keyword}).ForEach(article => placeRelative(article.Name,block));
  }

  public void moveBlock(string block, Vec3 offset, Vec3 rotation)
  {
    placeRelative(block, block, new BlockChange(offset, rotation));
    delete(block);
  }

  public void moveBlock(string[] blocks, Vec3 offset, Vec3 rotation)
  {
    foreach(var block in blocks){
      moveBlock(block, offset, rotation);
    }
  }

  public void moveGroup(string keyword, Vec3 offset, Vec3 rotation){
    AutoAlteration.Blocks.GetArticlesWithKeywords(new string[] {keyword}).ForEach(article => moveBlock(article.Name, offset, rotation));
    AutoAlteration.Items.GetArticlesWithKeywords(new string[] {keyword}).ForEach(article => moveBlock(article.Name, offset, rotation));
  }

  public void moveGroup(string[] keywords, Vec3 offset, Vec3 rotation){
    AutoAlteration.Blocks.GetArticlesWithKeywords(keywords).ForEach(article => moveBlock(article.Name, offset, rotation));
    AutoAlteration.Items.GetArticlesWithKeywords(keywords).ForEach(article => moveBlock(article.Name, offset, rotation));
  }

  public void placeStagedBlocks(){//TODO Try sort by coordinates for correct connections (for example Gatefinish)
    foreach (var block in stagedBlocks){
      if(block.name.Contains("_CustomBlock")){//TODO untested
        if(embeddedBlocks.Any(x => x.Name == block.name)){
          embedBlock(block.name.Replace("_CustomBlock",""));
        }
      }
      if (AutoAlteration.Blocks.hasArticle(block.name)){
        CGameCtnBlock newBlock = map.PlaceBlock(block.name,new(0,0,0),Direction.North);
        newBlock.IsFree = true;
        newBlock.AbsolutePositionInMap = block.absolutePosition;
        newBlock.PitchYawRoll = block.pitchYawRoll;
      }
      if (AutoAlteration.Items.hasArticle(block.name)){
        map.PlaceAnchoredObject(new Ident(block.name, new Id(26), "Nadeo"),block.absolutePosition,block.pitchYawRoll);
      }
      //TODO Embedded Blocks
    } 
    stagedBlocks = new List<Block>();
  }

  public void delete(string block){
    List<int> indexes;
    if (AutoAlteration.Blocks.hasArticle(block)){
      List<CGameCtnBlock> modifiedblocks = map.Blocks.ToList();
      indexes = modifiedblocks.FindAll(b => b.BlockModel.Id == block).Select(b => modifiedblocks.IndexOf(b)).ToList();
      foreach(int index in indexes){
        //TODO delete Platform underneath
        modifiedblocks.RemoveAt(index);
      }
      map.Blocks = modifiedblocks;
    }

    if (AutoAlteration.Items.hasArticle(block)){
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
  public void deleteGroup(string[] keywords){
    AutoAlteration.Blocks.GetArticlesWithKeywords(keywords).ForEach(article => delete(article.Name));
    AutoAlteration.Items.GetArticlesWithKeywords(keywords).ForEach(article => delete(article.Name));
  }
  public void deleteGroup(string keyword){
    AutoAlteration.Blocks.GetArticlesWithKeywords(new string[] {keyword}).ForEach(article => delete(article.Name));
    AutoAlteration.Items.GetArticlesWithKeywords(new string[] {keyword}).ForEach(article => delete(article.Name));
  }
}