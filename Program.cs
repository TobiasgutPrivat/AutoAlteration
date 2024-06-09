// using System.Numerics;
using GBX.NET;
using Newtonsoft.Json;
//Initial load
//"C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/"
//"C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/"
Alteration.load("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");

//Code for Execution (change for your use)
//Folder Processing
string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2024/";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Nadeo/Spring 2024/";
// Alteration.alterFolder(new CPBoost(), sourcefolder, destinationFolder + "Spring 2024 CPBoost/", "CPBoost");

// //Single File Processing
string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template.Map.Gbx";
// string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test CPBoost.Map.Gbx";
Alteration.alterFile(new STTF(), sourceFile, "STTF");
// Position position = new Position(new Vec3(0, 0, 0), new Vec3(0, 1, 0));
// Console.WriteLine(position.pitchYawRoll);
// position.addPosition(new Vec3(0, 0, 0), new Vec3(0, 0, 0));
// Console.WriteLine(position.pitchYawRoll);


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
        map.placeRelative("PlatformTechCheckpoint","PlatformTechCheckpoint",new Position(new Vec3(0,0,0),new Vec3(0,0,0)).rotate(new Vec3(PI,0,0)).rotate(new Vec3(0,PI*0.5f,0)));
        map.placeRelative("PlatformTechCheckpoint","PlatformTechCheckpoint",new Position(new Vec3(0,0,0),new Vec3(0,0,0)).rotate(new Vec3(PI,0,0)).rotate(new Vec3(0,PI*0.5f,0)).rotate(new Vec3(0,0,PI*0.5f)));
        map.placeStagedBlocks();
    }
}