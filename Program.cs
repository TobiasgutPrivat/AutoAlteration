// using System.Numerics;
using System.Numerics;
using GBX.NET;
using Newtonsoft.Json;
//Initial load
//"C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/"
//"C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/"
// Alteration.load("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");

//Code for Execution (change for your use)
//Folder Processing
// string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2024/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/Spring 2024/";
// Alteration.alterFolder(new Snow(), sourcefolder, destinationFolder + "Spring 2024 Snow/", "Snow");

QuaternionLib q1 = RotationMatrixLib.ToQuaternion(-Math.PI*0.5,0,0);
Matrix4x4 matrix1 = Matrix4x4.CreateFromQuaternion(new Quaternion((float)q1.x,(float)q1.y,(float)q1.z,(float)q1.w));
QuaternionLib q2 = RotationMatrixLib.ToQuaternion(0,Math.PI*0.5,0);
Matrix4x4 matrix2 = Matrix4x4.CreateFromQuaternion(new Quaternion((float)q2.x,(float)q2.y,(float)q2.z,(float)q2.w));
Matrix4x4 matrix = Matrix4x4.Multiply(matrix1,matrix2);
Vector3 scale;
Quaternion rotation;
Vector3 translation;
Matrix4x4.Decompose(matrix,out scale,out rotation,out translation);
EulerAnglesLib angles = RotationMatrixLib.ToEulerAngles(new QuaternionLib(){ x = rotation.X, y = rotation.Y, z = rotation.Z, w = rotation.W });
Console.WriteLine(angles.roll + " " + angles.pitch + " " + angles.yaw);

//Full Folder Processing
string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Nadeo/Carswitch/Spring_2024_Snowcarswitch";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Nadeo/Carswitch/";
// Alteration.alterAll(new SnowCarswitchToRally(), sourcefolder, destinationFolder, "Rally");

// //Single File Processing
string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template.Map.Gbx";
// string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test CPBoost.Map.Gbx";
// Alteration.alterFile(new Test(), sourceFile, "Test");


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
        map.placeRelative("PlatformTechCheckpoint","RoadBumpDeadEnd_CustomBlock",new Position(new Vec3(0,0,0),new Vec3(0,0,0)).rotate(new Vec3(PI,0,0)));
        map.placeRelative("PlatformTechCheckpoint","RoadDirtDeadEnd_CustomBlock",new Position(new Vec3(0,0,0),new Vec3(0,0,0)).rotate(new Vec3(PI,0,0)).rotate(new Vec3(0,PI*0.3f,0)));
        map.placeRelative("PlatformTechCheckpoint","RoadIceDeadEnd_CustomBlock",new Position(new Vec3(0,0,0),new Vec3(0,0,0)).rotate(new Vec3(PI,0,0)).rotate(new Vec3(0,PI*0.3f,0)).rotate(new Vec3(0,0,PI*0.5f)));
        map.placeStagedBlocks();
    }
}