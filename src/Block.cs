using GBX.NET;
using GBX.NET.Engines.Game;

enum BlockType
{
    Block,
    Item,
    CustomBlock,
    CustomItem
}

class Block {
    public BlockType blockType;
    public Article article;
    public string name;
    public Position position;
    public DifficultyColor color;
    public bool IsFree;
    public bool IsClip;
    public bool IsGhost;
    public bool IsGround;

    public Block(CGameCtnBlock block,Article fromArticle,  Article article, Position placePosition)
    {
        initBlock(block,fromArticle);
        this.article = article;
        this.name = article.name;
        this.blockType = article.type;
        position.addPosition(fromArticle.position);
        position.addPosition(placePosition);
        position.subtractPosition(article.position);
    }

    private void initBlock(CGameCtnBlock block, Article fromArticle) {
        color = block.Color;
        blockType = BlockType.Block;
        name = block.BlockModel.Id;
        IsFree = block.IsFree;
        IsClip = block.IsClip;
        IsGhost = block.IsGhost;
        IsGround = block.IsGround;
        Vec3 absolutePosition;
        Vec3 pitchYawRoll;
        if (block.IsFree){
            absolutePosition = (Vec3)block.AbsolutePositionInMap;
            pitchYawRoll = (Vec3)block.PitchYawRoll;
        }else{
            getDirectionOffset(fromArticle,block.Direction,out absolutePosition,out pitchYawRoll);
            absolutePosition += new Vec3(block.Coord.X * 32,block.Coord.Y * 8 - 64,block.Coord.Z * 32);
        }
        position = new Position(absolutePosition,pitchYawRoll);
    }

    public static void getDirectionOffset(Article article, Direction direction, out Vec3 absolutePosition, out Vec3 pitchYawRoll) {
        switch (direction){
            case Direction.North:
                pitchYawRoll = new Vec3(0,0,0);
                absolutePosition = new (-32,0,-32);
                break;
            case Direction.East:
                pitchYawRoll = new Vec3(Alteration.PI * 1.5f,0,0);
                absolutePosition = new (0,0,-32);
                absolutePosition += new Vec3((article.length - 1) * 32,0,0); 
                break;
            case Direction.South:
                pitchYawRoll = new Vec3(Alteration.PI,0,0);
                absolutePosition = new (0,0,0);
                absolutePosition += new Vec3((article.width - 1)*32,0,(article.length - 1)*32); 
                break;
            case Direction.West:
                pitchYawRoll = new Vec3(Alteration.PI * 0.5f,0,0);
                absolutePosition = new (-32,0,0);
                absolutePosition += new Vec3(0,0,(article.width - 1)*32);
                break;
            default:
                pitchYawRoll = new Vec3(0,0,0);
                absolutePosition = new(0,0,0);
                break;
        }
    }

    public Block(CGameCtnAnchoredObject item,Article fromArticle, Article article,Position placePosition){
        initBlock(item);
        this.article = article;
        this.name = article.name;
        this.blockType = article.type;
        position.addPosition(fromArticle.position);
        position.addPosition(placePosition);
        position.subtractPosition(article.position);
    }

    public void initBlock(CGameCtnAnchoredObject item){
        blockType = BlockType.Item;
        name = item.ItemModel.Id;
        position = new Position(item.AbsolutePositionInMap,item.PitchYawRoll);
    }

    public bool isInGrid(){
        if (position.coords.X % 32 != 0 || position.coords.Y % 8 != 0 || position.coords.Z % 32 != 0){
        return false;
        }
        if (round(position.pitchYawRoll.Y) != 0 || round(position.pitchYawRoll.Z) != 0){
        return false;
        }
        if(round(position.pitchYawRoll.X % ((float)Math.PI/2)) == 0){
        return false;
        }
        return true;
    }

    public static float round(float number){
        number = (float)Math.Round(number,3);
        number = Math.Abs(number);
        return number;
    }
}