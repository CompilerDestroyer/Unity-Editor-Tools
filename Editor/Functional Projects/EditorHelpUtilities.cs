using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace FMPUtils.Editor
{
    public class EditorHelpUtilities
    {
        public static bool DoesAssetAtPathExist(string assetDatabaseFilePath)
        {
            return string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(assetDatabaseFilePath)) == false;
        }

        public static bool DisplayConfirmDialog(string title, string text, string okText = "Proceed", string cancelText = "Cancel")
        {
            return EditorUtility.DisplayDialog(title, text, okText, cancelText);
        }

        /// <summary>
        /// Returns the path to the folder, not including the trailing "/" of the folder. 
        /// If the asset is at top level, will return an empty string
        /// </summary>
        public static string GetAssetFolderPathFromAssetFilePath(string assetDatabaseFilePath)
        {
            if (string.IsNullOrEmpty(assetDatabaseFilePath))
                return string.Empty;
            int lastSlashIndex = assetDatabaseFilePath.LastIndexOf('/');
            if (lastSlashIndex < 0)
                return string.Empty;
            return assetDatabaseFilePath.Substring(0, lastSlashIndex);
        }
    }
}