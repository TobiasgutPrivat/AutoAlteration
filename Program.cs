//Initial load
Alteration.load("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/"); //Path to this project folder

//Code for Execution (change for your use)

//Folder Processing
string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2024/";
string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Nadeo/Spring 2024/";
AutoAlteration.alterFolder(new FreeWheel(), sourcefolder, destinationFolder + "Spring 2024 FreeWheel temp/", "FreeWheel");
// AutoAlteration.alterFolder(new OneUP(), sourcefolder, destinationFolder + "Spring 2024 1-UP/", "1-UP");

// //Single File Processing
// string sourceFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2020 alpha/Spring 2020 - S12.map.gbx";
// string destinationFile = "C:/Users/Tobias/Documents/Trackmania2020/Maps/My-Maps/Altered Nadeo/Spring 2020 CPFull/t01 Spring 2020 CPFull.map.gbx";
// AutoAlteration.alterFile(new FreeWheel(), sourceFile, destinationFile, "FreeWheel");