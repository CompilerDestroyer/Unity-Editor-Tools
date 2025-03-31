using UnityEngine;
using UnityEngine.UIElements;
using CompilerDestroyer.Editor.UIElements;

namespace CompilerDestroyer.Editor.Utilities
{
    internal class UtilitiesDocumentation
    {
        private static readonly float marginLeftRight = 15f;

        private static readonly string RemoveScriptingDefineSymbolsFromBuildInfo =
            $"Add scripting define symbols to <color=#4EC393>{nameof(RemoveScriptingDefineSymbolsFromBuild)}</color>.{nameof(RemoveScriptingDefineSymbolsFromBuild.defines)} list. " +
            $"These symbols will be automatically removed during the build process, " +
            $"but restored afterwards to maintain your development environment settings.";

        private static readonly string GitDependencyManagerInfo =
            $"Automatically checks for and installs git dependencies listed in <color=#4EC393>package.json</color> files.\n\n" +
            $"• Detects new packages added via Unity's Package Manager\n" +
            $"• Parses <color=#4EC393>gitDependencies</color> from package.json\n" +
            $"• Prompts to install missing dependencies (unless in batch mode)\n" +
            $"• Cleanly handles installation progress and completion";


        internal static VisualElement UtilitiesVisualElement()
        {
            VisualElement rootVisualElement = new VisualElement();

            Header utilitiesHeader = new Header();
            utilitiesHeader.text = GlobalVariables.UtilitiesName;
            utilitiesHeader.style.marginTop = 5f;
            utilitiesHeader.style.marginBottom = marginLeftRight;
            utilitiesHeader.style.marginLeft = marginLeftRight;
            utilitiesHeader.style.marginRight = marginLeftRight;



            VisualElement RemoveScriptingDefineSymbolsFromBuildDocumentation = MakeDocumentationElement(GlobalVariables.RemoveDefineSymbolsFromBuildName, RemoveScriptingDefineSymbolsFromBuildInfo);
            VisualElement GitDependencyManagerDocumentation = MakeDocumentationElement(GlobalVariables.GitDependencyManagerName, GitDependencyManagerInfo);


            rootVisualElement.Add(utilitiesHeader);
            rootVisualElement.Add(RemoveScriptingDefineSymbolsFromBuildDocumentation);
            rootVisualElement.Add(GitDependencyManagerDocumentation);
            return rootVisualElement;
        }



        private static VisualElement MakeDocumentationElement(string documentationHeader, string documetationLabel)
        {
            VisualElement visualElement = new VisualElement();
            Foldout oneDocumentation = new Foldout();
            oneDocumentation.text = documentationHeader;
            oneDocumentation.style.fontSize = 13;
            oneDocumentation.style.unityFontStyleAndWeight = FontStyle.Bold;
            oneDocumentation.style.marginLeft = marginLeftRight;
            oneDocumentation.style.marginBottom = 4f;
            oneDocumentation.AddToClassList(GlobalVariables.ListViewFoldoutStyleName);

            InfoBox documentationInfoBox = new InfoBox(documetationLabel, InfoBoxIconType.None, 0f);
            documentationInfoBox.style.marginBottom = 5f;
            documentationInfoBox.style.marginLeft = marginLeftRight;
            documentationInfoBox.style.marginRight = marginLeftRight;
            documentationInfoBox.style.whiteSpace = WhiteSpace.Normal;

            oneDocumentation.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue)
                {
                    documentationInfoBox.style.display = DisplayStyle.Flex;
                }
                else
                {
                    documentationInfoBox.style.display = DisplayStyle.None;
                }

            });


            visualElement.Add(oneDocumentation);
            visualElement.Add(documentationInfoBox);
            return visualElement;
        }
    }
}