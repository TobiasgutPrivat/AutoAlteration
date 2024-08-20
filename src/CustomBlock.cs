using GBX.NET;
using GBX.NET.LZO;
using GBX.NET.ZLib;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
public class CustomBlock
{
  public CGameItemModel customBlock;
  public CGameBlockItem Block;
  public string Name;
  public CGameCommonItemEntityModelEdition Item;
  public IList<CPlugCrystal.Layer> Layers;
  public BlockType Type;
  public CustomBlock(string blockPath)
  { 
    Gbx.LZO = new MiniLZO();
    Gbx.ZLib = new ZLib();
    customBlock = Gbx.Parse<CGameItemModel>(blockPath);
    
    if (blockPath.Contains(".Block.gbx", StringComparison.OrdinalIgnoreCase)){
      Type = BlockType.Block;
      Block = (CGameBlockItem)customBlock.EntityModelEdition;
      Name = Path.GetFileName(blockPath)[..^10];
      Layers = Block.CustomizedVariants[0].Crystal.Layers;
    }
    else if (blockPath.Contains(".Item.gbx", StringComparison.OrdinalIgnoreCase)){
      Type = BlockType.Item;
      Item = (CGameCommonItemEntityModelEdition)customBlock.EntityModelEdition;
      Layers = Item.MeshCrystal.Layers;
      Name = Path.GetFileName(blockPath)[..^9];
    }
  }

  public void Save(string path)
  { 
    if (!Directory.Exists(Path.GetDirectoryName(path)))
      {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
      }
    customBlock.Save(path);
  }
}