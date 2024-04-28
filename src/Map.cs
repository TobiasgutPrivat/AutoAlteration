using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
class Map
{
Gbx<CGameCtnChallenge> gbx;
CGameCtnChallenge map;
List<Block> stagedBlocks = new List<Block>();
  public Map(string mapPath)
  { 
    Gbx.LZO = new MiniLZO();
    gbx = Gbx.Parse<CGameCtnChallenge>(mapPath);
    map = gbx.Node;
  }

  public void save(string Path)
  { 
    gbx.Save(Path);
  }

  public void placeAtBlocks(string atBlockModel, string newBlockModel,Int3 offset){
        foreach (var block in map.GetBlocks().Where(x => x.BlockModel.Id == atBlockModel)){
          stagedBlocks.Add(new Block(newBlockModel,block.Coord + offset,block.Direction));
        }
  }

  // public void placeAtItems(string itemId){
  //   foreach (var item in map.GetAnchoredObjects().Where(x => x.ItemModel.Id == itemId)){
  //     // stagedPlacements.Add(new StagedBlock(block.BlockModel.Id,block.Coord + new Int3(1,2,3),block.Direction));
  //   }
  // }

  public void placeStagedBlocks(){
    foreach (var block in stagedBlocks){
      switch (block.blockType)
      {
        case BlockType.Block:
          map.PlaceBlock(block.model,block.coord,block.direction);
          break;
        case BlockType.Item:
          // map.PlaceAnchoredObject(block.model,block.absolutePosition,block.pitchYawRoll);
          Console.WriteLine("Not yet implemented");
          break;
        default:
          Console.WriteLine("Invalid Blocktype");
          break;
      }
    } 
    stagedBlocks = new List<Block>();
  }

  public void deleteBlock(string blockName){
    if (map.Blocks is List<CGameCtnBlock> blocks){
      // blocks.RemoveAll() RemoveWhere(x => x.Name == blockName);
    }
  }

  // public void runStagedActions(){
  //   foreach(var stagedPlacement in stagedPlacements){
  //     map.PlaceBlock(stagedPlacement.blockModel,stagedPlacement.coord,stagedPlacement.direction);
  //   }
  // }
}