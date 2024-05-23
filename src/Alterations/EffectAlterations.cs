using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Exceptions;
class EffectAlteration: Alteration {
    public static float PI = (float)Math.PI;
    static string[] StartBlock = new string[] {"PlatformTechStart","RoadTechStart","RoadDirtStart","RoadBumpStart","RoadIceStart","PlatformTechStart","PlatformDirtStart","PlatformIceStart","PlatformGrassStart","PlatformPlasticStart","PlatformWaterStart"};
    static string[] MultilapBlock = new string[] {"PlatformTechMultilap","RoadTechMultilap","RoadDirtMultilap","RoadBumpMultilap","RoadIceMultilap","RoadWaterMultilap","PlatformTechMultilap","PlatformDirtMultilap","PlatformIceMultilap","PlatformGrassMultilap","PlatformPlasticMultilap","PlatformWaterMultilap"};
    static string[] IceWallRight = new string[] {"RoadIceWithWallMultilapRight","RoadIceWithWallCheckpointRight"};
    static string[] IceWallLeft = new string[] {"RoadIceWithWallMultilapLeft","RoadIceWithWallCheckpointLeft"};
    static string[] DiagIceWallsRightRight = new string[] {"RoadIceWithWallDiagRightMultilapRight","RoadIceWithWallDiagRightCheckpointRight"};
    static string[] DiagIceWallsLeftRight = new string[] {"RoadIceWithWallDiagLeftMultilapRight","RoadIceWithWallDiagLeftCheckpointRight"};
    static string[] DiagIceWallsRightLeft = new string[] {"RoadIceWithWallDiagRightMultilapLeft","RoadIceWithWallDiagRightCheckpointLeft"};
    static string[] DiagIceWallsLeftLeft = new string[] {"RoadIceWithWallDiagLeftMultilapLeft","RoadIceWithWallDiagLeftCheckpointLeft"};
    static string[] CPRoadBlock = new string[] {"RoadTechCheckpoint","RoadTechCheckpointSlopeUp","RoadTechCheckpointSlopeDown","RoadDirtCheckpoint","RoadDirtCheckpointSlopeUp","RoadDirtCheckpointSlopeDown","RoadBumpCheckpoint","RoadBumpCheckpointSlopeUp","RoadBumpCheckpointSlopeDown","RoadIceCheckpoint","RoadIceCheckpointSlopeUp","RoadIceCheckpointSlopeDown","RoadWaterCheckpoint"};
    static string[] CPPlatformBlock = new string[] {"PlatformTechCheckpoint","PlatformPlasticCheckpoint","PlatformDirtCheckpoint","PlatformIceCheckpoint","PlatformGrassCheckpoint","PlatformWaterCheckpoint","OpenTechRoadCheckpoint","OpenDirtRoadCheckpoint","OpenIceRoadCheckpoint","OpenGrassRoadCheckpoint"};
    static string[] CPPlatformTilt = new string[] {"PlatformTechCheckpointSlope2Right","PlatformPlasticCheckpointSlope2Right","PlatformDirtCheckpointSlope2Right","PlatformIceCheckpointSlope2Right","PlatformGrassCheckpointSlope2Right","OpenTechRoadCheckpointSlope2Right","OpenDirtRoadCheckpointSlope2Right","OpenIceRoadCheckpointSlope2Right","OpenGrassRoadCheckpointSlope2Right","PlatformTechCheckpointSlope2Left","PlatformPlasticCheckpointSlope2Left","PlatformDirtCheckpointSlope2Left","PlatformIceCheckpointSlope2Left","PlatformGrassCheckpointSlope2Left","OpenTechRoadCheckpointSlope2Left","OpenDirtRoadCheckpointSlope2Left","OpenIceRoadCheckpointSlope2Left","OpenGrassRoadCheckpointSlope2Left","PlatformTechCheckpointSlope2Up","PlatformPlasticCheckpointSlope2Up","PlatformDirtCheckpointSlope2Up","PlatformIceCheckpointSlope2Up","PlatformGrassCheckpointSlope2Up","OpenTechRoadCheckpointSlope2Up","OpenDirtRoadCheckpointSlope2Up","OpenIceRoadCheckpointSlope2Up","OpenGrassRoadCheckpointSlope2Up","PlatformTechCheckpointSlope2Down","PlatformPlasticCheckpointSlope2Down","PlatformDirtCheckpointSlope2Down","PlatformIceCheckpointSlope2Down","PlatformGrassCheckpointSlope2Down","OpenTechRoadCheckpointSlope2Down","OpenDirtRoadCheckpointSlope2Down","OpenIceRoadCheckpointSlope2Down","OpenGrassRoadCheckpointSlope2Down"};
    static string[] CPRoadBlockTilt = new string[] {"RoadTechCheckpointTiltLeft","RoadTechCheckpointTiltRight","RoadDirtCheckpointTiltLeft","RoadDirtCheckpointTiltRight","RoadBumpCheckpointTiltLeft","RoadBumpCheckpointTiltRight"};
    static string[] GateStart32m = new string[] {"GateStartLeft32m","GateStartCenter32m","GateStartRight32m"};
    static string[] GateStart16m = new string[] {"GateStartLeft16m","GateStartCenter16m","GateStartRight16m"};
    static string[] GateStart8m = new string[] {"GateStartLeft8m","GateStartCenter8m","GateStartRight8m"};
    static string[] GateCP32m = new string[] {"GateCheckpointLeft32m","GateCheckpointCenter32mv2","GateCheckpointRight32m","GateMultilapLeft32m","GateMultilapCenter32m","GateMultilapRight32m"};
    static string[] GateCP16m = new string[] {"GateCheckpointLeft16m","GateCheckpointCenter16mv2","GateCheckpointRight16m","GateMultilapLeft16m","GateMultilapCenter16m","GateMultilapRight16m"};
    static string[] GateCP8m = new string[] {"GateCheckpointLeft8m","GateCheckpointCenter8mv2","GateCheckpointRight8m","GateMultilapLeft8m","GateMultilapCenter8m","GateMultilapRight8m"};
    static string[] DiagRight = new string[]{"RoadTechDiagRightMultilap","RoadDirtDiagRightMultilap","RoadBumpDiagRightMultilap","RoadIceDiagRightMultilap","RoadTechDiagRightCheckpoint","RoadDirtDiagRightCheckpoint","RoadBumpDiagRightCheckpoint"};
    static string[] DiagLeft = new string[]{"RoadTechDiagLeftMultilap","RoadDirtDiagLeftMultilap","RoadBumpDiagLeftMultilap","RoadIceDiagLeftMultilap","RoadTechDiagLeftCheckpoint","RoadDirtDiagLeftCheckpoint","RoadBumpDiagLeftCheckpoint"};

    public static void placeCPEffect(Map map, string Effect,int forwardOffset,Vec3 rotation){
        Inventory CPMultiLap = Blocks.select("Checkpoint");
        CPMultiLap.add(Blocks.GetArticles("Multilap"));

        string GateSpecial = "GateSpecial" + Effect;
        map.placeRelative(MultilapBlock, GateSpecial,new BlockChange(new Vec3(0,-16,forwardOffset),rotation));
        map.placeRelative(CPRoadBlock, GateSpecial,new BlockChange(new Vec3(0,-16,forwardOffset),rotation));
        map.placeRelative(CPPlatformBlock, GateSpecial,new BlockChange(new Vec3(0,-16,forwardOffset),rotation));
        map.placeRelative(CPPlatformTilt, GateSpecial,new BlockChange(new Vec3(0,-8,forwardOffset),rotation));
        map.placeRelative(CPRoadBlockTilt, GateSpecial,new BlockChange(new Vec3(0,-12,forwardOffset),rotation));
        map.placeRelative(DiagRight, GateSpecial,new EffectDiagBlockChange(new Vec3(0,-16,forwardOffset),rotation,LeftRight.Right));
        map.placeRelative(DiagLeft, GateSpecial,new EffectDiagBlockChange(new Vec3(0,-16,forwardOffset),rotation,LeftRight.Left));
        map.placeRelative("GateCheckpoint", GateSpecial,new BlockChange(new Vec3(0,0,forwardOffset),rotation));

        string GateSpecial32m = "GateSpecial32m" + Effect;
        map.placeRelative(GateCP32m,GateSpecial32m,new BlockChange(new Vec3(0,0,forwardOffset),rotation));
        map.placeRelative("GateCheckpointCenter24m",GateSpecial32m,new BlockChange(new Vec3(0,0,forwardOffset),rotation));
        map.placeRelative(GateCP16m,"GateSpecial16m" + Effect,new BlockChange(new Vec3(0,0,forwardOffset),rotation));
        map.placeRelative(GateCP8m,"GateSpecial8m" + Effect,new BlockChange(new Vec3(0,0,forwardOffset),rotation));

        map.placeRelative(IceWallRight,GateSpecial32m,new BlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation));
        map.placeRelative(IceWallRight,GateSpecial32m,new BlockChange(new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation));
        map.placeRelative(IceWallLeft,GateSpecial32m,new BlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation));
        map.placeRelative(IceWallLeft,GateSpecial32m,new BlockChange(new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation));

        map.placeRelative(DiagIceWallsRightRight,GateSpecial32m,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Right));
        map.placeRelative(DiagIceWallsRightRight,GateSpecial32m,new EffectDiagBlockChange(new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation,LeftRight.Right));
        map.placeRelative(DiagIceWallsRightLeft,GateSpecial32m,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Right));
        map.placeRelative(DiagIceWallsRightLeft,GateSpecial32m,new EffectDiagBlockChange(new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation,LeftRight.Right));

        map.placeRelative(DiagIceWallsLeftRight,GateSpecial32m,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Left));
        map.placeRelative(DiagIceWallsLeftRight,GateSpecial32m,new EffectDiagBlockChange(new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation,LeftRight.Left));
        map.placeRelative(DiagIceWallsLeftLeft,GateSpecial32m,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Left));
        map.placeRelative(DiagIceWallsLeftLeft,GateSpecial32m,new EffectDiagBlockChange(new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation,LeftRight.Left));

        map.placeStagedBlocks();
    }
    public static void placeStartEffect(Map map, string Effect,int forwardOffset,Vec3 rotation){
        map.placeRelative(StartBlock, "GateSpecial" + Effect,new BlockChange(new Vec3(0,-16,forwardOffset),rotation));
        map.placeRelative("RoadWaterStart", "GateSpecial" + Effect,new BlockChange(new Vec3(0,-16,forwardOffset-4),rotation));
        map.placeRelative(GateStart32m,"GateSpecial32m" + Effect,new BlockChange(new Vec3(0,0,forwardOffset-10),rotation));
        map.placeRelative(GateStart16m,"GateSpecial16m" + Effect,new BlockChange(new Vec3(0,0,forwardOffset-10),rotation));
        map.placeRelative(GateStart8m,"GateSpecial8m" + Effect,new BlockChange(new Vec3(0,0,forwardOffset-10),rotation));
        map.placeStagedBlocks();
    }
}
class NoBrake: EffectAlteration {
    public override void run(Map map){
        placeCPEffect(map,"NoBrake",1,Vec3.Zero);
        placeStartEffect(map,"NoBrake",1,Vec3.Zero);
    }
}
class Cruise: EffectAlteration {
    public override void run(Map map){
        placeCPEffect(map,"Cruise",1,Vec3.Zero);
    }
}

class Fragile: EffectAlteration {
    public override void run(Map map){
        placeCPEffect(map,"Fragile",1,Vec3.Zero);
        placeStartEffect(map,"Fragile",1,Vec3.Zero);
    }
}

class SlowMo: EffectAlteration {
    public override void run(Map map){
        placeCPEffect(map,"SlowMotion",1,Vec3.Zero);
        placeStartEffect(map,"SlowMotion",1,Vec3.Zero);
    }
}

class NoSteer: EffectAlteration {
    public override void run(Map map){
        placeCPEffect(map,"NoSteering",1,Vec3.Zero);
        placeStartEffect(map,"NoSteering",1,Vec3.Zero);
    }
}

class Glider: EffectAlteration {
    public override void run(Map map){
        placeCPEffect(map,"Boost",1,Vec3.Zero);//TODO Boost (Red/Yellow)
        placeStartEffect(map,"Boost",1,Vec3.Zero);
    }
}

class Reactor: EffectAlteration {
    public override void run(Map map){
        placeCPEffect(map,"Boost",1,Vec3.Zero);
        placeStartEffect(map,"Boost",1,Vec3.Zero);
    }
}

class ReactorDown: EffectAlteration {
    public override void run(Map map){
        placeCPEffect(map,"Boost",1,new Vec3(PI,0,0));
        placeStartEffect(map,"Boost",1,new Vec3(PI,0,0));
    }
}

class FreeWheel: EffectAlteration {
    public override void run(Map map){
        placeCPEffect(map,"NoEngine",1,Vec3.Zero);
        placeStartEffect(map,"NoEngine",3,Vec3.Zero);
        placeCPEffect(map,"Turbo",0,Vec3.Zero);
        placeStartEffect(map,"Turbo",0,Vec3.Zero);
    }
}

enum LeftRight
{
    Left,
    Right
}

class EffectDiagBlockChange : BlockChange{

    static float PI = (float)Math.PI;
    LeftRight side;
    public EffectDiagBlockChange(LeftRight side) : base(){this.side = side;}
    public EffectDiagBlockChange(Vec3 absolutePosition,LeftRight side) : base(absolutePosition){this.side = side;}
    public EffectDiagBlockChange(Vec3 absolutePosition, Vec3 pitchYawRoll,LeftRight side) : base(absolutePosition,pitchYawRoll){this.side = side;}

    public override void changeBlock(CGameCtnBlock ctnBlock,Block @block){
        switch (ctnBlock.Direction){
            case Direction.North:
                block.relativeOffset(new Vec3(64,0,0));
                break;
            case Direction.East:
                block.relativeOffset(new Vec3(64,0,-32));
                break;
            case Direction.South:
                block.relativeOffset(new Vec3(0,0,-32));
                break;
            case Direction.West:
                block.relativeOffset(new Vec3(0,0,0));
                break;
        }

        switch (side){
            case LeftRight.Right:
                block.relativeOffset(new Vec3(-23.1f,0,10.6f));
                block.pitchYawRoll += new Vec3(PI * -0.148f,0f,0);
                break;
            case LeftRight.Left:
                block.relativeOffset(new Vec3(-37.3f,0,24.8f));
                block.pitchYawRoll += new Vec3(PI * 0.148f,0,0);
                break;
        }

        block.relativeOffset(absolutePosition);
        block.pitchYawRoll += pitchYawRoll;
    }
}