using UnityEngine;
using UnityEngine.UIElements;
using CompilerDestroyer.Editor.UIElements;
using System.Collections.Generic;
using System;
using CompilerDestroyer.Editor.Attributes;


namespace CompilerDestroyer.Editor.EditorTools
{
    internal sealed class LibrariesDocumentation
    {
        private static VisualElement rootVisualElement;

        private static readonly float marginLeftRight = 15f;
        private static readonly float marginBottom = 15f;

        private static readonly string coloredReadonlyHeader = $"<color={GlobalVariables.classColorHexaDec}>{nameof(ReadOnlyAttribute)}</color>";
        private static readonly string coloredHeaderHeader = $"<color={GlobalVariables.classColorHexaDec}>{nameof(Header)}</color>";
        private static readonly string coloredInfoBoxHeader = $"<color={GlobalVariables.classColorHexaDec}>{nameof(InfoBox)}</color>";
        private static readonly string coloredLineHeader = $"<color={GlobalVariables.classColorHexaDec}>{nameof(Line)}</color>";
        private static readonly string coloredSettingsPanelHeader = $"<color={GlobalVariables.classColorHexaDec}>{nameof(SettingsPanel)}</color>";
        private static readonly string coloredToolbarSearchPanelHeader = $"<color={GlobalVariables.classColorHexaDec}>{nameof(ToolbarSearchPanel)}</color>";

        private static readonly string readonlyAttributeInfo = nameof(ReadOnlyAttribute) + " allows you to make fields readonly.";
        private static readonly string headerUIElementInfo = "Basic general label for headers. Default font size is 18.";
        private static readonly string infoBoxUIElementInfo = "A custom box. " + nameof(InfoBoxIconType) + " can be used to determine icon type.";
        private static readonly string lineUIElementInfo = "A line that can be used to draw lines.";
        private static readonly string settingsPanelUIElementInfo = "You can use " + nameof(SettingsPanel) + " to create unity's project settings like UIElements. In order to add items, " +
        "you need to use " + nameof(TreeViewItemData<string>) + "<" + nameof(String) + ">" + " and in order to add functionality to it, you need to add a " 
            + nameof(VisualElement) + " to the a dictionary with same name" + " as " + nameof(TreeViewItemData<string>) + ".";
        private static readonly string toolbarSearchPanelUIElementInfo = "Same as ToolbarSearchField but search implemented with strings.";





        private static readonly string readonlyCodeExample = "[<color=" + GlobalVariables.classColorHexaDec + ">ReadOnly</color>] <color=" + GlobalVariables.typeColorHexaDec 
            + ">public int</color> health = " +
            "<color=" + GlobalVariables.floatColorHexaDec + ">100</color>;";

        private static readonly string headerCodeExample = "<color=" + GlobalVariables.classColorHexaDec + ">Header</color> <color=" + GlobalVariables.variableColorHexaDec
            + "> header</color> =" +
            " <color=" + GlobalVariables.typeColorHexaDec + "> new</color> <color=" + GlobalVariables.classColorHexaDec + "> Header</color>(<color=" + GlobalVariables.stringColorHexaDec 
            + ">\"Basic Header\"</color>);";

        private static readonly string infoBoxCodeExample = "<color=" + GlobalVariables.classColorHexaDec + ">InfoBox</color> <color=" + GlobalVariables.variableColorHexaDec
            + "> infoBox</color> =" +
    " <color=" + GlobalVariables.typeColorHexaDec + "> new</color> <color=" + GlobalVariables.classColorHexaDec + "> InfoBox</color>(<color=" + GlobalVariables.stringColorHexaDec 
            + ">\"This is an InfoBox\"</color>, <color=" +
            GlobalVariables.enumColorHexaDec + ">" + nameof(InfoBoxIconType) + "</color>." + nameof(InfoBoxIconType.Info) + ", <color=" + GlobalVariables.floatColorHexaDec 
            + ">3f</color>);";

        private static readonly string lineCodeExample =
            "<color=" + GlobalVariables.classColorHexaDec + ">Line</color> <color=" + GlobalVariables.variableColorHexaDec + ">line</color> = " +
            "<color=" + GlobalVariables.typeColorHexaDec + ">new</color> <color=" + GlobalVariables.classColorHexaDec + ">Line</color>(<color=" + GlobalVariables.floatColorHexaDec
            + ">4f</color>, <color=" + GlobalVariables.typeColorHexaDec + ">false</color>, <color=" + GlobalVariables.structColorHexaDec + ">Color</color>.red);" +

            "\n<color=" + GlobalVariables.variableColorHexaDec + ">line</color>.style.height = <color=" + GlobalVariables.floatColorHexaDec + ">1f</color>;" +
            "\n<color=" + GlobalVariables.variableColorHexaDec + ">line</color>.style.width = <color=" + GlobalVariables.floatColorHexaDec + ">100f</color>;";


        private static readonly string settingsPanelCodeExample =
            "<color=" + GlobalVariables.classColorHexaDec + ">List</color><<color=" + GlobalVariables.classColorHexaDec + ">TreeViewItemData</color><<color=" 
            + GlobalVariables.typeColorHexaDec + ">string</color>>> " +
            "<color=" + GlobalVariables.variableColorHexaDec + ">items</color> = <color=" + GlobalVariables.typeColorHexaDec + ">new</color> <color=" + GlobalVariables.classColorHexaDec
            + ">List</color><<color=" + GlobalVariables.classColorHexaDec + ">TreeViewItemData</color><<color=" + GlobalVariables.typeColorHexaDec + ">string</color>>>();" +

            "\n<color=" + GlobalVariables.classColorHexaDec + ">TreeViewItemData</color><<color=" + GlobalVariables.typeColorHexaDec + ">string</color>> " +
            "<color=" + GlobalVariables.variableColorHexaDec + ">example1TreeViewItemData</color> = " +
            "<color=" + GlobalVariables.typeColorHexaDec + ">new</color> <color=" + GlobalVariables.classColorHexaDec + ">TreeViewItemData</color><<color=" + GlobalVariables.typeColorHexaDec
            + ">string</color>>(<color=" + GlobalVariables.floatColorHexaDec + ">0</color>, <color=" + GlobalVariables.stringColorHexaDec + ">\"Example 1\"</color>);" +

            "\n<color=" + GlobalVariables.classColorHexaDec + ">TreeViewItemData</color><<color=" + GlobalVariables.typeColorHexaDec + ">string</color>> " +
            "<color=" + GlobalVariables.variableColorHexaDec + ">example2TreeViewItemData</color> = " +
            "<color=" + GlobalVariables.typeColorHexaDec + ">new</color> <color=" + GlobalVariables.classColorHexaDec + ">TreeViewItemData</color><<color=" 
            + GlobalVariables.typeColorHexaDec + ">string</color>>(<color=" + GlobalVariables.floatColorHexaDec + ">1</color>, <color=" + GlobalVariables.stringColorHexaDec 
            + ">\"Example 2\"</color>);" +

            "\n<color=" + GlobalVariables.variableColorHexaDec + ">items</color>.<color=" + GlobalVariables.methodColorHexaDec + ">Add</color>(<color="
            + GlobalVariables.variableColorHexaDec + ">example1TreeViewItemData</color>);" +
            "\n<color=" + GlobalVariables.variableColorHexaDec + ">items</color>.<color=" + GlobalVariables.methodColorHexaDec + ">Add</color>(<color="
            + GlobalVariables.variableColorHexaDec + ">example2TreeViewItemData</color>);" +

            "\n\n<color=" + GlobalVariables.classColorHexaDec + ">Dictionary</color><<color=" + GlobalVariables.typeColorHexaDec + ">string</color>, <color="
            + GlobalVariables.classColorHexaDec + ">VisualElement</color>> " +
            "<color=" + GlobalVariables.variableColorHexaDec + ">itemsVisualElementsDict</color> = <color=" + GlobalVariables.typeColorHexaDec + ">new</color> <color=" 
            + GlobalVariables.classColorHexaDec + ">Dictionary</color><<color=" + GlobalVariables.typeColorHexaDec + ">string</color>, <color=" + GlobalVariables.classColorHexaDec 
            + ">VisualElement</color>>();" +

            "\n<color=" + GlobalVariables.variableColorHexaDec + ">itemsVisualElementsDict</color>.<color=" + GlobalVariables.methodColorHexaDec + ">Add</color>(<color=" 
            + GlobalVariables.stringColorHexaDec + ">\"Example 1\"</color>, <color=" + GlobalVariables.typeColorHexaDec + ">new</color> <color=" + GlobalVariables.classColorHexaDec
            + ">Label</color>(<color=" + GlobalVariables.stringColorHexaDec + ">\"I am Example 1\"</color>));" +
            "\n<color=" + GlobalVariables.variableColorHexaDec + ">itemsVisualElementsDict</color>.<color=" + GlobalVariables.methodColorHexaDec + ">Add</color>(<color=" 
            + GlobalVariables.stringColorHexaDec + ">\"Example 2\"</color>, <color=" + GlobalVariables.typeColorHexaDec + ">new</color> <color=" + GlobalVariables.classColorHexaDec 
            + ">Label</color>(<color=" + GlobalVariables.stringColorHexaDec + ">\"I am Example 2\"</color>));" +

            "\n\n<color=" + GlobalVariables.classColorHexaDec + ">SettingsPanel</color> <color=" + GlobalVariables.variableColorHexaDec + ">panel</color> = " +
            "<color=" + GlobalVariables.typeColorHexaDec + ">new</color> <color=" + GlobalVariables.classColorHexaDec + ">SettingsPanel</color>(<color=" 
            + GlobalVariables.typeColorHexaDec + ">ref</color> <color=" + GlobalVariables.variableColorHexaDec + ">items</color>, <color=" + GlobalVariables.typeColorHexaDec 
            + ">ref</color> <color=" + GlobalVariables.variableColorHexaDec + ">itemsVisualElementsDict</color>);" +

            "\n<color=" + GlobalVariables.variableColorHexaDec + ">panel</color>.style.width =  <color=" + GlobalVariables.floatColorHexaDec + ">310f</color>;" +
            "\n<color=" + GlobalVariables.variableColorHexaDec + ">panel</color>.style.height =  <color=" + GlobalVariables.floatColorHexaDec + ">310f</color>;";


        private static readonly string toolbarSearchPanelCodeExample =
            "<color=" + GlobalVariables.classColorHexaDec + ">List</color><<color=" + GlobalVariables.typeColorHexaDec + ">string</color>> " +
            "<color=" + GlobalVariables.variableColorHexaDec + ">toolbarSearchList</color> = <color=" + GlobalVariables.typeColorHexaDec + ">new</color> <color=" 
            + GlobalVariables.classColorHexaDec + ">List</color><<color=" + GlobalVariables.typeColorHexaDec + ">string</color>>()" + " { " +
            "<color=" + GlobalVariables.stringColorHexaDec + ">\"Level Editor\"</color>, " +
            "<color=" + GlobalVariables.stringColorHexaDec + ">\"Terrain Licker\"</color>, " +
            "<color=" + GlobalVariables.stringColorHexaDec + ">\"Inspector Destroyer\"</color>, " +
            "<color=" + GlobalVariables.stringColorHexaDec + ">\"Mesh Consumer\"</color>};" +

            "\n<color=" + GlobalVariables.classColorHexaDec + ">List</color><<color=" + GlobalVariables.typeColorHexaDec + ">string</color>> " +
            "<color=" + GlobalVariables.variableColorHexaDec + ">resultList</color> = <color=" + GlobalVariables.typeColorHexaDec + ">new</color> <color=" + GlobalVariables.classColorHexaDec 
            + ">List</color><<color=" + GlobalVariables.typeColorHexaDec + ">string</color>>();" +

            "\n\n<color=" + GlobalVariables.classColorHexaDec + ">ListView</color> " +
            "<color=" + GlobalVariables.variableColorHexaDec + ">listView</color> = <color=" + GlobalVariables.typeColorHexaDec + ">new</color> <color=" + GlobalVariables.classColorHexaDec + 
            ">ListView</color>(<color=" + GlobalVariables.variableColorHexaDec + ">toolbarSearchList</color>, <color=" + GlobalVariables.floatColorHexaDec + ">15</color>);" +
            "\n<color=" + GlobalVariables.variableColorHexaDec + ">listView</color>.makeItem = () => <color=" + GlobalVariables.typeColorHexaDec + ">new</color> <color=" 
            + GlobalVariables.classColorHexaDec + ">Label</color>();" +
            "\n<color=" + GlobalVariables.variableColorHexaDec + ">listView</color>.bindItem = (<color=" + GlobalVariables.variableColorHexaDec + ">element</color>, <color=" 
            + GlobalVariables.variableColorHexaDec + ">index</color>) => (<color=" + GlobalVariables.variableColorHexaDec + ">element</color> <color=" + GlobalVariables.typeColorHexaDec
            + ">as</color> <color=" + GlobalVariables.classColorHexaDec + ">Label</color>).text = <color=" + GlobalVariables.variableColorHexaDec + ">listView</color>.itemsSource[<color=" 
            + GlobalVariables.variableColorHexaDec + ">index</color>] <color=" + GlobalVariables.typeColorHexaDec + ">as</color> <color=" + GlobalVariables.typeColorHexaDec + ">string</color>;" 
            +

            "\n\n<color=" + GlobalVariables.classColorHexaDec + ">Action</color> <color=" + GlobalVariables.variableColorHexaDec + ">OnEmpty</color> = () =>" +
            "{ " +
            "<color=" + GlobalVariables.variableColorHexaDec + ">listView</color>.itemsSource = <color=" + GlobalVariables.variableColorHexaDec + ">toolbarSearchList</color>;" +
            "<color=" + GlobalVariables.variableColorHexaDec + ">listView</color>.<color=" + GlobalVariables.methodColorHexaDec + ">Rebuild</color>();" +
            "};" +

            "\n<color=" + GlobalVariables.classColorHexaDec + ">Action</color> <color=" + GlobalVariables.variableColorHexaDec + ">OnFilled</color> = () =>" +
            "{ " +
            "<color=" + GlobalVariables.variableColorHexaDec + ">listView</color>.itemsSource = <color=" + GlobalVariables.variableColorHexaDec + ">resultList</color>;" +
            "<color=" + GlobalVariables.variableColorHexaDec + ">listView</color>.<color=" + GlobalVariables.methodColorHexaDec + ">Rebuild</color>();" +
            "};" +

            "\n\n<color=" + GlobalVariables.classColorHexaDec + ">ToolbarSearchPanel</color> " +
            "<color=" + GlobalVariables.variableColorHexaDec + ">toolbarSearchPanel</color> = <color=" + GlobalVariables.typeColorHexaDec + ">new</color> <color="
            + GlobalVariables.classColorHexaDec + ">ToolbarSearchPanel</color>(<color=" + GlobalVariables.variableColorHexaDec + ">toolbarSearchList</color>, <color="
            + GlobalVariables.variableColorHexaDec + ">resultList</color>, <color=" + GlobalVariables.variableColorHexaDec + ">OnEmpty</color>, <color=" + GlobalVariables.variableColorHexaDec 
            + ">OnFilled</color>);";



        internal static VisualElement LibrariesDocumentationElement()
        {
            rootVisualElement = new VisualElement();

            LibrariesHeader();
            CollapseExpandButtons();

            AttributesHeader();
            AttributeReadonlyExample();

            UIElementsHeader();
            HeaderExample();
            InfoBoxExample();
            LineExample();
            SettingsPanelExample();
            ToolbarSearchPanelExample();

            return rootVisualElement;
        }

        private static void LibrariesHeader()
        {
            Header librariesHeader = new Header();
            librariesHeader.text = GlobalVariables.LibrariesName;
            librariesHeader.style.marginTop = 5f;
            librariesHeader.style.marginLeft = marginLeftRight;
            librariesHeader.style.marginBottom = marginLeftRight;

            rootVisualElement.Add(librariesHeader);
        }
        private static void CollapseExpandButtons()
        {
            VisualElement collapsExpandContainer = new VisualElement();
            collapsExpandContainer.style.flexDirection = FlexDirection.Row;
            collapsExpandContainer.style.marginLeft = marginLeftRight;
            collapsExpandContainer.style.marginRight = marginLeftRight;
            collapsExpandContainer.style.marginBottom = 4f;


            Button collapseAll = new Button();
            collapseAll.text = "Collapse All";
            collapseAll.clicked += () => 
            {
                List<Foldout> allFoldouts = rootVisualElement.Query<Foldout>().Build().ToList();
                for (int i = 0; i < allFoldouts.Count; i++)
                {
                    allFoldouts[i].value = false;
                }
            };

            Button expandAll = new Button();
            expandAll.text = "Expand All";
            expandAll.clicked += () =>
            {
                List<Foldout> allFoldouts = rootVisualElement.Query<Foldout>().Build().ToList();
                for (int i = 0; i < allFoldouts.Count; i++)
                {
                    allFoldouts[i].value = true;
                }
            };

            collapsExpandContainer.Add(collapseAll);
            collapsExpandContainer.Add(expandAll);
            rootVisualElement.Add(collapsExpandContainer);
        }


        #region Attributes
        private static void AttributesHeader()
        {
            VisualElement attributesContainer = new VisualElement();
            attributesContainer.style.flexDirection = FlexDirection.Row;
            attributesContainer.style.marginBottom = 4f;

            Header attributes = new Header(GlobalVariables.AttributesName, 14f);
            attributes.style.marginLeft = marginLeftRight;

            Label attributesNameSpace = new Label("namespace: " + nameof(CompilerDestroyer) + "." + nameof(Editor) + "." + nameof(Attributes));
            attributesNameSpace.style.unityTextAlign = TextAnchor.LowerCenter;
            attributesNameSpace.style.color = Color.grey;

            Line attributeLine = new Line();
            attributeLine.style.marginLeft = marginLeftRight;
            attributeLine.style.marginRight = marginLeftRight;


            attributesContainer.Add(attributes);
            attributesContainer.Add(attributesNameSpace);
            rootVisualElement.Add(attributesContainer);
            rootVisualElement.Add(attributeLine);
        }
        private static void AttributeReadonlyExample()
        {
            Label readonlyExampleLabel = new Label("health");
            readonlyExampleLabel.SetEnabled(false);
            readonlyExampleLabel.style.unityTextAlign = TextAnchor.MiddleLeft;

            IntegerField readonlyIntField = new IntegerField();
            readonlyIntField.value = 100;
            readonlyIntField.SetEnabled(false);

            VisualElement readonlyExample = LibraryExampleElement(readonlyCodeExample, readonlyExampleLabel, readonlyIntField);
            VisualElement readonlyDocumentation = MakeDocumentationElement(coloredReadonlyHeader, readonlyAttributeInfo, readonlyExample);

            rootVisualElement.Add(readonlyDocumentation);
        }
        #endregion

        #region UIElements
        private static void UIElementsHeader()
        {
            VisualElement uiElementsContainer = new VisualElement();
            uiElementsContainer.style.flexDirection = FlexDirection.Row;
            uiElementsContainer.style.marginBottom = 4f;
            uiElementsContainer.style.marginTop = 8f;

            Header uiElements = new Header(GlobalVariables.UIElementsName, 14f);
            uiElements.style.marginLeft = marginLeftRight;


            Label uiElementsNameSpace = new Label("namespace: " + nameof(CompilerDestroyer) + "." + nameof(Editor) + "." + nameof(UIElements));
            uiElementsNameSpace.style.unityTextAlign = TextAnchor.LowerCenter;
            uiElementsNameSpace.style.color = Color.grey;


            Line uiElementsLine = new Line();
            uiElementsLine.style.marginLeft = marginLeftRight;
            uiElementsLine.style.marginRight = marginLeftRight;

            uiElementsContainer.Add(uiElements);
            uiElementsContainer.Add(uiElementsNameSpace);

            rootVisualElement.Add(uiElementsContainer);
            rootVisualElement.Add(uiElementsLine);
        }


        private static void HeaderExample()
        {
            Header header = new Header("Basic " + nameof(Header));
            VisualElement headerExample = LibraryExampleElement(headerCodeExample, null, header);
            VisualElement headerDocumentation = MakeDocumentationElement(coloredHeaderHeader, headerUIElementInfo, headerExample);

            rootVisualElement.Add(headerDocumentation);
        }
        private static void InfoBoxExample()
        {
            InfoBox infoBox = new InfoBox("An " + nameof(InfoBox) + " can be used to give information", InfoBoxIconType.Info, 3f);
            VisualElement infoBoxExample = LibraryExampleElement(infoBoxCodeExample, null, infoBox);
            VisualElement infoBoxDocumentation = MakeDocumentationElement(coloredInfoBoxHeader, infoBoxUIElementInfo, infoBoxExample);

            rootVisualElement.Add(infoBoxDocumentation);
        }
        private static void LineExample()
        {
            Line line = new Line(4f, false, Color.red);
            line.style.height = 1f;
            line.style.width = 120f;

            VisualElement lineExample = LibraryExampleElement(lineCodeExample, null, line);
            VisualElement lineDocumentation = MakeDocumentationElement(coloredLineHeader, lineUIElementInfo, lineExample);

            rootVisualElement.Add(lineDocumentation);
        }
        private static void SettingsPanelExample()
        {
            List<TreeViewItemData<string>> items = new List<TreeViewItemData<string>>();
            TreeViewItemData<string> example1TreeViewItemData = new TreeViewItemData<string>(0, "Example 1");
            TreeViewItemData<string> example2TreeViewItemData = new TreeViewItemData<string>(1, "Example 2");
            items.Add(example1TreeViewItemData);
            items.Add(example2TreeViewItemData);
            Dictionary<string, VisualElement> itemsVisualElementsDict = new Dictionary<string, VisualElement>();
            itemsVisualElementsDict.Add("Example 1", new Label("I am example 1"));
            itemsVisualElementsDict.Add("Example 2", new Label("I am example 2"));

            SettingsPanel panel = new SettingsPanel(ref items, ref itemsVisualElementsDict);
            panel.style.width = 310;
            panel.style.height = 310f;

            VisualElement settingsPanelExample = LibraryExampleElement(settingsPanelCodeExample, null, panel);
            VisualElement settingsPanelDocumentation = MakeDocumentationElement(coloredSettingsPanelHeader, settingsPanelUIElementInfo, settingsPanelExample);

            rootVisualElement.Add(settingsPanelDocumentation);
        }

        private static void ToolbarSearchPanelExample()
        {
            List<string> toolbarSearchList = new List<string>() { "Level Editor", "Terrain Licker", "Inspector Destroyer", "Mesh Consumer" };
            List<string> resultList = new List<string>();

            VisualElement searchBarContainer = new VisualElement();


            ListView listView = new ListView(toolbarSearchList, 15);
            listView.makeItem = () => new Label();
            listView.bindItem = (element, index) => (element as Label).text = listView.itemsSource[index] as string;

            void OnEmpty()
            {
                listView.itemsSource = toolbarSearchList;
                listView.Rebuild();
            }
            void OnFilled()
            {
                listView.itemsSource = resultList;
                listView.Rebuild();
            }
            ToolbarSearchPanel toolbarSearchPanel = new ToolbarSearchPanel(toolbarSearchList, resultList, OnEmpty, OnFilled);


            VisualElement toolbarSearchPanelExample = LibraryExampleElement(toolbarSearchPanelCodeExample, null, searchBarContainer);
            VisualElement toolbarSearchPanelDocumentation = MakeDocumentationElement(coloredToolbarSearchPanelHeader, toolbarSearchPanelUIElementInfo, toolbarSearchPanelExample);
            searchBarContainer.Add(toolbarSearchPanel);
            searchBarContainer.Add(listView);

            rootVisualElement.Add(toolbarSearchPanelDocumentation);
        }
        #endregion




        private static VisualElement MakeDocumentationElement(string documentationHeader, string documentationLabel, VisualElement exampleElement = null)
        {
            VisualElement visualElement = new VisualElement();
            Foldout oneDocumentation = new Foldout();
            oneDocumentation.text = documentationHeader;
            oneDocumentation.style.fontSize = 13;
            oneDocumentation.style.marginLeft = marginLeftRight;
            oneDocumentation.style.marginBottom = 5f;
            oneDocumentation.style.unityFontStyleAndWeight = FontStyle.Bold;
            oneDocumentation.AddToClassList(GlobalVariables.ListViewFoldoutStyleName);

            Label documentationInfoBox = new Label(documentationLabel);
            documentationInfoBox.style.marginBottom = 4f;
            documentationInfoBox.style.marginLeft = marginLeftRight;
            documentationInfoBox.style.marginRight = marginLeftRight;
            documentationInfoBox.style.whiteSpace = WhiteSpace.Normal;


            oneDocumentation.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue)
                {
                    documentationInfoBox.style.display = DisplayStyle.Flex;
                    exampleElement.style.display = DisplayStyle.Flex;
                }
                else
                {
                    documentationInfoBox.style.display = DisplayStyle.None;
                    exampleElement.style.display = DisplayStyle.None;
                }
            });


            visualElement.Add(oneDocumentation);
            visualElement.Add(documentationInfoBox);
            visualElement.Add(exampleElement);
            return visualElement;
        }
        private static VisualElement LibraryExampleElement(string codeExample, Label exampleLabel = null, VisualElement exampleVisualElement = null)
        {
            VisualElement codeAndElementContainer = new VisualElement();
            codeAndElementContainer.style.marginLeft = marginLeftRight;
            codeAndElementContainer.style.marginRight = marginLeftRight;
            codeAndElementContainer.style.marginBottom = marginBottom;
            codeAndElementContainer.style.flexDirection = FlexDirection.Row;
            codeAndElementContainer.style.justifyContent = Justify.SpaceBetween;


            InfoBox codeBox = new InfoBox(codeExample, InfoBoxIconType.None, 0f);
            codeBox.style.unityFontStyleAndWeight = FontStyle.Bold;

            VisualElement fieldContainer = new VisualElement();
            fieldContainer.style.alignContent = Align.FlexEnd;
            fieldContainer.style.flexDirection = FlexDirection.Row;


            if (exampleLabel != null)
            {
                fieldContainer.Add(exampleLabel);
            }
            if (exampleVisualElement != null)
            {
                fieldContainer.Add(exampleVisualElement);
            }
            codeAndElementContainer.Add(codeBox);
            codeAndElementContainer.Add(fieldContainer);
            return codeAndElementContainer;
        }
    }
}