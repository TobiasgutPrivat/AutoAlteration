using GBX.NET;
using GBX.NET.Engines.Game;
class EffectAlterations {
    static float PI = (float)Math.PI;
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

    private static void placeCPEffect(Map map, string Effect,int forwardOffset,Vec3 rotation){
        BlockChange GateSpecial = new BlockChange(BlockType.Block,"GateSpecial" + Effect,new Vec3(0,-16,forwardOffset),rotation);
        map.placeRelative(MultilapBlock, GateSpecial);
        map.placeRelative(CPRoadBlock, GateSpecial);
        map.placeRelative(CPPlatformBlock, GateSpecial);
        map.placeRelative(CPPlatformTilt, new BlockChange(BlockType.Block,"GateSpecial" + Effect,new Vec3(0,-8,forwardOffset),rotation));
        map.placeRelative(CPRoadBlockTilt, new BlockChange(BlockType.Block,"GateSpecial" + Effect,new Vec3(0,-12,forwardOffset),rotation));
        map.placeRelative(DiagRight,new EffectDiagBlockChange(BlockType.Block,"GateSpecial" + Effect,new Vec3(0,-16,forwardOffset),rotation,LeftRight.Right));
        map.placeRelative(DiagLeft,new EffectDiagBlockChange(BlockType.Block,"GateSpecial" + Effect,new Vec3(0,-16,forwardOffset),rotation,LeftRight.Left));
        map.placeRelative(GateCP32m,new BlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(0,0,forwardOffset),rotation));
        map.placeRelative("GateCheckpointCenter24m",new BlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(0,0,forwardOffset),rotation));
        map.placeRelative(GateCP16m,new BlockChange(BlockType.Item,"GateSpecial16m" + Effect,new Vec3(0,0,forwardOffset),rotation));
        map.placeRelative(GateCP8m,new BlockChange(BlockType.Item,"GateSpecial8m" + Effect,new Vec3(0,0,forwardOffset),rotation));
        map.placeRelative("GateCheckpoint",new BlockChange(BlockType.Block,"GateSpecial" + Effect,new Vec3(0,0,forwardOffset),rotation));

        map.placeRelative(IceWallRight,new BlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation));
        map.placeRelative(IceWallRight,new BlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation));
        map.placeRelative(IceWallLeft,new BlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation));
        map.placeRelative(IceWallLeft,new BlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation));

        map.placeRelative(DiagIceWallsRightRight,new EffectDiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Right));
        map.placeRelative(DiagIceWallsRightRight,new EffectDiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation,LeftRight.Right));
        map.placeRelative(DiagIceWallsRightLeft,new EffectDiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Right));
        map.placeRelative(DiagIceWallsRightLeft,new EffectDiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation,LeftRight.Right));

        map.placeRelative(DiagIceWallsLeftRight,new EffectDiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Left));
        map.placeRelative(DiagIceWallsLeftRight,new EffectDiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation,LeftRight.Left));
        map.placeRelative(DiagIceWallsLeftLeft,new EffectDiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Left));
        map.placeRelative(DiagIceWallsLeftLeft,new EffectDiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation,LeftRight.Left));

        //todo

        map.placeStagedBlocks();
    }
    private static void placeStartEffect(Map map, string Effect,int forwardOffset,Vec3 rotation){
        map.placeRelative(StartBlock, new BlockChange(BlockType.Block,"GateSpecial" + Effect,new Vec3(0,-16,forwardOffset),rotation));
        map.placeRelative("RoadWaterStart", new BlockChange(BlockType.Block,"GateSpecial" + Effect,new Vec3(0,-16,forwardOffset-4),rotation));
        map.placeRelative(GateStart32m,new BlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(0,0,forwardOffset-10),rotation));
        map.placeRelative(GateStart16m,new BlockChange(BlockType.Item,"GateSpecial16m" + Effect,new Vec3(0,0,forwardOffset-10),rotation));
        map.placeRelative(GateStart8m,new BlockChange(BlockType.Item,"GateSpecial8m" + Effect,new Vec3(0,0,forwardOffset-10),rotation));
        map.placeStagedBlocks();
    }

    public static void NoBrake(Map map){
        placeCPEffect(map,"NoBrake",1,Vec3.Zero);
        placeStartEffect(map,"NoBrake",1,Vec3.Zero);
    }

    public static void Cruise(Map map){
        placeCPEffect(map,"Cruise",1,Vec3.Zero);
    }

    public static void Fragile(Map map){
        placeCPEffect(map,"Fragile",1,Vec3.Zero);
        placeStartEffect(map,"Fragile",1,Vec3.Zero);
    }

    public static void SlowMo(Map map){
        placeCPEffect(map,"SlowMotion",1,Vec3.Zero);
        placeStartEffect(map,"SlowMotion",1,Vec3.Zero);
    }

    public static void NoSteer(Map map){
        placeCPEffect(map,"NoSteering",1,Vec3.Zero);
        placeStartEffect(map,"NoSteering",1,Vec3.Zero);
    }

    public static void Glider(Map map){
        placeCPEffect(map,"Boost",1,Vec3.Zero);
        placeStartEffect(map,"Boost",1,Vec3.Zero);
    }
    public static void Reactor(Map map){
        placeCPEffect(map,"Boost",1,Vec3.Zero);
        placeStartEffect(map,"Boost",1,Vec3.Zero);
    }
    public static void ReactorDown(Map map){
        placeCPEffect(map,"Boost",1,new Vec3(PI,0,0));
        placeStartEffect(map,"Boost",1,new Vec3(PI,0,0));
    }

    public static void FreeWheel(Map map){
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
    public EffectDiagBlockChange(BlockType blockType, string model,LeftRight side) : base(blockType,model){this.side = side;}
    public EffectDiagBlockChange(BlockType blockType, string model, Vec3 absolutePosition,LeftRight side) : base(blockType,model,absolutePosition){this.side = side;}
    public EffectDiagBlockChange(BlockType blockType, string model, Vec3 absolutePosition, Vec3 pitchYawRoll,LeftRight side) : base(blockType,model,absolutePosition,pitchYawRoll){this.side = side;}
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
        
        if (model != "") {
            block.blockType = blockType;
            block.model = model;
        }
        block.relativeOffset(absolutePosition);
        block.pitchYawRoll += pitchYawRoll;
    }
}