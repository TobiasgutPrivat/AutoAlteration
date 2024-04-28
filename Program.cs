using System.Collections;
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
// Map map = new Map("C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Mapping\\RC Racing.Map.Gbx");
// Alterations.NoBrakes(map);
// map.save("C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Mapping\\RC Racing Altered.Map.Gbx");


Gbx.LZO = new MiniLZO();
Gbx<CGameCtnChallenge> gbx = Gbx.Parse<CGameCtnChallenge>("C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Mapping\\RC Racing.Map.Gbx");
CGameCtnChallenge map = gbx.Node;

foreach (CGameCtnBlock test in map.GetBlocks().Where(x => x.Name == "PlatformTechDiag1")){
test.IsFree = true;
test.AbsolutePositionInMap =  new(test.Coord.X * 32,test.Coord.Y * 8 - 64,test.Coord.Z * 32);
    switch (test.Direction){
        case Direction.North:
                test.PitchYawRoll = new (0,3.141528f,3.141528f);
            break;
        case Direction.East:
            break;
        case Direction.South:
            break;
        case Direction.West:
            break;
    }
Console.WriteLine(test.AbsolutePositionInMap);
}
gbx.Save("C:\\Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Mapping\\RC Racing Altered.Map.Gbx");

//PlatformTechFinish
//PlatformTechStart
//DecoPlatformIceBase
//PlatformTechBase
//PlatformPlasticSlope2End