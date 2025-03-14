using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;
using CompilerDestroyer.Editor.UIElements;
using CompilerDestroyer.Editor.EditorTools;

namespace CompilerDestroyer.Editor.ToolsManager
{
    public sealed class EditorToolsSettings : EditorWindow
    {
        private const string toolsName = "Tools";
        private const string textureCombinerName = "Texture Combiner";
        private const string packageInitializerName = "Package Initializer";


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
            // Tools
            List<TreeViewItemData<string>> toolChildren = new List<TreeViewItemData<string>>();
            TreeViewItemData<string> textureManipulatorSetting = new TreeViewItemData<string>(0, textureCombinerName);
            TreeViewItemData<string> packageInitializerSetting = new TreeViewItemData<string>(1, packageInitializerName);
            toolChildren.Add(textureManipulatorSetting);
            toolChildren.Add(packageInitializerSetting);
            TreeViewItemData<string> toolsSetting = new TreeViewItemData<string>(2, toolsName, toolChildren);

            // Tools


            TreeViewItemData<string> documentationSetting = new TreeViewItemData<string>(3, documentationName);


            rootDict.Add(toolsName, null);
            rootDict.Add(textureCombinerName, CreateMetallicSmoothness.ConvertRoughnessToMetallicSmoothnessVisualElement());
            rootDict.Add(packageInitializerName, PackageInitializer.PackageInitializerVisualElement());
            rootDict.Add(documentationName, null);


            projectSettingsList.Add(toolsSetting);
            projectSettingsList.Add(documentationSetting);

            SettingsPanel settingsWindow = new SettingsPanel(ref projectSettingsList, ref rootDict);


            rootVisualElement.Add(settingsWindow);
        }
    }
}
