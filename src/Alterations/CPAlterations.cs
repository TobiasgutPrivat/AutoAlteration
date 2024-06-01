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
        
        // map.replace("RoadIceWithWallCheckpointRight","RoadIceWithWallStraight",BlockType.Block);
        // map.replace("RoadIceWithWallCheckpointLeft","RoadIceWithWallStraight",BlockType.Block);
        // map.replace("RoadIceWithWallDiagRightCheckpointRight","RoadIceDiagLeftWithWallStraight",BlockType.Block);
        // map.replace("RoadIceWithWallDiagLeftCheckpointRight","RoadIceDiagLeftWithWallStraight",BlockType.Block,new DiagBlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));
        // map.replace("RoadIceWithWallDiagRightCheckpointLeft","RoadIceDiagRightWithWallStraight",BlockType.Block);
        // map.replace("RoadIceWithWallDiagLeftCheckpointLeft","RoadIceDiagRightWithWallStraight",BlockType.Block,new DiagBlockChange(new Vec3(32,0,32), new Vec3(PI,0,0)));

        map.placeStagedBlocks();
    }
}

class CPFull : Alteration{
    public override void run(Map map){
        Inventory roadStraight = inventory.select("Road&Straight");
        // map.replaceKeyword(roadStraight,"!Wall&!WithWall&!Tilt",new[] {"Checkpoint"},new[] {"Straight"});
        // map.replaceKeyword(roadStraight,"Tilt",new[] {"Checkpoint","Right"},new[] {"Straight"});
        map.replace("RoadIceWithWallStraight","RoadIceWithWallCheckpointLeft",BlockType.Block);
        map.replace("RoadIceDiagLeftWithWallStraight","RoadIceWithWallDiagRightCheckpointRight",BlockType.Block);
        map.replace("RoadIceDiagRightWithWallStraight","RoadIceWithWallDiagRightCheckpointLeft",BlockType.Block);
        
        // map.replaceKeyword(roadStraight,"X2&&!Ice",new[] {"Checkpoint"},new[] {"Straight","X2"});

        // map.replaceKeyword(roadStraight,"Base",new[] {"Checkpoint"},new[] {"Base"});
        // map.replaceKeyword(roadStraight,"Slope2&Straight",new[] {"Checkpoint","Up"},new[] {"Straight"});
        map.replace("PlatformWaterRampBase","PlatformWaterCheckpoint",BlockType.Block);
        

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