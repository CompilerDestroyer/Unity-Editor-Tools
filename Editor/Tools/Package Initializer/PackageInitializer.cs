using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using UnityEditor;
using System.Linq;
using System.IO;
using CompilerDestroyer.Editor.UIElements;
using System.Text;


namespace CompilerDestroyer.Editor.EditorTools
{
    internal class PackageInitializer
    {
        private static string packagesInitializerInfo = "When Editor Tools first installed into a project, " + GlobalVariables.PackagesInitializerName + " " +
            "will remove all of the false packages in the list below. It will be available in all projects that installs Editor Tools.";
        private static readonly string savePath = PackageInitializerSave.instance.GetSavePath();
        private static int globalMarginLeftRight = 15;
        private static int globalMarginBottom = 20;
        private static int globalMiniBottomMargin = 7;

        private static AddAndRemoveRequest addRemoveOfPackageInitializer;
        private static ListView builtInPackagesListView;
        private static ListView customPackageListView;
        private static ListView assetStorePackagesListView;


        [InitializeOnLoadMethod]
        private static void InitializePackage()
        {
            if (PackageInitializerSave.instance != null)
            {
                if (!PackageInitializerSave.instance.isPackageInitializerEnabled) return;
            }


            if (!SessionState.GetBool("isAlreadyInitializedPackageInitializer", false))
            {
                SessionState.SetBool("isAlreadyInitializedPackageInitializer", true);

                if (PackageInitializerSave.instance != null)
                {
                    if (!PackageInitializerSave.instance.isPackageInitializerAlreadyRan)
                    {



                        PackageInitializerSave.instance.isPackageInitializerAlreadyRan = true;
                        PackageInitializerSave.instance.Save();
                    }
                }
            }
        }


        internal static VisualElement PackageInitializerVisualElement()
        {
            VisualElement rootVisualElement = new VisualElement();


            VisualElement spacer = new VisualElement();
            spacer.style.height = 5f;
            spacer.style.whiteSpace = WhiteSpace.Normal;
            rootVisualElement.Add(spacer);

            VisualElement toolLabelAndDisableContainer = new VisualElement();
            toolLabelAndDisableContainer.style.marginBottom = globalMarginBottom;
            toolLabelAndDisableContainer.style.flexDirection = FlexDirection.Row;

            Label toolLabel = new Label(GlobalVariables.PackagesInitializerName);
            toolLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            toolLabel.style.fontSize = 18;
            toolLabel.style.whiteSpace = WhiteSpace.Normal;
            toolLabel.style.marginLeft = globalMarginLeftRight;
            toolLabelAndDisableContainer.Add(toolLabel);

            Toggle disablePackageInitializer = new Toggle();
            disablePackageInitializer.style.alignSelf = Align.FlexEnd;

            disablePackageInitializer.value = true;
            toolLabelAndDisableContainer.Add(disablePackageInitializer);
            rootVisualElement.Add(toolLabelAndDisableContainer);


            VisualElement WholePackageInitializerContainer = new VisualElement();
            disablePackageInitializer.RegisterCallback<ChangeEvent<bool>>(evt =>
            {
                WholePackageInitializerContainer.SetEnabled(disablePackageInitializer.value);
                PackageInitializerSave.instance.isPackageInitializerEnabled = disablePackageInitializer.value;
                PackageInitializerSave.instance.Save();
            });


            InfoBox packagesInitializerBox = new InfoBox(packagesInitializerInfo + "\n", InfoBoxIconType.Info, 10f);
            packagesInitializerBox.style.marginBottom = globalMiniBottomMargin;
            packagesInitializerBox.style.marginLeft = globalMarginLeftRight;
            packagesInitializerBox.style.marginRight = globalMarginLeftRight;
            WholePackageInitializerContainer.Add(packagesInitializerBox);

            VisualElement savePathContainer = new VisualElement();
            savePathContainer.style.flexDirection = FlexDirection.Row;

            Label savePathLabel = new Label("Save Path: ");
            savePathLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            savePathContainer.Add(savePathLabel);

            TextField savePathTextField = new TextField();
            savePathTextField.style.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.3f);
            savePathTextField.value = savePath;
            savePathTextField.style.marginLeft = 15;
            savePathTextField.style.marginRight = 15;
            savePathTextField.style.whiteSpace = WhiteSpace.Normal;
            savePathTextField.style.flexShrink = 1f;
            savePathTextField.isReadOnly = true;
            savePathTextField.selectAllOnMouseUp = true;
            savePathTextField.multiline = true;
            savePathContainer.Add(savePathTextField);
            packagesInitializerBox.Add(savePathContainer);

            VisualElement builtinPackageList = BuiltInPackagesList();
            builtinPackageList.style.maxHeight = 300f;
            builtinPackageList.style.marginLeft = globalMarginLeftRight;
            builtinPackageList.style.marginRight = globalMarginLeftRight;
            builtinPackageList.style.marginBottom = globalMiniBottomMargin;
            WholePackageInitializerContainer.Add(builtinPackageList);

            VisualElement customPackageList = CustomPackageList();
            customPackageList.style.maxHeight = 300f;
            customPackageList.style.marginLeft = globalMarginLeftRight;
            customPackageList.style.marginRight = globalMarginLeftRight;
            customPackageList.style.marginBottom = globalMiniBottomMargin;

            WholePackageInitializerContainer.Add(customPackageList);

            VisualElement assetStorePackageList = AssetStorePackagesListView();
            assetStorePackageList.style.maxHeight = 300f;
            assetStorePackageList.style.marginLeft = globalMarginLeftRight;
            assetStorePackageList.style.marginRight = globalMarginLeftRight;
            assetStorePackageList.style.marginBottom = globalMarginLeftRight;

            WholePackageInitializerContainer.Add(assetStorePackageList);

            Button updateButton = new Button();
            updateButton.text = "Update Packages";
            updateButton.style.marginLeft = globalMarginLeftRight;
            updateButton.style.marginRight = globalMarginLeftRight;
            updateButton.style.marginBottom = globalMiniBottomMargin;
            updateButton.clicked += () =>
            {
                List<string> addList = new List<string>();
                List<string> removeList = new List<string>();

                if (PackageInitializerSave.instance != null)
                {
                    // Install or remove built-in unity packages
                    for (int i = 0; i < PackageInitializerSave.instance.builtInPackages.Count; i++)
                    {
                        Package currentBuiltinPackage = PackageInitializerSave.instance.builtInPackages[i];
                        if (currentBuiltinPackage.shouldPackageInstalled)
                        {
                            addList.Add(currentBuiltinPackage.packageName);
                        }
                        else if (!currentBuiltinPackage.shouldPackageInstalled)
                        {
                            removeList.Add(currentBuiltinPackage.packageName);
                        }
                    }

                    // Install or remove git packages
                    for (int i = 0; i < PackageInitializerSave.instance.customPackages.Count; i++)
                    {
                        Package currentGitPackage = PackageInitializerSave.instance.customPackages[i];
                        if (currentGitPackage.shouldPackageInstalled)
                        {
                            addList.Add(currentGitPackage.packageName);
                        }
                        else if (!currentGitPackage.shouldPackageInstalled)
                        {
                            removeList.Add(currentGitPackage.packageName);
                        }
                    }

                    // Install or remove asset store packages
                    List<string> unityPackages = FindUnityPackages(GlobalVariables.CurrentAssetStorePath);
                    if (unityPackages.Count > 0)
                    {
                        for (int i = 0; i < PackageInitializerSave.instance.assetStorePackages.Count; i++)
                        {
                            Package currentAssetStorePackage = PackageInitializerSave.instance.assetStorePackages[i];

                            if (currentAssetStorePackage.shouldPackageInstalled)
                            {
                                string currentPackageInstallPath = unityPackages.Find((packageName) => Path.GetFileNameWithoutExtension(packageName) == currentAssetStorePackage.packageName);
                                AssetDatabase.ImportPackage(currentPackageInstallPath, false);
                            }
                        }
                    }

                }

                StringBuilder a = new StringBuilder();
                for (int i = 0; i < addList.ToArray().Length; i++)
                {
                    a.Append(addList[i] + "\n");
                }
                StringBuilder b = new StringBuilder();
                for (int i = 0; i < removeList.ToArray().Length; i++)
                {
                    b.Append(removeList[i] + "\n");
                }
                File.WriteAllText(Application.dataPath + "\\added.txt", a.ToString());
                File.WriteAllText(Application.dataPath + "\\removed.txt", b.ToString());
                AssetDatabase.Refresh();
                if (addList.Count > 0 || removeList.Count > 0)
                {
                    addRemoveOfPackageInitializer = Client.AddAndRemove(addList.ToArray(), removeList.ToArray());
                    EditorApplication.update += AddOrRemoveProgress;
                }
                AssetDatabase.Refresh();
            };
            WholePackageInitializerContainer.Add(updateButton);


            rootVisualElement.Add(WholePackageInitializerContainer);
            return rootVisualElement;
        }
        

        private static VisualElement BuiltInPackagesList()
        {
            VisualElement rootVisualElement = new VisualElement();

            builtInPackagesListView = new ListView(
               PackageInitializerSave.instance.builtInPackages,
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


                   if (PackageInitializerSave.instance.builtInPackages.Count > 0 && PackageInitializerSave.instance.builtInPackages != null)
                   {
                       toggle.value = PackageInitializerSave.instance.builtInPackages[index].shouldPackageInstalled;
                       label.text = PackageInitializerSave.instance.builtInPackages[index].packageName;
                   }

                   toggle.RegisterCallback<ClickEvent>((clickEvent) =>
                   {
                       toggle.RegisterValueChangedCallback(evt =>
                       {
                           if (evt.currentTarget == toggle)
                           {
                               toggle.value = evt.newValue;

                               PackageInitializerSave.instance.builtInPackages[index].shouldPackageInstalled = toggle.value;
                               PackageInitializerSave.instance.Save();
                           }
                       });
                   });

                   label.RegisterCallback<ClickEvent>(evt =>
                   {
                       if (evt.currentTarget == label)
                       {
                           toggle.value = !toggle.value;
                           Package clickedPackage = builtInPackagesListView.selectedItem as Package;
                           clickedPackage.shouldPackageInstalled = toggle.value;

                           PackageInitializerSave.instance.Save();
                           evt.StopImmediatePropagation();
                       }
                   });

               });
            builtInPackagesListView.selectionType = SelectionType.Single;
            builtInPackagesListView.style.flexGrow = 1;
            builtInPackagesListView.showFoldoutHeader = true;
            builtInPackagesListView.showBoundCollectionSize = false;
            builtInPackagesListView.showBorder = true;
            builtInPackagesListView.headerTitle = "Built-in Packages";
            builtInPackagesListView.viewDataKey = "BuiltInPackages";


            PackageInitializerSave.instance.Save();
            rootVisualElement.Add(builtInPackagesListView);

            return rootVisualElement;
        }
        private static VisualElement CustomPackageList()
        {
            VisualElement rootVisualElement = new VisualElement();

            customPackageListView = new ListView(PackageInitializerSave.instance.customPackages, 24, MakeItemCustomPackageListView, BindCustomPackageListView);

            customPackageListView.onAdd = view =>
            {
                view.itemsSource.Add(new Package());
                PackageInitializerSave.instance.Save();

                customPackageListView.Rebuild();
            };
            customPackageListView.onRemove = view =>
            {
                List<Package> list = view.itemsSource as List<Package>;

                if (view.selectedItem == null)
                {
                    int itemsSourceCount = view.itemsSource.Count;
                    if (itemsSourceCount - 1 != -1)
                    {
                        list.RemoveAt(itemsSourceCount - 1);
                        PackageInitializerSave.instance.Save();
                        customPackageListView.Rebuild();
                    }
                }
                else
                {
                    list.RemoveAt(view.selectedIndex);
                    PackageInitializerSave.instance.Save();
                    customPackageListView.Rebuild();
                }

            };
            customPackageListView.itemsAdded += addedItems =>
            {
                PackageInitializerSave.instance.Save();
                customPackageListView.Rebuild();

            };
            customPackageListView.itemsRemoved += removedItems =>
            {
                if (removedItems.Count() == PackageInitializerSave.instance.customPackages.Count)
                {
                    EditorApplication.update += RemoveItemsWhenZeroEnteredToCollectionSize;
                    return;
                }
                PackageInitializerSave.instance.Save();
                customPackageListView.Rebuild();

            };




            customPackageListView.viewDataKey = "Custom Packages";
            customPackageListView.selectionType = SelectionType.Single;
            customPackageListView.style.flexGrow = 1;
            customPackageListView.showFoldoutHeader = true;
            customPackageListView.showAddRemoveFooter = true;
            customPackageListView.showBoundCollectionSize = true;
            customPackageListView.showBorder = true;
            customPackageListView.headerTitle = "Packages";

            rootVisualElement.Add(customPackageListView);

            return rootVisualElement;
        }
        private static void RemoveItemsWhenZeroEnteredToCollectionSize()
        {
            if (customPackageListView.itemsSource.Count != 0) return;

            PackageInitializerSave.instance.Save();
            customPackageListView.Rebuild();

            EditorApplication.update -= RemoveItemsWhenZeroEnteredToCollectionSize;
        }
        private static VisualElement MakeItemCustomPackageListView()
        {
            VisualElement row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.justifyContent = Justify.SpaceBetween;
            row.style.paddingLeft = 10;
            row.style.paddingRight = 10;

            Toggle toggle = new Toggle();
            toggle.style.marginRight = 10;

            TextField textField = new TextField();
            textField.style.flexGrow = 1;
            textField.style.unityTextAlign = TextAnchor.MiddleLeft;

            row.Add(toggle);
            row.Add(textField);

            return row;
        }
        private static void BindCustomPackageListView(VisualElement element, int index)
        {
            Toggle toggle = element.Q<Toggle>();
            TextField textField = element.Q<TextField>();


            if (index < PackageInitializerSave.instance.customPackages.Count && PackageInitializerSave.instance.customPackages != null)
            {
                if (PackageInitializerSave.instance.customPackages[index] != null)
                {
                    toggle.value = PackageInitializerSave.instance.customPackages[index].shouldPackageInstalled;
                    textField.value = PackageInitializerSave.instance.customPackages[index].packageName;
                }
            }


            toggle.RegisterCallback<ChangeEvent<bool>>(ChangeEvent =>
            {
                PackageInitializerSave.instance.customPackages[index].shouldPackageInstalled = ChangeEvent.newValue;
                PackageInitializerSave.instance.Save();
            });

            textField.RegisterCallback<ChangeEvent<string>>(ChangeEvent =>
            {
                if (index < PackageInitializerSave.instance.customPackages.Count)
                {
                    PackageInitializerSave.instance.customPackages[index].packageName = ChangeEvent.newValue;
                    PackageInitializerSave.instance.Save();
                }
            });
        }
        private static void AddOrRemoveProgress()
        {
            if (addRemoveOfPackageInitializer.IsCompleted)
            {
                EditorApplication.update -= AddOrRemoveProgress;
            }
        }
        private static VisualElement AssetStorePackagesListView()
        {
            VisualElement rootVisualElement = new VisualElement();
            List<string> unityPackages = FindUnityPackages(GlobalVariables.CurrentAssetStorePath);

            if (PackageInitializerSave.instance.assetStorePackages != null)
            {
                for (int i = 0; i < unityPackages.Count; i++)
                {
                    string currentPackageName = unityPackages[i];
                    PackageInitializerSave.instance.assetStorePackages.Add(new Package(Path.GetFileNameWithoutExtension(currentPackageName), false));
                }
            }


            assetStorePackagesListView = new ListView(PackageInitializerSave.instance.assetStorePackages, 30, MakeItemAssetStorePackagesListView, BindAssetStorePackagesListView);

            assetStorePackagesListView.selectionType = SelectionType.Multiple;
            assetStorePackagesListView.style.flexGrow = 1;
            assetStorePackagesListView.showFoldoutHeader = true;
            assetStorePackagesListView.showBoundCollectionSize = false;
            assetStorePackagesListView.showBorder = true;
            assetStorePackagesListView.headerTitle = "Asset Store Packages";
            assetStorePackagesListView.viewDataKey = "AssetStorePackages";
            rootVisualElement.Add(assetStorePackagesListView);
            return rootVisualElement;
        }
        private static VisualElement MakeItemAssetStorePackagesListView()
        {
            VisualElement row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.justifyContent = Justify.SpaceBetween;
            row.style.paddingLeft = 10;
            row.style.paddingRight = 10;

            Toggle toggle = new Toggle();
            Image image = new Image();
            Label label = new Label();
            toggle.style.marginRight = 10f;
            image.style.width = 20f;
            image.style.height = 20f;
            image.style.marginRight = 10f;
            image.style.flexDirection = FlexDirection.Row;
            image.style.alignItems = Align.Center;
            image.style.justifyContent = Justify.FlexStart;
            image.style.height = Length.Percent(100);
            image.image = EditorGUIUtility.IconContent(GlobalVariables.UnityLogoIconName).image as Texture2D;
            label.style.flexGrow = 1;
            label.style.unityTextAlign = TextAnchor.MiddleLeft;


            row.Add(toggle);
            row.Add(image);
            row.Add(label);

            return row;
        }
        private static void BindAssetStorePackagesListView(VisualElement element, int index)
        {
            Toggle toggle = element.Q<Toggle>();
            Image image = element.Q<Image>();
            Label label = element.Q<Label>();


            if (PackageInitializerSave.instance.assetStorePackages.Count > 0 && PackageInitializerSave.instance.assetStorePackages != null)
            {
                toggle.value = PackageInitializerSave.instance.assetStorePackages[index].shouldPackageInstalled;
                label.text = PackageInitializerSave.instance.assetStorePackages[index].packageName;
            }


            toggle.RegisterCallback<ClickEvent>((clickEvent) =>
            {
                toggle.RegisterValueChangedCallback(evt =>
                {
                    if (evt.currentTarget == toggle)
                    {
                        toggle.value = evt.newValue;

                        PackageInitializerSave.instance.assetStorePackages[index].shouldPackageInstalled = toggle.value;
                        PackageInitializerSave.instance.Save();
                    }
                });
            });

            image.RegisterCallback<ClickEvent>(evt =>
            {
                if (evt.currentTarget == image)
                {
                    toggle.value = !toggle.value;
                    Package clickedPackage = assetStorePackagesListView.selectedItem as Package;
                    clickedPackage.shouldPackageInstalled = toggle.value;

                    PackageInitializerSave.instance.Save();
                    evt.StopImmediatePropagation();
                }
            });

            label.RegisterCallback<ClickEvent>(evt =>
            {
                if (evt.currentTarget == label)
                {
                    toggle.value = !toggle.value;

                    Package clickedPackage = assetStorePackagesListView.selectedItem as Package;
                    clickedPackage.shouldPackageInstalled = toggle.value;

                    PackageInitializerSave.instance.Save();
                    evt.StopImmediatePropagation();
                }
            });
        }

        private static List<string> FindUnityPackages(string assetStorePath)
        {
            List<string> unityPackages = new List<string>();

            if (Directory.Exists(assetStorePath))
            {
                string[] unityPackageFiles = Directory.GetFiles(assetStorePath, "*.unitypackage", SearchOption.AllDirectories);

                if (unityPackageFiles.Length > 0)
                {
                    for (int i = 0; i < unityPackageFiles.Length; i++)
                    {
                        unityPackages.Add(unityPackageFiles[i]);
                    }
                }
                else
                {
                    Debug.Log("There are no .unitypackage files found.");
                }
            }
            else
            {
                Debug.Log($"Directory not found: {assetStorePath}");
            }
            if (unityPackages.Count > 0)
            {
                unityPackages.Distinct().ToList().Sort();
            }

            return unityPackages;
        }

        [MenuItem("Tools/Dene")]
        static void DeneOc()
        {
            searchRequest = Client.SearchAll();

            EditorApplication.update += ListBuiltInPackagesProgress;
        }

        private static SearchRequest searchRequest;
        private static List<string> currentBuiltInPackageNames = new List<string>();
        private static List<string> deprecated = new List<string>()
        {
            "com.unity.modules.hierarchycore",
            "com.unity.modules.subsystems",
            "com.unity.burst",
            "com.unity.collections",
            "com.unity.render-pipelines.core",
            "com.unity.ext.nunit",
            "com.unity.mathematics",
            "com.unity.nuget.mono-cecil",
            "com.unity.test-framework.performance",
            "com.unity.searcher",
            "com.unity.shadergraph",
            "com.unity.rendering.light-transport",
            "com.unity.render-pipelines.universal-config",
            ""
        };

        private static void ListBuiltInPackagesProgress()
        {
            if (searchRequest.IsCompleted)
            {
                if (searchRequest.Status == StatusCode.Success)
                {
                    foreach (var package in searchRequest.Result)
                    {
                        currentBuiltInPackageNames.Add(package.name);
                    }
                }

                EditorApplication.update -= ListBuiltInPackagesProgress;

                foreach (var package in searchRequest.Result)
                {
                    currentBuiltInPackageNames.Add(package.name);
                }



                //currentBuiltInPackageNames.RemoveAll(item => deprecated.Contains(item));

                //List<string> uniqueList = currentBuiltInPackageNames.Distinct().ToList();
                //currentBuiltInPackageNames = uniqueList;
                //currentBuiltInPackageNames.Sort();

                foreach (var item in currentBuiltInPackageNames)
                {
                    Debug.Log(item);
                }
            }
        }
    }
}

