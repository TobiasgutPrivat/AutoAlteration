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

### Available Alterations
Those are the currently available Alterations (for CLI Copy names from here)

<strong>Environment Alterations</strong>
- Stadium
- Snow
- Rally
- Desert

<strong>Effect Alterations</strong>
- NoBrake
- Cruise
- Fragile
- SlowMo
- NoSteer
- Glider
- Reactor
- ReactorDown
- FreeWheel
- Antibooster

<strong>Checkpoint Alterations</strong>
- STTF
- CPFull
- CPBoost

<strong>Finish Alterations</strong>
- OneUP
- OneDown
- OneLeft
- OneRight
- TwoUP

<strong>Other Alterations</strong>
- YepTree
- NoItems

## Known Issues
- The position of Items which are snapped onto something are read incorrectly, resulting in small offsets.
- Some Map Templates have different height offset when transforming to freeblock mode

## Contribution
You are welcome to contribute to the Project by creating Pull Requests on GitHub

For mature functionality or questions to your contribution please contact me on Discord (Tobias2g#5288)

Your Feedback or Ideas are also valueable, so you can write me that too.

A guide to implementing Alterations will come soon

# Temp
github Acces token: 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