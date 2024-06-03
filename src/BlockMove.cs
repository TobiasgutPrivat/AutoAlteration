using System.Numerics;
using GBX.NET;
using GBX.NET.Engines.Game;

class BlockMove{
    public static float PI = (float)Math.PI;
    public Vec3 absolutePosition;
    public Vec3 pitchYawRoll;

    public BlockMove(){
        this.absolutePosition = Vec3.Zero;
        this.pitchYawRoll = Vec3.Zero;
    }
    public BlockMove( Vec3 absolutePosition){
        this.absolutePosition = absolutePosition;
        this.pitchYawRoll = Vec3.Zero;
    }
    public BlockMove( Vec3 absolutePosition, Vec3 pitchYawRoll){
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