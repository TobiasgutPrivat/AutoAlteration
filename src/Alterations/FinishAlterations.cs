using GBX.NET;

class FinishAlteration: Alteration {
    public Inventory finishArticles = inventory.select("Finish");
}
class OneUP: FinishAlteration {
    public override void Run(Map map){
        map.Move(finishArticles, new(new Vec3(0,8,0)));
    }
}
class TwoUP: FinishAlteration {
    public override void Run(Map map){
        map.Move(finishArticles, new(new Vec3(0,16,0)));
    }
}
class OneRight: FinishAlteration {
    public override void Run(Map map){
        map.Move(finishArticles, new(new Vec3(-32,0,0)));
    }
}
class OneLeft: FinishAlteration {
    public override void Run(Map map){
        map.Move(finishArticles, new(new Vec3(32,0,0)));
    }
}
class OneDown: FinishAlteration {
    public override void Run(Map map){
        map.Move(finishArticles, new(new Vec3(0,-8,0)));
    }
}