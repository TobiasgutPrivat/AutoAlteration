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
    //handle variants
    //should be moved elsewhere, needed for Vanilla Blocks (those always have one customized variant)
    // if (Type == BlockType.Block) {
    //   if (customBlock.EntityModelEdition is not null){
    //     CGameBlockItem Block = (CGameBlockItem)customBlock.EntityModelEdition;
        // Block.ArchetypeBlockInfoId = customBlock.Name;
        // if (Block.CustomizedVariants is not null){
        //   Block.CustomizedVariants.ForEach(x => {
        //     x.Id = 1001000;
            // x.Properties = CGameBlockItem.MobilProperties;
          // });

          // copying to 1000000 -> can't be placed
          // Block.CustomizedVariants.Add(new CGameBlockItem.Mobil(){
          //   Id = 1000000,
          //   Crystal = Block.CustomizedVariants[0].Crystal,
          // });

          //ID's:
          // Sometimes the altered mesh is not applied ingame, and replaced by ingame stored mesh probably
          // some blocks require always 1000000
          // some blocks require always 1001000
          // some blocks require 1000000 for air and 1001000 for normal

          // 1000 -> not applied at all
          // Last 3 digits have no known effect so far
    //     }
    //   }
    // }

    //normal save
    if (!Directory.Exists(Path.GetDirectoryName(path)))
      {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
      }
    customBlock.Save(path);
  }
}