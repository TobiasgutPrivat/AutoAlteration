using GBX.NET;
using GBX.NET.Engines.Game;
class CPAlterations{
    static float PI = (float)Math.PI;
    public static void CPBoost(Map map){
        //CP -> Turbo
        map.replace("RoadIceWithWallCheckpointRight","RoadIceWithWallSpecialTurboRight");
        map.replace("RoadIceWithWallCheckpointLeft","RoadIceWithWallSpecialTurboLeft");
        map.replace("RoadIceWithWallDiagRightCheckpointRight","RoadIceWithWallDiagRightSpecialTurboRight");
        map.replace("RoadIceWithWallDiagLeftCheckpointRight","RoadIceWithWallDiagLeftSpecialTurboRight");
        map.replace("RoadIceWithWallDiagRightCheckpointLeft","RoadIceWithWallDiagRightSpecialTurboLeft");
        map.replace("RoadIceWithWallDiagLeftCheckpointLeft","RoadIceWithWallDiagLeftSpecialTurboLeft");
        map.replace("RoadTechCheckpoint","RoadTechSpecialTurbo");
        map.replace("RoadTechCheckpointSlopeUp","RoadTechSpecialTurboSlopeUp");
        map.replace("RoadTechCheckpointSlopeDown","RoadTechSpecialTurboSlopeDown");
        map.replace("RoadDirtCheckpoint","RoadDirtSpecialTurbo");
        map.replace("RoadDirtCheckpointSlopeUp","RoadDirtSpecialTurboSlopeUp");
        map.replace("RoadDirtCheckpointSlopeDown","RoadDirtSpecialTurboSlopeDown");
        map.replace("RoadBumpCheckpoint","RoadBumpSpecialTurbo");
        map.replace("RoadBumpCheckpointSlopeUp","RoadBumpSpecialTurboSlopeUp");
        map.replace("RoadBumpCheckpointSlopeDown","RoadBumpSpecialTurboSlopeDown");
        map.replace("RoadIceCheckpoint","RoadIceSpecialTurbo");
        map.replace("RoadIceCheckpointSlopeUp","RoadIceSpecialTurboSlopeUp");
        map.replace("RoadIceCheckpointSlopeDown","RoadIceSpecialTurboSlopeDown");
        map.replace("RoadWaterCheckpoint","RoadWaterSpecialTurbo");
        map.replace("GateCheckpoint","GateSpecialTurbo");
        map.replace("PlatformTechCheckpoint","PlatformTechSpecialTurbo");
        map.replace("PlatformTechCheckpointSlope2Up","PlatformTechSpecialTurboSlope2Up");
        map.replace("PlatformTechCheckpointSlope2Down","PlatformTechSpecialTurboSlope2Down");
        map.replace("PlatformTechCheckpointSlope2Right","PlatformTechSpecialTurboTilt2Right");
        map.replace("PlatformTechCheckpointSlope2Left","PlatformTechSpecialTurboTilt2Left");
        map.replace("PlatformPlasticCheckpoint","PlatformPlasticSpecialTurbo");
        map.replace("PlatformPlasticCheckpointSlope2Up","PlatformPlasticSpecialTurboSlope2Up");
        map.replace("PlatformPlasticCheckpointSlope2Down","PlatformPlasticSpecialTurboSlope2Down");
        map.replace("PlatformPlasticCheckpointSlope2Right","PlatformPlasticSpecialTurboTilt2Right");
        map.replace("PlatformPlasticCheckpointSlope2Left","PlatformPlasticSpecialTurboTilt2Left");
        map.replace("PlatformDirtCheckpoint","PlatformDirtSpecialTurbo");
        map.replace("PlatformDirtCheckpointSlope2Up","PlatformDirtSpecialTurboSlope2Up");
        map.replace("PlatformDirtCheckpointSlope2Down","PlatformDirtSpecialTurboSlope2Down");
        map.replace("PlatformDirtCheckpointSlope2Right","PlatformDirtSpecialTurboTilt2Right");
        map.replace("PlatformDirtCheckpointSlope2Left","PlatformDirtSpecialTurboTilt2Left");
        map.replace("PlatformIceCheckpoint","PlatformIceSpecialTurbo");
        map.replace("PlatformIceCheckpointSlope2Up","PlatformIceSpecialTurboSlope2Up");
        map.replace("PlatformIceCheckpointSlope2Down","PlatformIceSpecialTurboSlope2Down");
        map.replace("PlatformIceCheckpointSlope2Right","PlatformIceSpecialTurboTilt2Right");
        map.replace("PlatformIceCheckpointSlope2Left","PlatformIceSpecialTurboTilt2Left");
        map.replace("PlatformGrassCheckpoint","PlatformGrassSpecialTurbo");
        map.replace("PlatformGrassCheckpointSlope2Up","PlatformGrassSpecialTurboSlope2Up");
        map.replace("PlatformGrassCheckpointSlope2Down","PlatformGrassSpecialTurboSlope2Down");
        map.replace("PlatformGrassCheckpointSlope2Right","PlatformGrassSpecialTurboTilt2Right");
        map.replace("PlatformGrassCheckpointSlope2Left","PlatformGrassSpecialTurboTilt2Left");
        map.replace("PlatformWaterCheckpoint","PlatformWaterSpecialTurbo");
        map.replace("RoadTechCheckpointTiltLeft","RoadTechSpecialTurboTiltLeft");
        map.replace("RoadTechCheckpointTiltRight","RoadTechSpecialTurboTiltRight");
        map.replace("RoadDirtCheckpointTiltLeft","RoadDirtSpecialTurboTiltLeft");
        map.replace("RoadDirtCheckpointTiltRight","RoadDirtSpecialTurboTiltRight");
        map.replace("RoadBumpCheckpointTiltLeft","RoadBumpSpecialTurboTiltLeft");
        map.replace("RoadBumpCheckpointTiltRight","RoadBumpSpecialTurboTiltRight");
        map.replace("RoadTechDiagRightCheckpoint","RoadTechSpecialTurboDiagRight");
        map.replace("RoadDirtDiagRightCheckpoint","RoadDirtSpecialTurboDiagRight");
        map.replace("RoadBumpDiagRightCheckpoint","RoadBumpSpecialTurboDiagRight");
        map.replace("RoadTechDiagLeftCheckpoint","RoadTechSpecialTurboDiagLeft");
        map.replace("RoadDirtDiagLeftCheckpoint","RoadDirtSpecialTurboDiagLeft");
        map.replace("RoadBumpDiagLeftCheckpoint","RoadBumpSpecialTurboDiagLeft");

        map.replace("GateCheckpointLeft32m","GateSpecialTurboLeft32m");
        map.replace("GateCheckpointCenter32mv2","GateSpecialTurboCenter32mv2");
        map.replace("GateCheckpointRight32m","GateSpecialTurboRight32m");
        map.replace("GateCheckpointLeft16m","GateSpecialTurboLeft16m");
        map.replace("GateCheckpointCenter16mv2","GateSpecialTurboCenter16mv2");
        map.replace("GateCheckpointRight16m","GateSpecialTurboRight16m");
        map.replace("GateCheckpointLeft8m","GateSpecialTurboLeft8m");
        map.replace("GateCheckpointCenter8mv2","GateSpecialTurboCenter8mv2");
        map.replace("GateCheckpointRight8m","GateSpecialTurboRight8m");
        //TODO WallCPs
        // map.replace("PlatformTechWallCheckpointUp",""));
        // map.replace("PlatformTechWallCheckpointDown",""));
        // map.replace("PlatformTechWallCheckpointLeft",""));
        // map.replace("PlatformTechWallCheckpointRight",""));

        // //Turbo -> CP
        map.replace("RoadIceWithWallSpecialTurboRight","RoadIceWithWallCheckpointRight");
        map.replace("RoadIceWithWallSpecialTurboLeft","RoadIceWithWallCheckpointLeft");
        map.replace("RoadIceWithWallDiagRightSpecialTurboRight","RoadIceWithWallDiagRightCheckpointRight");
        map.replace("RoadIceWithWallDiagLeftSpecialTurboRight","RoadIceWithWallDiagLeftCheckpointRight");
        map.replace("RoadIceWithWallDiagRightSpecialTurboLeft","RoadIceWithWallDiagRightCheckpointLeft");
        map.replace("RoadIceWithWallDiagLeftSpecialTurboLeft","RoadIceWithWallDiagLeftCheckpointLeft");
        map.replace("RoadTechSpecialTurbo","RoadTechCheckpoint");//TODO Issue here removing Boost (Example Spring 2020 - S15)
        map.replace("RoadTechSpecialTurboSlopeUp","RoadTechCheckpointSlopeUp");
        map.replace("RoadTechSpecialTurboSlopeDown","RoadTechCheckpointSlopeDown");
        map.replace("RoadDirtSpecialTurbo","RoadDirtCheckpoint");
        map.replace("RoadDirtSpecialTurboSlopeUp","RoadDirtCheckpointSlopeUp");
        map.replace("RoadDirtSpecialTurboSlopeDown","RoadDirtCheckpointSlopeDown");
        map.replace("RoadBumpSpecialTurbo","RoadBumpCheckpoint");
        map.replace("RoadBumpSpecialTurboSlopeUp","RoadBumpCheckpointSlopeUp");
        map.replace("RoadBumpSpecialTurboSlopeDown","RoadBumpCheckpointSlopeDown");
        map.replace("RoadIceSpecialTurbo","RoadIceCheckpoint");
        map.replace("RoadIceSpecialTurboSlopeUp","RoadIceCheckpointSlopeUp");
        map.replace("RoadIceSpecialTurboSlopeDown","RoadIceCheckpointSlopeDown");
        map.replace("RoadWaterSpecialTurbo","RoadWaterCheckpoint");
        map.replace("GateSpecialTurbo","GateCheckpoint");
        map.replace("PlatformTechSpecialTurbo","PlatformTechCheckpoint");
        map.replace("PlatformTechSpecialTurboSlope2Up","PlatformTechCheckpointSlope2Up");
        map.replace("PlatformTechSpecialTurboSlope2Down","PlatformTechCheckpointSlope2Down");
        map.replace("PlatformTechSpecialTurboTilt2Right","PlatformTechCheckpointSlope2Right");
        map.replace("PlatformTechSpecialTurboTilt2Left","PlatformTechCheckpointSlope2Left");
        map.replace("PlatformPlasticSpecialTurbo","PlatformPlasticCheckpoint");
        map.replace("PlatformPlasticSpecialTurboSlope2Up","PlatformPlasticCheckpointSlope2Up");
        map.replace("PlatformPlasticSpecialTurboSlope2Down","PlatformPlasticCheckpointSlope2Down");
        map.replace("PlatformPlasticSpecialTurboTilt2Right","PlatformPlasticCheckpointSlope2Right");
        map.replace("PlatformPlasticSpecialTurboTilt2Left","PlatformPlasticCheckpointSlope2Left");
        map.replace("PlatformDirtSpecialTurbo","PlatformDirtCheckpoint");
        map.replace("PlatformDirtSpecialTurboSlope2Up","PlatformDirtCheckpointSlope2Up");
        map.replace("PlatformDirtSpecialTurboSlope2Down","PlatformDirtCheckpointSlope2Down");
        map.replace("PlatformDirtSpecialTurboTilt2Right","PlatformDirtCheckpointSlope2Right");
        map.replace("PlatformDirtSpecialTurboTilt2Left","PlatformDirtCheckpointSlope2Left");
        map.replace("PlatformIceSpecialTurbo","PlatformIceCheckpoint");
        map.replace("PlatformIceSpecialTurboSlope2Up","PlatformIceCheckpointSlope2Up");
        map.replace("PlatformIceSpecialTurboSlope2Down","PlatformIceCheckpointSlope2Down");
        map.replace("PlatformIceSpecialTurboTilt2Right","PlatformIceCheckpointSlope2Right");
        map.replace("PlatformIceSpecialTurboTilt2Left","PlatformIceCheckpointSlope2Left");
        map.replace("PlatformGrassSpecialTurbo","PlatformGrassCheckpoint");
        map.replace("PlatformGrassSpecialTurboSlope2Up","PlatformGrassCheckpointSlope2Up");
        map.replace("PlatformGrassSpecialTurboSlope2Down","PlatformGrassCheckpointSlope2Down");
        map.replace("PlatformGrassSpecialTurboTilt2Right","PlatformGrassCheckpointSlope2Right");
        map.replace("PlatformGrassSpecialTurboTilt2Left","PlatformGrassCheckpointSlope2Left");
        map.replace("PlatformWaterSpecialTurbo","PlatformWaterCheckpoint");
        map.replace("RoadTechSpecialTurboTiltLeft","RoadTechCheckpointTiltLeft");
        map.replace("RoadTechSpecialTurboTiltRight","RoadTechCheckpointTiltRight");
        map.replace("RoadDirtSpecialTurboTiltLeft","RoadDirtCheckpointTiltLeft");
        map.replace("RoadDirtSpecialTurboTiltRight","RoadDirtCheckpointTiltRight");
        map.replace("RoadBumpSpecialTurboTiltLeft","RoadBumpCheckpointTiltLeft");
        map.replace("RoadBumpSpecialTurboTiltRight","RoadBumpCheckpointTiltRight");
        map.replace("RoadTechSpecialTurboDiagRight","RoadTechDiagRightCheckpoint");
        map.replace("RoadDirtSpecialTurboDiagRight","RoadDirtDiagRightCheckpoint");
        map.replace("RoadBumpSpecialTurboDiagRight","RoadBumpDiagRightCheckpoint");
        map.replace("RoadTechSpecialTurboDiagLeft","RoadTechDiagLeftCheckpoint");
        map.replace("RoadDirtSpecialTurboDiagLeft","RoadDirtDiagLeftCheckpoint");
        map.replace("RoadBumpSpecialTurboDiagLeft","RoadBumpDiagLeftCheckpoint");
        map.replace("GateSpecialTurboLeft32m","GateCheckpointLeft32m");

        map.replace("GateSpecialTurboCenter32mv2","GateCheckpointCenter32mv2");
        map.replace("GateSpecialTurboRight32m","GateCheckpointRight32m");
        map.replace("GateSpecialTurboLeft16m","GateCheckpointLeft16m");
        map.replace("GateSpecialTurboCenter16mv2","GateCheckpointCenter16mv2");
        map.replace("GateSpecialTurboRight16m","GateCheckpointRight16m");
        map.replace("GateSpecialTurboLeft8m","GateCheckpointLeft8m");
        map.replace("GateSpecialTurboCenter8mv2","GateCheckpointCenter8mv2");
        map.replace("GateSpecialTurboRight8m","GateCheckpointRight8m");
        map.placeStagedBlocks();

        //TODO same for red boost (Turbo -> Turbo2)
    }

    private static void CPLess(Map map){//TODO Platform under the CP doesn't get removed (public when working)
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
        map.replace("RoadIceWithWallCheckpointRight","RoadIceWithWallStraight");
        map.replace("RoadIceWithWallCheckpointLeft","RoadIceWithWallStraight");
        map.replace("RoadIceWithWallDiagRightCheckpointRight","RoadIceDiagLeftWithWallStraight");
        map.replace("RoadIceWithWallDiagLeftCheckpointRight","RoadIceDiagLeftWithWallStraight",new DiagBlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadIceWithWallDiagRightCheckpointLeft","RoadIceDiagRightWithWallStraight");
        map.replace("RoadIceWithWallDiagLeftCheckpointLeft","RoadIceDiagRightWithWallStraight",new DiagBlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadTechCheckpoint","RoadTechStraight");
        map.replace("RoadTechCheckpointSlopeUp","RoadTechSlopeUpStraight");
        map.replace("RoadTechCheckpointSlopeDown","RoadTechSlopeDownStraight",new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadDirtCheckpoint","RoadDirtStraight");
        map.replace("RoadDirtCheckpointSlopeUp","RoadDirtSlopeUpStraight");
        map.replace("RoadDirtCheckpointSlopeDown","RoadDirtSlopeUpStraight",new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadBumpCheckpoint","RoadBumpStraight");
        map.replace("RoadBumpCheckpointSlopeUp","RoadBumpSlopeUpStraight");
        map.replace("RoadBumpCheckpointSlopeDown","RoadBumpSlopeUpStraight",new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadIceCheckpoint","RoadIceStraight");
        map.replace("RoadIceCheckpointSlopeUp","RoadIceSlopeUpStraight");
        map.replace("RoadIceCheckpointSlopeDown","RoadIceSlopeUpStraight",new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadWaterCheckpoint","RoadWaterStraight");
        map.delete("GateCheckpoint");
        map.replace("PlatformTechCheckpoint","PlatformTechBase");
        map.replace("PlatformTechCheckpointSlope2Up","PlatformTechSlope2Straight");
        map.replace("PlatformTechCheckpointSlope2Down","PlatformTechSlope2Straight",new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("PlatformTechCheckpointSlope2Right","PlatformTechSlope2Straight",new BlockChange(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0)));
        map.replace("PlatformTechCheckpointSlope2Left","PlatformTechSlope2Straight",new BlockChange(new Vec3(32,0,0), new Vec3(PI*1.5f,0,0)));
        map.replace("PlatformPlasticCheckpoint","PlatformPlasticBase");
        map.replace("PlatformPlasticCheckpointSlope2Up","PlatformPlasticSlope2Straight");
        map.replace("PlatformPlasticCheckpointSlope2Down","PlatformPlasticSlope2Straight",new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("PlatformPlasticCheckpointSlope2Right","PlatformPlasticSlope2Straight",new BlockChange(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0)));
        map.replace("PlatformPlasticCheckpointSlope2Left","PlatformPlasticSlope2Straight",new BlockChange(new Vec3(32,0,0), new Vec3(PI*1.5f,0,0)));
        map.replace("PlatformDirtCheckpoint","PlatformDirtBase");
        map.replace("PlatformDirtCheckpointSlope2Up","PlatformDirtSlope2Straight");
        map.replace("PlatformDirtCheckpointSlope2Down","PlatformDirtSlope2Straight",new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("PlatformDirtCheckpointSlope2Right","PlatformDirtSlope2Straight",new BlockChange(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0)));
        map.replace("PlatformDirtCheckpointSlope2Left","PlatformDirtSlope2Straight",new BlockChange(new Vec3(32,0,0), new Vec3(PI*1.5f,0,0)));
        map.replace("PlatformIceCheckpoint","PlatformIceBase");
        map.replace("PlatformIceCheckpointSlope2Up","PlatformIceSlope2Straight");
        map.replace("PlatformIceCheckpointSlope2Down","PlatformIceSlope2Straight",new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("PlatformIceCheckpointSlope2Right","PlatformIceSlope2Straight",new BlockChange(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0)));
        map.replace("PlatformIceCheckpointSlope2Left","PlatformIceSlope2Straight",new BlockChange(new Vec3(32,0,0), new Vec3(PI*1.5f,0,0)));
        map.replace("PlatformGrassCheckpoint","PlatformGrassBase");
        map.replace("PlatformGrassCheckpointSlope2Up","PlatformGrassSlope2Straight");
        map.replace("PlatformGrassCheckpointSlope2Down","PlatformGrassSlope2Straight",new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("PlatformGrassCheckpointSlope2Right","PlatformGrassSlope2Straight",new BlockChange(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0)));
        map.replace("PlatformGrassCheckpointSlope2Left","PlatformGrassSlope2Straight",new BlockChange(new Vec3(32,0,0), new Vec3(PI*1.5f,0,0)));
        map.replace("PlatformWaterCheckpoint","PlatformWaterRampBase");
        map.replace("RoadTechCheckpointTiltLeft","RoadTechTiltStraight",new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadTechCheckpointTiltRight","RoadTechTiltStraight");
        map.replace("RoadDirtCheckpointTiltLeft","RoadDirtTiltStraight",new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadDirtCheckpointTiltRight","RoadDirtTiltStraight");
        map.replace("RoadBumpCheckpointTiltLeft","RoadBumpTiltStraight",new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadBumpCheckpointTiltRight","RoadBumpTiltStraight");
        map.replace("PlatformTechWallCheckpointUp","PlatformTechWallStraight4");
        map.replace("PlatformTechWallCheckpointDown","PlatformTechWallStraight4");
        map.replace("PlatformTechWallCheckpointLeft","PlatformTechWallStraight4");
        map.replace("PlatformTechWallCheckpointRight","PlatformTechWallStraight4");
        map.replace("PlatformDirtWallCheckpointUp","PlatformDirtWallStraight4");
        map.replace("PlatformDirtWallCheckpointDown","PlatformDirtWallStraight4");
        map.replace("PlatformDirtWallCheckpointLeft","PlatformDirtWallStraight4");
        map.replace("PlatformDirtWallCheckpointRight","PlatformDirtWallStraight4");
        map.replace("PlatformIceWallCheckpointUp","PlatformIceWallStraight4");
        map.replace("PlatformIceWallCheckpointDown","PlatformIceWallStraight4");
        map.replace("PlatformIceWallCheckpointLeft","PlatformIceWallStraight4");
        map.replace("PlatformIceWallCheckpointRight","PlatformIceWallStraight4");
        map.replace("PlatformPlasticWallCheckpointUp","PlatformPlasticWallStraight4");
        map.replace("PlatformPlasticWallCheckpointDown","PlatformPlasticWallStraight4");
        map.replace("PlatformPlasticWallCheckpointLeft","PlatformPlasticWallStraight4");
        map.replace("PlatformPlasticWallCheckpointRight","PlatformPlasticWallStraight4");
        map.replace("PlatformGrassWallCheckpointUp","PlatformGrassWallStraight4");
        map.replace("PlatformGrassWallCheckpointDown","PlatformGrassWallStraight4");
        map.replace("PlatformGrassWallCheckpointLeft","PlatformGrassWallStraight4");
        map.replace("PlatformGrassWallCheckpointRight","PlatformGrassWallStraight4");
        map.replace("RoadTechDiagRightCheckpoint","RoadTechDiagRightStraightX2");
        map.replace("RoadDirtDiagRightCheckpoint","RoadDirtDiagRightStraightX2");
        map.replace("RoadBumpDiagRightCheckpoint","RoadBumpDiagRightStraightX2");
        map.replace("RoadTechDiagLeftCheckpoint","RoadTechDiagLeftStraightX2");
        map.replace("RoadDirtDiagLeftCheckpoint","RoadDirtDiagLeftStraightX2");
        map.replace("RoadBumpDiagLeftCheckpoint","RoadBumpDiagLeftStraightX2");
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

    public static void CPFull(Map map){//basicly opposite of STTF mirrord, direction is not considered
        map.replace("RoadIceWithWallStraight","RoadIceWithWallCheckpointRight");
        map.replace("RoadIceWithWallStraight","RoadIceWithWallCheckpointLeft");
        map.replace("RoadIceDiagLeftWithWallStraight","RoadIceWithWallDiagRightCheckpointRight");
        map.replace("RoadIceDiagRightWithWallStraight","RoadIceWithWallDiagRightCheckpointLeft");
        map.replace("RoadTechStraight","RoadTechCheckpoint");
        map.replace("RoadTechSlopeUpStraight","RoadTechCheckpointSlopeUp");
        map.replace("RoadDirtStraight","RoadDirtCheckpoint");
        map.replace("RoadDirtSlopeUpStraight","RoadDirtCheckpointSlopeUp");
        map.replace("RoadBumpStraight","RoadBumpCheckpoint");
        map.replace("RoadBumpSlopeUpStraight","RoadBumpCheckpointSlopeUp");
        map.replace("RoadIceStraight","RoadIceCheckpoint");
        map.replace("RoadIceSlopeUpStraight","RoadIceCheckpointSlopeUp");
        map.replace("RoadWaterStraight","RoadWaterCheckpoint");
        map.replace("PlatformTechBase","PlatformTechCheckpoint");
        map.replace("PlatformTechSlope2Straight","PlatformTechCheckpointSlope2Up");
        map.replace("PlatformPlasticBase","PlatformPlasticCheckpoint");
        map.replace("PlatformPlasticSlope2Straight","PlatformPlasticCheckpointSlope2Up");
        map.replace("PlatformDirtBase","PlatformDirtCheckpoint");
        map.replace("PlatformDirtSlope2Straight","PlatformDirtCheckpointSlope2Up");
        map.replace("PlatformIceBase","PlatformIceCheckpoint");
        map.replace("PlatformIceSlope2Straight","PlatformIceCheckpointSlope2Up");
        map.replace("PlatformGrassBase","PlatformGrassCheckpoint");
        map.replace("PlatformGrassSlope2Straight","PlatformGrassCheckpointSlope2Up");
        map.replace("PlatformWaterRampBase","PlatformWaterCheckpoint");
        map.replace("RoadTechTiltStraight","RoadTechCheckpointTiltRight");
        map.replace("RoadDirtTiltStraight","RoadDirtCheckpointTiltRight");
        map.replace("RoadBumpTiltStraight","RoadBumpCheckpointTiltRight");
        map.replace("RoadTechDiagRightStraightX2","RoadTechDiagRightCheckpoint");
        map.replace("RoadDirtDiagRightStraightX2","RoadDirtDiagRightCheckpoint");
        map.replace("RoadBumpDiagRightStraightX2","RoadBumpDiagRightCheckpoint");
        map.replace("RoadTechDiagLeftStraightX2","RoadTechDiagLeftCheckpoint");
        map.replace("RoadDirtDiagLeftStraightX2","RoadDirtDiagLeftCheckpoint");
        map.replace("RoadBumpDiagLeftStraightX2","RoadBumpDiagLeftCheckpoint");
        map.replace("PlatformTechWallStraight4","PlatformTechWallCheckpointUp",new BlockChange(new Vec3(0,0,32),new Vec3(PI*0.5f,0,0)));
        map.replace("PlatformDirtWallStraight4","PlatformTechWallCheckpointUp",new BlockChange(new Vec3(0,0,32),new Vec3(PI*0.5f,0,0)));
        map.replace("PlatformPlasticWallStraight4","PlatformTechWallCheckpointUp",new BlockChange(new Vec3(0,0,32),new Vec3(PI*0.5f,0,0)));
        map.replace("PlatformIceWallStraight4","PlatformTechWallCheckpointUp",new BlockChange(new Vec3(0,0,32),new Vec3(PI*0.5f,0,0)));
        map.replace("PlatformGrassWallStraight4","PlatformTechWallCheckpointUp",new BlockChange(new Vec3(0,0,32),new Vec3(PI*0.5f,0,0)));
        map.placeStagedBlocks();
    }
}

class DiagBlockChange : BlockChange{
    public DiagBlockChange() : base(){}
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
        
        block.relativeOffset(absolutePosition);
        block.pitchYawRoll += pitchYawRoll;
    }
}