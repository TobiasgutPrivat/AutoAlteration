using GBX.NET;
using GBX.NET.Engines.Game;

public enum BlockType
{
    Block,
    Item,
    CustomBlock,
    CustomItem,
    Pillar
}

public class Block {
    public BlockType blockType;
    public string name;
    public Position position = Position.Zero;
    public DifficultyColor color;
    public bool IsFree;
    public bool IsClip;
    public bool IsGround;
    public bool IsAir;
    public CGameCtnBlockSkin? Skin;
    public CGameCtnAnchoredObject? SnappedOnItem;
    public CGameCtnBlock? SnappedOnBlock;
    public CGameCtnAnchoredObject? PlacedOnItem;
    public Vec3 PivotPosition;
    public Byte3 BlockUnitCoord;
    public string Path = "";

    public Block(CGameCtnBlock block,Article fromArticle,  Article article, MoveChain ?moveChain)
    {
        color = block.Color;
        // blockType = BlockType.Block;
        // name = block.BlockModel.Id;
        IsFree = block.IsFree;
        IsClip = block.IsClip;
        IsGround = block.IsGround;
        Skin = block.Skin;
        IsAir = block.Bit21;
        position = GetBlockPosition(block);

        name = article.Name;
        blockType = article.Type;
        Path = article.Path;
        
        fromArticle.MoveChain.Apply(position,article);
        moveChain?.Apply(position,article);
        article.MoveChain.Subtract(position,article);
    }

    public static Position GetBlockPosition(CGameCtnBlock block) {
        if (block.IsFree){
            return new Position(block.AbsolutePositionInMap,block.PitchYawRoll);
        } else {
            Position position = new Position(new Vec3(block.Coord.X * 32,block.Coord.Y * 8 - 64,block.Coord.Z * 32));
            position.AddPosition(GetDirectionOffset(block));
            if (block.BlockModel.Id == "TrackWallArch1x4SideTop"){//only block with that issue found yet
                position.AddPosition(new(Vec3.Zero, new Vec3(0.5f*Alteration.PI, 0, 0)));
            }
            return new Position(new Vec3(block.Coord.X * 32,block.Coord.Y * 8 - 64,block.Coord.Z * 32)).AddPosition(GetDirectionOffset(block));// 64m offset depends on Map Template i think
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
            Direction.North => new((0, 0, 0), Vec3.Zero),
            Direction.East => new((article.Length * 32, 0, 0), (Alteration.PI * 1.5f, 0, 0)),
            Direction.South => new((article.Width * 32, 0, article.Length * 32), (Alteration.PI, 0, 0)),
            Direction.West => new((0, 0, article.Width * 32), (Alteration.PI * 0.5f, 0, 0)),
            _ => Position.Zero,
        };
    }


    public Block(CGameCtnAnchoredObject item,Article fromArticle, Article article,MoveChain ?moveChain){
        blockType = BlockType.Item;
        name = item.ItemModel.Id;
        // SnappedOnBlock = item.SnappedOnBlock;
        // SnappedOnItem = item.SnappedOnItem;
        // PlacedOnItem = item.PlacedOnItem;
        // BlockUnitCoord = item.BlockUnitCoord;
        color = item.Color;
        position = new Position(item.AbsolutePositionInMap,item.PitchYawRoll);
        position.Move(item.PivotPosition);

        name = article.Name;
        blockType = article.Type;
        Path = article.Path;

        fromArticle.MoveChain.Apply(position,article);
        moveChain?.Apply(position,article);
        article.MoveChain.Subtract(position,article);
    }
}