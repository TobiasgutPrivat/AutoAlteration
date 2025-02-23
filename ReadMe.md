<!--
TODO 
Mark which Alterations differ from Altered Nadeo
Fix Some Blocks wrong rotation -> Comments in Block.cs

Define Alteration Properties for Alterations
Apply Alteration Properties in UI

Rotation subtraction incorrect, testable using Tilted, but so far nowhere needed (probably rotation order issue)

Roadmap Customblock Alterations:
1. Issue with air mode
2. Issue Altering of EmbeddedBlocks don't appear ingame as embedded (can be done first)
   A Thought -> For Customblocks, it could be defined that the ones from AutoAlt do have unique names, and for mapspecific the Path would be included, mapspecific should not need to be matching to Vanilla Blocks
3. Opt. Solution for StaticModel-customblocks

Opt.:
Get WR for Map, extract releveant info (finish, cpOrder, nearest path rotation, Gearchanges)
Multiple Maps Alterations (like Combined, sections joined)
Make Configurable Alterations
apply on Nation Converter (check with BigBang)

Maybe make Keywords not case sensitive
-->
# Auto Alterations
Auto Alterations provides Functionality to automaticly create Trackmania Map-Alterations.

This Project is based on the GBX.NET Framework from BigBang (Nationsconverter Guy).

It's still in early development, so there will probably be some Bugs and Problems. Contribution to the repository is very welcome, feel free to create pull requests.

If you encounter problems or you have some questions/ideas etc. please contact me on Discord <strong>(Tobias2g#5288)</strong>

[Guide to implementing Alterations](https://docs.google.com/document/d/1h8qPXhsJ8d_mmbTFmXU-r2CUU0RKPhpXaWtdgJrJLeM/edit?usp=sharing)
## How to use
UI-App recommended for normal use.

[Download](https://1drv.ms/u/c/bf971998d3da6c52/Ec43uQOtQZdInmv-en32fmAB_-tn0nyNPD2hQr8z4424sw?e=ttExio)

Also Alteration Scripts can be defined using json files. [Example](https://1drv.ms/u/c/bf971998d3da6c52/EQbNHWn08upIkWh903vYe6YBgC39OcYKtmoFAoxtys3Pmg?e=289yQq)

To have more control, or to develop some stuff you need to setup the full Project (see Installation)

### Available Alterations
Those are the currently available Alterations

Submitting to Altered Nadeo still goes under their regulations (don't spam create AN Maps)

**Legend**:

Hover over Alterations to see Description

\* can differ from Altered Nadeo

<span style="color: gray;">gray:</span> can be incomplete

<strong>Effect Alterations</strong>
- <span style="color: gray;" title="places Cruise Effect on every Checkpoint (small offset to avoid skip)">Cruise*</span>
- <span style="color: gray;" title="places Fragile Effect on every Checkpoint (small offset to avoid skip)">Fragile</span>
- <span style="color: gray;" title="places Turbo and NoEngine Effect on every Checkpoint (small offset to avoid skip)">FreeWheel</span>
- <span style="color: gray;" title="places Yellow Reactor and NoEngine Effect on every Checkpoint (small offset to avoid skip)">Glider*</span>
- <span style="color: gray;" title="places NoBrake Effect on every Checkpoint (small offset to avoid skip)">NoBrake</span>
- <span style="color: gray;" title="replaces all Effects with their normal Block version">NoEffect</span>
- <span style="color: gray;" title="places NoSteering Effect on every Checkpoint (small offset to avoid skip)">NoSteer</span>
- <span title="replaces Checkpoint and Effect Blocks with random Effects">RandomDankness</span>
- <span title="replaces all Effects with random Effects">RandomEffects</span>
- <span title="places Red Reactor Effect on every Checkpoint">Reactor</span>
- <span title="places Red Reactor Effect on every Checkpoint (reactor rotated 180°)">ReactorDown</span>
- <span title="replaces all Effects with Red Turbo">RedEffects</span>
- <span title="replaces all Effects with RNG Turbo">RngBooster</span>
- <span style="color: gray;" title="places SlowMotion Effect on every Checkpoint (small offset to avoid skip)">SlowMo</span>

<strong>Environment Alterations</strong>
- <span title="adds Stadium Carswitch at Start and replaces all Carswitchgates">Stadium</span>
- <span title="adds Snow Carswitch at Start and replaces all Carswitchgates">Snow</span>
- <span title="adds Rally Carswitch at Start and replaces all Carswitchgates">Rally</span>
- <span title="adds Desert Carswitch at Start and replaces all Carswitchgates">Desert</span>

<strong>Finish Alterations</strong>
- <span title="moves the Finish one Tile back">OneBack</span>
- <span title="moves the Finish one Tile forward">OneForward</span>
- <span title="moves the Finish one Tile down">OneDown</span>
- <span title="moves the Finish one Tile to the left">OneLeft</span>
- <span title="moves the Finish one Tile to the right">OneRight</span>
- <span title="moves the Finish two Tiles up">TwoUP</span>
- <span title="tilt's the start and finish down">Inclined*</span>


<strong>Surface Alterations</strong>
- <span style="color: gray;" title="replaces all drivable surfaces with according Penalty surface">Penalty</span>
- <span style="color: gray;" title="replaces all roads with Bobsleigh">Bobsleigh</span>
- <span style="color: gray;" title="replaces all roads with Sausage">Sausage</span>
- <span style="color: gray;" title="removes all non-Pillar blocks, exccept for Start, Finish and Checkpoints">Surfaceless</span>


<strong>Other Alterations</strong>
- <span title="moves the Finish one Tile up">OneUP</span>
- <span title="Rotates all boosters and reactors by 180°">AntiBooster</span>
- <span title="Removes all boosters and reactors (replaces with base blocks)">Boosterless</span>
- <span title="Replaces all Effects with Engine Off">Broken</span>
- <span style="color: gray;" title="Blocks all Checkpoints with pillars">Checkpointnt</span>
- <span title="Swaps Boosters with Checkpoints">CPBoost</span>
- <span title="Replaces all Blocks with their checkpoint variant if available (Direction depends on original block)">CPFull*</span>
- <span title="Removes all boosters and reactors (replaces with base blocks)">CPLess</span>
- <span title="Links all CP's together">CPLink</span>
- <span title="Rotates all CP's by 90 degrees">CPsRotated</span>
- <span title="Moves the whole map by 1 million meters, making it feel like an earthquake">Earthquake</span>
- <span title="Replaces all checkpoints with red Turbo">Fast</span>
- <span title="Flips the whole map on its head">Flipped</span>
- <span title="Replaces all Blocks with their hole variant if available">Holes</span>
- <span title="Removes all Items">NoItems</span>
- <span title="Places some additional random Blocks (based on Blocks in the Map) with random Position">RandomBlocks</span>
- <span title="Replaces all CP's with a RingCP">RingCP</span>
- <span title="Deletes all Boosters and Reactors leaving Gaps">SpeedLimit</span>
- <span title="Moves the start 1 unit down">StartOneDown</span>
- <span title="Replaces all CP's with their normal Block-Variant">STTF</span>
- <span title="Tilt's the whole Map by a limited random Amount around all euler Angles">Tilted</span>
- <span title="Replaces all CP's with a Red Reactor">Yeet</span>
- <span title="Replaces all CP's with a Red Reactor rotated by 180°">YeetDown</span>
- <span title="Replaces all CP's with a Red Reactor, and moves the Finish up to the build limit">YeetMaxUp</span>
- <span title="Removes 10% of all Blocks">RandomHoles</span>
- <span title="Places a CP on every Tree">YepTree</span>
- <span title="Rotates all Blocks by 180°">Rotated</span>
- <span title="Spreads all Blocks apart by ~7%">Gaps</span>
- <span style="color: gray;" title="turns scenery into snow">SnowScenery*</span>

## Folder Application
Description how the new Maps are stored.

According to selected way:

**File**

Loads Map from selected {Source} (FilePath)

Saves altered Map to "{Destination}/{Previous FileName} {Name}.map.gbx"

**Folder**

Loads all Maps from selected {Source} (FolderPath, no subfolders)

Saves altered Maps to "{Destination}/{Previous FileName} {Name}.map.gbx"

**FullFolder**

Loads all Maps from selected {Source} (FolderPath, including subfolders)

Saves altered Maps to "{Destination}/{Previous Subfolder}/{SubfolderName} {Name}/{Previous FileName} {Name}.map.gbx"

## Alteration Scripts
For often used Alterations, scripts can be made.

**Store** in "%appdata%/AutoAlteration/scripts/" as *.json

**Format**:
>     [
>       {
>         "Type": "{Type: File/Folder/FullFolder}",
>         "Name": "{Name: used for Files/Mapname}",
>         "Source": "{Path to your existing File/Folder}",
>         "Destination": "{Path to your save Folder}",
>         "Alterations": [
>           "{Your Alteration}", //Names as defined previously
>           ... //further Alterations
>         ]
>       },
>       ... //further Configurations
>     ]

Has to follow json formatting rules

## Known Issues
- The position of Items which are snapped onto something are read incorrectly, resulting in small offsets.
- Some Map Templates have different height offset when transforming to freeblock mode

## Contribution
You are welcome to contribute to the Project by creating Pull Requests on GitHub

For mature functionality or questions to your contribution please contact me on Discord (Tobias2g#5288)

Your Feedback or Ideas are also valueable, so you can write me that too.

A guide to implementing Alterations will come soon

### Setup Project
1. Install git if you haven't: https://git-scm.com/downloads
2. Install Dotnet 8.0 sdk if you haven't: https://dotnet.microsoft.com/en-us/download/dotnet/8.0 (make sure nuget is available)
3. Clone this GitHub Repository into any Folder. You can use this Command in cmd:
> git clone https://github.com/TobiasgutPrivat/AutoAlteration.git /{path/to/your/directory}

<!--
There can be an issue with nuget source.
In that case make sure you have correct package source using:
> dotnet nuget add source https://api.nuget.org/v3/index.json
-->

# Technical Documentation
This Documentation contains
- Code Structure
- Data Strcucture/Definitions
- Some Process explanaitions

Further detailing Explanations are in code

## Note TODO Blocks

**requires**:
constructor from Item/Block, according to article Type
place into map

one base class to allow transfer as one list: 
Block: represent blocks which are already available (previously embedded/Vanilla Inventory)

Item(Block) for AnchoredObject: represent blocks which are already available in (previously embedded/Vanilla Inventory)

CustomBlock(Block) custom Blocks -> add path to constructor. requires embedding in map 
CustomItem(Item) custom Items -> add path to constructor. requires embedding in map

## Note TODO PositionChange
**constructor**: save Properties to move with

**Action**
input: Block, Article
modify: Article

## placing requirements Blocks


## Code Structure
Description of basic Purpose of all code-Files

### AutoAlteration.cs
Interface to main Alteration Functionality

### Alterations/
Contains All the Implementations of Map Alterations

Files are named according to Alteration Categories of Altered Nadeo

### Core/
Contains all surrounding Functionality

**AlterationConfig:** Global Configurations considering Datastructure, Keywords, etc. (Needs to be loaded before any Alteration)

**AlterationLogic:** Implements the Process of Altering Maps or Customblocks

**AlterationScript:** Represents a Script of Alteration executions, which can be loaded from json and be run.

**DevUtils:** Some Tools used during Develoment

**SingleOrList:** A Listrepresentation which can be created from a single instance or a List

### CustoBlocks/
Contains all about alteration of Customblocks

**CustomBlock:** Represents a Customblock/Item which can be altered

**CustomBlockAlteration:** Abstract definition of CustomBlock Alterations which can be applied on Customblocks

**CustomSurfaceAlteration:** Implementations of CustomBlock Alterations for surface changes

**CustomOtherAlteration:** Some Varying Implementations of CustomBlock Alterations

### Inventory/
Conatins All about available Articles and selection of them.

**Article:** Represents a Article (Block/Item) which can be placed in a Map. Contains Keyword indexing and matching (Keywords according to it's name) 

**ArticleImport:** Import of Articles available in Trackmania 2020

**Inventory:** A set of Articles, allows the Selection of Articles based on Keywords (The full/base Inventory with all available Articles including customblocks is in Alteration.inventory)

**InventoryChange:** Changes which will can be applied on an Inventory (Including Base, but not during an Alteration)

**KeywordEdit:** A copy of an Inventory with cloned Articles, used to align to Articles with changed set of Keywords (change Keywords within Articles -> see which Articles originaly had those Keywords -> use Alignment to place/replace)

### Maps/
Contains fundamental functionality for the Alteration of Maps

**Alteration:** Abstract defintion of Alterations which can be applied on Maps

**Block:** Represents a Placed Block, although not actually placed in a Map but used for staging before getting placed in.

**Map:** Represents a Map which can loaded, altered and saved. Provides a range of functionality for modifications, often using Inventory (wrapping an actual map)

### Positioning/
Contains all functionality for processing of Positions (coords and rotation)

**Move:** Diffrent Implementations of modifications which can be applied on a Position (sometimes with Articleinfo)

**MoveChain:** A chain of diffrent Moves, which can be applied on a position

**Position:** Represents a Position of a Block (coords and rotation)

**PosUtils:** some Functions for easier creation of Movechains (used in Alteration and InventoryChange)

## Data Structure

### Global Data
In CommonApplicationData ("C:\ProgramData\AutoAlteration\data")
DataFolder modifyable per DevMode, DevPath (for Development)

**CustomBlocks/:** Base Folder for customblocks/Items which can be imported into Inventory (Inventorychange: CustomBlockFolder)

- **CustomBlocks/Vanilla:** Folder with all vanilla blocks/items as customblocks/items

**Inventory/:** Data defining Keywords and vanilla Articles

- **Inventory/BlockData:** Vanilla Articles in json format: [{Height,Width,Length,type,Name,Theme,Defaultrotation}]

- **Inventory/Keywords:** most Keywords used to split up Article Names (Orderd by Length)

- **Inventory/KeywordsStart:** Keywordswhich should be used first

**LowCubeLayer:** used for invisible blocks

**IceSkin:** used for SnowScenery Wall skin

**script/:** place to store Alteration scriptfiles

**dev/** some data exports for debugging

### User Data
in ApplicationData ("%appdata%/Autoalteration/")
Data which can vary depending on Usage

**Keywords/KeywordsStart:** here additional Keywords can be defined

**CustomBlockSets:** If a customblock set is needed it get's generated into it's Folder AutoAlteration/{SetName}/

## Processes (To be documented)

### Map Alteration
From Map file to altered Map file (AutoAlteration.AlterFile)

1. Load Map using Map constructor

2. Call Alteration Process AlterationLogic.Alter

3. Clear embedded Blocks from last Map ("mapSpecific") from Folder and Inventory

4. If the set of alterations differs from last time, regenerate the Inventory
    1. Recreate Inventory from Vanilla Blockdata
    2. Apply Inventorychanges from all Alterations
    3. Apply some general modifications on Inventory for better Keyword Indexing

5. Add already embedded Customblocks to Inventory

6. For all customblockSets in Alterations, generate Set and add to Inventory for already embedded Blocks (Extracts to Temp Folder) 

7. Apply Alterations on Map (alteration.Run)

8. save map with new Name

### CustomBlock Alteration

### Inventory Selection

### InventoryEdit Alignment

## Trackmania circumstances

### Block Placement

**Vanilla Blocks**
- Class: CGameCtnBlock
- Name (blockModel): Name of Block in game
- Modes: normal (in grid), free, (is defined by block.isFree)
  - Normal: block.Coord defines position in grid (factor: (32m, 8m, 32m) ), block.Direction defines orientation as Cardinal Directions
  - Free: 
    - block.AbsolutePositionInMap: defines position as coordinates, 
    - block.PitchYawRoll: defines rotation as Vector (X=Yaw, Y=Pitch, Z=Roll)
      
      application order: 1. Yaw, 2. Roll, 3. Pitch

- Auto Alteration always places as Freeblock
- Some additional Properties: IsGhost, IsClip, Color, Skin, Bit21 (aka. IsAir)
  
  Per default they are just transfered from original block in Auto Alteration

**Vanilla Items**
- Class: CGameCtnAnchoredObject
- Name (Id): Name of Item in game
- Collection: here fixed to TM2020 (new Id(26))
- Author: here fixed to Nadeo
- item.AbsolutePositionInMap: defines position as coordinates, 
  - item.PitchYawRoll: defines rotation as Vector (X=Yaw, Y=Pitch, Z=Roll)
    
    application order: 1. Yaw, 2. Roll, 3. Pitch

- Some additional Properties: Color, Scale

**Custom Blocks**
- Requires block to be embedded in the Map
  - EntryName: "Blocks/" + block.name + ".Block.Gbx"
- Name (blockModel): block.name + ".Block.Gbx_CustomBlock";
- block.name can be a Path with Folders *Note
- Everything else: like normal Blocks

**Custom Items**
- Requires item to be embedded in the Map
  - EntryName: "Items/" + item.name + ".Item.Gbx"
- Name (Id): item.name + ".Item.Gbx"
- item.name can be a Path with Folders *Note
- Everything else: like normal Items

**\*Note CustomBlock Paths**
Usually when embedding customblocks they can have a full path.

In this Project only the Name is used for indexing and when embedding new Blocks, for simplicity reasons.

**Potential Problem** this can cause issues if two blocks have the same name but diffrent Path