using System.Numerics;
using GBX.NET;
using GBX.NET.Engines.Game;

class BlockChange{
    public static float PI = (float)Math.PI;
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
    // public void addBlockChange(BlockChange blockChange){
    //     relativeOffset(blockChange.absolutePosition);
    //     pitchYawRoll += blockChange.pitchYawRoll;
    // }
    public virtual void changeBlock(CGameCtnBlock ctnBlock,Block @block){
        block.relativeOffset(absolutePosition);
        block.pitchYawRoll += pitchYawRoll;
    }

    public virtual void changeItem(CGameCtnAnchoredObject ctnItem,Block @block){
        block.relativeOffset(absolutePosition);
        block.pitchYawRoll += pitchYawRoll;
    }

    // public void relativeOffset(Vec3 offset)
    // {
    //     Matrix4x4 rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(pitchYawRoll.X, pitchYawRoll.Y, pitchYawRoll.Z);//yaw,pitch,roll
    //     Vector3 offsetV3 = new Vector3(offset.X,offset.Y,offset.Z);
    //     Vector3 transformedOffset = Vector3.Transform(offsetV3, rotationMatrix);
    //     absolutePosition = absolutePosition + new Vec3(transformedOffset.X, transformedOffset.Y, transformedOffset.Z);
    // }
}