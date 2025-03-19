using System.IO;
using System;
using UnityEditor;
using UnityEngine;

namespace CompilerDestroyer.Editor
{
    public static class GlobalVariables
    {
        // Project Paths
        public const string NickName = "Compiler Destroyer";
        public const string PackageName = "com.compilerdestroyer.editortools";
        public const string ProjectsPath = "Packages/" + PackageName + "/Editor/Projects";
        public const string ProjectManagementPath = "Packages/" + PackageName + "/Editor/Project Managements";


        public const string RoughnessConverter = "Roughness Converter";
        public const string PackagesInitializerName = "Package Initializer";

        public const string UnityLogoIconName = "d_UnityLogo";
        public const string UnityInfoIconName = "console.infoicon@2x";
        public const string UnityErrorIconName = "Error@2x";
        public const string UnityWarnIconName = "Warning@2x";
        public const string UnityInstalledIconName = "Installed@2x";


        public static string windowsAssetStorePackagePath = @"Unity\Asset Store-5.x";
        public static string OsxEditorPackagePath = @"Library/Unity/Asset Store-5.x";
        public static string LinuxEditorPackagePath = ".local/share/unity3d/Asset Store-5.x";


        private static string AssetStorePath;
        public static string CurrentAssetStorePath
        {
            get
            {
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    AssetStorePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), windowsAssetStorePackagePath);
                }
                else if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    AssetStorePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), OsxEditorPackagePath);
                }
                else if (Application.platform == RuntimePlatform.LinuxEditor)
                {
                    AssetStorePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), LinuxEditorPackagePath);
                }
                else
                {
                    Debug.Log("Unsupported platform for Asset Store cache location.");
                }

                return AssetStorePath;
            }
        }




        private static readonly Color defaultLineDarkColor = new Color(0.1215686f, 0.1215686f, 0.1215686f, 1f);
        private static readonly Color defaultLineWhiteColor = new Color(0.6f, 0.6f, 0.6f, 1f);
        public static Color DefaultLineColor
        {
            get
            {
                if (EditorGUIUtility.isProSkin)
                {
                    return defaultLineDarkColor;
                }
                else
                {
                    return defaultLineWhiteColor;
                }
            }
        }

        private static readonly Color defaultBackgroundDarkColor = new Color(0.2352941f, 0.2352941f, 0.2352941f, 1f);
        private static readonly Color defaultBackgroundWhiteColor = new Color(0.7843138f, 0.7843138f, 0.7843138f, 1f);
        public static Color DefaultBackgroundColor
        {
            get
            {
                if (EditorGUIUtility.isProSkin)
                {
                    return defaultBackgroundDarkColor;
                }
                else
                {
                    return defaultBackgroundWhiteColor;
                }
            }
        }
    }
}
