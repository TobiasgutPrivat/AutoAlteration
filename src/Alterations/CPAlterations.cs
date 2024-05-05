using System.Net.NetworkInformation;
using GBX.NET;
using GBX.NET.Engines.Game;
class CPAlterations{
    static float PI = (float)Math.PI;
    public static void CPBoost(Map map){
        //CP -> Turbo
        map.replace("RoadIceWithWallCheckpointRight",new BlockChange(BlockType.Block,"RoadIceWithWallSpecialTurboRight"));
        map.replace("RoadIceWithWallCheckpointLeft",new BlockChange(BlockType.Block,"RoadIceWithWallSpecialTurboLeft"));
        map.replace("RoadIceWithWallDiagRightCheckpointRight",new DiagBlockChange(BlockType.Block,"RoadIceWithWallDiagRightSpecialTurboRight"));
        map.replace("RoadIceWithWallDiagLeftCheckpointRight",new DiagBlockChange(BlockType.Block,"RoadIceWithWallDiagLeftSpecialTurboRight"));
        map.replace("RoadIceWithWallDiagRightCheckpointLeft",new DiagBlockChange(BlockType.Block,"RoadIceWithWallDiagRightSpecialTurboLeft"));
        map.replace("RoadIceWithWallDiagLeftCheckpointLeft",new DiagBlockChange(BlockType.Block,"RoadIceWithWallDiagLeftSpecialTurboLeft"));
        map.replace("RoadTechCheckpoint",new BlockChange(BlockType.Block,"RoadTechSpecialTurbo"));
        map.replace("RoadTechCheckpointSlopeUp",new BlockChange(BlockType.Block,"RoadTechSpecialTurboSlopeUp"));
        map.replace("RoadTechCheckpointSlopeDown",new BlockChange(BlockType.Block,"RoadTechSpecialTurboSlopeDown"));
        map.replace("RoadDirtCheckpoint",new BlockChange(BlockType.Block,"RoadDirtSpecialTurbo"));
        map.replace("RoadDirtCheckpointSlopeUp",new BlockChange(BlockType.Block,"RoadDirtSpecialTurboSlopeUp"));
        map.replace("RoadDirtCheckpointSlopeDown",new BlockChange(BlockType.Block,"RoadDirtSpecialTurboSlopeDown"));
        map.replace("RoadBumpCheckpoint",new BlockChange(BlockType.Block,"RoadBumpSpecialTurbo"));
        map.replace("RoadBumpCheckpointSlopeUp",new BlockChange(BlockType.Block,"RoadBumpSpecialTurboSlopeUp"));
        map.replace("RoadBumpCheckpointSlopeDown",new BlockChange(BlockType.Block,"RoadBumpSpecialTurboSlopeDown"));
        map.replace("RoadIceCheckpoint",new BlockChange(BlockType.Block,"RoadIceSpecialTurbo"));
        map.replace("RoadIceCheckpointSlopeUp",new BlockChange(BlockType.Block,"RoadIceSpecialTurboSlopeUp"));
        map.replace("RoadIceCheckpointSlopeDown",new BlockChange(BlockType.Block,"RoadIceSpecialTurboSlopeDown"));
        map.replace("RoadWaterCheckpoint",new BlockChange(BlockType.Block,"RoadWaterSpecialTurbo"));
        map.replace("GateCheckpoint",new BlockChange(BlockType.Block,"GateSpecialTurbo"));
        map.replace("PlatformTechCheckpoint",new BlockChange(BlockType.Block,"PlatformTechSpecialTurbo"));
        map.replace("PlatformTechCheckpointSlope2Up",new BlockChange(BlockType.Block,"PlatformTechSpecialTurboSlope2Up"));
        map.replace("PlatformTechCheckpointSlope2Down",new BlockChange(BlockType.Block,"PlatformTechSpecialTurboSlope2Down"));
        map.replace("PlatformTechCheckpointSlope2Right",new BlockChange(BlockType.Block,"PlatformTechSpecialTurboSlope2Right"));
        map.replace("PlatformTechCheckpointSlope2Left",new BlockChange(BlockType.Block,"PlatformTechSpecialTurboSlope2Left"));
        map.replace("PlatformPlasticCheckpoint",new BlockChange(BlockType.Block,"PlatformPlasticSpecialTurbo"));
        map.replace("PlatformPlasticCheckpointSlope2Up",new BlockChange(BlockType.Block,"PlatformPlasticSpecialTurboSlope2Up"));
        map.replace("PlatformPlasticCheckpointSlope2Down",new BlockChange(BlockType.Block,"PlatformPlasticSpecialTurboSlope2Down"));
        map.replace("PlatformPlasticCheckpointSlope2Right",new BlockChange(BlockType.Block,"PlatformPlasticSpecialTurboSlope2Right"));
        map.replace("PlatformPlasticCheckpointSlope2Left",new BlockChange(BlockType.Block,"PlatformPlasticSpecialTurboSlope2Left"));
        map.replace("PlatformDirtCheckpoint",new BlockChange(BlockType.Block,"PlatformDirtSpecialTurbo"));
        map.replace("PlatformDirtCheckpointSlope2Up",new BlockChange(BlockType.Block,"PlatformDirtSpecialTurboSlope2Up"));
        map.replace("PlatformDirtCheckpointSlope2Down",new BlockChange(BlockType.Block,"PlatformDirtSpecialTurboSlope2Down"));
        map.replace("PlatformDirtCheckpointSlope2Right",new BlockChange(BlockType.Block,"PlatformDirtSpecialTurboSlope2Right"));
        map.replace("PlatformDirtCheckpointSlope2Left",new BlockChange(BlockType.Block,"PlatformDirtSpecialTurboSlope2Left"));
        map.replace("PlatformIceCheckpoint",new BlockChange(BlockType.Block,"PlatformIceSpecialTurbo"));
        map.replace("PlatformIceCheckpointSlope2Up",new BlockChange(BlockType.Block,"PlatformIceSpecialTurboSlope2Up"));
        map.replace("PlatformIceCheckpointSlope2Down",new BlockChange(BlockType.Block,"PlatformIceSpecialTurboSlope2Down"));
        map.replace("PlatformIceCheckpointSlope2Right",new BlockChange(BlockType.Block,"PlatformIceSpecialTurboSlope2Right"));
        map.replace("PlatformIceCheckpointSlope2Left",new BlockChange(BlockType.Block,"PlatformIceSpecialTurboSlope2Left"));
        map.replace("PlatformGrassCheckpoint",new BlockChange(BlockType.Block,"PlatformGrassSpecialTurbo"));
        map.replace("PlatformGrassCheckpointSlope2Up",new BlockChange(BlockType.Block,"PlatformGrassSpecialTurboSlope2Up"));
        map.replace("PlatformGrassCheckpointSlope2Down",new BlockChange(BlockType.Block,"PlatformGrassSpecialTurboSlope2Down"));
        map.replace("PlatformGrassCheckpointSlope2Right",new BlockChange(BlockType.Block,"PlatformGrassSpecialTurboSlope2Right"));
        map.replace("PlatformGrassCheckpointSlope2Left",new BlockChange(BlockType.Block,"PlatformGrassSpecialTurboSlope2Left"));
        map.replace("PlatformWaterCheckpoint",new BlockChange(BlockType.Block,"PlatformWaterSpecialTurbo"));
        map.replace("RoadTechCheckpointTiltLeft",new BlockChange(BlockType.Block,"RoadTechSpecialTurboTiltLeft"));
        map.replace("RoadTechCheckpointTiltRight",new BlockChange(BlockType.Block,"RoadTechSpecialTurboTiltRight"));
        map.replace("RoadDirtCheckpointTiltLeft",new BlockChange(BlockType.Block,"RoadDirtSpecialTurboTiltLeft"));
        map.replace("RoadDirtCheckpointTiltRight",new BlockChange(BlockType.Block,"RoadDirtSpecialTurboTiltRight"));
        map.replace("RoadBumpCheckpointTiltLeft",new BlockChange(BlockType.Block,"RoadBumpSpecialTurboTiltLeft"));
        map.replace("RoadBumpCheckpointTiltRight",new BlockChange(BlockType.Block,"RoadBumpSpecialTurboTiltRight"));
        map.replace("RoadTechDiagRightCheckpoint",new DiagBlockChange(BlockType.Block,"RoadTechSpecialTurboDiagRight"));
        map.replace("RoadDirtDiagRightCheckpoint",new DiagBlockChange(BlockType.Block,"RoadDirtSpecialTurboDiagRight"));
        map.replace("RoadBumpDiagRightCheckpoint",new DiagBlockChange(BlockType.Block,"RoadBumpSpecialTurboDiagRight"));
        map.replace("RoadTechDiagLeftCheckpoint",new DiagBlockChange(BlockType.Block,"RoadTechSpecialTurboDiagLeft"));
        map.replace("RoadDirtDiagLeftCheckpoint",new DiagBlockChange(BlockType.Block,"RoadDirtSpecialTurboDiagLeft"));
        map.replace("RoadBumpDiagLeftCheckpoint",new DiagBlockChange(BlockType.Block,"RoadBumpSpecialTurboDiagLeft"));

        map.replace("GateCheckpointLeft32m",new BlockChange(BlockType.Item,"GateSpecialTurboLeft32m"));
        map.replace("GateCheckpointCenter32mv2",new BlockChange(BlockType.Item,"GateSpecialTurboCenter32mv2"));
        map.replace("GateCheckpointRight32m",new BlockChange(BlockType.Item,"GateSpecialTurboRight32m"));
        map.replace("GateCheckpointLeft16m",new BlockChange(BlockType.Item,"GateSpecialTurboLeft16m"));
        map.replace("GateCheckpointCenter16mv2",new BlockChange(BlockType.Item,"GateSpecialTurboCenter16mv2"));
        map.replace("GateCheckpointRight16m",new BlockChange(BlockType.Item,"GateSpecialTurboRight16m"));
        map.replace("GateCheckpointLeft8m",new BlockChange(BlockType.Item,"GateSpecialTurboLeft8m"));
        map.replace("GateCheckpointCenter8mv2",new BlockChange(BlockType.Item,"GateSpecialTurboCenter8mv2"));
        map.replace("GateCheckpointRight8m",new BlockChange(BlockType.Item,"GateSpecialTurboRight8m"));

        // //Turbo -> CP
        map.replace("RoadIceWithWallSpecialTurboRight",new BlockChange(BlockType.Block,"RoadIceWithWallCheckpointRight"));
        map.replace("RoadIceWithWallSpecialTurboLeft",new BlockChange(BlockType.Block,"RoadIceWithWallCheckpointLeft"));
        map.replace("RoadIceWithWallDiagRightSpecialTurboRight",new DiagBlockChange(BlockType.Block,"RoadIceWithWallDiagRightCheckpointRight"));
        map.replace("RoadIceWithWallDiagLeftSpecialTurboRight",new DiagBlockChange(BlockType.Block,"RoadIceWithWallDiagLeftCheckpointRight"));
        map.replace("RoadIceWithWallDiagRightSpecialTurboLeft",new DiagBlockChange(BlockType.Block,"RoadIceWithWallDiagRightCheckpointLeft"));
        map.replace("RoadIceWithWallDiagLeftSpecialTurboLeft",new DiagBlockChange(BlockType.Block,"RoadIceWithWallDiagLeftCheckpointLeft"));
        map.replace("RoadTechSpecialTurbo",new BlockChange(BlockType.Block,"RoadTechCheckpoint"));//TODO Issue here removing Boost
        map.replace("RoadTechSpecialTurboSlopeUp",new BlockChange(BlockType.Block,"RoadTechCheckpointSlopeUp"));
        map.replace("RoadTechSpecialTurboSlopeDown",new BlockChange(BlockType.Block,"RoadTechCheckpointSlopeDown"));
        map.replace("RoadDirtSpecialTurbo",new BlockChange(BlockType.Block,"RoadDirtCheckpoint"));
        map.replace("RoadDirtSpecialTurboSlopeUp",new BlockChange(BlockType.Block,"RoadDirtCheckpointSlopeUp"));
        map.replace("RoadDirtSpecialTurboSlopeDown",new BlockChange(BlockType.Block,"RoadDirtCheckpointSlopeDown"));
        map.replace("RoadBumpSpecialTurbo",new BlockChange(BlockType.Block,"RoadBumpCheckpoint"));
        map.replace("RoadBumpSpecialTurboSlopeUp",new BlockChange(BlockType.Block,"RoadBumpCheckpointSlopeUp"));
        map.replace("RoadBumpSpecialTurboSlopeDown",new BlockChange(BlockType.Block,"RoadBumpCheckpointSlopeDown"));
        map.replace("RoadIceSpecialTurbo",new BlockChange(BlockType.Block,"RoadIceCheckpoint"));
        map.replace("RoadIceSpecialTurboSlopeUp",new BlockChange(BlockType.Block,"RoadIceCheckpointSlopeUp"));
        map.replace("RoadIceSpecialTurboSlopeDown",new BlockChange(BlockType.Block,"RoadIceCheckpointSlopeDown"));
        map.replace("RoadWaterSpecialTurbo",new BlockChange(BlockType.Block,"RoadWaterCheckpoint"));
        map.replace("GateSpecialTurbo",new BlockChange(BlockType.Block,"GateCheckpoint"));
        map.replace("PlatformTechSpecialTurbo",new BlockChange(BlockType.Block,"PlatformTechCheckpoint"));
        map.replace("PlatformTechSpecialTurboSlope2Up",new BlockChange(BlockType.Block,"PlatformTechCheckpointSlope2Up"));
        map.replace("PlatformTechSpecialTurboSlope2Down",new BlockChange(BlockType.Block,"PlatformTechCheckpointSlope2Down"));
        map.replace("PlatformTechSpecialTurboSlope2Right",new BlockChange(BlockType.Block,"PlatformTechCheckpointSlope2Right"));
        map.replace("PlatformTechSpecialTurboSlope2Left",new BlockChange(BlockType.Block,"PlatformTechCheckpointSlope2Left"));
        map.replace("PlatformPlasticSpecialTurbo",new BlockChange(BlockType.Block,"PlatformPlasticCheckpoint"));
        map.replace("PlatformPlasticSpecialTurboSlope2Up",new BlockChange(BlockType.Block,"PlatformPlasticCheckpointSlope2Up"));
        map.replace("PlatformPlasticSpecialTurboSlope2Down",new BlockChange(BlockType.Block,"PlatformPlasticCheckpointSlope2Down"));
        map.replace("PlatformPlasticSpecialTurboSlope2Right",new BlockChange(BlockType.Block,"PlatformPlasticCheckpointSlope2Right"));
        map.replace("PlatformPlasticSpecialTurboSlope2Left",new BlockChange(BlockType.Block,"PlatformPlasticCheckpointSlope2Left"));
        map.replace("PlatformDirtSpecialTurbo",new BlockChange(BlockType.Block,"PlatformDirtCheckpoint"));
        map.replace("PlatformDirtSpecialTurboSlope2Up",new BlockChange(BlockType.Block,"PlatformDirtCheckpointSlope2Up"));
        map.replace("PlatformDirtSpecialTurboSlope2Down",new BlockChange(BlockType.Block,"PlatformDirtCheckpointSlope2Down"));
        map.replace("PlatformDirtSpecialTurboSlope2Right",new BlockChange(BlockType.Block,"PlatformDirtCheckpointSlope2Right"));
        map.replace("PlatformDirtSpecialTurboSlope2Left",new BlockChange(BlockType.Block,"PlatformDirtCheckpointSlope2Left"));
        map.replace("PlatformIceSpecialTurbo",new BlockChange(BlockType.Block,"PlatformIceCheckpoint"));
        map.replace("PlatformIceSpecialTurboSlope2Up",new BlockChange(BlockType.Block,"PlatformIceCheckpointSlope2Up"));
        map.replace("PlatformIceSpecialTurboSlope2Down",new BlockChange(BlockType.Block,"PlatformIceCheckpointSlope2Down"));
        map.replace("PlatformIceSpecialTurboSlope2Right",new BlockChange(BlockType.Block,"PlatformIceCheckpointSlope2Right"));
        map.replace("PlatformIceSpecialTurboSlope2Left",new BlockChange(BlockType.Block,"PlatformIceCheckpointSlope2Left"));
        map.replace("PlatformGrassSpecialTurbo",new BlockChange(BlockType.Block,"PlatformGrassCheckpoint"));
        map.replace("PlatformGrassSpecialTurboSlope2Up",new BlockChange(BlockType.Block,"PlatformGrassCheckpointSlope2Up"));
        map.replace("PlatformGrassSpecialTurboSlope2Down",new BlockChange(BlockType.Block,"PlatformGrassCheckpointSlope2Down"));
        map.replace("PlatformGrassSpecialTurboSlope2Right",new BlockChange(BlockType.Block,"PlatformGrassCheckpointSlope2Right"));
        map.replace("PlatformGrassSpecialTurboSlope2Left",new BlockChange(BlockType.Block,"PlatformGrassCheckpointSlope2Left"));
        map.replace("PlatformWaterSpecialTurbo",new BlockChange(BlockType.Block,"PlatformWaterCheckpoint"));
        map.replace("RoadTechSpecialTurboTiltLeft",new BlockChange(BlockType.Block,"RoadTechCheckpointTiltLeft"));
        map.replace("RoadTechSpecialTurboTiltRight",new BlockChange(BlockType.Block,"RoadTechCheckpointTiltRight"));
        map.replace("RoadDirtSpecialTurboTiltLeft",new BlockChange(BlockType.Block,"RoadDirtCheckpointTiltLeft"));
        map.replace("RoadDirtSpecialTurboTiltRight",new BlockChange(BlockType.Block,"RoadDirtCheckpointTiltRight"));
        map.replace("RoadBumpSpecialTurboTiltLeft",new BlockChange(BlockType.Block,"RoadBumpCheckpointTiltLeft"));
        map.replace("RoadBumpSpecialTurboTiltRight",new BlockChange(BlockType.Block,"RoadBumpCheckpointTiltRight"));
        map.replace("RoadTechSpecialTurboDiagRight",new DiagBlockChange(BlockType.Block,"RoadTechDiagRightCheckpoint"));
        map.replace("RoadDirtSpecialTurboDiagRight",new DiagBlockChange(BlockType.Block,"RoadDirtDiagRightCheckpoint"));
        map.replace("RoadBumpSpecialTurboDiagRight",new DiagBlockChange(BlockType.Block,"RoadBumpDiagRightCheckpoint"));
        map.replace("RoadTechSpecialTurboDiagLeft",new DiagBlockChange(BlockType.Block,"RoadTechDiagLeftCheckpoint"));
        map.replace("RoadDirtSpecialTurboDiagLeft",new DiagBlockChange(BlockType.Block,"RoadDirtDiagLeftCheckpoint"));
        map.replace("RoadBumpSpecialTurboDiagLeft",new DiagBlockChange(BlockType.Block,"RoadBumpDiagLeftCheckpoint"));
        map.replace("GateSpecialTurboLeft32m",new BlockChange(BlockType.Block,"GateCheckpointLeft32m"));

        map.replace("GateSpecialTurboCenter32mv2",new BlockChange(BlockType.Item,"GateCheckpointCenter32mv2"));
        map.replace("GateSpecialTurboRight32m",new BlockChange(BlockType.Item,"GateCheckpointRight32m"));
        map.replace("GateSpecialTurboLeft16m",new BlockChange(BlockType.Item,"GateCheckpointLeft16m"));
        map.replace("GateSpecialTurboCenter16mv2",new BlockChange(BlockType.Item,"GateCheckpointCenter16mv2"));
        map.replace("GateSpecialTurboRight16m",new BlockChange(BlockType.Item,"GateCheckpointRight16m"));
        map.replace("GateSpecialTurboLeft8m",new BlockChange(BlockType.Item,"GateCheckpointLeft8m"));
        map.replace("GateSpecialTurboCenter8mv2",new BlockChange(BlockType.Item,"GateCheckpointCenter8mv2"));
        map.replace("GateSpecialTurboRight8m",new BlockChange(BlockType.Item,"GateCheckpointRight8m"));
        map.placeStagedBlocks();
    }

    public static void CPLess(Map map){//TODO Platform under the CP doesn't get removed
        map.delete("RoadIceWithWallCheckpointRight");
        map.delete("RoadIceWithWallCheckpointLeft");
        map.delete("RoadIceWithWallDiagRightCheckpointRight");
        map.delete("RoadIceWithWallDiagLeftCheckpointRight");
        map.delete("RoadIceWithWallDiagRightCheckpointLeft");
        map.delete("RoadIceWithWallDiagLeftCheckpointLeft");
        map.delete("RoadTechCheckpoint");
        map.delete("RoadTechCheckpointSlopeUp");
        map.delete("RoadTechCheckpointSlopeDown");
        map.delete("RoadDirtCheckpoint");
        map.delete("RoadDirtCheckpointSlopeUp");
        map.delete("RoadDirtCheckpointSlopeDown");
        map.delete("RoadBumpCheckpoint");
        map.delete("RoadBumpCheckpointSlopeUp");
        map.delete("RoadBumpCheckpointSlopeDown");
        map.delete("RoadIceCheckpoint");
        map.delete("RoadIceCheckpointSlopeUp");
        map.delete("RoadIceCheckpointSlopeDown");
        map.delete("RoadWaterCheckpoint");
        map.delete("GateCheckpoint");
        map.delete("PlatformTechCheckpoint");
        map.delete("PlatformTechCheckpointSlope2Up");
        map.delete("PlatformTechCheckpointSlope2Down");
        map.delete("PlatformTechCheckpointSlope2Right");
        map.delete("PlatformTechCheckpointSlope2Left");
        map.delete("PlatformPlasticCheckpoint");
        map.delete("PlatformPlasticCheckpointSlope2Up");
        map.delete("PlatformPlasticCheckpointSlope2Down");
        map.delete("PlatformPlasticCheckpointSlope2Right");
        map.delete("PlatformPlasticCheckpointSlope2Left");
        map.delete("PlatformDirtCheckpoint");
        map.delete("PlatformDirtCheckpointSlope2Up");
        map.delete("PlatformDirtCheckpointSlope2Down");
        map.delete("PlatformDirtCheckpointSlope2Right");
        map.delete("PlatformDirtCheckpointSlope2Left");
        map.delete("PlatformIceCheckpoint");
        map.delete("PlatformIceCheckpointSlope2Up");
        map.delete("PlatformIceCheckpointSlope2Down");
        map.delete("PlatformIceCheckpointSlope2Right");
        map.delete("PlatformIceCheckpointSlope2Left");
        map.delete("PlatformGrassCheckpoint");
        map.delete("PlatformGrassCheckpointSlope2Up");
        map.delete("PlatformGrassCheckpointSlope2Down");
        map.delete("PlatformGrassCheckpointSlope2Right");
        map.delete("PlatformGrassCheckpointSlope2Left");
        map.delete("PlatformWaterCheckpoint");
        map.delete("RoadTechCheckpointTiltLeft");
        map.delete("RoadTechCheckpointTiltRight");
        map.delete("RoadDirtCheckpointTiltLeft");
        map.delete("RoadDirtCheckpointTiltRight");
        map.delete("RoadBumpCheckpointTiltLeft");
        map.delete("RoadBumpCheckpointTiltRight");
        map.delete("RoadTechDiagRightCheckpoint");
        map.delete("RoadDirtDiagRightCheckpoint");
        map.delete("RoadBumpDiagRightCheckpoint");
        map.delete("RoadTechDiagLeftCheckpoint");
        map.delete("RoadDirtDiagLeftCheckpoint");
        map.delete("RoadBumpDiagLeftCheckpoint");
        map.delete("GateCheckpointLeft32m");
        map.delete("GateCheckpointCenter32mv2");
        map.delete("GateCheckpointRight32m");
        map.delete("GateCheckpointLeft16m");
        map.delete("GateCheckpointCenter16mv2");
        map.delete("GateCheckpointRight16m");
        map.delete("GateCheckpointLeft8m");
        map.delete("GateCheckpointCenter8mv2");
        map.delete("GateCheckpointRight8m");
    }

    public static void STTF(Map map){
        map.replace("RoadIceWithWallCheckpointRight",new BlockChange(BlockType.Block,"RoadIceWithWallStraight"));
        map.replace("RoadIceWithWallCheckpointLeft",new BlockChange(BlockType.Block,"RoadIceWithWallStraight"));
        map.replace("RoadIceWithWallDiagRightCheckpointRight",new DiagBlockChange(BlockType.Block,"RoadIceDiagLeftWithWallStraight"));
        map.replace("RoadIceWithWallDiagLeftCheckpointRight",new DiagBlockChange(BlockType.Block,"RoadIceDiagLeftWithWallStraight",new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadIceWithWallDiagRightCheckpointLeft",new DiagBlockChange(BlockType.Block,"RoadIceDiagRightWithWallStraight"));
        map.replace("RoadIceWithWallDiagLeftCheckpointLeft",new DiagBlockChange(BlockType.Block,"RoadIceDiagRightWithWallStraight",new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadTechCheckpoint",new BlockChange(BlockType.Block,"RoadTechStraight"));
        map.replace("RoadTechCheckpointSlopeUp",new BlockChange(BlockType.Block,"RoadTechSlopeUpStraight"));
        map.replace("RoadTechCheckpointSlopeDown",new BlockChange(BlockType.Block,"RoadTechSlopeDownStraight",new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadDirtCheckpoint",new BlockChange(BlockType.Block,"RoadDirtStraight"));
        map.replace("RoadDirtCheckpointSlopeUp",new BlockChange(BlockType.Block,"RoadDirtSlopeUpStraight"));
        map.replace("RoadDirtCheckpointSlopeDown",new BlockChange(BlockType.Block,"RoadDirtSlopeUpStraight",new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadBumpCheckpoint",new BlockChange(BlockType.Block,"RoadBumpStraight"));
        map.replace("RoadBumpCheckpointSlopeUp",new BlockChange(BlockType.Block,"RoadBumpSlopeUpStraight"));
        map.replace("RoadBumpCheckpointSlopeDown",new BlockChange(BlockType.Block,"RoadBumpSlopeUpStraight",new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadIceCheckpoint",new BlockChange(BlockType.Block,"RoadIceStraight"));
        map.replace("RoadIceCheckpointSlopeUp",new BlockChange(BlockType.Block,"RoadIceSlopeUpStraight"));
        map.replace("RoadIceCheckpointSlopeDown",new BlockChange(BlockType.Block,"RoadIceSlopeUpStraight",new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadWaterCheckpoint",new BlockChange(BlockType.Block,"RoadWaterStraight"));
        map.delete("GateCheckpoint");
        map.replace("PlatformTechCheckpoint",new BlockChange(BlockType.Block,"PlatformTechBase"));
        map.replace("PlatformTechCheckpointSlope2Up",new BlockChange(BlockType.Block,"PlatformTechSlope2Straight"));
        map.replace("PlatformTechCheckpointSlope2Down",new BlockChange(BlockType.Block,"PlatformTechSlope2Straight",Vec3.Zero, new Vec3(PI,0,0)));//TODO correct Offset
        map.replace("PlatformTechCheckpointSlope2Right",new BlockChange(BlockType.Block,"PlatformTechSpecialTurboSlope2Right",new Vec3(32,0,32), new Vec3(PI*0.5f,0,0)));
        map.replace("PlatformTechCheckpointSlope2Left",new BlockChange(BlockType.Block,"PlatformTechSpecialTurboSlope2Left",Vec3.Zero, new Vec3(PI*1.5f,0,0)));
        map.replace("PlatformPlasticCheckpoint",new BlockChange(BlockType.Block,"PlatformPlasticBase"));
        map.replace("PlatformPlasticCheckpointSlope2Up",new BlockChange(BlockType.Block,"PlatformPlasticSlope2Straight"));
        map.replace("PlatformPlasticCheckpointSlope2Down",new BlockChange(BlockType.Block,"PlatformPlasticSlope2Straight",Vec3.Zero, new Vec3(PI,0,0)));
        map.replace("PlatformPlasticCheckpointSlope2Right",new BlockChange(BlockType.Block,"PlatformPlasticSpecialTurboSlope2Right",new Vec3(32,0,32), new Vec3(PI*0.5f,0,0)));
        map.replace("PlatformPlasticCheckpointSlope2Left",new BlockChange(BlockType.Block,"PlatformPlasticSpecialTurboSlope2Left",Vec3.Zero, new Vec3(PI*1.5f,0,0)));
        map.replace("PlatformDirtCheckpoint",new BlockChange(BlockType.Block,"PlatformDirtBase"));
        map.replace("PlatformDirtCheckpointSlope2Up",new BlockChange(BlockType.Block,"PlatformDirtSlope2Straight"));
        map.replace("PlatformDirtCheckpointSlope2Down",new BlockChange(BlockType.Block,"PlatformDirtSlope2Straight",Vec3.Zero, new Vec3(PI,0,0)));
        map.replace("PlatformDirtCheckpointSlope2Right",new BlockChange(BlockType.Block,"PlatformDirtSpecialTurboSlope2Right",new Vec3(32,0,32), new Vec3(PI*0.5f,0,0)));
        map.replace("PlatformDirtCheckpointSlope2Left",new BlockChange(BlockType.Block,"PlatformDirtSpecialTurboSlope2Left",Vec3.Zero, new Vec3(PI*1.5f,0,0)));
        map.replace("PlatformIceCheckpoint",new BlockChange(BlockType.Block,"PlatformIceBase"));
        map.replace("PlatformIceCheckpointSlope2Up",new BlockChange(BlockType.Block,"PlatformIceSlope2Straight"));
        map.replace("PlatformIceCheckpointSlope2Down",new BlockChange(BlockType.Block,"PlatformIceSlope2Straight",Vec3.Zero, new Vec3(PI,0,0)));
        map.replace("PlatformIceCheckpointSlope2Right",new BlockChange(BlockType.Block,"PlatformIceSpecialTurboSlope2Right",new Vec3(32,0,32), new Vec3(PI*0.5f,0,0)));
        map.replace("PlatformIceCheckpointSlope2Left",new BlockChange(BlockType.Block,"PlatformIceSpecialTurboSlope2Left",Vec3.Zero, new Vec3(PI*1.5f,0,0)));
        map.replace("PlatformGrassCheckpoint",new BlockChange(BlockType.Block,"PlatformGrassBase"));
        map.replace("PlatformGrassCheckpointSlope2Up",new BlockChange(BlockType.Block,"PlatformGrassSlope2Straight"));
        map.replace("PlatformGrassCheckpointSlope2Down",new BlockChange(BlockType.Block,"PlatformGrassSlope2Straight",Vec3.Zero, new Vec3(PI,0,0)));
        map.replace("PlatformGrassCheckpointSlope2Right",new BlockChange(BlockType.Block,"PlatformGrassSpecialTurboSlope2Right",new Vec3(32,0,32), new Vec3(PI*0.5f,0,0)));
        map.replace("PlatformGrassCheckpointSlope2Left",new BlockChange(BlockType.Block,"PlatformGrassSpecialTurboSlope2Left",Vec3.Zero, new Vec3(PI*1.5f,0,0)));
        map.replace("PlatformWaterCheckpoint",new BlockChange(BlockType.Block,"PlatformWaterRampBase"));
        map.replace("RoadTechCheckpointTiltLeft",new BlockChange(BlockType.Block,"RoadTechTiltStraight",new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadTechCheckpointTiltRight",new BlockChange(BlockType.Block,"RoadTechTiltStraight"));
        map.replace("RoadDirtCheckpointTiltLeft",new BlockChange(BlockType.Block,"RoadDirtTiltStraight",new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadDirtCheckpointTiltRight",new BlockChange(BlockType.Block,"RoadDirtTiltStraight"));
        map.replace("RoadBumpCheckpointTiltLeft",new BlockChange(BlockType.Block,"RoadBumpTiltStraight",new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadBumpCheckpointTiltRight",new BlockChange(BlockType.Block,"RoadBumpTiltStraight"));
        map.replace("RoadTechDiagRightCheckpoint",new DiagBlockChange(BlockType.Block,"RoadTechDiagRightStraightX2"));
        map.replace("RoadDirtDiagRightCheckpoint",new DiagBlockChange(BlockType.Block,"RoadDirtDiagRightStraightX2"));
        map.replace("RoadBumpDiagRightCheckpoint",new DiagBlockChange(BlockType.Block,"RoadBumpDiagRightStraightX2"));
        map.replace("RoadTechDiagLeftCheckpoint",new DiagBlockChange(BlockType.Block,"RoadTechDiagLeftStraightX2"));
        map.replace("RoadDirtDiagLeftCheckpoint",new DiagBlockChange(BlockType.Block,"RoadDirtDiagLeftStraightX2"));
        map.replace("RoadBumpDiagLeftCheckpoint",new DiagBlockChange(BlockType.Block,"RoadBumpDiagLeftStraightX2"));
        map.delete("GateCheckpointLeft32m");
        map.delete("GateCheckpointCenter32mv2");
        map.delete("GateCheckpointRight32m");
        map.delete("GateCheckpointLeft16m");
        map.delete("GateCheckpointCenter16mv2");
        map.delete("GateCheckpointRight16m");
        map.delete("GateCheckpointLeft8m");
        map.delete("GateCheckpointCenter8mv2");
        map.delete("GateCheckpointRight8m");
        map.placeStagedBlocks();
    }

    public static void CPFull(){//STTF mirrord

    }
}

class DiagBlockChange : BlockChange{
    public DiagBlockChange(BlockType blockType, string model) : base(blockType,model){}
    public DiagBlockChange(BlockType blockType, string model, Vec3 absolutePosition) : base(blockType,model,absolutePosition){}
    public DiagBlockChange(BlockType blockType, string model, Vec3 absolutePosition, Vec3 pitchYawRoll) : base(blockType,model,absolutePosition,pitchYawRoll){}
    public DiagBlockChange(Vec3 absolutePosition) : base(absolutePosition){}
    public DiagBlockChange(Vec3 absolutePosition, Vec3 pitchYawRoll) : base(absolutePosition,pitchYawRoll){}

    public override void changeBlock(CGameCtnBlock ctnBlock,Block @block){
        switch (ctnBlock.Direction){
            case Direction.North:
                block.relativeOffset(new Vec3(0,0,0));
                break;
            case Direction.East:
                block.relativeOffset(new Vec3(0,0,-32));
                break;
            case Direction.South:
                block.relativeOffset(new Vec3(-64,0,-32));
                break;
            case Direction.West:
                block.relativeOffset(new Vec3(-64,0,0));
                break;
        }
        
        if (model != "") {
            block.blockType = blockType;
            block.model = model;
        }
        block.relativeOffset(absolutePosition);
        block.pitchYawRoll += pitchYawRoll;
    }
}