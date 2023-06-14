# Project Spirare Immersal Geo Location Updater

Project Spirare Immersal Geo Location Updater is a utility that enables you to update the geo-location details of an Immersal map.  
This tool is designed to be used in combination with SpirareEditor.

## Prerequisites

- [SpirareEditor](https://github.com/HoloLabInc/spirare-babylonjs) (version 1.1.0 or later)
- .NET SDK 6.0 or later (If you want to build the project)

## Build

Build the project using the .NET CLI. In your terminal, navigate to the project directory and run the following command:

```
dotnet publish -c Release
```

This will create a Release build of the application.

## Usage

### Download the Immersal map model

Download the glb model of your map from [Immersal Developer Portal](https://developers.immersal.com).

### Place the model with

Place the glb model in the appropriate position using the SpirareEditor.

<img width="640" alt="Place the model in SpirareEditor" src="https://github.com/HoloLabInc/ProjectSpirare-ImmersalGeoLocationUpdater/assets/4415085/eb9a9bc9-f91d-4730-bd18-b86940bb3aaf">

In the Inspector, you can get the following information:

- latitude
- longitude
- ellipsoidal height
- rotation (x y z w)

### Run the tool

Execute the following command using the information obtained from SpirareEditor.

```
.\ImmersalGeoLocationUpdater.exe <Immersal developer token> <map id> <latitude> <longitude> <ellipsoidal height> <rotation x> <rotation y> <rotation z> <rotation w>
```

## License

MIT
