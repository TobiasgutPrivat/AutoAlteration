using GBX.NET;

class Surfaceless: Alteration {
    public override void run(Map map){    
        map.delete(inventory.articles.Where(x => x.Surface != "").Select(x => x.Name).ToArray());
    }
}