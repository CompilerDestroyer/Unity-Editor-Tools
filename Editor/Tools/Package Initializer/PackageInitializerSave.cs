
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;


namespace CompilerDestroyer.Editor.EditorTools
{

    [FilePath(GlobalVariables.PackageName + "/packageinitializersave.binary", FilePathAttribute.Location.PreferencesFolder)]
    public class PackageInitializerSave : ScriptableSingleton<PackageInitializerSave>
    {
        [SerializeField] internal bool isPackageInitializerAlreadyRan = false;
        [SerializeField] internal bool isPackageInitializerEnabled = true;
        [SerializeField] public List<Package> builtInPackages = new List<Package>();
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
    public class Package : IComparable<Package>
    {
        [SerializeField] public string packageName;
        [SerializeField] internal bool shouldPackageInstalled;


        internal Package()
        {

        }
        internal Package(string name, bool shouldPackageInstalled)
        {
            packageName = name;
            this.shouldPackageInstalled = shouldPackageInstalled;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Package other)
                return this.packageName == other.packageName;
            return false;
        }

        public override int GetHashCode()
        {
            return packageName.GetHashCode();
        }

        public int CompareTo(Package other)
        {
            if (other == null) return 1;

            return string.Compare(this.packageName, other.packageName, StringComparison.Ordinal);
        }
    }
}