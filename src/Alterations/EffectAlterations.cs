using GBX.NET;
using GBX.NET.Engines.Game;
class EffectAlteration: Alteration {
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
        string GateSpecial = "GateSpecial" + Effect;
        map.placeRelative(MultilapBlock, GateSpecial,BlockType.Block ,new BlockChange(new Vec3(0,-16,forwardOffset),rotation));
        map.placeRelative(CPRoadBlock, GateSpecial,BlockType.Block,new BlockChange(new Vec3(0,-16,forwardOffset),rotation));
        map.placeRelative(CPPlatformBlock, GateSpecial,BlockType.Block,new BlockChange(new Vec3(0,-16,forwardOffset),rotation));
        map.placeRelative(CPPlatformTilt, GateSpecial,BlockType.Block,new BlockChange(new Vec3(0,-8,forwardOffset),rotation));
        map.placeRelative(CPRoadBlockTilt, GateSpecial,BlockType.Block,new BlockChange(new Vec3(0,-12,forwardOffset),rotation));
        map.placeRelative(DiagRight, GateSpecial,BlockType.Block,new EffectDiagBlockChange(new Vec3(0,-16,forwardOffset),rotation,LeftRight.Right));
        map.placeRelative(DiagLeft, GateSpecial,BlockType.Block,new EffectDiagBlockChange(new Vec3(0,-16,forwardOffset),rotation,LeftRight.Left));
        map.placeRelative("GateCheckpoint", GateSpecial,BlockType.Block,new BlockChange(new Vec3(0,0,forwardOffset),rotation));

        string GateSpecial32m = "GateSpecial32m" + Effect;
        map.placeRelative(GateCP32m,GateSpecial32m,BlockType.Item,new BlockChange(new Vec3(0,0,forwardOffset),rotation));
        map.placeRelative("GateCheckpointCenter24m",GateSpecial32m,BlockType.Item,new BlockChange(new Vec3(0,0,forwardOffset),rotation));
        map.placeRelative(GateCP16m,"GateSpecial16m" + Effect,BlockType.Item,new BlockChange(new Vec3(0,0,forwardOffset),rotation));
        map.placeRelative(GateCP8m,"GateSpecial8m" + Effect,BlockType.Item,new BlockChange(new Vec3(0,0,forwardOffset),rotation));

        map.placeRelative(IceWallRight,GateSpecial32m,BlockType.Item,new BlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation));
        map.placeRelative(IceWallRight,GateSpecial32m,BlockType.Item,new BlockChange(new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation));
        map.placeRelative(IceWallLeft,GateSpecial32m,BlockType.Item,new BlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation));
        map.placeRelative(IceWallLeft,GateSpecial32m,BlockType.Item,new BlockChange(new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation));

        map.placeRelative(DiagIceWallsRightRight,GateSpecial32m,BlockType.Item,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Right));
        map.placeRelative(DiagIceWallsRightRight,GateSpecial32m,BlockType.Item,new EffectDiagBlockChange(new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation,LeftRight.Right));
        map.placeRelative(DiagIceWallsRightLeft,GateSpecial32m,BlockType.Item,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Right));
        map.placeRelative(DiagIceWallsRightLeft,GateSpecial32m,BlockType.Item,new EffectDiagBlockChange(new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation,LeftRight.Right));

        map.placeRelative(DiagIceWallsLeftRight,GateSpecial32m,BlockType.Item,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Left));
        map.placeRelative(DiagIceWallsLeftRight,GateSpecial32m,BlockType.Item,new EffectDiagBlockChange(new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation,LeftRight.Left));
        map.placeRelative(DiagIceWallsLeftLeft,GateSpecial32m,BlockType.Item,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Left));
        map.placeRelative(DiagIceWallsLeftLeft,GateSpecial32m,BlockType.Item,new EffectDiagBlockChange(new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation,LeftRight.Left));

        map.placeStagedBlocks();
    }
    public static void newplaceCPEffect(Map map, string Effect,int forwardOffset,Vec3 rotation){
        Inventory CPMultiLap = inventory.select("Checkpoint|Multilap&!Trigger");
        // BlockChange zero = new BlockChange(new Vec3(0,0,forwardOffset),rotation);

        Article GateSpecial = inventory.GetArticle("GateSpecial" + Effect);
        map.placeRelative(inventory.select("Checkpoint|Multilap&Trigger"), inventory.GetArticle("ObstaclePillar2m"));
        map.placeRelative(inventory.select("Checkpoint|Multilap&Trigger"), GateSpecial,new BlockChange(new Vec3(-16,-18,-16 + forwardOffset),rotation));
        // map.placeRelative(CPMultiLap.select("!Wall&!Slope2&!Slope&!Tilt&!DiagRight&!DiagLeft&!(RoadIce)&!Gate"), GateSpecial,new BlockChange(new Vec3(0,-16,forwardOffset),rotation));
        // map.placeRelative(CPMultiLap.select("Platform&Slope2"), GateSpecial,new BlockChange(new Vec3(0,-8,forwardOffset),rotation));
        // map.placeRelative(CPMultiLap.select("Platform&Wall"), GateSpecial,new BlockChange(new Vec3(0,0,forwardOffset),new Vec3(0,PI/2,0) + rotation));
        // map.placeRelative(CPMultiLap.selectString("Road").select("Tilt"), GateSpecial,new BlockChange(new Vec3(0,-10,forwardOffset),rotation));
        // map.placeRelative(CPMultiLap.selectString("Road").select("Slope"), GateSpecial,new BlockChange(new Vec3(0,-12,forwardOffset),rotation));
        // map.placeRelative(CPMultiLap.select("DiagRight&!RoadIce"), GateSpecial,new EffectDiagBlockChange(new Vec3(0,-16,forwardOffset),rotation,LeftRight.Right));
        // map.placeRelative(CPMultiLap.select("DiagLeft&!RoadIce"), GateSpecial,new EffectDiagBlockChange(new Vec3(0,-16,forwardOffset),rotation,LeftRight.Left));
        // map.placeRelative(inventory.GetArticle("GateCheckpoint"), GateSpecial, zero);

        // inventory.select("Gate&(Checkpoint|Multilap)").add(Effect).remove(new[] {"Multilap","Checkpoint","Left","Right","Center","v2"}).placeRelative(map, zero);

        // map.placeRelative(CPMultiLap.select("RoadIce&!WithWall&!DiagRight&!DiagLeft"), GateSpecial,new BlockChange(new Vec3(0,-8,forwardOffset),rotation));
        // map.placeRelative(CPMultiLap.select("RoadIce&DiagRight&!WithWall"), GateSpecial,new EffectDiagBlockChange(new Vec3(0,-8,forwardOffset),rotation,LeftRight.Right));
        // map.placeRelative(CPMultiLap.select("RoadIce&DiagLeft&!WithWall"), GateSpecial,new EffectDiagBlockChange(new Vec3(0,-8,forwardOffset),rotation,LeftRight.Left));

        // Inventory BobsleighWall = CPMultiLap.select("RoadIce&WithWall");
        // Article GateSpecial32m = inventory.GetArticle("GateSpecial32m" + Effect);
        // map.placeRelative(BobsleighWall.select("Right&!(DiagRight|DiagLeft)"),GateSpecial32m,new BlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation));
        // map.placeRelative(BobsleighWall.select("Right&!(DiagRight|DiagLeft)"),GateSpecial32m,new BlockChange(new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation));
        // map.placeRelative(BobsleighWall.select("Left&!(DiagRight|DiagLeft)"),GateSpecial32m,new BlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation));
        // map.placeRelative(BobsleighWall.select("Left&!(DiagRight|DiagLeft)"),GateSpecial32m,new BlockChange(new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation));

        // map.placeRelative(BobsleighWall.select("DiagRight&Right"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Right));
        // map.placeRelative(BobsleighWall.select("DiagRight&Right"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation,LeftRight.Right));
        // map.placeRelative(BobsleighWall.select("DiagRight&Left"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Right));
        // map.placeRelative(BobsleighWall.select("DiagRight&Left"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation,LeftRight.Right));

        // map.placeRelative(BobsleighWall.select("DiagLeft&Right"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Left));
        // map.placeRelative(BobsleighWall.select("DiagLeft&Right"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation,LeftRight.Left));
        // map.placeRelative(BobsleighWall.select("DiagLeft&Left"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Left));
        // map.placeRelative(BobsleighWall.select("DiagLeft&Left"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation,LeftRight.Left));

        map.placeStagedBlocks();
    }
    public static void placeStartEffect(Map map, string Effect,int forwardOffset,Vec3 rotation){
        Inventory start = inventory.select("MapStart");
        Article GateSpecial = inventory.GetArticle("GateSpecial" + Effect);
        map.placeRelative(start.select("!Water&!(RoadIce)"), GateSpecial,new BlockChange(new Vec3(0,-16,forwardOffset),rotation));
        map.placeRelative(start.select("RoadIce"), GateSpecial,new BlockChange(new Vec3(0,-8,forwardOffset),rotation));
        map.placeRelative(inventory.GetArticle("RoadWaterStart"), GateSpecial,new BlockChange(new Vec3(0,-16,forwardOffset-2),rotation));
        inventory.select("MapStart&Gate").add(Effect).remove(new string[] {"MapStart", "Left", "Right", "Center", "v2" }).placeRelative(map,new BlockChange(new Vec3(0,0,forwardOffset-10),rotation));
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
        placeCPEffect(map,"BoostOriented",1,Vec3.Zero);
        placeStartEffect(map,"BoostOriented",1,Vec3.Zero);
    }
}

class Reactor: EffectAlteration {
    public override void run(Map map){
        placeCPEffect(map,"Boost2Oriented",1,Vec3.Zero);
        placeStartEffect(map,"Boost2Oriented",1,Vec3.Zero);
    }
}

class ReactorDown: EffectAlteration {
    public override void run(Map map){
        placeCPEffect(map,"Boost2",1,new Vec3(PI,0,0));
        placeStartEffect(map,"Boost2",1,new Vec3(PI,0,0));
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