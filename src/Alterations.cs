using GBX.NET;
using GBX.NET.Exceptions;
class EffectAlterations {
    static float PI = (float)Math.PI;
    static string[] StartBlock = new string[] {"PlatformTechStart","RoadTechStart","RoadDirtStart","RoadBumpStart","RoadIceStart","RoadWaterStart","PlatformTechStart","PlatformDirtStart","PlatformIceStart","PlatformGrassStart","PlatformPlasticStart","PlatformWaterStart"};
    static string[] MultilapBlock = new string[] {"PlatformTechMultilap","RoadTechMultilap","RoadDirtMultilap","RoadBumpMultilap","RoadIceMultilap","RoadWaterMultilap","PlatformTechMultilap","PlatformDirtMultilap","PlatformIceMultilap","PlatformGrassMultilap","PlatformPlasticMultilap","PlatformWaterMultilap"};
    //TODO missing MultilapIceWalls
    static string[] CheckpointRoadBlock = new string[] {"RoadTechCheckpoint","RoadTechCheckpointSlopeUp","RoadTechCheckpointSlopeDown","RoadTechCheckpointTiltLeft","RoadTechCheckpointTiltRight","RoadDirtCheckpoint","RoadDirtCheckpointSlopeUp","RoadDirtCheckpointSlopeDown","RoadDirtCheckpointTiltLeft","RoadDirtCheckpointTiltRight","RoadBumpCheckpoint","RoadBumpCheckpointSlopeUp","RoadBumpCheckpointSlopeDown","RoadBumpCheckpointTiltLeft","RoadBumpCheckpointTiltRight","RoadIceCheckpoint","RoadIceCheckpointSlopeUp","RoadIceCheckpointSlopeDown","RoadWaterCheckpoint","GateCheckpoint"};
    //TODO missing RoadIceWalls
    static string[] CheckpointPlatformBlock = new string[] {"PlatformTechCheckpoint","PlatformTechCheckpointSlope2Up","PlatformTechCheckpointSlope2Down","PlatformTechCheckpointSlope2Right","PlatformTechCheckpointSlope2Left","PlatformPlasticCheckpoint","PlatformPlasticCheckpointSlope2Up","PlatformPlasticCheckpointSlope2Down","PlatformPlasticCheckpointSlope2Right","PlatformPlasticCheckpointSlope2Left","PlatformDirtCheckpoint","PlatformDirtCheckpointSlope2Up","PlatformDirtCheckpointSlope2Down","PlatformDirtCheckpointSlope2Right","PlatformDirtCheckpointSlope2Left","PlatformIceCheckpoint","PlatformIceCheckpointSlope2Up","PlatformIceCheckpointSlope2Down","PlatformIceCheckpointSlope2Right","PlatformIceCheckpointSlope2Left","PlatformGrassCheckpoint","PlatformGrassCheckpointSlope2Up","PlatformGrassCheckpointSlope2Down","PlatformGrassCheckpointSlope2Right","PlatformGrassCheckpointSlope2Left","PlatformWaterCheckpoint"};
    static string[] GateCPStart32m = new string[] {"GateCheckpointLeft32m","GateCheckpointCenter32mv2","GateCheckpointRight32m","GateStartLeft32m","GateStartCenter32m","GateStartRight32m","GateMultilapLeft32m","GateMultilapCenter32m","GateMultilapRight32m"};
    static string[] GateCPStart16m = new string[] {"GateCheckpointLeft16m","GateCheckpointCenter16mv2","GateCheckpointRight16m","GateStartLeft16m","GateStartCenter16m","GateStartRight16m","GateMultilapLeft16m","GateMultilapCenter16m","GateMultilapRight16m"};
    static string[] GateCPStart8m = new string[] {"GateCheckpointLeft8m","GateCheckpointCenter8mv2","GateCheckpointRight8m","GateStartLeft8m","GateStartCenter8m","GateStartRight8m","GateMultilapLeft8m","GateMultilapCenter8m","GateMultilapRight8m"};
    static string[] DiagRight = new string[]{"RoadTechDiagRightMultilap","RoadDirtDiagRightMultilap","RoadBumpDiagRightMultilap","RoadIceDiagRightMultilap","RoadTechDiagRightCheckpoint","RoadDirtDiagRightCheckpoint","RoadBumpDiagRightCheckpoint"};
    static string[] DiagLeft = new string[]{"RoadTechDiagLeftMultilap","RoadDirtDiagLeftMultilap","RoadBumpDiagLeftMultilap","RoadIceDiagLeftMultilap","RoadTechDiagLeftCheckpoint","RoadDirtDiagLeftCheckpoint","RoadBumpDiagLeftCheckpoint"};
    public static void NoBrakes(Map map){
        map.placeRelative(StartBlock,"GateSpecialNoBrake",BlockType.Block,new Int3(0,-16,1));
        map.placeRelative(MultilapBlock,"GateSpecialNoBrake",BlockType.Block,new Int3(0,-16,1));
        map.placeRelative(CheckpointRoadBlock,"GateSpecialNoBrake",BlockType.Block,new Int3(0,-16,1));
        map.placeRelative(CheckpointPlatformBlock,"GateSpecialNoBrake",BlockType.Block,new Int3(0,-16,1));
        map.placeRelative(DiagRight,"GateSpecialNoBrake",BlockType.Block,new Vec3(-23.9f,-16,-20.8f),new Vec3(PI * -0.1454f,0f,0));
        map.placeRelative(DiagLeft,"GateSpecialNoBrake",BlockType.Block,new Vec3(-37.2f,-16,25.1f),new Vec3(PI * 0.1454f,0,0));

        map.placeRelative(GateCPStart32m,"GateSpecial32mNoBrake",BlockType.Item,new Int3(0,0,1));
        map.placeRelative(GateCPStart16m,"GateSpecial16mNoBrake",BlockType.Item,new Int3(0,0,1));
        map.placeRelative(GateCPStart8m,"GateSpecial8mNoBrake",BlockType.Item,new Int3(0,0,1));
        map.placeStagedBlocks();
    }

    public static void CPFull(Map map){
    }
}