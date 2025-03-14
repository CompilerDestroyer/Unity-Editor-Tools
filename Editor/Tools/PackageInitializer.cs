using UnityEngine;
using UnityEngine.UIElements;
using UnityEditorInternal;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using UnityEditor;
using System.IO;


namespace CompilerDestroyer.Editor.EditorTools
{

    [FilePath("com.compilerdestroyer.editortools.packageinitializer/packageinitializer.save", FilePathAttribute.Location.PreferencesFolder)]
    internal class PackageInitializerSave : ScriptableSingleton<PackageInitializerSave>
    {

    }


    internal class PackageInitializer
    {
        private static string packagesInitializerName = "Packages Initializer";
        private static string packagesInitializerInfo = "Packages in the ";
        private static ListRequest listRequest;
        private static int globalMarginLeftRight = 15;

        internal static VisualElement PackageInitializerVisualElement()
        {
            VisualElement rootVisualElement = new VisualElement();

            VisualElement spacer = new VisualElement();
            spacer.style.height = 5f;
            spacer.style.whiteSpace = WhiteSpace.Normal;
            rootVisualElement.Add(spacer);

            Label toolLabel = new Label(packagesInitializerName);
            toolLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            toolLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            toolLabel.style.fontSize = 18;
            toolLabel.style.whiteSpace = WhiteSpace.Normal;
            toolLabel.style.marginBottom = 20f;
            toolLabel.style.marginLeft = globalMarginLeftRight;
            rootVisualElement.Add(toolLabel);

            //Label packagesInitializerLabel = new Label(packagesInitializerInfo);
            //packagesInitializerLabel.style.marginLeft = globalMarginLeftRight;
            //rootVisualElement.Add(packagesInitializerLabel);


            listRequest = Client.List(true);
            EditorApplication.update += Progress;

            VisualElement builtinPackageList = BuiltInPackagesList();
            builtinPackageList.style.marginLeft = globalMarginLeftRight;
            builtinPackageList.style.marginRight = globalMarginLeftRight;
            rootVisualElement.Add(builtinPackageList);

            Button button = new Button();
            button.text = "Remove";
            button.clicked += () =>
            {
                addAndRemoveRequest = Client.AddAndRemove(null, builtInPackageNames.ToArray());
                EditorApplication.update += AddOrRemoveProgress;
            };


            rootVisualElement.Add(RemoveList());

            rootVisualElement.Add(button);
            return rootVisualElement;
        }
        private static AddAndRemoveRequest addAndRemoveRequest;

        private static List<string> builtInPackageNames = new List<string>();
        private static void AddOrRemoveProgress()
        {
            if (listRequest.IsCompleted)
            {
                if (listRequest.Status == StatusCode.Success)
                {
                    Debug.Log("Sucess");
                }

                EditorApplication.update -= AddOrRemoveProgress;
            }

        }
        private static void Progress()
        {
            if (listRequest.IsCompleted)
            {
                if (listRequest.Status == StatusCode.Success)
                {
                    foreach (var package in listRequest.Result)
                    {
                        // Check for built-in packages
                        if (package.name.StartsWith("com.unity"))
                        {
                            builtInPackageNames.Add(package.name);
                        }
                    }
                }

                EditorApplication.update -= Progress;
            }
        }
        private static ListView builtInPackagesListView;
        private static VisualElement BuiltInPackagesList()
        {
            VisualElement rootVisualElement = new VisualElement();

            builtInPackagesListView = new ListView(
                builtInPackageNames,
                itemHeight: 24,
                makeItem: () =>
                {
                    VisualElement row = new VisualElement();
                    row.style.flexDirection = FlexDirection.Row;
                    row.style.justifyContent = Justify.SpaceBetween;
                    row.style.paddingLeft = 10;
                    row.style.paddingRight = 10;

                    Toggle toggle = new Toggle();
                    toggle.style.marginRight = 10;

                    Label label = new Label();
                    label.style.flexGrow = 1;
                    label.style.unityTextAlign = TextAnchor.MiddleLeft;

                    row.Add(toggle);
                    row.Add(label);

                    return row;
                },
                bindItem: (element, index) =>
                {
                    Toggle toggle = element.Q<Toggle>();
                    Label label = element.Q<Label>();

                    label.text = builtInPackageNames[index];

                    toggle.RegisterValueChangedCallback(evt =>
                    {
                        Debug.Log($"Toggled '{builtInPackageNames[index]}' = {evt.newValue}");
                    });
                    label.RegisterCallback<ClickEvent>(evt =>
                    {
                        toggle.value = !toggle.value;
                    });

                });

            builtInPackagesListView.selectionType = SelectionType.None;
            builtInPackagesListView.style.flexGrow = 1;
            builtInPackagesListView.showFoldoutHeader = true;
            builtInPackagesListView.showBoundCollectionSize = false;
            builtInPackagesListView.headerTitle = "Built-in Packages";

            rootVisualElement.Add(builtInPackagesListView);

            return rootVisualElement;
        }
        private static VisualElement RemoveList()
        {
            VisualElement rootVisualElement = new VisualElement();

            List<string> items = new List<string> { "Item 1", "Item 2", "Item 3", "Item 4" };

            ListView listView = new ListView(
                items,
                itemHeight: 22,
                makeItem: () =>
                {
                    // Container with margins
                    VisualElement container = new VisualElement();
                    container.style.marginLeft = 15;
                    container.style.marginRight = 15;

                    TextField textField = new TextField();
                    textField.style.flexGrow = 1;

                    container.Add(textField);
                    return container;
                },
                bindItem: (element, i) =>
                {
                    TextField textField = element.Q<TextField>();
                    textField.value = items[i];

                    // Sync back to data list
                    textField.RegisterValueChangedCallback(evt =>
                    {
                        items[i] = evt.newValue;
                    });
                }
            );

            listView.style.flexGrow = 1;
            listView.showAddRemoveFooter = true;
            listView.headerTitle = "Remove List";
            listView.showFoldoutHeader = true;
            return rootVisualElement;

        }
    }
}