using GBX.NET;
using GBX.NET.Engines.Game;

class BlockChange{
    public Vec3 absolutePosition;
    public Vec3 pitchYawRoll;

    public BlockChange(){
        this.absolutePosition = Vec3.Zero;
        this.pitchYawRoll = Vec3.Zero;
    }
    public BlockChange( Vec3 absolutePosition){
        this.absolutePosition = absolutePosition;
        this.pitchYawRoll = Vec3.Zero;
    }
    public BlockChange( Vec3 absolutePosition, Vec3 pitchYawRoll){
        this.absolutePosition = absolutePosition;
        this.pitchYawRoll = pitchYawRoll;
    }
    public virtual void changeBlock(CGameCtnBlock ctnBlock,Block @block){
        block.relativeOffset(absolutePosition);
        block.pitchYawRoll += pitchYawRoll;
    }

    public virtual void changeItem(CGameCtnAnchoredObject ctnItem,Block @block){
        block.relativeOffset(absolutePosition);
        block.pitchYawRoll += pitchYawRoll;
    }
}