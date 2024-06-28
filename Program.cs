﻿using GBX.NET;
using GBX.NET.Engines.Game;
using Newtonsoft.Json;
//Initial load
//"C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/"
//"C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/"
Alteration.load("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");
// Alteration.inventory.checkDuplicates();//TODO
// Alteration.inventory.analyzeKeywords();

//Code for Execution (change for your use)
//Folder Processing
// string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2024/";
// string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/Spring 2024/";
// Alteration.alterFolder(new STTF(), sourcefolder, destinationFolder + "Spring 2024 STTF/", "STTF");

//Full Folder Processing
string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Auto Altered Nadeo/";
Alteration.alterAll(new FreeWheel(), sourcefolder, destinationFolder, "FreeWheel");

// //Single File Processing
string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test Template.Map.Gbx";
// string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Test CPBoost.Map.Gbx";
// Alteration.alterFile(new FreeWheel(), sourceFile, "FreeWheel");

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
        map.placeRelative(inventory.GetArticle("PlatformTechCheckpoint"),inventory.GetArticle("PlatformTechCheckpoint"),new Position(new Vec3(0,16,0),new Vec3(0,0,0)));

        Position position = new Position(new Vec3(800,100,900),new Vec3(0,0,0));
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
        map.placeStagedBlocks();
    }

    public void placeblock(Map map,string name,Vec3 position,Vec3 rotation) {
        CGameCtnBlock newBlock = map.map.PlaceBlock(name,new(0,0,0),Direction.North);
        newBlock.IsFree = true;
        newBlock.AbsolutePositionInMap = position;
        newBlock.PitchYawRoll = rotation;
    }
}