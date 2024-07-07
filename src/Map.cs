using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using System.IO.Compression;
using GBX.NET.ZLib;
class Map
{
  Gbx<CGameCtnChallenge> gbx;
  public CGameCtnChallenge map;
  public List<Block> stagedBlocks = new();
  public List<string> embeddedBlocks = new();
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
    CleanDupes();
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

  private void CleanDupes()
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
    Console.WriteLine(string.Join(",", map.OpenReadEmbeddedZipData().Entries.Select(x => x.Name)));
  }
  private void EmbedItem(string name, string path){
    map.UpdateEmbeddedZipData((ZipArchive zipArchive) =>
    {
      ZipArchiveEntry entry = zipArchive.CreateEntry("Items\\" + name + ".Item.Gbx");
        using Stream entryStream = entry.Open();
        using FileStream fileStream = File.OpenRead(path);
        fileStream.CopyTo(entryStream);
    });
    Console.WriteLine(string.Join(",", map.OpenReadEmbeddedZipData().Entries.Select(x => x.Name)));
  }

  public void PlaceRelative(string atBlock, string newBlock,Position ?positionChange = null){
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == atBlock)){
      stagedBlocks.Add(new Block(ctnBlock,GetArticle(atBlock),GetArticle(newBlock),positionChange));
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == atBlock)){
      stagedBlocks.Add(new Block(ctnItem,GetArticle(atBlock),GetArticle(newBlock),positionChange));
    }
  }

  public void PlaceRelative(string[] atBlocks, string newBlock,Position ?positionChange = null)=>
    atBlocks.ToList().ForEach(atBlock => PlaceRelative(atBlock, newBlock, positionChange));

  public void PlaceRelative(Inventory inventory, string newBlock,Position ?positionChange = null) =>
    PlaceRelative(inventory.Names(), newBlock, positionChange);

  public void PlaceRelative(Article atArticle, Article newArticle, Position ?positionChange = null){
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == atArticle.Name)){
      stagedBlocks.Add(new Block(ctnBlock,atArticle,newArticle,positionChange));
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == atArticle.Name)){
      stagedBlocks.Add(new Block(ctnItem,atArticle,newArticle,positionChange));
    }
  }

  public void PlaceRelative(Inventory inventory, Article newArticle, Position ?positionChange = null){
    inventory.articles.ForEach(a => PlaceRelative(a, newArticle, positionChange));
  }

  public void Replace(string oldBlock, string newBlock,Position ?positionChange = null){
    PlaceRelative(oldBlock, newBlock, positionChange);
    Delete(oldBlock);
  }

  public void Replace(string[] oldBlocks, string newBlock,Position ?positionChange = null){
    PlaceRelative(oldBlocks, newBlock, positionChange);
    Delete(oldBlocks);
  }

  public void Replace(Article oldArticle, Article article, Position ?positionChange = null){
    PlaceRelative(oldArticle, article,positionChange);
    Delete(oldArticle.Name);
  }

  public void Replace(Inventory inventory, Article article, Position ?positionChange = null){
    PlaceRelative(inventory, article,positionChange);
    Delete(inventory);
  }

  public void Move(Article article, Position position)
  {
    foreach (CGameCtnBlock ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == article.Name)){
      Position blockPosition = Block.GetBlockPosition(ctnBlock).AddPosition(position);
      ctnBlock.AbsolutePositionInMap = blockPosition.coords;
      ctnBlock.Coord = new Int3((int)blockPosition.coords.X/32, (int)blockPosition.coords.Y/8, (int)blockPosition.coords.Z/32);
      ctnBlock.PitchYawRoll = blockPosition.pitchYawRoll;
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == article.Name)){
      Position blockPosition = new Position(ctnItem.AbsolutePositionInMap,ctnItem.PitchYawRoll);
      blockPosition.AddPosition(position);
      ctnItem.AbsolutePositionInMap = blockPosition.coords;
      ctnItem.PitchYawRoll = blockPosition.pitchYawRoll;
    }
  }

  public void Move(string[] blocks, Position position)
  {
    foreach(var block in blocks){
      Move(GetArticle(block), position);
    }
  }
  public void Move(Inventory inventory, Position position)
  {
    Move(inventory.Names(), position);
  }

  public void PlaceStagedBlocks(){
    foreach (var block in stagedBlocks){
      PlaceBlock(block);
    } 
    stagedBlocks = new List<Block>();
  }

  public void PlaceBlock(Block block){
    switch (block.blockType){
        case BlockType.Block:
        case BlockType.Pillar:
          PlaceTypeBlock(block);
          break;
        case BlockType.Item:
          map.PlaceAnchoredObject(new Ident(block.name, new Id(26), "Nadeo"),block.position.coords,block.position.pitchYawRoll);
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
          map.PlaceAnchoredObject(new Ident(block.name + ".Item.Gbx", new Id(26), "Nadeo"),block.position.coords,block.position.pitchYawRoll);
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

  public void Delete(string Block){
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
  public void Delete(string[] blocks){
    foreach(var block in blocks){
      Delete(block);
    }
  }
  public void Delete(Inventory inventory){
    Delete(inventory.Names());
  }

  public static Article GetArticle(string name) =>
    Alteration.inventory.GetArticle(name);
}