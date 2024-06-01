using GBX.NET;
using GBX.NET.Engines.Game;
class EffectAlteration: Alteration {
    public static void placeCPEffect(Map map, string Effect,int forwardOffset,Vec3 rotation){
        Inventory CPMultiLap = inventory.select("Checkpoint|Multilap");
        BlockChange zero = new BlockChange(new Vec3(0,0,forwardOffset),rotation);

        Article GateSpecial = inventory.GetArticle("GateSpecial" + Effect);
        map.placeRelative(CPMultiLap.select("!Tilt&!DiagRight&!DiagLeft&!(Road&Ice)&!Gate"), GateSpecial,new BlockChange(new Vec3(0,-16,forwardOffset),rotation));
        map.placeRelative(CPMultiLap.select("Platform&Tilt"), GateSpecial,new BlockChange(new Vec3(0,-8,forwardOffset),rotation));
        map.placeRelative(CPMultiLap.select("Road&Tilt"), GateSpecial,new BlockChange(new Vec3(0,-12,forwardOffset),rotation));
        map.placeRelative(CPMultiLap.select("DiagRight&!Ice"), GateSpecial,new EffectDiagBlockChange(new Vec3(0,-16,forwardOffset),rotation,LeftRight.Right));
        map.placeRelative(CPMultiLap.select("DiagLeft&!Ice"), GateSpecial,new EffectDiagBlockChange(new Vec3(0,-16,forwardOffset),rotation,LeftRight.Left));
        map.placeRelative("GateCheckpoint", GateSpecial, zero);

        inventory.select("Gate&(Checkpoint|Multilap)").add(Effect).remove(new[] {"Multilap","Checkpoint","Left","Right","Center","v2"}).placeRelative(map, zero);

        map.placeRelative(CPMultiLap.select("Road&Ice&!WithWall&!DiagRight&!DiagLeft"), GateSpecial,new BlockChange(new Vec3(0,-8,forwardOffset),rotation));
        map.placeRelative(CPMultiLap.select("Road&DiagRight&Ice&!WithWall"), GateSpecial,new EffectDiagBlockChange(new Vec3(0,-8,forwardOffset),rotation,LeftRight.Right));
        map.placeRelative(CPMultiLap.select("Road&DiagLeft&Ice&!WithWall"), GateSpecial,new EffectDiagBlockChange(new Vec3(0,-8,forwardOffset),rotation,LeftRight.Left));

        Inventory BobsleighWall = CPMultiLap.select("Road&Ice&WithWall");
        Article GateSpecial32m = inventory.GetArticle("GateSpecial32m" + Effect);
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
        Inventory start = inventory.select("MapStart");
        Article GateSpecial = inventory.GetArticle("GateSpecial" + Effect);
        map.placeRelative(start.select("!Water&!(Road&Ice)"), GateSpecial,new BlockChange(new Vec3(0,-16,forwardOffset),rotation));
        map.placeRelative(start.select("Road&Ice"), GateSpecial,new BlockChange(new Vec3(0,-8,forwardOffset),rotation));
        map.placeRelative("RoadWaterStart", GateSpecial,new BlockChange(new Vec3(0,-16,forwardOffset-2),rotation));
        inventory.select("Start&!Slope").add(Effect).remove(new string[] {"Start", "Left", "Right", "Center", "v2" }).placeRelative(map,new BlockChange(new Vec3(0,0,forwardOffset-10),rotation));
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