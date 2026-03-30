using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using UnityEngine.UIElements;

namespace CompilerDestroyer.Editor.EditorTools
{
    internal sealed class SpriteSlicer
    {
        private static readonly int globalMarginLeftRight = 15;

        internal static VisualElement SliceSelectedTextureVisualElement()
        {
            VisualElement rootVisualElement = new VisualElement();

            VisualElement spacer = new VisualElement();
            spacer.style.height = 5f;
            spacer.style.whiteSpace = WhiteSpace.Normal;
            

            Label toolLabel = new Label(GlobalVariables.SpriteSlicerName);
            toolLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            toolLabel.style.fontSize = 18;
            toolLabel.style.whiteSpace = WhiteSpace.Normal;
            toolLabel.style.marginLeft = globalMarginLeftRight;
            toolLabel.style.marginBottom = 10f;

            Object[] textures = Selection.objects as Object[];

            Button sliceButton = new Button();
            FloatField xSliceFloatField = new FloatField();
            FloatField ySlicefloatField = new FloatField();
            sliceButton.text = "Slice";
            sliceButton.clicked += delegate
            {
                if (xSliceFloatField.value == 0 || ySlicefloatField.value == 0)
                {
                    Debug.LogError($"Slice X or Slice Y cannot be {xSliceFloatField.value}.");

                    return;
                }

                for (int i = 0; i < textures.Length; i++)
                {
                    string texturePath = AssetDatabase.GetAssetPath(textures[i]);
                    SliceSprite(texturePath, (int)xSliceFloatField.value, (int)ySlicefloatField.value);
                }
            };

            VisualElement container = new VisualElement();
            container.style.borderLeftWidth = globalMarginLeftRight;
            container.style.borderRightWidth = globalMarginLeftRight;


            Label xSliceLabel = new Label("Slice X");
            Label ySliceLabel = new Label("Slice Y");


            container.Add(xSliceLabel);
            container.Add(xSliceFloatField);

            container.Add(ySliceLabel);
            container.Add(ySlicefloatField);
            container.Add(sliceButton);

            rootVisualElement.Add(spacer);
            rootVisualElement.Add(toolLabel);
            rootVisualElement.Add(container);
            return rootVisualElement;
        }





        private static void SliceSprite(string texturePath, int sizeX, int sizeY)
        {
            TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            if (importer == null)
                return;

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.spritePixelsPerUnit = 100f;
            importer.filterMode = FilterMode.Bilinear;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.maxTextureSize = 2048;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.crunchedCompression = false;
            importer.compressionQuality = 100;
            importer.isReadable = true;
            importer.textureShape = TextureImporterShape.Texture2D;
            importer.npotScale = TextureImporterNPOTScale.None;

            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();

            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            if (texture == null)
                return;

            SpriteDataProviderFactories factory = new SpriteDataProviderFactories();
            factory.Init();

            ISpriteEditorDataProvider dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
            dataProvider.InitSpriteEditorDataProvider();

            List<SpriteRect> spriteRects = new List<SpriteRect>();
            List<SpriteNameFileIdPair> nameFileIdPairs = new List<SpriteNameFileIdPair>();

            int frameNumber = 0;

            for (int y = texture.height - sizeY; y >= 0; y -= sizeY)
            {
                for (int x = 0; x <= texture.width - sizeX; x += sizeX)
                {
                    GUID spriteId = GUID.Generate();

                    SpriteRect spriteRect = new SpriteRect
                    {
                        name = $"{texture.name}_{frameNumber}",
                        spriteID = spriteId,
                        rect = new Rect(x, y, sizeX, sizeY),
                        alignment = SpriteAlignment.Custom,
                        pivot = Vector2.zero
                    };

                    spriteRects.Add(spriteRect);
                    nameFileIdPairs.Add(new SpriteNameFileIdPair(spriteRect.name, spriteRect.spriteID));
                    frameNumber++;
                }
            }

            dataProvider.SetSpriteRects(spriteRects.ToArray());

            ISpriteNameFileIdDataProvider nameFileIdProvider = dataProvider.GetDataProvider<ISpriteNameFileIdDataProvider>();
            nameFileIdProvider.SetNameFileIdPairs(nameFileIdPairs);

            dataProvider.Apply();
            importer.SaveAndReimport();

            Debug.Log($"Sliced {frameNumber} sprites from {texturePath}");
        }
    }
}
