using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
class EffectAlterations {
    static float PI = (float)Math.PI;
    static string[] StartBlock = new string[] {"PlatformTechStart","RoadTechStart","RoadDirtStart","RoadBumpStart","RoadIceStart","RoadWaterStart","PlatformTechStart","PlatformDirtStart","PlatformIceStart","PlatformGrassStart","PlatformPlasticStart","PlatformWaterStart"};
    static string[] MultilapBlock = new string[] {"PlatformTechMultilap","RoadTechMultilap","RoadDirtMultilap","RoadBumpMultilap","RoadIceMultilap","RoadWaterMultilap","PlatformTechMultilap","PlatformDirtMultilap","PlatformIceMultilap","PlatformGrassMultilap","PlatformPlasticMultilap","PlatformWaterMultilap"};
    static string[] IceWallRight = new string[] {"RoadIceWithWallMultilapRight","RoadIceWithWallCheckpointRight"};
    static string[] IceWallLeft = new string[] {"RoadIceWithWallMultilapLeft","RoadIceWithWallCheckpointLeft"};
    static string[] DiagIceWallsRightRight = new string[] {"RoadIceWithWallDiagRightMultilapRight","RoadIceWithWallDiagRightCheckpointRight"};
    static string[] DiagIceWallsLeftRight = new string[] {"RoadIceWithWallDiagLeftMultilapRight","RoadIceWithWallDiagLeftCheckpointRight"};
    static string[] DiagIceWallsRightLeft = new string[] {"RoadIceWithWallDiagRightMultilapLeft","RoadIceWithWallDiagRightCheckpointLeft"};
    static string[] DiagIceWallsLeftLeft = new string[] {"RoadIceWithWallDiagLeftMultilapLeft","RoadIceWithWallDiagLeftCheckpointLeft"};
    static string[] CPRoadBlock = new string[] {"RoadTechCheckpoint","RoadTechCheckpointSlopeUp","RoadTechCheckpointSlopeDown","RoadDirtCheckpoint","RoadDirtCheckpointSlopeUp","RoadDirtCheckpointSlopeDown","RoadBumpCheckpoint","RoadBumpCheckpointSlopeUp","RoadBumpCheckpointSlopeDown","RoadIceCheckpoint","RoadIceCheckpointSlopeUp","RoadIceCheckpointSlopeDown","RoadWaterCheckpoint","GateCheckpoint"};
    static string[] CPPlatformBlock = new string[] {"PlatformTechCheckpoint","PlatformTechCheckpointSlope2Up","PlatformTechCheckpointSlope2Down","PlatformTechCheckpointSlope2Right","PlatformTechCheckpointSlope2Left","PlatformPlasticCheckpoint","PlatformPlasticCheckpointSlope2Up","PlatformPlasticCheckpointSlope2Down","PlatformPlasticCheckpointSlope2Right","PlatformPlasticCheckpointSlope2Left","PlatformDirtCheckpoint","PlatformDirtCheckpointSlope2Up","PlatformDirtCheckpointSlope2Down","PlatformDirtCheckpointSlope2Right","PlatformDirtCheckpointSlope2Left","PlatformIceCheckpoint","PlatformIceCheckpointSlope2Up","PlatformIceCheckpointSlope2Down","PlatformIceCheckpointSlope2Right","PlatformIceCheckpointSlope2Left","PlatformGrassCheckpoint","PlatformGrassCheckpointSlope2Up","PlatformGrassCheckpointSlope2Down","PlatformGrassCheckpointSlope2Right","PlatformGrassCheckpointSlope2Left","PlatformWaterCheckpoint"};
    static string[] CPPlatformBlockTilt = new string[] {"RoadTechCheckpointTiltLeft","RoadTechCheckpointTiltRight","RoadDirtCheckpointTiltLeft","RoadDirtCheckpointTiltRight","RoadBumpCheckpointTiltLeft","RoadBumpCheckpointTiltRight"};
    static string[] GateCPStart32m = new string[] {"GateCheckpointLeft32m","GateCheckpointCenter32mv2","GateCheckpointRight32m","GateStartLeft32m","GateStartCenter32m","GateStartRight32m","GateMultilapLeft32m","GateMultilapCenter32m","GateMultilapRight32m"};
    static string[] GateCPStart16m = new string[] {"GateCheckpointLeft16m","GateCheckpointCenter16mv2","GateCheckpointRight16m","GateStartLeft16m","GateStartCenter16m","GateStartRight16m","GateMultilapLeft16m","GateMultilapCenter16m","GateMultilapRight16m"};
    static string[] GateCPStart8m = new string[] {"GateCheckpointLeft8m","GateCheckpointCenter8mv2","GateCheckpointRight8m","GateStartLeft8m","GateStartCenter8m","GateStartRight8m","GateMultilapLeft8m","GateMultilapCenter8m","GateMultilapRight8m"};
    static string[] DiagRight = new string[]{"RoadTechDiagRightMultilap","RoadDirtDiagRightMultilap","RoadBumpDiagRightMultilap","RoadIceDiagRightMultilap","RoadTechDiagRightCheckpoint","RoadDirtDiagRightCheckpoint","RoadBumpDiagRightCheckpoint"};
    static string[] DiagLeft = new string[]{"RoadTechDiagLeftMultilap","RoadDirtDiagLeftMultilap","RoadBumpDiagLeftMultilap","RoadIceDiagLeftMultilap","RoadTechDiagLeftCheckpoint","RoadDirtDiagLeftCheckpoint","RoadBumpDiagLeftCheckpoint"};
    private static void placeEffect(Map map, string Effect,int forwardOffset,Vec3 rotation){
        BlockChange GateSpecial = new BlockChange(BlockType.Block,"GateSpecial" + Effect,new Vec3(0,-16,forwardOffset),rotation);
        map.placeRelative(StartBlock, GateSpecial);
        map.placeRelative(MultilapBlock, GateSpecial);
        map.placeRelative(CPRoadBlock, GateSpecial);
        map.placeRelative(CPPlatformBlock, GateSpecial);
        map.placeRelative(CPPlatformBlockTilt, new BlockChange(BlockType.Block,"GateSpecial" + Effect,new Vec3(0,-12,forwardOffset),rotation));
        map.placeRelative(DiagRight,new DiagBlockChange(BlockType.Block,"GateSpecial" + Effect,new Vec3(0,-16,forwardOffset),rotation,LeftRight.Right));
        map.placeRelative(DiagLeft,new DiagBlockChange(BlockType.Block,"GateSpecial" + Effect,new Vec3(0,-16,forwardOffset),rotation,LeftRight.Left));
        map.placeRelative(GateCPStart32m,new BlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(0,0,forwardOffset),rotation));
        map.placeRelative(GateCPStart16m,new BlockChange(BlockType.Item,"GateSpecial16m" + Effect,new Vec3(0,0,forwardOffset),rotation));
        map.placeRelative(GateCPStart8m,new BlockChange(BlockType.Item,"GateSpecial8m" + Effect,new Vec3(0,0,forwardOffset),rotation));

        map.placeRelative(IceWallRight,new BlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation));
        map.placeRelative(IceWallRight,new BlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation));
        map.placeRelative(IceWallLeft,new BlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation));
        map.placeRelative(IceWallLeft,new BlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation));

        map.placeRelative(DiagIceWallsRightRight,new DiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Right));
        map.placeRelative(DiagIceWallsRightRight,new DiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation,LeftRight.Right));
        map.placeRelative(DiagIceWallsRightLeft,new DiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Right));
        map.placeRelative(DiagIceWallsRightLeft,new DiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation,LeftRight.Right));

        map.placeRelative(DiagIceWallsLeftRight,new DiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Left));
        map.placeRelative(DiagIceWallsLeftRight,new DiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation,LeftRight.Left));
        map.placeRelative(DiagIceWallsLeftLeft,new DiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Left));
        map.placeRelative(DiagIceWallsLeftLeft,new DiagBlockChange(BlockType.Item,"GateSpecial32m" + Effect,new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation,LeftRight.Left));

        map.placeStagedBlocks();
    }

    public static void NoBrakes(Map map){
        placeEffect(map,"NoBrake",1,Vec3.Zero);
    }

    public static void Cruise(Map map){
        placeEffect(map,"Cruise",1,Vec3.Zero);
    }

    public static void Fragile(Map map){
        placeEffect(map,"Fragile",1,Vec3.Zero);
    }

    public static void SlowMo(Map map){
        placeEffect(map,"SlowMotion",1,Vec3.Zero);
    }

    public static void NoSteer(Map map){
        placeEffect(map,"NoSteering",1,Vec3.Zero);
    }

    public static void Glider(Map map){
        placeEffect(map,"Boost",1,Vec3.Zero);
    }
    public static void Reactor(Map map){
        placeEffect(map,"Boost",1,Vec3.Zero);
    }
    public static void ReactorDown(Map map){
        placeEffect(map,"Boost",1,new Vec3(PI,0,0));
    }

    public static void FreeWheel(Map map){
        placeEffect(map,"NoEngine",1,Vec3.Zero);
        placeEffect(map,"Turbo",0,Vec3.Zero);
    }
}

enum LeftRight
{
    Left,
    Right
}

class DiagBlockChange : BlockChange{

    static float PI = (float)Math.PI;
    LeftRight side;
    public DiagBlockChange(BlockType blockType, string model,LeftRight side) : base(blockType,model){this.side = side;}
    public DiagBlockChange(BlockType blockType, string model, Vec3 absolutePosition,LeftRight side) : base(blockType,model,absolutePosition){this.side = side;}
    public DiagBlockChange(BlockType blockType, string model, Vec3 absolutePosition, Vec3 pitchYawRoll,LeftRight side) : base(blockType,model,absolutePosition,pitchYawRoll){this.side = side;}
    public DiagBlockChange(Vec3 absolutePosition,LeftRight side) : base(absolutePosition){this.side = side;}
    public DiagBlockChange(Vec3 absolutePosition, Vec3 pitchYawRoll,LeftRight side) : base(absolutePosition,pitchYawRoll){this.side = side;}

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