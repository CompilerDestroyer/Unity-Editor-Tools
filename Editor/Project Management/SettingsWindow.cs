using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.IO;
using UnityEngine;
using CompilerDestroyer.Editor.UIElements;

namespace CompilerDestroyer.Editor.ToolsManager
{
    public sealed class EditorToolsSettings : EditorWindow
    {
        private const string documentationName = "Documentation";
        private const string scriptDescriptionName = "Script Description";

        private static readonly Vector2 minWindowSize = new Vector2(310f, 200f);

        private List<TreeViewItemData<string>> projectSettingsList = new List<TreeViewItemData<string>>();
        private Dictionary<string, VisualElement> rootDict = new Dictionary<string, VisualElement>();

        [MenuItem("Tools/Compiler Destroyer/Editor Tools")]
        private static void ShowWindow()
        {
            EditorToolsSettings settingsWindow = GetWindow<EditorToolsSettings>();
            settingsWindow.titleContent.text = "Editor Tools Settings";
            settingsWindow.titleContent.image = EditorGUIUtility.FindTexture("SettingsIcon");
            settingsWindow.minSize = minWindowSize;
        }

        public void CreateGUI()
        {

            TreeViewItemData<string> documentationSetting = new TreeViewItemData<string>(2, documentationName);
            TreeViewItemData<string> scriptDescriptionSetting = new TreeViewItemData<string>(1, scriptDescriptionName);


            rootDict.Add(documentationName, null);
            rootDict.Add(scriptDescriptionName, null);


            projectSettingsList.Add(documentationSetting);
            projectSettingsList.Add(scriptDescriptionSetting);

            SettingsPanel settingsWindow = new SettingsPanel(ref projectSettingsList, ref rootDict);

            rootVisualElement.Add(settingsWindow);
        }
    }
}
