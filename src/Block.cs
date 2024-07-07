using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;

enum BlockType
{
    Block,
    Item,
    CustomBlock,
    CustomItem,
    Pillar
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
        color = block.Color;
        blockType = BlockType.Block;
        name = block.BlockModel.Id;
        IsFree = block.IsFree;
        IsClip = block.IsClip;
        IsGround = block.IsGround;
        Skin = block.Skin;
        IsAir = block.Bit21;
        position = GetBlockPosition(block);

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
        blockType = BlockType.Item;
        name = item.ItemModel.Id;
        color = item.Color;
        position = new Position(item.AbsolutePositionInMap,item.PitchYawRoll);
        this.article = article;
        this.name = article.Name;
        this.blockType = article.Type;
        position.AddPosition(fromArticle.Position);
        position.AddPosition(placePosition ?? Position.Zero);
        position.SubtractPosition(article.Position);
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