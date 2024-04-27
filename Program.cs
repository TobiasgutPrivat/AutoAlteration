using System.Runtime.CompilerServices;
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.LZO;
// using Newtonsoft.Json;

Gbx.LZO = new MiniLZO();

var gbx = Gbx.Parse<CGameCtnChallenge>("C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Mapping\\RC Racing.Map.Gbx");
var map = gbx.Node;

foreach (var block in map.GetGhostBlocks().GroupBy(x => x.Name))
{
    Console.WriteLine($"{block.Key}: {block.Count()}");
}
CGameCtnBlock selectedBlock = map.GetBlocks().FirstOrDefault(block => block.Name == "DecoPlatformIceBase");
// string json = Newtonsoft.Json.JsonConvert.SerializeObject(selectedBlock); 
// CGameCtnBlock copiedBlock = Newtonsoft.Json.JsonConvert.DeserializeObject<CGameCtnBlock>(json);
// Console.WriteLine(copiedBlock.Name);
map.PlaceBlock(selectedBlock.BlockModel,new Int3(selectedBlock.Coord.X,selectedBlock.Coord.Y + 1,selectedBlock.Coord.Z),selectedBlock.Direction);
map.RemoveAllBlocks ;
// map.PlaceBlock(selectedBlock.BlockModel,new Int3(selectedBlock.Coord.X,selectedBlock.Coord.Y + 2,selectedBlock.Coord.Z),selectedBlock.Direction);
// Console.WriteLine(selectedBlock.Name);

gbx.Save("C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Mapping\\RC Racing Altered.Map.Gbx");
// var otherblock = map.GetGhostBlocks(). GetBlocks().Last().BlockModel