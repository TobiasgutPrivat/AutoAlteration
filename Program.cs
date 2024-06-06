using GBX.NET;
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
string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template.Map.Gbx";
// string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test CPBoost.Map.Gbx";
// Alteration.alterFile(new Test(), sourceFile, "Test");

float PI = (float)Math.PI;
Position position = new Position(new Vec3(0, 0, 0), new Vec3(0, PI, 0));
position.addPosition(new Position(new Vec3(0, 0, 0), new Vec3(0, 0, 0)));
Console.WriteLine(position.pitchYawRoll.ToString());

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

        Article from = new("PlatformTechCheckpoint",BlockType.Block);
        Article to = new("PlatformTechCheckpoint",BlockType.Block);
        
        from.position.addPosition(new Position(new Vec3(0,32,0),new Vec3(0,0,0)));
        map.placeRelative(from,to,new Position(new Vec3(0,0,0),new Vec3(0,0,0)));
        from.position.addPosition(new Position(new Vec3(0,0,0),new Vec3(0,0,PI*0.5f)));
        map.placeRelative(from,to,new Position(new Vec3(0,0,0),new Vec3(0,0,0)));
        from.position.addPosition(new Position(new Vec3(0,32,0),new Vec3(0,0,0)));
        map.placeRelative(from,to,new Position(new Vec3(0,0,0),new Vec3(0,0,0)));
        map.placeStagedBlocks();
    }
}