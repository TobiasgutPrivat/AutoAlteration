class OneUP: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,8,0));
        map.PlaceStagedBlocks();
    }
}
class TwoUP: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,16,0));
        map.PlaceStagedBlocks();
    }
}
class OneRight: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(-32,0,0));
        map.PlaceStagedBlocks();
    }
}
class OneLeft: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(32,0,0));
        map.PlaceStagedBlocks();
    }
}
class OneDown: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,-8,0));
        map.PlaceStagedBlocks();
    }
}
class OneBack: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,0,32));
        map.PlaceStagedBlocks();
    }
}