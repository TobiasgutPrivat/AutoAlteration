using GBX.NET;
class Alterations {
    public static void NoBrakes(Map map){
        string[] blockSelections = new string[] {"PlatformTechStart","PlatformTechCheckpoint","PlatformIceCheckpoint","RoadTechCheckpoint"};
        foreach(var block in blockSelections){
            map.placeRelative(block,"GateSpecialNoBrake",BlockType.Block,new Int3(0,-16,1));
        }
        map.placeRelative("GateCheckpointRight32m","GateSpecial32mNoBrake",BlockType.Item,new Int3(0,0,1));
        map.placeStagedBlocks();
    }

    public static void CPFull(Map map){
    }
}