using GBX.NET;

class Surfaceless: Alteration {
    public override void run(Map map){    
        Inventory Blocks = inventory.select(BlockType.Block);
        map.placeRelative(Blocks.select("MapStart").names(),"GateStartCenter32m",BlockType.Item);
        map.placeRelative(Blocks.select("Checkpoint").names(),"GateCheckpointCenter32m",BlockType.Item);
        map.placeRelative(Blocks.select("Finish").names(),"GateFinishCenter32m",BlockType.Item);
        map.delete(Blocks.names());
        map.placeStagedBlocks();
    }
}
class YepTree: Alteration {
    public override void run(Map map){    
        string[] Trees = inventory.select("Small&(Tree|Cactus|Fir|Palm|Cypress)").names();
        Trees = Trees.Append("Spring").ToArray();
        Trees = Trees.Append("Summer").ToArray();
        Trees = Trees.Append("Winter").ToArray();
        Trees = Trees.Append("Fall").ToArray();
        map.placeRelative(Trees,"GateCheckpointCenter8mv2",BlockType.Item,new Position(Vec3.Zero,Vec3.Zero));
        map.placeStagedBlocks();
    }
}