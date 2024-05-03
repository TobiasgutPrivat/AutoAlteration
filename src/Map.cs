using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
class Map
{
Gbx<CGameCtnChallenge> gbx;
public CGameCtnChallenge map;
public List<Block> stagedBlocks = new List<Block>();
  public Map(string mapPath)
  { 
    Gbx.LZO = new MiniLZO();
    gbx = Gbx.Parse<CGameCtnChallenge>(mapPath);
    map = gbx.Node;
    map.Chunks.Get<CGameCtnChallenge.Chunk03043040>().Version = 4;
  }

  public void save(string Path)
  { 
    map.AuthorTime = null;
    map.GoldTime = null;
    map.SilverTime = null;
    map.BronzeTime = null;
    map.AuthorScore = 0;
    gbx.Save(Path);
  }

  public void embedBlock(){//TODO Embeddings
    // map.UpdateEmbeddedZipData();
  }

  public void placeRelative(string atModelId, BlockChange blockChange){
    foreach (var ctnBlock in map.GetBlocks().Where(x => x.BlockModel.Id == atModelId)){//blocks
      Block block = new Block(ctnBlock);
      blockChange.changeBlock(ctnBlock,block);
      stagedBlocks.Add(block);
    }
    foreach (var ctnItem in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == atModelId)){//items
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

  public void replace(string oldModel, BlockChange blockChange)
  {
    placeRelative(oldModel,blockChange);
    deleteBlock(oldModel);
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

  public void deleteBlock(string modelId){
    List<int> indexes;
    List<CGameCtnBlock> modifiedblocks = map.Blocks.ToList();
    indexes = modifiedblocks.FindAll(block => block.BlockModel.Id == modelId).Select(block => modifiedblocks.IndexOf(block)).ToList();
    indexes.Reverse();
    foreach(int index in indexes){
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