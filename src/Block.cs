using GBX.NET;
using GBX.NET.Engines.Game;
using System.Numerics;

enum BlockType
{
    Block,
    Item
}

class Block {
    public BlockType blockType;
    public string model;
    public Vec3 absolutePosition;
    public Vec3 pitchYawRoll;

    public Block(CGameCtnBlock block){
        blockType = BlockType.Block;
        model = block.BlockModel.Id;
        if (block.IsFree){
            absolutePosition = (Vec3)block.AbsolutePositionInMap;
            pitchYawRoll = (Vec3)block.PitchYawRoll;
        }else{
            switch (block.Direction){
                case Direction.North:
                    pitchYawRoll = new Vec3(0,0,0);
                    absolutePosition = new (-32,0,-32);
                    break;
                case Direction.East:
                    pitchYawRoll = new Vec3(3.141528f * 1.5f,0,0);
                    absolutePosition = new (0,0,-32);
                    break;
                case Direction.South:
                    pitchYawRoll = new Vec3(3.141528f,0,0);
                    absolutePosition = new (0,0,0);
                    break;
                case Direction.West:
                    pitchYawRoll = new Vec3(3.141528f * 0.5f,0,0);
                    absolutePosition = new (-32,0,0);
                    break;
                default:
                    pitchYawRoll = new Vec3(0,0,0);
                    absolutePosition = new(0,0,0);
                    break;
            }
            absolutePosition = absolutePosition + new Vec3(block.Coord.X * 32,block.Coord.Y * 8 - 64,block.Coord.Z * 32);
        }
    }
    public Block(CGameCtnAnchoredObject item){
        blockType = BlockType.Item;
        model = item.ItemModel.Id;
        absolutePosition = item.AbsolutePositionInMap;
        pitchYawRoll = item.PitchYawRoll;
    }

    public void relativeOffset(Vec3 offset)
    {
        Matrix4x4 rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(pitchYawRoll.X, pitchYawRoll.Y, pitchYawRoll.Z);//yaw,pitch,roll
        Vector3 offsetV3 = new Vector3(offset.X,offset.Y,offset.Z);
        Vector3 transformedOffset = Vector3.Transform(offsetV3, rotationMatrix);
        absolutePosition = absolutePosition + new Vec3(transformedOffset.X, transformedOffset.Y, transformedOffset.Z);
    }
}