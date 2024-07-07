using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;

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
    public bool IsGround;
    public bool IsAir;
    public CGameCtnBlockSkin? Skin;
    public string Path = "";

    public Block(CGameCtnBlock block,Article fromArticle,  Article article, Position ?placePosition)
    {
        InitBlock(block);
        this.article = article;
        this.name = article.Name;
        this.blockType = article.Type;
        this.Path = article.Path;
        
        position.AddPosition(fromArticle.Position);
        placePosition ??= Position.Zero;
        if (placePosition.multiplyBySize){
            position.AddPosition(new Position(new Vec3(placePosition.coords.X * article.Width, placePosition.coords.Y, placePosition.coords.Z * article.Length), placePosition.pitchYawRoll));
        } else {
            position.AddPosition(placePosition);
        }
        position.SubtractPosition(article.Position);
    }

    private void InitBlock(CGameCtnBlock block) {
        color = block.Color;
        blockType = BlockType.Block;
        name = block.BlockModel.Id;
        IsFree = block.IsFree;
        IsClip = block.IsClip;
        IsGround = block.IsGround;
        Skin = block.Skin;
        
        if (block.Bit17){
            Console.WriteLine("Air");
        }
        IsAir = block.Bit21;
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
        Article ?article = Alteration.inventory.GetArticle(block.BlockModel.Id);
        if (article == null){
            Console.WriteLine("No article found for model: " + block.BlockModel.Id);
            return Position.Zero;
        }
        return block.Direction switch
        {
            Direction.North => new((-32, 0, -32), Vec3.Zero),
            Direction.East => new(((article.Length - 1) * 32, 0, -32), (Alteration.PI * 1.5f, 0, 0)),
            Direction.South => new(((article.Width - 1) * 32, 0, (article.Length - 1) * 32), (Alteration.PI, 0, 0)),
            Direction.West => new((-32, 0, (article.Width - 1) * 32), (Alteration.PI * 0.5f, 0, 0)),
            _ => Position.Zero,
        };
    }

    public Block(CGameCtnAnchoredObject item,Article fromArticle, Article article,Position ?placePosition){
        InitItem(item);
        this.article = article;
        this.name = article.Name;
        this.blockType = article.Type;
        position.AddPosition(fromArticle.Position);
        position.AddPosition(placePosition ?? Position.Zero);
        position.SubtractPosition(article.Position);
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