using GBX.NET;
using GBX.NET.Engines.Game;

class FinishAlterations {
    static string[] FinishBlock = new string[] {"PlatformTechFinish","RoadTechFinish","RoadDirtFinish","RoadBumpFinish","RoadIceFinish","RoadWaterFinish","PlatformTechFinish","PlatformDirtFinish","PlatformIceFinish","PlatformGrassFinish","PlatformPlasticFinish","PlatformWaterFinish"};
    static string[] MultilapBlock = new string[] {"PlatformTechMultilap","RoadTechMultilap","RoadDirtMultilap","RoadBumpMultilap","RoadIceMultilap","RoadWaterMultilap","PlatformTechMultilap","PlatformDirtMultilap","PlatformIceMultilap","PlatformGrassMultilap","PlatformPlasticMultilap","PlatformWaterMultilap"};
    static string[] FinishItem = new string[] {"GateFinish8m","GateFinish16m","GateFinish32m","GateFinishCenter8mv2","GateFinishCenter16mv2","GateFinishCenter32mv2"};
    static string[] MultilapItem = new string[] {"GateFinish8m","GateFinish16m","GateFinish32m","GateFinishCenter8mv2","GateFinishCenter16mv2","GateFinishCenter32mv2"};
    private static void moveFinish(Map map, Vec3 offset){
        map.moveBlock(FinishBlock, offset, Vec3.Zero);
        map.moveBlock(MultilapBlock, offset, Vec3.Zero);
        map.moveItem(FinishItem, offset, Vec3.Zero);
        map.moveItem(MultilapItem, offset, Vec3.Zero);

        map.moveBlock("GateFinish", offset, Vec3.Zero);
        map.moveBlock("GateExpandableFinish", offset, Vec3.Zero);
        map.placeStagedBlocks();
    }

    public static void OneUP(Map map){
        moveFinish(map, new Vec3(0,8,0));
    }
    public static void TwoUP(Map map){
        moveFinish(map, new Vec3(0,16,0));
    }
    public static void OneRight(Map map){
        moveFinish(map, new Vec3(-32,0,0));
    }
    public static void OneLeft(Map map){
        moveFinish(map, new Vec3(32,0,0));
    }
    public static void OneDown(Map map){
        moveFinish(map, new Vec3(0,-8,0));
    }
}