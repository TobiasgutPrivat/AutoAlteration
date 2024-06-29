using GBX.NET;

class Surfaceless: Alteration {
    public override void Run(Map map){    
        Inventory Blocks = inventory.select(BlockType.Block);
        map.PlaceRelative(Blocks.select("MapStart").names(),"GateStartCenter32m");
        map.PlaceRelative(Blocks.select("Checkpoint").names(),"GateCheckpointCenter32m");
        map.PlaceRelative(Blocks.select("Finish").names(),"GateFinishCenter32m");
        map.delete(Blocks.names());
        map.PlaceStagedBlocks();
    }
}
class YepTree: Alteration {
    public override void Run(Map map){    
        string[] Trees = inventory.select("Small&(Tree|Cactus|Fir|Palm|Cypress)").names();
        Trees = Trees.Append("Spring").ToArray();
        Trees = Trees.Append("Summer").ToArray();
        Trees = Trees.Append("Winter").ToArray();
        Trees = Trees.Append("Fall").ToArray();
        map.PlaceRelative(Trees,"GateCheckpointCenter8mv2",new Position(Vec3.Zero,Vec3.Zero));
        map.PlaceStagedBlocks();
    }
}

class Flipped: Alteration {
    public override void Run(Map map){//Prototype
        //Dimensions normal Stadium
        // (1,9,1)
        // (48,38,48)
        inventory.edit().replace(map);
        map.stagedBlocks.ForEach(x => x.position.coords = new Vec3(1536-x.position.coords.X, 240-x.position.coords.Y, 1536-x.position.coords.Z));  
        map.stagedBlocks.ForEach(x => x.position.Rotate(new Vec3(0, PI, 0)));
        map.PlaceStagedBlocks();
    }
}