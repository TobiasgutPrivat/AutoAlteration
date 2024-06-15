// using System.Numerics;
using System.Numerics;
using GBX.NET;
using GBX.NET.Engines.Game;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Newtonsoft.Json;
//Initial load
//"C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/"
//"C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/"
Alteration.load("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");

//Code for Execution (change for your use)
//Folder Processing
string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2024/";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/Spring 2024/";
// Alteration.alterFolder(new STTF(), sourcefolder, destinationFolder + "Spring 2024 STTF/", "STTF");

//Full Folder Processing
// string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Nadeo/Carswitch/Spring_2024_Snowcarswitch";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Nadeo/Carswitch/";
// Alteration.alterAll(new SnowCarswitchToRally(), sourcefolder, destinationFolder, "Rally");

// Vec3 pithYawRoll = new Vec3(1,1,1);
//  double Roll = 1;
// Matrix<double> RotationX = DenseMatrix.OfArray(new double[,] {
//     { 1, 0, 0},
//     { 0, Math.Cos(Roll), -Math.Sin(Roll)},
//     { 0, Math.Sin(Roll), Math.Cos(Roll)},
// });
// Console.WriteLine(RotationX);
// Console.WriteLine(RotationX[1,2]);
// Matrix4x4 rotationMatrix4x4 = Matrix4x4.CreateRotationX(pithYawRoll.Z);

// Console.WriteLine(rotationMatrix4x4);



// //Single File Processing
string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template.Map.Gbx";
// string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test CPBoost.Map.Gbx";
Alteration.alterFile(new Test(), sourceFile, "Test");

//Development Section -----------------------------------------------------------------------------------------------------------------------
void testBlock(string Name) {
    Map map = new Map("C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template.Map.Gbx");
    map.map.PlaceBlock(Name, new Int3(10, 10, 10), Direction.North);
    map.map.PlaceAnchoredObject(new Ident(Name, new Id(26), "Nadeo"), new Int3(320, 80, 320), new Vec3(0,0,0));
    map.save("C:/Users/Tobias/Documents/Trackmania2020/Maps/Test.Map.Gbx");
}
void stringToName(string projectFolder) {
    string json = File.ReadAllText(projectFolder + "src/Vanilla/Items.json");
    string[] lines = JsonConvert.DeserializeObject<string[]>(json);
    string[] articles = lines.Select(line => line.Split('/')[line.Split('/').Length-1].Trim()).ToArray();
    json = JsonConvert.SerializeObject(articles);
    File.WriteAllText(projectFolder + "src/Vanilla/ItemNames.json", json);
}

class Test : Alteration {
    public override void run(Map map){
        float PI = (float)Math.PI;

        map.placeRelative("PlatformTechCheckpoint","PlatformTechCheckpoint",new Position(new Vec3(0,0,0),new Vec3(0,0,0)).rotate(new Vec3(PI,0,0)));
        map.placeRelative("PlatformTechCheckpoint","PlatformTechCheckpoint",new Position(new Vec3(0,0,0),new Vec3(0,0,0)).rotate(new Vec3(PI,0,0)).rotate(new Vec3(0,0,PI*0.5f)));
        map.placeRelative("PlatformTechCheckpoint","PlatformTechCheckpoint",new Position(new Vec3(0,0,0),new Vec3(0,0,0)).rotate(new Vec3(PI,0,0)).rotate(new Vec3(0,0,PI*0.5f)).rotate(new Vec3(0,PI*0.3f,0)));
        map.placeRelative("PlatformTechCheckpoint","PlatformTechCheckpoint",new Position(new Vec3(0,0,0),new Vec3(0,0,0)).rotate(new Vec3(PI,0,0)).rotate(new Vec3(0,0,PI*0.5f)).move(new Vec3(0,0,32)).rotate(new Vec3(0,PI*0.3f,0)));
        map.placeRelative("PlatformTechCheckpoint","PlatformTechCheckpoint",new Position(new Vec3(0,0,0),new Vec3(0,0,0)).rotate(new Vec3(PI,0,0)).rotate(new Vec3(0,0,PI*0.5f)).move(new Vec3(0,0,32)).rotate(new Vec3(0,PI*0.3f,0)).rotate(new Vec3(PI*0.5f,0,0)));

        Position position = new Position(new Vec3(800,100,800),new Vec3(0,0,0));
        placeblock(map,"PlatformTechCheckpoint",position.coords,position.pitchYawRoll);
        position.rotate(new Vec3(PI,0,0));
        placeblock(map,"PlatformTechCheckpoint",position.coords,position.pitchYawRoll);
        position.move(new Vec3(0,-16,0));
        placeblock(map,"PlatformTechCheckpoint",position.coords,position.pitchYawRoll);
        position.rotate(new Vec3(0,-PI*0.5f,0));
        placeblock(map,"PlatformTechCheckpoint",position.coords,position.pitchYawRoll);
        position.move(new Vec3(0,-16,0));
        placeblock(map,"PlatformTechCheckpoint",position.coords,position.pitchYawRoll);
        position.rotate(new Vec3(0,0,PI*0.5f));
        placeblock(map,"PlatformTechCheckpoint",position.coords,position.pitchYawRoll);
        position.move(new Vec3(0,-16,0));
        placeblock(map,"PlatformTechCheckpoint",position.coords,position.pitchYawRoll);
        // placeblock(map,"PlatformTechCheckpoint",new Vec3(800,100,800),new Vec3(0,0,0));
        // placeblock(map,"PlatformTechCheckpoint",new Vec3(800,100,800),new Vec3(PI,0,0));
        // placeblock(map,"PlatformTechCheckpoint",new Vec3(800,100,800),new Vec3(PI,PI*0.3f,0));
        // placeblock(map,"PlatformTechCheckpoint",new Vec3(800,100,800),new Vec3(PI,PI*0.3f,PI*0.5f));

        // placeblock(map,"PlatformTechCheckpoint",new Vec3(900,100,900),new Vec3(0,0,0));
        // placeblock(map,"PlatformTechCheckpoint",new Vec3(900,100,900),new Vec3(0,0,PI));
        // placeblock(map,"PlatformTechCheckpoint",new Vec3(900,100,900),new Vec3(0,PI*0.3f,PI));
        // placeblock(map,"PlatformTechCheckpoint",new Vec3(900,100,900),new Vec3(PI*0.5f,0,PI));
        map.placeStagedBlocks();
    }

    public void placeblock(Map map,string name,Vec3 position,Vec3 rotation) {
        CGameCtnBlock newBlock = map.map.PlaceBlock(name,new(0,0,0),Direction.North);
        newBlock.IsFree = true;
        newBlock.AbsolutePositionInMap = position;
        newBlock.PitchYawRoll = rotation;
    }
}