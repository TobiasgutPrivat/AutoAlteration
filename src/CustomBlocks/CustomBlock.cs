using GBX.NET;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;

public class CustomBlock
{
  public CGameItemModel customBlock;
  public string Name;
  public List<CPlugCrystal> MeshCrystals = [];
  public BlockType Type;
  public CustomBlock(string blockPath)
  { 
    customBlock = Gbx.Parse<CGameItemModel>(blockPath);
    
    if (blockPath.Contains(".Block.gbx", StringComparison.OrdinalIgnoreCase)){
      Type = BlockType.Block;
      if (customBlock.EntityModelEdition is not null){
        CGameBlockItem Block = (CGameBlockItem)customBlock.EntityModelEdition;
        if (Block.CustomizedVariants is not null){
          // Note: if no CustomizedVariants, then Archetype is used -> cannot be altered
          MeshCrystals.AddRange(Block.CustomizedVariants.Select(x => x.Crystal).OfType<CPlugCrystal>().ToList());
        }
      }
      //TODO handle EntityModel
      Name = Path.GetFileName(blockPath)[..^10];
    }
    else if (blockPath.Contains(".Item.gbx", StringComparison.OrdinalIgnoreCase)){
      Type = BlockType.Item;
      if (customBlock.EntityModelEdition is not null){
        CGameCommonItemEntityModelEdition Item = (CGameCommonItemEntityModelEdition)customBlock.EntityModelEdition;
        if (Item.MeshCrystal is not null){
          MeshCrystals.Add(Item.MeshCrystal);
        }
      }

      if (customBlock.EntityModel is not null){
        CGameCommonItemEntityModel Item = (CGameCommonItemEntityModel)customBlock.EntityModel;
        // if (Item.MeshCrystal is not null){ //TODO
        //   MeshCrystals.Add(Item.MeshCrystal);
        // }
      }
      Name = Path.GetFileName(blockPath)[..^9];
    } else {
      throw new Exception("Invalid Filetype");
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