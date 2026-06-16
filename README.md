# Efficient Questing
[![Latest Release](https://img.shields.io/github/v/release/DashTheDev/7D2D-EfficientQuesting)](https://github.com/DashTheDev/7D2D-EfficientQuesting/releases/latest)
<br>
![7D2D v2.6](https://img.shields.io/badge/7D2D-v2.6-brightgreen)
<br>
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A [7 Days to Die](https://store.steampowered.com/app/251570/7_Days_to_Die/) mod that adds a collection of quality of life improvements to questing.

- Click a quest icon on the map to set it as your tracked quest
- Automatically tracks the next closest quest on quest advanced, completed or failed

## Installation
Download the [latest release](https://github.com/DashTheDev/7D2D-EfficientQuesting/releases/latest), unzip and drop the `EfficientQuesting` folder into your game's `Mods` directory.

> [!NOTE]  
> Users only need to install this mod on the client!

## Development Setup
1. Ensure you have [Visual Studio](https://visualstudio.microsoft.com/) and [7D2D](https://store.steampowered.com/app/251570/7_Days_to_Die/) installed
2. Clone the repository
3. Create a `Local.props` file in the root of the project with the following content:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    <PropertyGroup>
        <!-- Insert game path here (i.e Z:\SteamLibrary\steamapps\common\7 Days To Die) -->
        <GamePath>YOUR_GAME_PATH_GOES_HERE</GamePath>
    </PropertyGroup>
</Project>
```

4. Open the solution in [Visual Studio](https://visualstudio.microsoft.com/) and build.