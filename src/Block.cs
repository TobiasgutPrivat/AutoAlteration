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
    public Position position = Position.Zero;
    public DifficultyColor color;
    public bool IsFree;
    public bool IsClip;
    public bool IsGhost;
    public bool IsGround;

    public Block(CGameCtnBlock block,Article fromArticle,  Article article, Position ?placePosition)
    {
        InitBlock(block);
        this.article = article;
        this.name = article.name;
        this.blockType = article.type;
        
        position.AddPosition(fromArticle.position);
        placePosition ??= Position.Zero;
        if (placePosition.multiplyBySize){
            position.AddPosition(new Position(new Vec3(placePosition.coords.X * article.width, placePosition.coords.Y, placePosition.coords.Z * article.length), placePosition.pitchYawRoll));
        } else {
            position.AddPosition(placePosition);
        }
        position.SubtractPosition(article.position);
    }

    private void InitBlock(CGameCtnBlock block) {
        color = block.Color;
        blockType = BlockType.Block;
        name = block.BlockModel.Id;
        IsFree = block.IsFree;
        IsClip = block.IsClip;
        IsGhost = block.IsGhost;
        IsGround = block.IsGround;
        position = GetBlockPosition(block);
    }

    public static Position GetBlockPosition(CGameCtnBlock block) {
        if (block.IsFree){
            return new Position(block.AbsolutePositionInMap,block.PitchYawRoll);
        } else {
            return new Position(new Vec3(block.Coord.X * 32,block.Coord.Y * 8 - 64,block.Coord.Z * 32)).AddPosition(GetDirectionOffset(block));
        }
    }

    public static Position GetDirectionOffset(CGameCtnBlock block) {
        Article article = Alteration.inventory.GetArticle(block.BlockModel.Id);
        return block.Direction switch
        {
            Direction.North => new((-32, 0, -32), Vec3.Zero),
            Direction.East => new(((article.length - 1) * 32, 0, -32), (Alteration.PI * 1.5f, 0, 0)),
            Direction.South => new(((article.width - 1) * 32, 0, (article.length - 1) * 32), (Alteration.PI, 0, 0)),
            Direction.West => new((-32, 0, (article.width - 1) * 32), (Alteration.PI * 0.5f, 0, 0)),
            _ => Position.Zero,
        };
    }

    public Block(CGameCtnAnchoredObject item,Article fromArticle, Article article,Position ?placePosition){
        InitItem(item);
        this.article = article;
        this.name = article.name;
        this.blockType = article.type;
        position.AddPosition(fromArticle.position);
        position.AddPosition(placePosition ?? Position.Zero);
        position.SubtractPosition(article.position);
    }

    public void InitItem(CGameCtnAnchoredObject item){
        blockType = BlockType.Item;
        name = item.ItemModel.Id;
        position = new Position(item.AbsolutePositionInMap,item.PitchYawRoll);
    }

    public bool IsInGrid(){
        if (position.coords.X % 32 != 0 || position.coords.Y % 8 != 0 || position.coords.Z % 32 != 0){
        return false;
        }
        if (Round(position.pitchYawRoll.Y) != 0 || Round(position.pitchYawRoll.Z) != 0){
        return false;
        }
        if(Round(position.pitchYawRoll.X % ((float)Math.PI/2)) == 0){
        return false;
        }
        return true;
    }

    public static float Round(float number){
        number = (float)Math.Round(number,3);
        number = Math.Abs(number);
        return number;
    }
}