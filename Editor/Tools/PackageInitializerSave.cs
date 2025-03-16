
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;


namespace CompilerDestroyer.Editor.EditorTools
{
    [Serializable]
    internal class Package
    {
        [SerializeField] internal string packageName;
        [SerializeField] internal bool shouldPackageInstalled;


        internal Package()
        {

        }
        internal Package(string name)
        {
            packageName = name;
            shouldPackageInstalled = true;
        }
    }

    [FilePath(GlobalVariables.PackageName + ".packageinitializer/packageinitializer.binary", FilePathAttribute.Location.PreferencesFolder)]
    internal class PackageInitializerSave : ScriptableSingleton<PackageInitializerSave>
    {

        internal List<Package> builtInPackages = new List<Package>
        {
            new Package("com.unity.ai.navigation"),
            new Package("com.unity.collab-proxy"),
            new Package("com.unity.ide.rider"),
            new Package("com.unity.ide.visualstudio"),
            new Package("com.unity.inputsystem"),
            new Package("com.unity.modules.accessibility"),
            new Package("com.unity.modules.ai"),
            new Package("com.unity.modules.androidjni"),
            new Package("com.unity.modules.animation"),
            new Package("com.unity.modules.assetbundle"),
            new Package("com.unity.modules.audio"),
            new Package("com.unity.modules.cloth"),
            new Package("com.unity.modules.director"),
            new Package("com.unity.modules.imageconversion"),
            new Package("com.unity.modules.imgui"),
            new Package("com.unity.modules.jsonserialize"),
            new Package("com.unity.modules.particlesystem"),
            new Package("com.unity.modules.physics"),
            new Package("com.unity.modules.physics2d"),
            new Package("com.unity.modules.screencapture"),
            new Package("com.unity.modules.terrain"),
            new Package("com.unity.modules.terrainphysics"),
            new Package("com.unity.modules.tilemap"),
            new Package("com.unity.modules.ui"),
            new Package("com.unity.modules.ulelements"),
            new Package("com.unity.modules.umbra"),
            new Package("com.unity.modules.unityanalytics"),
            new Package("com.unity.modules.unitywebrequest"),
            new Package("com.unity.modules.unitywebrequestassetbundle"),
            new Package("com.unity.modules.unitywebrequestaudio"),
            new Package("com.unity.modules.unitywebrequesttexture"),
            new Package("com.unity.modules.unitywebrequestwww"),
            new Package("com.unity.modules.vehicles"),
            new Package("com.unity.modules.video"),
            new Package("com.unity.modules.vr"),
            new Package("com.unity.modules.wind"),
            new Package("com.unity.modules.xr"),
            new Package("com.unity.multiplayer.center"),
            new Package("com.unity.render-pipelines.universal"),
            new Package("com.unity.test-framework"),
            new Package("com.unity.timeline"),
            new Package("com.unity.ugui"),
            new Package("com.unity.visualscripting")
        };
        [SerializeField] internal List<Package> customPackages = new List<Package>();
        [SerializeField] internal List<Package> assetStorePackages = new List<Package>();

        internal void SaveThis()
        {
            EditorUtility.SetDirty(this);
            Save(true);
        }
        internal string GetSavePath() => GetFilePath();
    }
}