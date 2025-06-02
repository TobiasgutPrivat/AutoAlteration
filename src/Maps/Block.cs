using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;

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
    public bool IsGhost;
    public bool IsAir;
    public CGameCtnBlockSkin? Skin;
    // public CGameCtnAnchoredObject? SnappedOnItem;
    // public CGameCtnBlock? SnappedOnBlock;
    // public CGameCtnAnchoredObject? PlacedOnItem;
    // public Vec3 PivotPosition;
    // public Byte3 BlockUnitCoord;
    public CGameWaypointSpecialProperty? WaypointSpecialProperty; //Stores CPLink info
    public string? AnchorTreeId;
    // public int BlockFlags; //not needed, causes issue with placing
    public short ItemFlags; //Stores relevant placement info
    public string Path = "";

    public void PlaceInMap(CGameCtnChallenge map, bool revertFreeBlock){
        switch (blockType){
            case BlockType.Block:
            case BlockType.CustomBlock:
            case BlockType.Pillar:
                PlaceBlockInMap(map, revertFreeBlock);
                break;
            case BlockType.Item:
            case BlockType.CustomItem:
                PlaceItemInMap(map);
                break;
        }
    }

    #region Blocks
        
    public Block(CGameCtnBlock block,Article fromArticle,  Article article, int freeBlockHeightOffset, MoveChain ?moveChain)
    {
        color = block.Color;
        IsFree = block.IsFree;
        IsClip = block.IsClip;
        IsGhost = block.IsGhost;
        // BlockFlags = block.Flags;

        IsGround = block.IsGround;
        Skin = block.Skin;
        IsAir = block.Bit21;
        WaypointSpecialProperty = block.WaypointSpecialProperty;
        position = GetBlockPosition(block, freeBlockHeightOffset);

        if(block.Author is null || !block.Author.Contains("AutoAltTag")) {
            article.DefaultRotation?.Apply(position,article);
        };

        name = article.Name;
        blockType = article.Type;
        Path = article.Path;
        
        fromArticle.MoveChain.Apply(position,article); // specific like for CPGate-Position
        moveChain?.Apply(position,article); // the Altering moves
        article.MoveChain.Subtract(position,article); // specific like for CPGate-Position
    }

    public static Position GetBlockPosition(CGameCtnBlock block, int freeBlockHeightOffset) {
        if (block.IsFree){
            return new Position(block.AbsolutePositionInMap,block.PitchYawRoll);
        } else {
            Position position = new Position(new Vec3(block.Coord.X * 32,block.Coord.Y * 8 - freeBlockHeightOffset ,block.Coord.Z * 32));
            position.AddPosition(GetDirectionOffset(block));
            return position;
        }
    }

    public static Position GetDirectionOffset(CGameCtnBlock block) {
        Article ?article = Alteration.inventory.GetArticle(block.BlockModel.Id.Replace(".Block.Gbx_CustomBlock",""));
        if (article == null){
            Console.WriteLine("No article found for model: " + block.BlockModel.Id.Replace(".Block.Gbx_CustomBlock",""));
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

    public static Position GetDefaultRotationOffset(CGameCtnBlock block) {
        return new Position(Vec3.Zero, Vec3.Zero);//TODO
    }

    private void PlaceBlockInMap(CGameCtnChallenge map, bool revertFreeBlock) {
        CGameCtnBlock block = map.PlaceBlock(name,new(0,0,0),Direction.North);
        float yaw = (float)Math.Round(position.pitchYawRoll.X / (Alteration.PI/2),5);
        if (!IsFree && revertFreeBlock 
            && position.coords.X % 32 == 0 && position.coords.Y % 8 == 0 && position.coords.Z % 32 == 0
            && position.pitchYawRoll.Y == 0 && position.pitchYawRoll.Z == 0
            && yaw % 1 == 0 
            ) {
            block.IsFree = false;
            switch (yaw) {
                case 0:
                    block.Direction = Direction.North;
                    break;
                case 1:
                case -3:
                    block.Direction = Direction.West;
                    break;
                case 2:
                case -2:
                    block.Direction = Direction.South;
                    break;
                case 3:
                case -1:
                    block.Direction = Direction.East;
                    break;
                default:
                    throw new Exception("Invalid direction");
            }
            Vec3 offset = -GetDirectionOffset(block).coords;

            block.Coord = new Int3(
                (int)(position.coords.X + offset.X)  / 32, 
                (int)(position.coords.Y + offset.Y + map.DecoBaseHeightOffset*8) / 8 , 
                (int)(position.coords.Z + offset.Z)/ 32
                );
            block.IsGhost = IsGhost;
        } else {
            block.IsFree = true;
            block.AbsolutePositionInMap = position.coords;
            block.PitchYawRoll = position.pitchYawRoll;
            block.Coord = new Int3((int)block.AbsolutePositionInMap.Value.X / 32, (int)block.AbsolutePositionInMap.Value.Y / 8, (int)block.AbsolutePositionInMap.Value.Z / 32);
        }

        block.WaypointSpecialProperty = WaypointSpecialProperty;
        block.IsClip = IsClip;
        block.IsGround = IsGround;
        block.Color = color;
        block.Skin = Skin;
        block.Bit21 = IsAir;
        if(block.Author is null || !block.Author.Contains("AutoAltTag")) {
            block.Author += "AutoAltTag";
        }
        // block.Flags = BlockFlags;
    }

    #endregion

    #region Items
    
    public Block(CGameCtnAnchoredObject item,Article fromArticle, Article article,MoveChain ?moveChain){
        blockType = BlockType.Item;
        name = item.ItemModel.Id;

        // SnappedOnBlock = item.SnappedOnBlock;
        // SnappedOnItem = item.SnappedOnItem;
        // PlacedOnItem = item.PlacedOnItem;
        // BlockUnitCoord = item.BlockUnitCoord;
        AnchorTreeId = item.AnchorTreeId;
        WaypointSpecialProperty = item.WaypointSpecialProperty;
        color = item.Color;
        ItemFlags = item.Flags;

        position = new Position(item.AbsolutePositionInMap,item.PitchYawRoll);
        position.Move(item.PivotPosition); // PivotPosition is Offset which comes from snapping

        name = article.Name;
        blockType = article.Type;
        Path = article.Path;

        fromArticle.MoveChain.Apply(position,article);
        moveChain?.Apply(position,article);
        article.MoveChain.Subtract(position,article);
    }

    private void PlaceItemInMap(CGameCtnChallenge map){
        CGameCtnAnchoredObject item = map.PlaceAnchoredObject(new Ident(name, new Id(26), "Nadeo"),position.coords,position.pitchYawRoll);
        // item.SnappedOnItem = SnappedOnItem;
        // item.SnappedOnBlock = SnappedOnBlock;
        // item.PlacedOnItem = PlacedOnItem;
        // item.PivotPosition = PivotPosition;
        // item.BlockUnitCoord = BlockUnitCoord;
        item.WaypointSpecialProperty = WaypointSpecialProperty;
        item.Color = color;
        item.AnchorTreeId = AnchorTreeId;
        item.Scale = 1;
        item.Flags = ItemFlags;
        // item.PackDesc could be used for tagging
    }
    #endregion
}