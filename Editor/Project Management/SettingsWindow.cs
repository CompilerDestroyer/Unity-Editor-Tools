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
        private const string toolsName = "Tools";
        private const string toolsName2 = "Tools";


        private const string documentationName = "Documentation";

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
            TreeViewItemData<string> toolsSetting = new TreeViewItemData<string>(1, toolsName);
            TreeViewItemData<string> documentationSetting = new TreeViewItemData<string>(2, documentationName);




            rootDict.Add(toolsName, null);
            rootDict.Add(documentationName, null);


            projectSettingsList.Add(toolsSetting);
            projectSettingsList.Add(documentationSetting);

            SettingsPanel settingsWindow = new SettingsPanel(ref projectSettingsList, ref rootDict);

            rootVisualElement.Add(settingsWindow);
        }
    }
}
