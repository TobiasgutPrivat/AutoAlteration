using System.Numerics;
using GBX.NET;
using GBX.NET.Engines.Game;
using MathNet.Numerics.LinearAlgebra;
using Newtonsoft.Json;
//Initial load
//"C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/"
//"C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/"
// Alteration.load("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");
// Alteration.inventory.checkDuplicates();//TODO
// Alteration.inventory.analyzeKeywords();

//Code for Execution (change for your use)
//Folder Processing
string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2024/";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/Spring 2024/";
// Alteration.alterFolder(new STTF(), sourcefolder, destinationFolder + "Spring 2024 STTF/", "STTF");

//Full Folder Processing
// string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Nadeo/Carswitch/Spring_2024_Snowcarswitch";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Nadeo/Carswitch/";
// Alteration.alterAll(new SnowCarswitchToRally(), sourcefolder, destinationFolder, "Rally");

// //Single File Processing
string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template.Map.Gbx";
// string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test CPBoost.Map.Gbx";
// Alteration.alterFile(new Flipped(), sourceFile, "Flipped");

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
        // EffectAlteration.newplaceCPEffect(map,"Turbo");
        map.map.PlaceBlock("PlatformTechCheckpoint", new Int3(48,38,48), Direction.North);
        map.map.PlaceBlock("PlatformTechCheckpoint", new Int3(1,9,1), Direction.North);
        map.placeStagedBlocks();
    }

    public void placeblock(Map map,string name,Vec3 position,Vec3 rotation) {
        CGameCtnBlock newBlock = map.map.PlaceBlock(name,new(0,0,0),Direction.North);
        newBlock.IsFree = true;
        newBlock.AbsolutePositionInMap = position;
        newBlock.PitchYawRoll = rotation;
    }
}