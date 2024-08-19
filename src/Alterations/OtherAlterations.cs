using GBX.NET;

class Surfaceless: Alteration {
    public override void Run(Map map){    
        Inventory Blocks = inventory.Select(BlockType.Block);
        map.PlaceRelative(Blocks.Select("MapStart").Names(),"GateStartCenter32m");
        map.PlaceRelative(Blocks.Select("Checkpoint").Names(),"GateCheckpointCenter32m");
        map.PlaceRelative(Blocks.Select("Finish").Names(),"GateFinishCenter32m");
        map.Delete(Blocks.Names());
        map.PlaceStagedBlocks();
    }
}
class YepTree: Alteration {
    public override void Run(Map map){    
        string[] Trees = inventory.Select("Small&(Tree|Cactus|Fir|Palm|Cypress)").Names();
        Trees = Trees.Append("Spring").ToArray();
        Trees = Trees.Append("Summer").ToArray();
        Trees = Trees.Append("Winter").ToArray();
        Trees = Trees.Append("Fall").ToArray();
        map.PlaceRelative(Trees,"GateCheckpointCenter8mv2");
        map.PlaceStagedBlocks();
    }
}

class Flipped: Alteration {
    public override void Run(Map map){//Prototype TODO a lot
        //Dimensions normal Stadium
        // (1,9,1)
        // (48,38,48)
        inventory.Edit().Replace(map);
        map.stagedBlocks.ForEach(x => x.position.coords = new Vec3(x.position.coords.X, 240-x.position.coords.Y, 1536-x.position.coords.Z));  //X: 1536-
        map.stagedBlocks.ForEach(x => x.position.Rotate(new Vec3(0, PI, 0)));
        map.PlaceStagedBlocks();
    }
}

class NoItems: Alteration {
    public override void Run(Map map){
        map.Delete(inventory.Select(BlockType.Item).Select("!MapStart&!Finish").Names());
        map.PlaceStagedBlocks();
    }
}

class Rotated: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select(BlockType.Block),RotateMid(PI,0,0));
        map.PlaceStagedBlocks();
    }
}

class Mini : Alteration {
    public override void Run(Map map){
        inventory.AddKeyword("MiniBlock").Replace(map);
        map.Delete(inventory);
        map.stagedBlocks.ForEach(x => x.position.coords = new Vec3(x.position.coords.X / 2, x.position.coords.Y / 2 + 4, x.position.coords.Z / 2));//4 is offset for normal stadium
        map.PlaceStagedBlocks();
    }

    public override void ChangeInventory()
    {
        AddCustomBlocks("MiniBlock");
    }
}
class Invisible : Alteration {
    public override void Run(Map map){
        inventory.Select("!MapStart&!Finish&!Checkpoint").AddKeyword("InvisibleBlock").Replace(map);
        map.PlaceStagedBlocks();
    }

    public override void ChangeInventory()
    {
        AddCustomBlocks("InvisibleBlock");
    }
}