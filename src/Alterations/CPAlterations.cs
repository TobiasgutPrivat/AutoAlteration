class CPBoost : Alteration{
    public override void Run(Map map){
        inventory.Select(BlockType.Block).Select("Checkpoint").RemoveKeyword("Checkpoint").AddKeyword("Turbo").Replace(map);
        inventory.Select(BlockType.Block).Select("Turbo").RemoveKeyword("Turbo").AddKeyword("Checkpoint").Replace(map);
        inventory.Select(BlockType.Block).Select("Turbo2").RemoveKeyword("Turbo2").AddKeyword("Checkpoint").Replace(map);
        inventory.Select(BlockType.Item).Select("Checkpoint").RemoveKeyword(new[] {"Right","Left","Center","Checkpoint","v2"}).AddKeyword("Turbo").Replace(map);
        inventory.Select(BlockType.Item).Select("Turbo").AddKeyword("Center").RemoveKeyword("Turbo").AddKeyword("Checkpoint").Replace(map);
        inventory.Select(BlockType.Item).Select("Turbo2").AddKeyword("Center").RemoveKeyword("Turbo2").AddKeyword("Checkpoint").Replace(map);
        map.Replace("GateSpecial4mTurbo","GateCheckpointCenter8mv2",Move(2,0,0));//untested
        map.PlaceStagedBlocks();
    }
}
class CPLess : Alteration{
    public override void Run(Map map){
        map.Delete(inventory.Select("Checkpoint"));
    }
}
class STTF : Alteration{
    public override void Run(Map map){
        map.Delete(inventory.Select("Checkpoint&(Ring|Gate)"));
        inventory.Select(BlockType.Block).Select("Checkpoint").RemoveKeyword("Checkpoint").Replace(map);
        map.PlaceStagedBlocks();
    }

    public override void ChangeInventory(){
        AddNoCPBlocks();
    }
}

class CPFull : Alteration{
    public override void Run(Map map){
        inventory.Select(BlockType.Block).AddKeyword("Checkpoint").Replace(map);
        map.PlaceStagedBlocks();
    }
    public override void ChangeInventory(){
        AddNoCPBlocks();
    }
}

