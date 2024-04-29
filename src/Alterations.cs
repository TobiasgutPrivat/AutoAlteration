using GBX.NET;
class Alterations {
    public static void NoBrakes(Map map){
        string[] blockSelections = new string[] {"PlatformTechStart","PlatformTechCheckpoint","PlatformIceCheckpoint","RoadTechCheckpoint"};
        foreach(var block in blockSelections){
            map.placeRelative(block,"GateSpecialNoBrake",BlockType.Block,new Int3(0,-16,1));
        }
        map.placeStagedBlocks();
    }
}