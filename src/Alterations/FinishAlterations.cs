using GBX.NET;

class OneUP: Alteration {
    public override void run(Map map){
        map.move(GetArticles("Finish"), new Vec3(0,8,0));
    }
}
class TwoUP: Alteration {
    public override void run(Map map){
        map.move(GetArticles("Finish"), new Vec3(0,16,0));
    }
}
class OneRight: Alteration {
    public override void run(Map map){
        map.move(GetArticles("Finish"), new Vec3(-32,0,0));
    }
}
class OneLeft: Alteration {
    public override void run(Map map){
        map.move(GetArticles("Finish"), new Vec3(32,0,0));
    }
}
class OneDown: Alteration {
    public override void run(Map map){
        map.move(GetArticles("Finish"), new Vec3(0,-8,0));
    }
}