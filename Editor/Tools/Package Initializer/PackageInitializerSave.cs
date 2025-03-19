
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;


namespace CompilerDestroyer.Editor.EditorTools
{

    [FilePath(GlobalVariables.PackageName + "/packageinitializersave.binary", FilePathAttribute.Location.PreferencesFolder)]
    internal class PackageInitializerSave : ScriptableSingleton<PackageInitializerSave>
    {
        [SerializeField] internal bool isPackageInitializerAlreadyRan = false;
        [SerializeField] internal bool isPackageInitializerEnabled = true;
        [SerializeField] internal List<Package> builtInPackages = new List<Package>
        {
            new Package("com.unity.ai.navigation", true),
            new Package("com.unity.collab-proxy", true),
            new Package("com.unity.ide.rider", true),
            new Package("com.unity.ide.visualstudio", true),
            new Package("com.unity.inputsystem", true),
            new Package("com.unity.modules.accessibility", true),
            new Package("com.unity.modules.ai", true),
            new Package("com.unity.modules.androidjni", true),
            new Package("com.unity.modules.animation", true),
            new Package("com.unity.modules.assetbundle", true),
            new Package("com.unity.modules.audio", true),
            new Package("com.unity.modules.cloth", true),
            new Package("com.unity.modules.director", true),
            new Package("com.unity.modules.imageconversion", true),
            new Package("com.unity.modules.imgui", true),
            new Package("com.unity.modules.jsonserialize", true),
            new Package("com.unity.modules.particlesystem", true),
            new Package("com.unity.modules.physics", true),
            new Package("com.unity.modules.physics2d", true),
            new Package("com.unity.modules.screencapture", true),
            new Package("com.unity.modules.terrain", true),
            new Package("com.unity.modules.terrainphysics", true),
            new Package("com.unity.modules.tilemap", true),
            new Package("com.unity.modules.ui", true),
            new Package("com.unity.modules.ulelements", true),
            new Package("com.unity.modules.umbra", true),
            new Package("com.unity.modules.unityanalytics", true),
            new Package("com.unity.modules.unitywebrequest", true),
            new Package("com.unity.modules.unitywebrequestassetbundle", true),
            new Package("com.unity.modules.unitywebrequestaudio", true),
            new Package("com.unity.modules.unitywebrequesttexture", true),
            new Package("com.unity.modules.unitywebrequestwww", true),
            new Package("com.unity.modules.vehicles", true),
            new Package("com.unity.modules.video", true),
            new Package("com.unity.modules.vr", true),
            new Package("com.unity.modules.wind", true),
            new Package("com.unity.modules.xr", true),
            new Package("com.unity.multiplayer.center", true),
            new Package("com.unity.render-pipelines.universal", true),
            new Package("com.unity.test-framework", true),
            new Package("com.unity.timeline", true),
            new Package("com.unity.ugui", true),
            new Package("com.unity.visualscripting", true)
        };
        [SerializeField] internal List<Package> customPackages = new List<Package>();
        [SerializeField] internal List<Package> assetStorePackages = new List<Package>();

        internal void Save()
        {
            EditorUtility.SetDirty(this);
            Save(true);
        }
        internal string GetSavePath() => GetFilePath();
    }


    [Serializable]
    internal class Package
    {
        [SerializeField] internal string packageName;
        [SerializeField] internal bool shouldPackageInstalled;


        internal Package()
        {

        }
        internal Package(string name, bool shouldPackageInstalled)
        {
            packageName = name;
            this.shouldPackageInstalled = shouldPackageInstalled;
        }
    }
}