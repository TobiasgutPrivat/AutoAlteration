using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;

class BlockChange{
    public BlockType blockType;
    public string model;
    public Vec3 absolutePosition;
    public Vec3 pitchYawRoll;

    public BlockChange(BlockType blockType, string model){
        this.blockType = blockType;
        this.model = model;
    }
    public BlockChange(BlockType blockType, string model, Vec3 absolutePosition){
        this.blockType = blockType;
        this.model = model;
        this.absolutePosition = absolutePosition;
    }
    public BlockChange(BlockType blockType, string model, Vec3 absolutePosition, Vec3 pitchYawRoll){
        this.blockType = blockType;
        this.model = model;
        this.absolutePosition = absolutePosition;
        this.pitchYawRoll = pitchYawRoll;
    }
    public BlockChange(Vec3 absolutePosition){
        this.model = "";
        this.absolutePosition = absolutePosition;
    }
    public BlockChange(Vec3 absolutePosition, Vec3 pitchYawRoll){
        this.model = "";
        this.absolutePosition = absolutePosition;
        this.pitchYawRoll = pitchYawRoll;
    }
    
    public virtual void changeBlock(CGameCtnBlock ctnBlock,Block @block){
        if (model != "") {
            block.blockType = blockType;
            block.model = model;
        }
        block.relativeOffset(absolutePosition);
        block.pitchYawRoll += pitchYawRoll;
    }
    public virtual void changeItem(CGameCtnAnchoredObject ctnItem,Block @block){
        if (model != "") {
            block.blockType = blockType;
            block.model = model;
        }
        block.relativeOffset(absolutePosition);
        block.pitchYawRoll += pitchYawRoll;
    }
}