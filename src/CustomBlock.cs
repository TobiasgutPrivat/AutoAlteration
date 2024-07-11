using GBX.NET;
using GBX.NET.LZO;
using GBX.NET.ZLib;
using GBX.NET.Engines.GameData;
class CustomBlock
{
  public CGameItemModel customBlock;
  public CGameBlockItem BlockItem;
  public CustomBlock(string mapPath)
  { 
    Gbx.LZO = new MiniLZO();
    Gbx.ZLib = new ZLib();
    customBlock = Gbx.Parse<CGameItemModel>(mapPath);
    Console.WriteLine(customBlock.Name);
    BlockItem = (CGameBlockItem)customBlock.EntityModelEdition;
    BlockItem.CustomizedVariants.First().Crystal.Materials.ToList().ForEach(x => Console.WriteLine(x.MaterialUserInst.Link));
  }
}