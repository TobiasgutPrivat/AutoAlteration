using GBX.NET;

class Surfaceless: Alteration {
    public override void run(Map map){    
        map.delete(inventory.articles.Where(x => x.Surface != "").Select(x => x.Name).ToArray());
    }
}
class YepTree: Alteration {
    public override void run(Map map){    
        string[] Trees = inventory.select("Tree|Cactus|Fir|Palm|Cypress").names();
        Trees = Trees.Append("Spring").ToArray();
        Trees = Trees.Append("Summer").ToArray();
        Trees = Trees.Append("Winter").ToArray();
        Trees = Trees.Append("Fall").ToArray();
        map.placeRelative(Trees,"GateCheckpointCenter8mv2",new BlockChange(Vec3.Zero,Vec3.Zero));
        // map.placeRelative(Trees,"GateCheckpointCenter8mv2",new BlockChange(Vec3.Zero,new Vec3(PI*0.5f,0,0)));
        map.placeStagedBlocks();
    }
}