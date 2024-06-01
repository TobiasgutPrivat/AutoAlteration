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
        inventory.select(BlockType.Block).select("Checkpoint").remove("Checkpoint").replace(map);
        map.placeStagedBlocks();
    }
}

class CPFull : Alteration{
    public override void run(Map map){
        inventory.select(BlockType.Block).add("Checkpoint").replace(map);
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