using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using UnityEditor;
using System.Linq;
using System.IO;
using System;


namespace CompilerDestroyer.Editor.EditorTools
{
    internal class PackageInitializer
    {
        private static string packagesInitializerInfo = "When Editor Tools first installed into a project, " + GlobalVariables.PackagesInitializerName + " " +
            "will remove all of the false packages in the list below.\n It will be available in all projects that installs Editor Tools.";
        private static readonly string savePath = PackageInitializerSave.instance.GetSavePath();
        private static int globalMarginLeftRight = 15;





        private static ListRequest customPackageslistRequest;
        private static AddAndRemoveRequest addAndRemoveRequest;
        

        public static List<string> currentBuiltInPackageNames = new List<string>();
        private static ListView builtInPackagesListView;
        private static ListView customPackageListView;
        private static ListView assetStorePackagesListView;
        internal static VisualElement PackageInitializerVisualElement()
        {
            VisualElement rootVisualElement = new VisualElement();
            VisualElement spacer = new VisualElement();
            spacer.style.height = 5f;
            spacer.style.whiteSpace = WhiteSpace.Normal;
            rootVisualElement.Add(spacer);

            Label toolLabel = new Label(GlobalVariables.PackagesInitializerName);
            toolLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            toolLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            toolLabel.style.fontSize = 18;
            toolLabel.style.whiteSpace = WhiteSpace.Normal;
            toolLabel.style.marginBottom = 20f;
            toolLabel.style.marginLeft = globalMarginLeftRight;
            rootVisualElement.Add(toolLabel);




            Box packagesInitializerBox = new Box();
            Color notSoBlack = new Color(Color.black.r, Color.black.g, Color.black.b, 0.3f);
            packagesInitializerBox.style.backgroundColor = notSoBlack;
            packagesInitializerBox.style.marginBottom = 20f;
            packagesInitializerBox.style.marginLeft = globalMarginLeftRight;
            packagesInitializerBox.style.marginRight = globalMarginLeftRight;
            float borderOfBox = 15;
            packagesInitializerBox.style.borderTopLeftRadius = borderOfBox;
            packagesInitializerBox.style.borderTopRightRadius = borderOfBox;
            packagesInitializerBox.style.borderBottomLeftRadius = borderOfBox;
            packagesInitializerBox.style.borderBottomRightRadius = borderOfBox;
            rootVisualElement.Add(packagesInitializerBox);

            Label packagesInitializerInfoLabel = new Label(packagesInitializerInfo + "\n\n" + "Save Path: ");

            packagesInitializerInfoLabel.style.marginTop = 5f;
            packagesInitializerInfoLabel.style.marginBottom = 5f;
            packagesInitializerInfoLabel.style.marginLeft = globalMarginLeftRight;
            packagesInitializerInfoLabel.style.marginRight = globalMarginLeftRight;
            packagesInitializerInfoLabel.style.whiteSpace = WhiteSpace.Normal;
            packagesInitializerBox.Add(packagesInitializerInfoLabel);

            TextField savePathLabel = new TextField();
            savePathLabel.value = savePath;
            savePathLabel.style.marginLeft = globalMarginLeftRight;
            savePathLabel.style.marginRight = globalMarginLeftRight;
            savePathLabel.style.whiteSpace = WhiteSpace.Normal;

            savePathLabel.isReadOnly = true;
            savePathLabel.selectAllOnMouseUp = false;
            savePathLabel.multiline = true;
            packagesInitializerBox.Add(savePathLabel);

            //packageslistRequest = Client.List(true);
            //EditorApplication.update += ListProgress;

            //VisualElement builtinPackageList = BuiltInPackagesList();
            //builtinPackageList.style.maxHeight = 300f;
            //builtinPackageList.style.marginLeft = globalMarginLeftRight;
            //builtinPackageList.style.marginRight = globalMarginLeftRight;
            //builtinPackageList.style.marginBottom = globalMarginLeftRight;
            //rootVisualElement.Add(builtinPackageList);



            VisualElement customPackageList = CustomPackageList();
            customPackageList.style.maxHeight = 300f;
            customPackageList.style.marginLeft = globalMarginLeftRight;
            customPackageList.style.marginRight = globalMarginLeftRight;
            customPackageList.style.marginBottom = globalMarginLeftRight;

            rootVisualElement.Add(customPackageList);



            VisualElement assetStorePackageList = AssetStorePackagesListView();
            assetStorePackageList.style.maxHeight = 300f;
            assetStorePackageList.style.marginLeft = globalMarginLeftRight;
            assetStorePackageList.style.marginRight = globalMarginLeftRight;
            assetStorePackageList.style.marginBottom = globalMarginLeftRight;

            rootVisualElement.Add(assetStorePackageList);




            Button updateButton = new Button();
            updateButton.text = "Update";
            updateButton.clicked += () =>
            {
                addAndRemoveRequest = Client.AddAndRemove(null, currentBuiltInPackageNames.ToArray());
                EditorApplication.update += AddOrRemoveProgress;
            };
            rootVisualElement.Add(updateButton);
            Button saveButton = new Button();
            saveButton.clicked += () =>
            {
                PackageInitializerSave.instance.SaveThis();
            };
            saveButton.text = "Save";
            rootVisualElement.Add(saveButton);
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
                                PackageInitializerSave.instance.SaveThis();
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

                            PackageInitializerSave.instance.SaveThis();
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


            PackageInitializerSave.instance.SaveThis();
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
                PackageInitializerSave.instance.SaveThis();

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
                        PackageInitializerSave.instance.SaveThis();
                        customPackageListView.Rebuild();
                    }
                }
                else
                {
                    list.RemoveAt(view.selectedIndex);
                    PackageInitializerSave.instance.SaveThis();
                    customPackageListView.Rebuild();
                }

            };
            customPackageListView.itemsAdded += addedItems =>
            {
                PackageInitializerSave.instance.SaveThis();
                customPackageListView.Rebuild();

            };
            customPackageListView.itemsRemoved += removedItems =>
            {
                if (removedItems.Count() == PackageInitializerSave.instance.customPackages.Count)
                {
                    EditorApplication.update += RemoveItemsWhenZeroEnteredToCollectionSize;
                    return;
                }
                PackageInitializerSave.instance.SaveThis();
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

            PackageInitializerSave.instance.SaveThis();
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
                PackageInitializerSave.instance.SaveThis();
            });

            textField.RegisterCallback<ChangeEvent<string>>(ChangeEvent =>
            {
                if (index < PackageInitializerSave.instance.customPackages.Count)
                {
                    PackageInitializerSave.instance.customPackages[index].packageName = ChangeEvent.newValue;
                    PackageInitializerSave.instance.SaveThis();
                }
            });
        }
        private static void AddOrRemoveProgress()
        {
            if (customPackageslistRequest.IsCompleted)
            {
                EditorApplication.update -= AddOrRemoveProgress;
            }
        }






        private static VisualElement AssetStorePackagesListView()
        {
            VisualElement rootVisualElement = new VisualElement();
            foreach (var item in FindUnityPackages(GetAssetStoreCachePath()))
            {
                PackageInitializerSave.instance.assetStorePackages.Add(new Package(item));
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
            image.style.height = Length.Percent(100);
            image.image = EditorGUIUtility.IconContent("d_UnityLogo").image as Texture2D;
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
                        PackageInitializerSave.instance.SaveThis();
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

                    PackageInitializerSave.instance.SaveThis();
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

                    PackageInitializerSave.instance.SaveThis();
                    evt.StopImmediatePropagation();
                }
            });
        }



        private static List<string> FindUnityPackages(string folderPath)
        {
            List<string> unityPackages = new List<string>();

            if (Directory.Exists(folderPath))
            {
                string[] unityPackageFiles = Directory.GetFiles(folderPath, "*.unitypackage", SearchOption.AllDirectories);

                if (unityPackageFiles.Length > 0)
                {
                    Debug.Log($"Found {unityPackageFiles.Length} UnityPackage(s):");
                    foreach (var file in unityPackageFiles)
                    {
                        unityPackages.Add(Path.GetFileName(file));
                    }
                }
                else
                {
                    Debug.Log("No .unitypackage files found.");
                }
            }
            else
            {
                Debug.LogError($"Directory not found: {folderPath}");
            }
            return unityPackages;
        }
        private static string GetAssetStoreCachePath()
        {
            string path = string.Empty;

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Unity\Asset Store-5.x");
            }
            else if (Application.platform == RuntimePlatform.OSXEditor)
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), @"Library/Unity/Asset Store-5.x");
            }
            else if (Application.platform == RuntimePlatform.LinuxEditor)
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".local/share/unity3d/Asset Store-5.x");
            }
            else
            {
                Debug.LogError("Unsupported platform for Asset Store cache location.");
            }

            return path;
        }
    }
   



        //private static void ListBuiltInPackagesProgress()
        //{
        //    if (customPackageslistRequest.IsCompleted)
        //    {
        //        if (customPackageslistRequest.Status == StatusCode.Success)
        //        {
        //            foreach (var package in customPackageslistRequest.Result)
        //            {
        //                // Check for built-in packages
        //                if (package.name.StartsWith("com.unity"))
        //                {
        //                    if (UnityEditor.PackageManager.PackageInfo.FindForPackageName(package.name) != null)
        //                    {
        //                        builtInInstalledPackageNames.Add(package.name);
        //                    }
        //                }
        //            }
        //        }
        //        List<string> uniqueList = builtInInstalledPackageNames.Distinct().ToList();
        //        builtInInstalledPackageNames = uniqueList;
        //        builtInInstalledPackageNames.Sort();
        //        builtInPackagesListView.Rebuild();
        //        EditorApplication.update -= ListBuiltInPackagesProgress;
        //    }
        //}
    }




//private static string[] packagesToExcludeFromRemove = new string[]
//{
//            "com.unity.burst",
//            "com.unity.collections",
//            "com.unity.render-pipelines.core",
//            "com.unity.ext.nunit",
//            "com.unity.mathematics",
//            "com.unity.nuget.mono-cecil",
//            "com.unity.test-framework.performance",
//            "com.unity.searcher",
//            "com.unity.shadergraph",
//            "com.unity.rendering.light-transport",
//            "com.unity.render-pipelines.universal-config",
//            "com.unity.modules.hierarchycore",
//            "com.unity.modules.subsystems"
//};