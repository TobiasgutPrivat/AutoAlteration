using System.Dynamic;
using GBX.NET;

enum BlockType
{
    Block,
    Item
}

class Block {
    public BlockType blockType;
    public string model;
    public Int3 coord;
    public Vec3 absolutePosition;
    public Vec3 pitchYawRoll;
    public Direction direction;
    public Block(string model, Int3 coord, Direction direction){
        this.blockType = BlockType.Block;
        this.model = model;
        this.coord = coord;
        this.direction = direction;
    }
    public Block(string model, Vec3 absolutePosition, Vec3 pitchYawRoll){
        this.blockType = BlockType.Block;
        this.model = model;
        this.absolutePosition = absolutePosition;
        this.pitchYawRoll = pitchYawRoll;
    }
}