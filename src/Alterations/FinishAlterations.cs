using GBX.NET;
using GBX.NET.Engines.Game;

// class FinishAlteration: Alteration {
//     static string[] FinishBlock = new string[] {"PlatformTechFinish","RoadTechFinish","RoadDirtFinish","RoadBumpFinish","RoadIceFinish","RoadWaterFinish","PlatformTechFinish","PlatformDirtFinish","PlatformIceFinish","PlatformGrassFinish","PlatformPlasticFinish","PlatformWaterFinish"};
//     static string[] MultilapBlock = new string[] {"PlatformTechMultilap","RoadTechMultilap","RoadDirtMultilap","RoadBumpMultilap","RoadIceMultilap","RoadWaterMultilap","PlatformTechMultilap","PlatformDirtMultilap","PlatformIceMultilap","PlatformGrassMultilap","PlatformPlasticMultilap","PlatformWaterMultilap"};
//     static string[] FinishItem = new string[] {"GateFinish8m","GateFinish16m","GateFinish32m","GateFinishCenter8mv2","GateFinishCenter16mv2","GateFinishCenter32mv2"};
//     static string[] MultilapItem = new string[] {"GateFinish8m","GateFinish16m","GateFinish32m","GateFinishCenter8mv2","GateFinishCenter16mv2","GateFinishCenter32mv2"};
//     private static void moveFinish(Map map, Vec3 offset){
//         map.moveGroup( FinishBlock, offset, Vec3.Zero);
//         map.moveBlock(MultilapBlock, offset, Vec3.Zero);
//         map.moveBlock(FinishItem, offset, Vec3.Zero);
//         map.moveBlock(MultilapItem, offset, Vec3.Zero);

//         map.moveBlock("GateFinish", offset, Vec3.Zero);
//         map.moveBlock("GateExpandableFinish", offset, Vec3.Zero);
//         map.placeStagedBlocks();
//     }

class OneUP: Alteration {
    public static void OneUP(Map map){
        map.moveGroup("Finish", new Vec3(0,8,0), Vec3.Zero);
    }
}
class TwoUP: Alteration {
    public static void TwoUP(Map map){
        map.moveGroup("Finish", new Vec3(0,16,0), Vec3.Zero);
    }
}
class OneRight: Alteration {
    public static void OneRight(Map map){
        map.moveGroup("Finish", new Vec3(-32,0,0), Vec3.Zero);
    }
}
class OneLeft: Alteration {
    public static void OneLeft(Map map){
        map.moveGroup("Finish", new Vec3(32,0,0), Vec3.Zero);
    }
}
class OneDown: Alteration {
    public static void OneDown(Map map){
        map.moveGroup("Finish", new Vec3(0,-8,0), Vec3.Zero);
    }
}