# Auto Alterations
Auto Alterations provides Functionality to automaticly create Trackmania Map-Alterations.

This Project is based on the GBX.NET Framework from BigBang.

It's still in early development, so there will probably be some Bugs and Problems. Contribution to the repository is very welcome, feel free to create pull requests.

If this Instruction leaves some Questions or you have some ideas contact me on Discord (Tobias2g#5288)
## How to use
Some basic knowledge of Coding will be usefull but not required.

### Installation
1. Clone this GitHub Repository
2. You need some form of IDE, I recommend VSCode with C# Dev Kit 
3. After installation you can write your code in Program.cs and execute it with F5

### Usage
For Usage all Code should be written in Program.cs

To alterate a map you need a object of the class "Map"
> Map yourMapVar = new Map("SomeFolder/YourMap.Map.Gbx");

You can apply mutiple Alterations to your map like that:
> EffectAlterations.FreeWheel(yourMapVar);
\
> CPAlterations.STTF(yourMapVar);

All the Alterations are implemented in the "Alterations" Folder in the Project Folder.

Then you can save your map like that:

> yourMapVar.save("SomeFolder/YourMap Altered.Map.Gbx");

Here is an example on how to alter all maps in a Folder to FreeWheel:

>string sourcefolder = "C:/Users\\Tobias\\Documents\\Trackmania2020\\Maps\\Nadeo Maps\\Summer 2020\\";
\
>string destinationFolder = "C:/Users\\Tobias\\Documents\\Trackmania2020\\Maps\\My-Maps\\Altered Nadeo\\Summer 2020 FreeWheel\\";
\
>string[] files = Directory.GetFiles(sourcefolder);
\
>foreach (string file in files)
\
>{
\
>    Map map = new Map(file);
\
>    EffectAlterations.FreeWheel(map);
\
>    map.map.MapName = Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 8) + " FreeWheel";
\
>    map.save(destinationFolder + Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 8) + " 
\
>    FreeWheel.map.gbx");
\
>}

### Alterations
You can freely make new Alterations, your also asked to share them (see Contribution below)

The Alterations can be edited or new ones can be created within the "Alterations" Folder
\
They should be in a reasonable called class and need to be public statis Functions with a map Parameter, like that:
> public static void FreeWheel(Map map){}

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
> map.placeRelative("RoadTechCheckpoint", new BlockChange("GateSpecial" + Effect,new Vec3(0,-12,forwardOffset),rotation));

Any placed Blocks get staged and need to be placed later by calling:

> map.placeStagedBlocks();

**Replace** uses placeRelative and delete to replace the specified Blocks

Example: replace a RoadTechCheckpoint with a Booster
> map.replace("RoadTechCheckpoint",new BlockChange("RoadTechSpecialTurbo"));

**Further Functionality** : please look at other Alterations how to do it.

**Helper** 
\
https://github.com/TobiasgutPrivat/Auto-Alteration-Helper
\
I also made a Plugin to get the selected BlockModelId in editor (Also there will be more functionality added to it)
\
To install: Clone it into your Plugin Folder (Its not released yet)

## Known Issues/Bugs
- Probably some Alterations have incorrect Configurations
- A weird Problem when deleting boosters causing the map to not be loadable
- When deleting Blocks the base Platform stays there. (Good for replacing but bad for CPLess)
- Everything marked with //TODO in the Project
## Contribution
You are welcome to contribute to the Project by creating Pull Requests on GitHub

For mature functionality or questions to your contribution please contact me on Discord (Tobias2g#5288)

Your Feedback or Ideas are also valueable, so you can write me that too.
