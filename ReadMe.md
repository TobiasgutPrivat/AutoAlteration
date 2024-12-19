<!--
TODO 
integrate already embedded Blocks
Translate PivotPosition to Postion (maybe solved)
Mark which Alterations differ from Altered Nadeo
Issue with Altering Blocks, not placeable
Issue EmbeddedBlocks names -> flatten or use full path
Turn Blocktypes into inheriting classes

Testing:
Automatedtests for singular parts + Full processes
TestScript for complex selected

Definitions/Documentation:
Programm Interface
Folder Structures
Trackmania Properties (How embedding, positioning etc. works )

Split AutoAlterations.cs into Folder Interface/System
AlterScript.cs AlterationScripts
AlterLogic.cs: Alter Map/files/folders
Settings.cs: All data like Keywords, Paths (Singleton)

Opt.:
Performance: improve KeywordFilter, Alignment 
Think about Multiple finishes for reverse/combined etc.
Multi Map Alterations (Combined,Repeat/Multilap)
apply on Nation Converter (check with BigBang)

Alteration Ideen:
kleine lücken (1m pro block nach aussen schieben)
-->
# Auto Alterations
Auto Alterations provides Functionality to automaticly create Trackmania Map-Alterations.

This Project is based on the GBX.NET Framework from BigBang (Nationsconverter Guy).

It's still in early development, so there will probably be some Bugs and Problems. Contribution to the repository is very welcome, feel free to create pull requests.

If you encounter problems or you have some questions/ideas etc. please contact me on Discord <strong>(Tobias2g#5288)</strong>

[Guide to implementing Alterations](https://docs.google.com/document/d/1h8qPXhsJ8d_mmbTFmXU-r2CUU0RKPhpXaWtdgJrJLeM/edit?usp=sharing)
## How to use
UI-App recommended for normal use.

[Download](https://1drv.ms/u/c/bf971998d3da6c52/EdpzD5D_ModDlt00VIA7MOwBXzTzdBbICkWemIKLqgChnw?e=hie2hq)

Also Alteration Scripts can be defined using json files. [Example](https://1drv.ms/u/c/bf971998d3da6c52/EQbNHWn08upIkWh903vYe6YBgC39OcYKtmoFAoxtys3Pmg?e=289yQq)

To have more control, or to develop some stuff you need to setup the full Project (see Installation)

### Available Alterations
Those are the currently available Alterations

(Some Alterations, are not fully what they are in Altered Nadeo)

<strong>Effect Alterations</strong>
- Cruise
- Fragile
- FreeWheel
- Glider
- NoBrake
- NoEffect
- NoSteer
- RandomDankness
- RandomEffects
- Reactor
- ReactorDown
- RedEffects
- RngBooster
- SlowMo

<strong>Finish Alterations</strong>
- OneBack
- OneForward
- OneDown
- OneLeft
- OneRight
- OneUP
- TwoUP
- Inclined
- ThereAndBack

<strong>Environment Alterations</strong>
- Stadium
- Snow
- Rally
- Desert

<strong>GameMode Alterations</strong>
- Race
- Stunt

<strong>Other Alterations</strong>
- AntiBooster
- Boosterless
- Broken
- Checkpointnt
- CPBoost
- CPFull
- CPLess
- CPLink
- CPsRotated
- Earthquake
- Fast
- Flipped
- Holes
- NoItems
- RandomBlocks
- RingCP
- SpeedLimit
- StartOneDown
- STTF
- Tilted
- Yeet
- YeetDown
- YeetMaxUp
- YepTree

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
2. Install Dotnet 8.0 sdk if you haven't: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
3. Clone this GitHub Repository into any Folder (you can use this Command in cmd)
> git clone https://github.com/TobiasgutPrivat/AutoAlteration.git /{path/to/your/directory}

<!--
There can be an issue with nuget source.
In that case make sure you have correct package source using:
> dotnet nuget add source https://api.nuget.org/v3/index.json
-->

# Documentation

## Note TODO Blocks

**requires**:
constructor from Item/Block
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
## To be documented:

## Trackmania circumstances
### Positioning systems
### Embeddings

## Definitions
### Data Structure AutoAlteration
- Customblocks
- Inventory
- DevLog

## Alteration Scripts

## Design Decisions

### Map Altering

### Positioning
- MoveChains

### Inventory
- Keyword Splitting
- ToKeywords

### Alteration implementations
- Helperfunctions

### Customblocks
- Embedding
- FolderStructure

### Program interface
