using GBX.NET;
using GBX.NET.Engines.Game;

class CPBoost : Alteration{
    public override void run(Map map){
        
        inventory.select(BlockType.Block).select("Checkpoint").remove("Checkpoint").add("Turbo").replace(map);
        inventory.select(BlockType.Block).select("Turbo").remove("Turbo").add("Checkpoint").replace(map);
        inventory.select(BlockType.Block).select("Turbo2").remove("Turbo2").add("Checkpoint").replace(map);
        inventory.select(BlockType.Item).select("Checkpoint").remove(new[] {"Right","Left","Center","Checkpoint","v2"}).add("Turbo").replace(map);
        inventory.select(BlockType.Item).select("Turbo").add("Center").remove("Turbo").add("Checkpoint").replace(map);
        inventory.select(BlockType.Item).select("Turbo2").add("Center").remove("Turbo2").add("Checkpoint").replace(map);
        map.replace("GateSpecial4mTurbo","GateCheckpointCenter8mv2",new Position(new Vec3(2,0,0),Vec3.Zero));//untested
        map.placeStagedBlocks();
    }
}
class CPLess : Alteration{
    public override void run(Map map){
        map.delete(inventory.select("Checkpoint"));
    }
}
class STTF : Alteration{
    public override void run(Map map){
        map.delete(inventory.select("Checkpoint&(Ring|Gate)"));
        inventory.select(BlockType.Block).select("Checkpoint").remove("Checkpoint").replace(map);
        map.placeStagedBlocks();
    }
}

class CPFull : Alteration{
    public override void run(Map map){
        inventory.select(BlockType.Block).add("Checkpoint").replace(map);
        map.placeStagedBlocks();
    }
}

