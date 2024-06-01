using GBX.NET;
using GBX.NET.Engines.Game;

class CPBoost : Alteration{
    public override void run(Map map){
        // map.replaceKeyword("Checkpoint","Turbo");
        // map.replaceKeyword(inventory,"Checkpoint",new[] {"Turbo","Special"},new[] {"Checkpoint"});
        // map.replaceKeyword("SpecialTurbo","Checkpoint");
        // map.replaceKeyword(inventory,"Turbo",new[] {"Checkpoint"},new[] {"Turbo","Special"});
        map.placeStagedBlocks();
    }
}
class CPLess : Alteration{
    public override void run(Map map){
        map.delete(inventory.select("Checkpoint"));
    }
}
class STTF : Alteration{
    public override void run(Map map){
        map.delete(inventory.select("Checkpoint&(Ring|Gate)"));
        Inventory checkpoint = inventory.select("Checkpoint&!(Ring|Gate)");
        checkpoint.remove("Checkpoint").replace(map);
        checkpoint.select("Wall").remove("Checkpoint").remove("Up").remove("Down").remove("Right").remove("Left").replace(map);
        checkpoint.select("Slope2&Up").remove("Checkpoint").remove("Up").replace(map);
        checkpoint.select("Slope2&Down").remove("Checkpoint").remove("Down").replace(map,new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        checkpoint.select("Slope2&Right").remove("Checkpoint").remove("Right").replace(map,new BlockChange(new Vec3(0,0,32), new Vec3(PI*0.5f,0,0)));
        checkpoint.select("Slope2&Left").remove("Checkpoint").remove("Left").replace(map,new BlockChange(new Vec3(32,0,0), new Vec3(PI*1.5f,0,0)));

        checkpoint.select("Slope&Down").remove("Checkpoint").remove("Down").replace(map,new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        checkpoint.select("Slope&Up").remove("Checkpoint").remove("Up").replace(map);
        checkpoint.select("Tilt&Left").remove("Checkpoint").remove("Left").replace(map,new BlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        checkpoint.select("Tilt&Right").remove("Checkpoint").remove("Right").replace(map);
        
        map.replace("RoadIceWithWallCheckpointRight","RoadIceWithWallStraight");
        map.replace("RoadIceWithWallCheckpointLeft","RoadIceWithWallStraight");
        map.replace("RoadIceWithWallDiagRightCheckpointRight","RoadIceDiagLeftWithWallStraight");
        map.replace("RoadIceWithWallDiagLeftCheckpointRight","RoadIceDiagLeftWithWallStraight",new DiagBlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        map.replace("RoadIceWithWallDiagRightCheckpointLeft","RoadIceDiagRightWithWallStraight");
        map.replace("RoadIceWithWallDiagLeftCheckpointLeft","RoadIceDiagRightWithWallStraight",new DiagBlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));

        map.placeStagedBlocks();
    }
}

class CPFull : Alteration{
    public override void run(Map map){
        Inventory roadStraight = inventory.select("Road&Straight");
        // map.replaceKeyword(roadStraight,"!Wall&!WithWall&!Tilt",new[] {"Checkpoint"},new[] {"Straight"});
        // map.replaceKeyword(roadStraight,"Tilt",new[] {"Checkpoint","Right"},new[] {"Straight"});
        map.replace("RoadIceWithWallStraight","RoadIceWithWallCheckpointLeft");
        map.replace("RoadIceDiagLeftWithWallStraight","RoadIceWithWallDiagRightCheckpointRight");
        map.replace("RoadIceDiagRightWithWallStraight","RoadIceWithWallDiagRightCheckpointLeft");
        
        // map.replaceKeyword(roadStraight,"X2&&!Ice",new[] {"Checkpoint"},new[] {"Straight","X2"});

        // map.replaceKeyword(roadStraight,"Base",new[] {"Checkpoint"},new[] {"Base"});
        // map.replaceKeyword(roadStraight,"Slope2&Straight",new[] {"Checkpoint","Up"},new[] {"Straight"});
        map.replace("PlatformWaterRampBase","PlatformWaterCheckpoint");
        

        // map.replaceKeyword(inventory,"Platform&Wall&Straight&4",new[] {"Checkpoint","Up"},new[] {"Straight","4"},new BlockChange(new Vec3(0,0,32),new Vec3(PI*0.5f,0,0)));
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