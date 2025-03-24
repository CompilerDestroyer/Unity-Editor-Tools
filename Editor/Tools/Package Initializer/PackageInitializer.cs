using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using UnityEditor;
using System.Linq;
using System.IO;
using CompilerDestroyer.Editor.UIElements;

using PackageInfo = UnityEditor.PackageManager.PackageInfo;
using System;

namespace CompilerDestroyer.Editor.EditorTools
{
    internal class PackageInitializer
    {
        private static string packagesInitializerInfo = "When Editor Tools first installed into a project, " + GlobalVariables.PackagesInitializerName + " " +
            "will remove all of the false packages in the list below. It will be available in all projects that installs Editor Tools.";
        private static readonly string savePath = PackageInitializerSave.instance.GetSavePath();
        private static readonly int globalMarginLeftRight = 15;
        private static readonly int globalMarginBottom = 20;
        private static readonly int globalMiniBottomMargin = 7;


        private static List<string> currentBuiltinPackageNames = new List<string>();
        private static ListRequest listBuiltInPackages;
        private static List<string> removeCoreUnityPackages = new List<string>()
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
            "com.unity.render-pipelines.universal-config"
        };
        private static SearchRequest builtInPackageSearchRequest;
        private static AddAndRemoveRequest addRemoveOfPackageInitializer;

        private static ListView builtInPackagesListView;

        private static List<Package> temporaryBuiltinPackagesList;
        private static List<string> builtInPackagesSearchList = new List<string>();
        private static List<string> builtInPackagesSearchResultList = new List<string>();
        private static ToolbarSearchPanel builtInPackagesSearchPanel = new ToolbarSearchPanel();

        private static List<Package> temporaryCustomPackagesList;
        private static List<string> customPackagesSearchlist = new List<string>();
        private static List<string> customPackagesSearchResultList = new List<string>();
        private static ToolbarSearchPanel customPackagesSearchPanel = new ToolbarSearchPanel();
        private static ToolbarSearchPanel assetStorePackagesPanel = new ToolbarSearchPanel();

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
        private static VisualElement WholePackageInitializerContainer;
        internal static VisualElement PackageInitializerVisualElement()
        {
            VisualElement rootVisualElement = new VisualElement();


            VisualElement spacer = new VisualElement();
            spacer.style.height = 5f;
            spacer.style.whiteSpace = WhiteSpace.Normal;

            VisualElement toolLabelAndDisableContainer = new VisualElement();
            toolLabelAndDisableContainer.style.marginBottom = globalMarginBottom;
            toolLabelAndDisableContainer.style.flexDirection = FlexDirection.Row;

            Label toolLabel = new Label(GlobalVariables.PackagesInitializerName);
            toolLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            toolLabel.style.fontSize = 18;
            toolLabel.style.whiteSpace = WhiteSpace.Normal;
            toolLabel.style.marginLeft = globalMarginLeftRight;




            Toggle disablePackageInitializer = new Toggle();
            disablePackageInitializer.style.alignSelf = Align.FlexEnd;

            disablePackageInitializer.value = true;


            WholePackageInitializerContainer = new VisualElement();
            disablePackageInitializer.RegisterCallback<ChangeEvent<bool>>(evt =>
            {
                WholePackageInitializerContainer.SetEnabled(disablePackageInitializer.value);
                PackageInitializerSave.instance.isPackageInitializerEnabled = disablePackageInitializer.value;
                PackageInitializerSave.instance.Save();
            });



            VisualElement savePathContainer = new VisualElement();
            savePathContainer.style.flexDirection = FlexDirection.Row;
            savePathContainer.style.marginLeft = globalMarginLeftRight;
            savePathContainer.style.marginRight = globalMarginLeftRight;

            Label savePathLabel = new Label("Save Path: ");

            savePathLabel.style.unityTextAlign = TextAnchor.MiddleLeft;


            TextField savePathTextField = new TextField();
            savePathTextField.style.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.3f);
            savePathTextField.value = savePath;
            savePathTextField.style.whiteSpace = WhiteSpace.Normal;
            savePathTextField.style.flexShrink = 1f;
            savePathTextField.isReadOnly = true;
            savePathTextField.selectAllOnMouseUp = true;
            savePathTextField.multiline = true;


            if (PackageInitializerSave.instance.builtInPackages.Count == 0)
            {
                listBuiltInPackages = Client.List(false, true);
                EditorApplication.update += ListBuiltInPackagesProgress;
                EditorUtility.DisplayProgressBar(GlobalVariables.PackagesInitializerName, "Loading Built-In Packages", 0.5f);
            }
            else
            {
                BuiltInPackagesSearchPanel();

                builtInPackagesSearchPanel = new ToolbarSearchPanel(builtInPackagesSearchList, builtInPackagesSearchResultList, BuiltInPackageSearchIsEmpty, BuiltInPackageSearchIsFilled);
                builtInPackagesSearchPanel.style.alignSelf = Align.FlexEnd;
                builtInPackagesSearchPanel.style.marginRight = globalMarginLeftRight;

            }


            VisualElement builtInPackageList = BuiltInPackagesList();
            PackageInitializerSave.instance.Save();
            builtInPackagesListView.style.maxHeight = 300f;
            builtInPackagesListView.style.marginLeft = globalMarginLeftRight;
            builtInPackagesListView.style.marginRight = globalMarginLeftRight;
            builtInPackagesListView.style.marginBottom = globalMiniBottomMargin;


            CustomPackagesSearchPanel();
            customPackagesSearchPanel = new ToolbarSearchPanel(customPackagesSearchlist, customPackagesSearchResultList, CustomPackageSearchIsEmpty, CustomPackageSearchIsFilled);
            customPackagesSearchPanel.style.alignSelf = Align.FlexEnd;
            customPackagesSearchPanel.style.marginRight = globalMarginLeftRight;

            VisualElement customPackageList = CustomPackageList();
            customPackageList.style.maxHeight = 300f;
            customPackageList.style.marginLeft = globalMarginLeftRight;
            customPackageList.style.marginRight = globalMarginLeftRight;
            customPackageList.style.marginBottom = globalMiniBottomMargin;


            VisualElement assetStorePackageList = AssetStorePackagesListView();
            assetStorePackageList.style.maxHeight = 300f;
            assetStorePackageList.style.marginLeft = globalMarginLeftRight;
            assetStorePackageList.style.marginRight = globalMarginLeftRight;
            assetStorePackageList.style.marginBottom = globalMarginLeftRight;


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
                        PackageInfo currentBuiltInPackageInfo = PackageInfo.FindForPackageName(currentBuiltinPackage.packageName);
                        
                        if (currentBuiltinPackage.shouldPackageInstalled)
                        {
                            if (currentBuiltInPackageInfo == null)
                            {
                                addList.Add(currentBuiltinPackage.packageName);
                            }
                        }
                        else if (!currentBuiltinPackage.shouldPackageInstalled)
                        {
                            if (currentBuiltInPackageInfo != null)
                            {

                                if (!currentBuiltInPackageInfo.isDirectDependency)
                                {
                                    Debug.LogError($"Package: [{currentBuiltInPackageInfo.name}] is installed as dependency updating packages will stop. Do not try to remove it directly.");

                                    Package package = PackageInitializerSave.instance.builtInPackages.Find((package) => package.packageName == currentBuiltinPackage.packageName);

                                    package.shouldPackageInstalled = true;
                                    PackageInitializerSave.instance.Save();

                                    builtInPackagesListView.Rebuild();
                                }
                                else
                                {
                                    removeList.Add(currentBuiltinPackage.packageName);
                                }
                            }
                        }
                    }

                    // Install or remove git packages
                    for (int i = 0; i < PackageInitializerSave.instance.customPackages.Count; i++)
                    {
                        Package currentGitPackage = PackageInitializerSave.instance.customPackages[i];

                        if (string.IsNullOrEmpty(currentGitPackage.packageName))
                        {
                            Debug.LogError("Package name in the index: [" + i + "] is null. Cannot install null values. Package initializer will stop.");
                            return;
                        }
                        PackageInfo currentCustomPackageInfo = PackageInfo.FindForPackageName(currentGitPackage.packageName);

                        if (currentGitPackage.shouldPackageInstalled)
                        {
                            if (currentCustomPackageInfo == null)
                            {
                                addList.Add(currentGitPackage.packageName);
                            }
                        }
                        else if (!currentGitPackage.shouldPackageInstalled)
                        {
                            if (currentCustomPackageInfo != null)
                            {
                                if (!currentCustomPackageInfo.isDirectDependency)
                                {
                                    Debug.LogError($"Package: [{currentCustomPackageInfo.name}] is installed as dependency updating packages will stop. Do not try to remove it directly.");

                                    Package package = PackageInitializerSave.instance.customPackages.Find((package) => package.packageName == currentGitPackage.packageName);

                                    package.shouldPackageInstalled = true;
                                    PackageInitializerSave.instance.Save();

                                    customPackageListView.Rebuild();
                                }
                                else
                                {
                                    removeList.Add(currentGitPackage.packageName);
                                }
                            }
                        }
                    }

                    
                    if (addList.Count > 0 || removeList.Count > 0)
                    {
                        var finalAddList = addList.ToArray();
                        var finalRemoveList = removeList.ToArray();
                        Debug.Log(finalAddList.Length);
                        Debug.Log(finalRemoveList.Length);
                        Debug.Log(addList.Count);
                        Debug.Log(removeList.Count);


                        addRemoveOfPackageInitializer = Client.AddAndRemove(finalAddList, finalRemoveList);
                        EditorApplication.update += AddOrRemoveProgress;
                        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                    }
                }
            };


            rootVisualElement.Add(spacer);
            toolLabelAndDisableContainer.Add(toolLabel);
            toolLabelAndDisableContainer.Add(disablePackageInitializer);
            rootVisualElement.Add(toolLabelAndDisableContainer);
            savePathContainer.Add(savePathLabel);
            savePathContainer.Add(savePathTextField);
            WholePackageInitializerContainer.Add(savePathContainer);

            if (PackageInitializerSave.instance.builtInPackages.Count != 0)
            {
                WholePackageInitializerContainer.Add(builtInPackagesSearchPanel);
            }
            WholePackageInitializerContainer.Add(builtInPackageList);
            WholePackageInitializerContainer.Add(customPackagesSearchPanel);
            WholePackageInitializerContainer.Add(customPackageList);
            WholePackageInitializerContainer.Add(assetStorePackageList);
            WholePackageInitializerContainer.Add(updateButton);
            rootVisualElement.Add(WholePackageInitializerContainer);
            return rootVisualElement;
        }



        private static void BuiltInPackagesSearchPanel()
        {
            builtInPackagesSearchList = new List<string>();
            builtInPackagesSearchResultList = new List<string>();

            for (int i = 0; i < PackageInitializerSave.instance.builtInPackages.Count; i++)
            {
                builtInPackagesSearchList.Add(PackageInitializerSave.instance.builtInPackages[i].packageName);
            }
            temporaryBuiltinPackagesList = new List<Package>();
        }
        private static void BuiltInPackageSearchIsFilled()
        {
            temporaryBuiltinPackagesList.Clear();
            for (int i = 0; i < builtInPackagesSearchResultList.Count; i++)
            {
                string resultPackageName = builtInPackagesSearchResultList[i];

                Package package = PackageInitializerSave.instance.builtInPackages.Find((_element) => _element.packageName == resultPackageName);

                if (package != null)
                {
                    temporaryBuiltinPackagesList.Add(package);
                }
            }
            builtInPackagesListView.itemsSource = temporaryBuiltinPackagesList;
            builtInPackagesListView.RefreshItems();
        }

        private static void BuiltInPackageSearchIsEmpty()
        {
            for (int i = 0; i < PackageInitializerSave.instance.builtInPackages.Count; i++)
            {
                Package package = PackageInitializerSave.instance.builtInPackages[i];
                Package tempPackage = temporaryBuiltinPackagesList.Find((_element) => _element.packageName == package.packageName);
                if (tempPackage != null)
                {
                    package.shouldPackageInstalled = tempPackage.shouldPackageInstalled;
                }
            }
            PackageInitializerSave.instance.Save();


            temporaryBuiltinPackagesList.Clear();
            builtInPackagesListView.itemsSource = PackageInitializerSave.instance.builtInPackages;
            builtInPackagesListView.RefreshItems();
        }

        private static VisualElement BuiltInPackagesList()
        {
            VisualElement rootVisualElement = new VisualElement();
            builtInPackagesListView = new ListView(PackageInitializerSave.instance.builtInPackages, 24, MakeItemBuiltInListView, BindBuiltInListView);

            builtInPackagesListView.selectionType = SelectionType.Single;
            builtInPackagesListView.style.flexGrow = 1;
            builtInPackagesListView.showFoldoutHeader = true;
            builtInPackagesListView.showBoundCollectionSize = false;

            builtInPackagesListView.showBorder = true;
            builtInPackagesListView.headerTitle = "Built-in Packages";
            builtInPackagesListView.viewDataKey = "BuiltInPackages";


            rootVisualElement.Add(builtInPackagesListView);

            return rootVisualElement;
        }
        private static void BindBuiltInListView(VisualElement element, int index)
        {
            Toggle toggle = element.Q<Toggle>();
            Label label = element.Q<Label>();

            List<Package> builtInPackages = builtInPackagesListView.itemsSource as List<Package>;
            toggle.value = builtInPackages[index].shouldPackageInstalled;
            label.text = builtInPackages[index].packageName;

            toggle.RegisterCallback<ClickEvent>((clickEvent) =>
            {
                toggle.RegisterValueChangedCallback(evt =>
                {
                    if (evt.currentTarget == toggle)
                    {
                        toggle.value = evt.newValue;
                        string currentTogglesLabel = (evt.currentTarget as Toggle).parent.Q<Label>().text;
                        Package package = PackageInitializerSave.instance.builtInPackages.Find(element => element.packageName == currentTogglesLabel);
                        package.shouldPackageInstalled = toggle.value;
                        PackageInitializerSave.instance.Save();
                    }
                });
            });

            label.RegisterCallback<ClickEvent>(evt =>
            {
                if (evt.currentTarget == label)
                {
                    toggle.value = !toggle.value;

                    Package package = PackageInitializerSave.instance.builtInPackages.Find(element => element.packageName == (evt.currentTarget as Label).text);
                    package.shouldPackageInstalled = toggle.value;
                    PackageInitializerSave.instance.Save();
                    evt.StopImmediatePropagation();
                }
            });
        }
        private static VisualElement MakeItemBuiltInListView()
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
        }





        private static void CustomPackagesSearchPanel()
        {
            customPackagesSearchlist = new List<string>();
            customPackagesSearchResultList = new List<string>();

            for (int i = 0; i < PackageInitializerSave.instance.customPackages.Count; i++)
            {
                customPackagesSearchlist.Add(PackageInitializerSave.instance.customPackages[i].packageName);
            }
            temporaryCustomPackagesList = new List<Package>();
        }
        private static void CustomPackageSearchIsFilled()
        {
            temporaryCustomPackagesList.Clear();
            for (int i = 0; i < customPackagesSearchResultList.Count; i++)
            {
                string resultPackageName = customPackagesSearchResultList[i];
                Package package = PackageInitializerSave.instance.customPackages.Find((_element) => _element.packageName == resultPackageName);

                if (package != null)
                {
                    temporaryCustomPackagesList.Add(package);
                }
            }
            
            customPackageListView.bindItem = BindCustomPackagesForTemp;
            customPackageListView.itemsSource = temporaryCustomPackagesList;
            customPackageListView.Rebuild();
        }
        private static void BindCustomPackagesForTemp(VisualElement element, int index)
        {
            Toggle toggle = element.Q<Toggle>();
            TextField textField = element.Q<TextField>();

            List<Package> customPackages = customPackageListView.itemsSource as List<Package>;
            toggle.value = customPackages[index].shouldPackageInstalled;
            textField.value = customPackages[index].packageName;


            toggle.RegisterCallback<ClickEvent>((clickEvent) =>
            {
                toggle.RegisterValueChangedCallback(evt =>
                {
                    if (evt.currentTarget == toggle)
                    {
                        customPackages[index].shouldPackageInstalled = evt.newValue;
                        PackageInitializerSave.instance.Save();
                    }
                });
            });


            textField.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                if (index < PackageInitializerSave.instance.customPackages.Count)
                {
                    customPackages[index].packageName = evt.newValue;
                    PackageInitializerSave.instance.Save();
                }
            });
        }

        private static void CustomPackageSearchIsEmpty()
        {
            for (int i = 0; i < PackageInitializerSave.instance.customPackages.Count; i++)
            {
                Package package = PackageInitializerSave.instance.customPackages[i];
                Package tempPackage = temporaryCustomPackagesList.Find((_element) => _element.packageName == package.packageName);
                if (tempPackage != null)
                {

                    package.shouldPackageInstalled = tempPackage.shouldPackageInstalled;
                }
            }
            PackageInitializerSave.instance.Save();


            temporaryCustomPackagesList.Clear();
            customPackageListView.itemsSource = PackageInitializerSave.instance.customPackages;
            customPackageListView.Rebuild();

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


            toggle.RegisterCallback<ClickEvent>((clickEvent) =>
            {
                toggle.RegisterValueChangedCallback(evt =>
                {
                    if (evt.currentTarget == toggle)
                    {
                        PackageInitializerSave.instance.customPackages[index].shouldPackageInstalled = evt.newValue;
                        PackageInitializerSave.instance.Save();
                    }
                });
            });
          

            textField.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                if (index < PackageInitializerSave.instance.customPackages.Count)
                {
                    PackageInitializerSave.instance.customPackages[index].packageName = evt.newValue;
                    PackageInitializerSave.instance.Save();
                }
            });
        }

        private static VisualElement AssetStorePackagesListView()
        {
            VisualElement rootVisualElement = new VisualElement();
            List<string> unityPackages = FindUnityPackages(GlobalVariables.CurrentAssetStorePath);

            if (PackageInitializerSave.instance.assetStorePackages != null)
            {
                for (int i = 0; i < unityPackages.Count; i++)
                {
                    string currentPackageName = Path.GetFileNameWithoutExtension(unityPackages[i]);

                    Package package = PackageInitializerSave.instance.assetStorePackages.Find((_package) => _package.packageName == currentPackageName);
                    if (package == null)
                    {
                        PackageInitializerSave.instance.assetStorePackages.Add(new Package(currentPackageName, false));
                    }
                }

                PackageInitializerSave.instance.assetStorePackages.Sort();
                PackageInitializerSave.instance.Save();
            }



            assetStorePackagesListView = new ListView(PackageInitializerSave.instance.assetStorePackages, 30, MakeItemAssetStorePackagesListView, BindAssetStorePackagesListView);

            assetStorePackagesListView.selectionType = SelectionType.Single;
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
            image.style.flexShrink = 0;
            image.style.flexWrap = Wrap.NoWrap;

            //image.style.height = Length.Percent(100);
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
        private static void ImportTrueAssetStorePackages()
        {
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

        private static void AddOrRemoveProgress()
        {
            if (addRemoveOfPackageInitializer.IsCompleted)
            {
                EditorApplication.update -= AddOrRemoveProgress;
                EditorUtility.ClearProgressBar();


                if (addRemoveOfPackageInitializer.Result == null)
                {
                    if (addRemoveOfPackageInitializer.Error != null)
                    {
                        string cachedErrorMessage = addRemoveOfPackageInitializer.Error.message;
                        int leftBracket = cachedErrorMessage.IndexOf("[");
                        int rightBracket = cachedErrorMessage.IndexOf("]");


                        if (leftBracket != -1 && rightBracket != -1 && rightBracket > leftBracket)
                        {
                            string insideBrackets = cachedErrorMessage.Substring(leftBracket + 1, rightBracket - leftBracket - 1);
                       
                            if (!string.IsNullOrEmpty(insideBrackets))
                            {
                                Debug.LogError(GlobalVariables.NickName + " " + GlobalVariables.PackagesInitializerName + " " +
                                    "\nPackages has direct dependencies will be ignored: " + insideBrackets);
                            }
                        }
                    }
                }


                ImportTrueAssetStorePackages();
            }
            else
            {
                EditorUtility.DisplayProgressBar("Package Initializer", "Installing or removing packages...", 0.5f);
            }
            if (addRemoveOfPackageInitializer.Result == null)
            {
                EditorUtility.ClearProgressBar();
            }
            if (addRemoveOfPackageInitializer.Error != null)
            {
                if (addRemoveOfPackageInitializer.Error.message != null)
                {
                    EditorUtility.ClearProgressBar();
                }
            }
        }
        private static void ListBuiltInPackagesProgress()
        {
            if (listBuiltInPackages.IsCompleted)
            {
                if (listBuiltInPackages.Status == StatusCode.Success)
                {
                    foreach (var package in listBuiltInPackages.Result)
                    {
                        currentBuiltinPackageNames.Add(package.name);
                    }
                }
                EditorApplication.update -= ListBuiltInPackagesProgress;

                builtInPackageSearchRequest = Client.SearchAll();
                EditorApplication.update += SearchBuiltInPackagesProgress;
            }
        }
        private static void SearchBuiltInPackagesProgress()
        {
            if (builtInPackageSearchRequest.IsCompleted)
            {
                if (builtInPackageSearchRequest.Status == StatusCode.Success)
                {
                    List<string> packageNames = new List<string>();
                    foreach (PackageInfo package in builtInPackageSearchRequest.Result)
                    {
                        packageNames.Add(package.name);
                    }

                    packageNames.RemoveAll(item => removeCoreUnityPackages.Any(removeItem => removeItem == item));

                    packageNames.Distinct().ToList();
                    packageNames.Sort();


                    for (int i = 0; i < packageNames.Count; i++)
                    {
                        string packageName = packageNames[i];
                        if (PackageInitializerSave.instance.builtInPackages.Any((_packageName) => _packageName.packageName == packageName)) continue;

                        bool anyExist = currentBuiltinPackageNames.Any(item => item == packageName);

                        if (anyExist)
                        {
                            PackageInitializerSave.instance.builtInPackages.Add(new Package(packageName, true));
                        }
                        else
                        {
                            PackageInitializerSave.instance.builtInPackages.Add(new Package(packageName, false));
                        }
                    }


                    EditorApplication.update -= SearchBuiltInPackagesProgress;
                    PackageInitializerSave.instance.Save();
                    builtInPackagesListView.Rebuild();
                    BuiltInPackagesSearchPanel();


                    builtInPackagesSearchPanel = new ToolbarSearchPanel(builtInPackagesSearchList, builtInPackagesSearchResultList, BuiltInPackageSearchIsEmpty, BuiltInPackageSearchIsFilled);
                    builtInPackagesSearchPanel.style.alignSelf = Align.FlexEnd;
                    builtInPackagesSearchPanel.style.marginRight = globalMarginLeftRight;
                    WholePackageInitializerContainer.Insert(1, builtInPackagesSearchPanel);

                }

                EditorUtility.ClearProgressBar();
            }
        }

    }
}

