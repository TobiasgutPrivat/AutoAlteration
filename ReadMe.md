# Auto Alterations
Auto Alterations provides Functionality to automaticly create Trackmania Map-Alterations.

This Project is based on the GBX.NET Framework from BigBang (Nationsconverter Guy).

It's still in early development, so there will probably be some Bugs and Problems. Contribution to the repository is very welcome, feel free to create pull requests.

If this Instruction leaves some Questions or you have some ideas contact me on Discord (Tobias2g#5288)
## How to use
Some basic knowledge of Coding will be usefull but not required.

### Installation
1. Clone this GitHub Repository
2. You need some form of IDE, I recommend VSCode with C# Dev Kit 
3. You need Dotnet 7.0 and Nuget installed, also make sure to have correct package source using (dotnet nuget add source https://api.nuget.org/v3/index.json) in Console
4. After installation you can write your code in Program.cs and execute it with F5

### Usage
The Tool can be used by writting Code in Program.cs

#### Load
First the Inventory needs to be loaded with
> Alteration.load("currentProjectFolder");

for example

> Alteration.load("C:/Users/Tobias/Documents/Programmieren/GBX Test/AutoAlteration/");

#### Alteration
There are multiple Functions for Alterations in the AutoAlteration class

Example Alter All Maps in a Folder to FreeWheel:
> string sourcefolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Nadeo Maps/Spring 2024/";\
> string destinationFolder = "C:/Users/Tobias/Documents/Trackmania2020/Maps/Altered Nadeo/Spring 2024/Spring 2024 FreeWheel/";\
> AutoAlteration.alterFolder(new FreeWheel(), sourcefolder, destinationFolder, "FreeWheel");

sourceFolder: the Folder the Maps are in

destinationfolder: the Folder the Maps will be saved in

new FreeWheel(): declaration that the FreeWheel Alteration should be applied

Name: The Name that will be put at the end of the Map- and Filename

#### Multiple Alterations
You can apply Multiple Alterations at once by making a list of them like this:

> new List<Alteration>{new anAlteration(), new otherAlterations(), ...}

This for example alterates the maps with FreeWheel and OneUp:
>AutoAlteration.alterFolder(new List<Alteration>{new FreeWheel(), new OneUP()}, sourcefolder, destinationFolder, "FreeWheel (1-UP)");

### Implementation of Alterations
You can freely make new Alterations, your also asked to share them (see Contribution below) This will require some basic 

The Alterations can be edited or new ones can be created within the "Alterations" Folder

They should be in a reasonable called class and need to inherit the class Alteration and implement the run(Map map) function

Example for OneUp:
> class OneUP: Alteration {\
>    public override void run(Map map){\
>        map.move(Blocks.select("Finish"), new Vec3(0,8,0));\
>    }
>}

Within this Function you can use the Functions provided in the Map to alter it.
\
Here are some basic uses explained:

Generally BlockModelId is required to identify Blocks

**Delete** all Blocks off one type
> map.delete(yourBlockModelId);

This doesn't delete the Platform Blocks below it.

Example: delete all CheckpointRings:
> map.delete("GateCheckpoint");

**PlaceRelativ** : places a Block positioned relative to all Blocks off one Type
> map.placeRelative(BlockToAddTo,new BlockChange(BlockType, newBlock, positionOffset, rotationOffset));

The Blockchange defines the new Block to be placed.

Example: place a BoostRing at every RoadTechCheckpoint with an offset of 1m forwards
> map.placeRelative("RoadTechCheckpoint", "GateSpecialTurbo", new BlockChange(new Vec3(0,-12,forwardOffset),rotation));

Any placed Blocks get staged and need to be placed later by calling:

> map.placeStagedBlocks();

**Replace** uses placeRelative and delete to replace the specified Blocks

Example: replace a RoadTechCheckpoint with a Booster
> map.replace("RoadTechCheckpoint",new BlockChange("RoadTechSpecialTurbo"));

#### Block Selction

There is also the option to select Blocks by Keywords

Example place a Turboring at every CP or Multilap Block:

> map.placeRelative(Blocks.select("Checkpoint|Multilap"), "GateSpecialTurbo");

**Further Functionality** : please look at other Alterations how to do it.

**Helper** 
\
https://github.com/TobiasgutPrivat/Auto-Alteration-Helper
\
I also made a Plugin to get the selected BlockModelId in editor (Also there will be more functionality added to it)
\
To install: Clone it into your Plugin Folder (Its not released yet)

## Known Issues/Bugs
- When deleting Blocks the base Platform stays there. (Good for replacing but bad for CPLess)
- Some Issue With incorrect Relative Offset in complex Rotations

## Contribution
You are welcome to contribute to the Project by creating Pull Requests on GitHub

For mature functionality or questions to your contribution please contact me on Discord (Tobias2g#5288)

Your Feedback or Ideas are also valueable, so you can write me that too.
