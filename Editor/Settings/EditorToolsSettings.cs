using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;
using CompilerDestroyer.Editor.UIElements;
using CompilerDestroyer.Editor.EditorTools;

namespace CompilerDestroyer.Editor.ToolsManager
{
    internal sealed class EditorToolsSettings : EditorWindow
    {
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
            // Libraries
            TreeViewItemData<string> librariesSetting = new TreeViewItemData<string>(0, GlobalVariables.LibrariesName);
            // ----------------------

            // Tools
            List<TreeViewItemData<string>> toolChildren = new List<TreeViewItemData<string>>();
            TreeViewItemData<string> packageInitializerSetting = new TreeViewItemData<string>(1, GlobalVariables.PackagesInitializerName);
            TreeViewItemData<string> roughnessConverterSetting = new TreeViewItemData<string>(2, GlobalVariables.RoughnessConverterName);

            toolChildren.Add(packageInitializerSetting);
            toolChildren.Add(roughnessConverterSetting);
            TreeViewItemData<string> toolsSetting = new TreeViewItemData<string>(3, GlobalVariables.ToolsName, toolChildren);
            // ----------------------

            // Utilities
            TreeViewItemData<string> utilitiesSetting = new TreeViewItemData<string>(4, GlobalVariables.UtilitiesName);
            // ----------------------

            //rootDict.Add(GlobalVariables.LibrariesName, LibrariesDocumentation.LibrariesDocumentationElement());

            // Tools
            rootDict.Add(GlobalVariables.ToolsName, ToolsDocumentation.ToolsVisualElement());
            rootDict.Add(GlobalVariables.PackagesInitializerName, PackageInitializer.PackageInitializerVisualElement());
            rootDict.Add(GlobalVariables.RoughnessConverterName, RoughnessConverter.ConvertRoughnessToMetallicSmoothnessVisualElement());
            //rootDict.Add(GlobalVariables.UtilitiesName, UtilitiesDocumentation.UtilitiesVisualElement());


            //projectSettingsList.Add(librariesSetting);
            projectSettingsList.Add(toolsSetting);
            //projectSettingsList.Add(utilitiesSetting);



            SettingsPanel settingsWindow = new SettingsPanel(ref projectSettingsList, ref rootDict);


            rootVisualElement.Add(settingsWindow);
        }
    }
}
