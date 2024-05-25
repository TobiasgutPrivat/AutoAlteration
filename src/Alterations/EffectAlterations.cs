using GBX.NET;
using GBX.NET.Engines.Game;
class EffectAlteration: Alteration {
    public static float PI = (float)Math.PI;
    // static string[] GateStart32m = new string[] {"GateStartLeft32m","GateStartCenter32m","GateStartRight32m"};
    // static string[] GateStart16m = new string[] {"GateStartLeft16m","GateStartCenter16m","GateStartRight16m"};
    // static string[] GateStart8m = new string[] {"GateStartLeft8m","GateStartCenter8m","GateStartRight8m"};
    // static string[] GateCP32m = new string[] {"GateCheckpointLeft32m","GateCheckpointCenter32mv2","GateCheckpointRight32m","GateMultilapLeft32m","GateMultilapCenter32m","GateMultilapRight32m"};
    // static string[] GateCP16m = new string[] {"GateCheckpointLeft16m","GateCheckpointCenter16mv2","GateCheckpointRight16m","GateMultilapLeft16m","GateMultilapCenter16m","GateMultilapRight16m"};
    // static string[] GateCP8m = new string[] {"GateCheckpointLeft8m","GateCheckpointCenter8mv2","GateCheckpointRight8m","GateMultilapLeft8m","GateMultilapCenter8m","GateMultilapRight8m"};

    public static void placeCPEffect(Map map, string Effect,int forwardOffset,Vec3 rotation){
        Inventory CPMultiLap = Blocks.select("Checkpoint|Multilap");
        BlockChange zero = new BlockChange(new Vec3(0,0,forwardOffset),rotation);

        string GateSpecial = "GateSpecial" + Effect;
        map.placeRelative(CPMultiLap.select("!Tilt&!DiagRight&!DiagLeft&!(Road&Ice)&!Gate"), GateSpecial,new BlockChange(new Vec3(0,-16,forwardOffset),rotation));
        map.placeRelative(CPMultiLap.select("Platform&Tilt"), GateSpecial,new BlockChange(new Vec3(0,-8,forwardOffset),rotation));
        map.placeRelative(CPMultiLap.select("Road&Tilt"), GateSpecial,new BlockChange(new Vec3(0,-12,forwardOffset),rotation));
        map.placeRelative(CPMultiLap.select("DiagRight&!Ice"), GateSpecial,new EffectDiagBlockChange(new Vec3(0,-16,forwardOffset),rotation,LeftRight.Right));
        map.placeRelative(CPMultiLap.select("DiagLeft&!Ice"), GateSpecial,new EffectDiagBlockChange(new Vec3(0,-16,forwardOffset),rotation,LeftRight.Left));
        map.placeRelative("GateCheckpoint", GateSpecial, zero);

        map.placeOtherKeywords(Items,"Gate&(Checkpoint|Multilap)", new[]{Effect,"Special"}, new[] {"Multilap","Checkpoint","Left","Right","Center","v2"}, zero);

        map.placeRelative(CPMultiLap.select("Road&Ice&!WithWall&!DiagRight&!DiagLeft"), GateSpecial,new BlockChange(new Vec3(0,-8,forwardOffset),rotation));
        map.placeRelative(CPMultiLap.select("Road&DiagRight&Ice&!WithWall"), GateSpecial,new EffectDiagBlockChange(new Vec3(0,-8,forwardOffset),rotation,LeftRight.Right));
        map.placeRelative(CPMultiLap.select("Road&DiagLeft&Ice&!WithWall"), GateSpecial,new EffectDiagBlockChange(new Vec3(0,-8,forwardOffset),rotation,LeftRight.Left));

        Inventory BobsleighWall = CPMultiLap.select("Road&Ice&WithWall");
        string GateSpecial32m = "GateSpecial32m" + Effect;
        map.placeRelative(BobsleighWall.select("Right"),GateSpecial32m,new BlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation));
        map.placeRelative(BobsleighWall.select("Right"),GateSpecial32m,new BlockChange(new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation));
        map.placeRelative(BobsleighWall.select("Left"),GateSpecial32m,new BlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation));
        map.placeRelative(BobsleighWall.select("Left"),GateSpecial32m,new BlockChange(new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation));

        map.placeRelative(BobsleighWall.select("DiagRight&Right"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Right));
        map.placeRelative(BobsleighWall.select("DiagRight&Right"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation,LeftRight.Right));
        map.placeRelative(BobsleighWall.select("DiagRight&Left"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Right));
        map.placeRelative(BobsleighWall.select("DiagRight&Left"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation,LeftRight.Right));

        map.placeRelative(BobsleighWall.select("DiagLeft&Right"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Left));
        map.placeRelative(BobsleighWall.select("DiagLeft&Right"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(10,12,16+forwardOffset),new Vec3(0,0,PI/2) + rotation,LeftRight.Left));
        map.placeRelative(BobsleighWall.select("DiagLeft&Left"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(16,10,16+forwardOffset),new Vec3(PI,PI,0) + rotation,LeftRight.Left));
        map.placeRelative(BobsleighWall.select("DiagLeft&Left"),GateSpecial32m,new EffectDiagBlockChange(new Vec3(22,12,16+forwardOffset),new Vec3(0,0,-PI/2) + rotation,LeftRight.Left));

        map.placeStagedBlocks();
    }
    public static void placeStartEffect(Map map, string Effect,int forwardOffset,Vec3 rotation){
        Inventory start = Blocks.select("Start&!Slope2&!Deco");
        map.placeRelative(start.select("!Water&!(Road&Ice)"), "GateSpecial" + Effect,new BlockChange(new Vec3(0,-16,forwardOffset),rotation));
        map.placeRelative(start.select("Road&Ice"), "GateSpecial" + Effect,new BlockChange(new Vec3(0,-8,forwardOffset),rotation));
        map.placeRelative("RoadWaterStart", "GateSpecial" + Effect,new BlockChange(new Vec3(0,-16,forwardOffset-2),rotation));
        map.placeOtherKeywords(Items, "Start&!Slope", new[]{Effect,"Special"}, new[] {"Start","Left","Right","Center","v2"}, new BlockChange(new Vec3(0,0,forwardOffset-10),rotation));
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
        placeCPEffect(map,"Boost2",1,Vec3.Zero);
        placeStartEffect(map,"Boost2",1,Vec3.Zero);
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