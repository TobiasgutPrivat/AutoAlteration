using GBX.NET;
class Alterations {
    public static void NoBrakes(Map map){
        string[] blockSelections = new string[] {"PlatformTechStart","PlatformTechCheckpoint","PlatformIceCheckpoint","RoadTechCheckpoint"};
        foreach(var block in blockSelections){
            map.placeAtBlocks(block,"GateSpecialNoBrake",new Int3(0,-2,0));
        }
        map.placeStagedBlocks();
    }
}