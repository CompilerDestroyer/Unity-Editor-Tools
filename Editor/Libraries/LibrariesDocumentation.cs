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
        private static string variableColorHexaDec = "#80B6B3";
        private static string classColorHexaDec = "#4EC393";
        private static string structColorHexaDec = "#81B87C";
        private static string typeColorHexaDec = "#54879A";
        private static string stringColorHexaDec = "#CC9074";
        private static string enumColorHexaDec = "#98BF79";
        private static string floatColorHexaDec = "#AFC69C";
        private static string methodColorHexaDec = "#CCD090";


        private static VisualElement rootVisualElement;

        private static readonly float marginLeftRight = 15f;
        private static readonly float marginBottom = 15f;
        private static readonly string readonlyAttributeInfo = nameof(ReadOnlyAttribute) + " allows you to make fields readonly.";
        private static readonly string headerUIElementInfo = "Basic general label for headers. Default font size is 18.";
        private static readonly string infoBoxUIElementInfo = "A custom box. " + nameof(InfoBoxIconType) + " can be used to determine icon type.";
        private static readonly string lineUIElementInfo = "A line that can be used to draw lines.";
        private static readonly string settingsPanelUIElementInfo = "You can use " + nameof(SettingsPanel) + " to create unity's project settings like UIElements. In order to add items, " +
        "you need to use " + nameof(TreeViewItemData<string>) + "<" + nameof(String) + ">" + " and in order to add functionality to it, you need to add a " 
            + nameof(VisualElement) + " to the a dictionary with same name" + " as " + nameof(TreeViewItemData<string>) + ".";
        private static readonly string toolbarSearchPanelUIElementInfo = "Same as ToolbarSearchField but search implemented with strings.";



        private static readonly string readonlyCodeExample = "[<color=" + classColorHexaDec + ">ReadOnly</color>] <color=" + typeColorHexaDec + ">public int</color> health = " +
            "<color=" + floatColorHexaDec + ">100</color>;";

        private static readonly string headerCodeExample = "<color=" + classColorHexaDec + ">Header</color> <color=" + variableColorHexaDec + "> header</color> =" +
            " <color=" + typeColorHexaDec + "> new</color> <color=" + classColorHexaDec + "> Header</color>(<color=" + stringColorHexaDec + ">\"Basic Header\"</color>);";

        private static readonly string infoBoxCodeExample = "<color=" + classColorHexaDec + ">InfoBox</color> <color=" + variableColorHexaDec + "> infoBox</color> =" +
    " <color=" + typeColorHexaDec + "> new</color> <color=" + classColorHexaDec + "> InfoBox</color>(<color=" + stringColorHexaDec + ">\"This is an InfoBox\"</color>, <color=" +
            enumColorHexaDec + ">" + nameof(InfoBoxIconType) + "</color>." + nameof(InfoBoxIconType.Info) + ", <color=" + floatColorHexaDec + ">3f</color>);";

        private static readonly string lineCodeExample =
            "<color=" + classColorHexaDec + ">Line</color> <color=" + variableColorHexaDec + ">line</color> = " +
            "<color=" + typeColorHexaDec + ">new</color> <color=" + classColorHexaDec + ">Line</color>(<color=" + floatColorHexaDec + ">4f</color>, <color=" + typeColorHexaDec + ">false</color>, <color=" + structColorHexaDec + ">Color</color>.red);" +

            "\n<color=" + variableColorHexaDec + ">line</color>.style.height = <color=" + floatColorHexaDec + ">1f</color>;" +
            "\n<color=" + variableColorHexaDec + ">line</color>.style.width = <color=" + floatColorHexaDec + ">100f</color>;";


        private static readonly string settingsPanelCodeExample =
            "<color=" + classColorHexaDec + ">List</color><<color=" + classColorHexaDec + ">TreeViewItemData</color><<color=" + typeColorHexaDec + ">string</color>>> " +
            "<color=" + variableColorHexaDec + ">items</color> = <color=" + typeColorHexaDec + ">new</color> <color=" + classColorHexaDec + ">List</color><<color=" + classColorHexaDec + ">TreeViewItemData</color><<color=" + typeColorHexaDec + ">string</color>>>();" +

            "\n<color=" + classColorHexaDec + ">TreeViewItemData</color><<color=" + typeColorHexaDec + ">string</color>> " +
            "<color=" + variableColorHexaDec + ">example1TreeViewItemData</color> = " +
            "<color=" + typeColorHexaDec + ">new</color> <color=" + classColorHexaDec + ">TreeViewItemData</color><<color=" + typeColorHexaDec + ">string</color>>(<color=" + floatColorHexaDec + ">0</color>, <color=" + stringColorHexaDec + ">\"Example 1\"</color>);" +

            "\n<color=" + classColorHexaDec + ">TreeViewItemData</color><<color=" + typeColorHexaDec + ">string</color>> " +
            "<color=" + variableColorHexaDec + ">example2TreeViewItemData</color> = " +
            "<color=" + typeColorHexaDec + ">new</color> <color=" + classColorHexaDec + ">TreeViewItemData</color><<color=" + typeColorHexaDec + ">string</color>>(<color=" + floatColorHexaDec + ">1</color>, <color=" + stringColorHexaDec + ">\"Example 2\"</color>);" +

            "\n<color=" + variableColorHexaDec + ">items</color>.<color=" + methodColorHexaDec + ">Add</color>(<color=" + variableColorHexaDec + ">example1TreeViewItemData</color>);" +
            "\n<color=" + variableColorHexaDec + ">items</color>.<color=" + methodColorHexaDec + ">Add</color>(<color=" + variableColorHexaDec + ">example2TreeViewItemData</color>);" +

            "\n\n<color=" + classColorHexaDec + ">Dictionary</color><<color=" + typeColorHexaDec + ">string</color>, <color=" + classColorHexaDec + ">VisualElement</color>> " +
            "<color=" + variableColorHexaDec + ">itemsVisualElementsDict</color> = <color=" + typeColorHexaDec + ">new</color> <color=" + classColorHexaDec + ">Dictionary</color><<color=" + typeColorHexaDec + ">string</color>, <color=" + classColorHexaDec + ">VisualElement</color>>();" +

            "\n<color=" + variableColorHexaDec + ">itemsVisualElementsDict</color>.<color=" + methodColorHexaDec + ">Add</color>(<color=" + stringColorHexaDec + ">\"Example 1\"</color>, <color=" + typeColorHexaDec + ">new</color> <color=" + classColorHexaDec + ">Label</color>(<color=" + stringColorHexaDec + ">\"I am Example 1\"</color>));" +
            "\n<color=" + variableColorHexaDec + ">itemsVisualElementsDict</color>.<color=" + methodColorHexaDec + ">Add</color>(<color=" + stringColorHexaDec + ">\"Example 2\"</color>, <color=" + typeColorHexaDec + ">new</color> <color=" + classColorHexaDec + ">Label</color>(<color=" + stringColorHexaDec + ">\"I am Example 2\"</color>));" +

            "\n\n<color=" + classColorHexaDec + ">SettingsPanel</color> <color=" + variableColorHexaDec + ">panel</color> = " +
            "<color=" + typeColorHexaDec + ">new</color> <color=" + classColorHexaDec + ">SettingsPanel</color>(<color=" + typeColorHexaDec + ">ref</color> <color=" + variableColorHexaDec + ">items</color>, <color=" + typeColorHexaDec + ">ref</color> <color=" + variableColorHexaDec + ">itemsVisualElementsDict</color>);" +

            "\n<color=" + variableColorHexaDec + ">panel</color>.style.width =  <color=" + floatColorHexaDec + ">310f</color>;" +
            "\n<color=" + variableColorHexaDec + ">panel</color>.style.height =  <color=" + floatColorHexaDec + ">310f</color>;";


        private static readonly string toolbarSearchPanelCodeExample =
            "<color=" + classColorHexaDec + ">List</color><<color=" + typeColorHexaDec + ">string</color>> " +
            "<color=" + variableColorHexaDec + ">toolbarSearchList</color> = <color=" + typeColorHexaDec + ">new</color> <color=" + classColorHexaDec + ">List</color><<color=" + typeColorHexaDec + ">string</color>>()" + " { " +
            "<color=" + stringColorHexaDec + ">\"Level Editor\"</color>, " +
            "<color=" + stringColorHexaDec + ">\"Terrain Licker\"</color>, " +
            "<color=" + stringColorHexaDec + ">\"Inspector Destroyer\"</color>, " +
            "<color=" + stringColorHexaDec + ">\"Mesh Consumer\"</color>};" +

            "\n<color=" + classColorHexaDec + ">List</color><<color=" + typeColorHexaDec + ">string</color>> " +
            "<color=" + variableColorHexaDec + ">resultList</color> = <color=" + typeColorHexaDec + ">new</color> <color=" + classColorHexaDec + ">List</color><<color=" + typeColorHexaDec + ">string</color>>();" +

            "\n\n<color=" + classColorHexaDec + ">ListView</color> " +
            "<color=" + variableColorHexaDec + ">listView</color> = <color=" + typeColorHexaDec + ">new</color> <color=" + classColorHexaDec + ">ListView</color>(<color=" + variableColorHexaDec + ">toolbarSearchList</color>, <color=" + floatColorHexaDec + ">15</color>);" +
            "\n<color=" + variableColorHexaDec + ">listView</color>.makeItem = () => <color=" + typeColorHexaDec + ">new</color> <color=" + classColorHexaDec + ">Label</color>();" +
            "\n<color=" + variableColorHexaDec + ">listView</color>.bindItem = (<color=" + variableColorHexaDec + ">element</color>, <color=" + variableColorHexaDec + ">index</color>) => (<color=" + variableColorHexaDec + ">element</color> <color=" + typeColorHexaDec + ">as</color> <color=" + classColorHexaDec + ">Label</color>).text = <color=" + variableColorHexaDec + ">listView</color>.itemsSource[<color=" + variableColorHexaDec + ">index</color>] <color=" + typeColorHexaDec + ">as</color> <color=" + typeColorHexaDec + ">string</color>;" +

            "\n\n<color=" + classColorHexaDec + ">Action</color> <color=" + variableColorHexaDec + ">OnEmpty</color> = () =>" +
            "{ " +
            "<color=" + variableColorHexaDec + ">listView</color>.itemsSource = <color=" + variableColorHexaDec + ">toolbarSearchList</color>;" +
            "<color=" + variableColorHexaDec + ">listView</color>.<color=" + methodColorHexaDec + ">Rebuild</color>();" +
            "};" +

            "\n<color=" + classColorHexaDec + ">Action</color> <color=" + variableColorHexaDec + ">OnFilled</color> = () =>" +
            "{ " +
            "<color=" + variableColorHexaDec + ">listView</color>.itemsSource = <color=" + variableColorHexaDec + ">resultList</color>;" +
            "<color=" + variableColorHexaDec + ">listView</color>.<color=" + methodColorHexaDec + ">Rebuild</color>();" +
            "};" +

            "\n\n<color=" + classColorHexaDec + ">ToolbarSearchPanel</color> " +
            "<color=" + variableColorHexaDec + ">toolbarSearchPanel</color> = <color=" + typeColorHexaDec + ">new</color> <color=" + classColorHexaDec + ">ToolbarSearchPanel</color>(<color=" + variableColorHexaDec + ">toolbarSearchList</color>, <color=" + variableColorHexaDec + ">resultList</color>, <color=" + variableColorHexaDec + ">OnEmpty</color>, <color=" + variableColorHexaDec + ">OnFilled</color>);";



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
            Header attributes = new Header(GlobalVariables.AttributesName, 14f);
            attributes.style.marginLeft = marginLeftRight;
            attributes.style.marginBottom = 4f;
            Line attributeLine = new Line();
            attributeLine.style.marginLeft = marginLeftRight;
            attributeLine.style.marginRight = marginLeftRight;

            rootVisualElement.Add(attributes);
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
            VisualElement readonlyDocumentation = MakeDocumentationElement(nameof(ReadOnlyAttribute), readonlyAttributeInfo, readonlyExample);

            rootVisualElement.Add(readonlyDocumentation);
        }
        #endregion

        #region UIElements
        private static void UIElementsHeader()
        {
            Header uiElements = new Header(GlobalVariables.UIElementsName, 14f);
            uiElements.style.marginLeft = marginLeftRight;
            uiElements.style.marginBottom = 4f;
            uiElements.style.marginTop = 8f;

            Line uiElementsLine = new Line();
            uiElementsLine.style.marginLeft = marginLeftRight;
            uiElementsLine.style.marginRight = marginLeftRight;

            rootVisualElement.Add(uiElements);
            rootVisualElement.Add(uiElementsLine);
        }
        private static void HeaderExample()
        {
            Header header = new Header("Basic " + nameof(LibrariesHeader));
            VisualElement headerExample = LibraryExampleElement(headerCodeExample, null, header);
            VisualElement headerDocumentation = MakeDocumentationElement(nameof(LibrariesHeader), headerUIElementInfo, headerExample);

            rootVisualElement.Add(headerDocumentation);
        }
        private static void InfoBoxExample()
        {
            InfoBox infoBox = new InfoBox("An " + nameof(InfoBox) + " can be used to give information", InfoBoxIconType.Info, 3f);
            VisualElement infoBoxExample = LibraryExampleElement(infoBoxCodeExample, null, infoBox);
            VisualElement infoBoxDocumentation = MakeDocumentationElement(nameof(InfoBox), infoBoxUIElementInfo, infoBoxExample);

            rootVisualElement.Add(infoBoxDocumentation);
        }
        private static void LineExample()
        {
            Line line = new Line(4f, false, Color.red);
            line.style.height = 1f;
            line.style.width = 120f;

            VisualElement lineExample = LibraryExampleElement(lineCodeExample, null, line);
            VisualElement lineDocumentation = MakeDocumentationElement(nameof(Line), lineUIElementInfo, lineExample);

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
            VisualElement settingsPanelDocumentation = MakeDocumentationElement(nameof(SettingsPanel), settingsPanelUIElementInfo, settingsPanelExample);

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
            VisualElement toolbarSearchPanelDocumentation = MakeDocumentationElement(nameof(ToolbarSearchPanel), toolbarSearchPanelUIElementInfo, toolbarSearchPanelExample);
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