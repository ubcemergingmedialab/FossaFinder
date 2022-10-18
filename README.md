# FossaFinder 

This is a WebGL build, currently without any VR features.  You can toggle on/off the SceneData that shows up setting the 'Display Scene Data Info' checkbox in the `GuidedYourManager` Game Object.

## Prerequisites
1. Unity v2018.2.15f
2. Node npx (suggested)

## Getting Started
1. Clone the repository
2. Open the project.  The scene that is in active development is `Main`.

## Testing WebGL builds

If you want to test features for the actual target (WebGL), there are multiple options after building the project (see below).  Do note that certain plugins (such as Oculus SDK) will not work with WebGL, and have been turned off using Web Assembly Definitions.

### Node
1. Go to the Builds folder, open up a terminal and do `npx http-server`.  
2. This will spin up a local server and you can copy/paste the URL to see your changes.
3. CTRL + c, to stop the service.

### Unity
In the project, go to File -> Build And Run.

***application information***

| Dependency | Version     | Description                           |
|------------|-------------|---------------------------------------|
| Unity      | v2018.2.15f | Game development platform             |
| SteamVR    | v1.2.3      | SDK for Vive and Microsoft MR support |
| Oculus     | v20.0       | SDK for Oculus support                |
| VRTK       | v3.3.0      | General-purpose VR SDK                |

Requires whitney font

Includes [VRTK](https://vrtoolkit.readme.io/) and [SteamVR](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647)
