using GBX.NET;
using GBX.NET.Engines.Game;

enum BlockType
{
    Block,
    Item
}

class Block {
    public BlockType blockType;
    public string name;
    public Position position = new Position();
    public DifficultyColor color;

    public Block(CGameCtnBlock block,Article fromArticle,  Article article, Position placePosition)
    {
        initBlock(block,fromArticle);
        this.name = article.name;
        this.blockType = article.type;
        // if (fromArticle.posCorection != null) {
        //     position.move(fromArticle.posCorection.block(block));
        // }
        position.addPosition(fromArticle.position);
        position.addPosition(placePosition);
        position.subtractPosition(article.position);
    }

    private void initBlock(CGameCtnBlock block, Article fromArticle) {
        color = block.Color;
        blockType = BlockType.Block;
        name = block.BlockModel.Id;
        Vec3 absolutePosition;
        Vec3 pitchYawRoll;
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
                    pitchYawRoll = new Vec3(Alteration.PI * 1.5f,0,0);
                    absolutePosition = new (0,0,-32);
                    absolutePosition += new Vec3((fromArticle.length - 1) * 32,0,0); 
                    break;
                case Direction.South:
                    pitchYawRoll = new Vec3(Alteration.PI,0,0);
                    absolutePosition = new (0,0,0);
                    absolutePosition += new Vec3((fromArticle.width - 1)*32,0,(fromArticle.length - 1)*32); 
                    break;
                case Direction.West:
                    pitchYawRoll = new Vec3(Alteration.PI * 0.5f,0,0);
                    absolutePosition = new (-32,0,0);
                    absolutePosition += new Vec3(0,0,(fromArticle.width - 1)*32);
                    break;
                default:
                    pitchYawRoll = new Vec3(0,0,0);
                    absolutePosition = new(0,0,0);
                    break;
            }
            absolutePosition += new Vec3(block.Coord.X * 32,block.Coord.Y * 8 - 64,block.Coord.Z * 32);
        }
        position.addPosition(absolutePosition,pitchYawRoll);
    }

    public Block(CGameCtnAnchoredObject item,Article fromArticle, Article article,Position placePosition){
        initBlock(item);

        this.name = article.name;
        this.blockType = article.type;
        // if (fromArticle.posCorection != null) {
        //     position.move(fromArticle.posCorection.item(item));
        // }
        position.addPosition(fromArticle.position);
        position.addPosition(placePosition);
        position.subtractPosition(article.position);
    }

    public void initBlock(CGameCtnAnchoredObject item){
        blockType = BlockType.Item;
        name = item.ItemModel.Id;
        position = new Position(item.AbsolutePositionInMap,item.PitchYawRoll);
    }
}