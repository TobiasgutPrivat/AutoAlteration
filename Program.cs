using GBX.NET;
using GBX.NET.Engines.Game;
using Newtonsoft.Json;
//Initial load
//"C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/"
//"C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/"
// Alteration.load("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");

//Code for Execution (change for your use)
//Folder Processing
string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2024/";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Nadeo/Spring 2024/";
// Alteration.alterFolder(new CPBoost(), sourcefolder, destinationFolder + "Spring 2024 CPBoost/", "CPBoost");

// //Single File Processing
string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/My Maps/RC Racing.Map.Gbx";
// string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test CPBoost.Map.Gbx";
Alteration.alterFile(new Test(), sourceFile, "Test");

// float PI = (float)Math.PI;


// double[,] rotationMatrix = RotationMatrix.CreateRotationMatrix(0,Math.PI,0);//roll,pitch,yaw
// Console.WriteLine(RotationMatrix.GetEulerAngles(rotationMatrix));





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
        CGameCtnBlock baseBlock = map.map.PlaceBlock("PlatformTechCheckpoint",new(0,0,0),Direction.North);
        baseBlock.IsFree = true;
        baseBlock.AbsolutePositionInMap = new Vec3(700,60,700);
        baseBlock.PitchYawRoll = new Vec3(0,0,0);

        Vec3 pitchYawRoll = new Vec3(PI*0.5f,PI*0.5f,PI*0.5f);
        CGameCtnBlock Block1 = map.map.PlaceBlock("PlatformTechCheckpoint",new(0,0,0),Direction.North);
        Block1.IsFree = true;
        Block1.AbsolutePositionInMap = new Vec3(700,60,700);
        Block1.PitchYawRoll = pitchYawRoll;

        double [,] rotationMatrix = RotationMatrix.CreateRotationMatrix(pitchYawRoll.X,-pitchYawRoll.Y,-pitchYawRoll.Z);
        double [,] rotationMatrix2 = RotationMatrix.CreateRotationMatrix(0,0,PI*0.5f);
        double [,] result = RotationMatrix.MultiplyMatrices(rotationMatrix, rotationMatrix2);
        Vec3 eulerAngles = RotationMatrix.GetEulerAngles(result );
        Console.WriteLine(eulerAngles);

        CGameCtnBlock Block2 = map.map.PlaceBlock("PlatformTechCheckpoint",new(0,0,0),Direction.North);
        Block2.IsFree = true;
        Block2.AbsolutePositionInMap = new Vec3(700,60,700);

        Block2.PitchYawRoll = eulerAngles;

        // map.placeStagedBlocks();
    }
}