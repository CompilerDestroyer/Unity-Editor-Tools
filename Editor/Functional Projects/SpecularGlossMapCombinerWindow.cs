using System.Text;
using UnityEngine;
using UnityEditor;

namespace FMPUtils.Editor
{
    /// <summary>
    /// An editor script to combine a specular and a gloss map (from Mixamo) such that the gloss map 
    /// grey scale value is inserted as alpha channel to the specular map values. This will generate 
    /// the texture for the Unity 5 Standard Specular setup at least. 
    /// Information taken from https://forum.unity.com/threads/how-do-i-use-a-gloss-map.297752/
    /// </summary>
    public class SpecularGlossMapCombinerWindow : EditorWindow
    {
        private Texture2D specularMap;
        private Texture2D glossMap;
        private bool useAutoNaming;
        private string outputTextureName;
        private StringBuilder requirementsMessageSB = new StringBuilder();
        private GUIStyle requirementsErrorStyle;

        [MenuItem("Tools/Specular-Gloss Map Combiner")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<SpecularGlossMapCombinerWindow>(false, "Specular-Gloss Map Combiner", true);
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Specular-Gloss Map Combiner", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            specularMap = (Texture2D)EditorGUILayout.ObjectField("Specular Map:", specularMap, typeof(Texture2D), false);
            glossMap = (Texture2D)EditorGUILayout.ObjectField("Gloss Map:", glossMap, typeof(Texture2D), false);
            bool useAutoNamingPrev = useAutoNaming;
            useAutoNaming = EditorGUILayout.Toggle("Use Auto Naming", useAutoNaming);
            if (!useAutoNamingPrev && useAutoNaming)
                outputTextureName = specularMap != null ? specularMap.name + "CombinedMap" : "SpecGlossCombinedMap";
            using (new EditorGUI.DisabledScope(useAutoNaming))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Output Texture Name", GUILayout.MaxWidth(150));
                outputTextureName = EditorGUILayout.TextField(outputTextureName);
                EditorGUILayout.EndHorizontal();
            }
            requirementsMessageSB.Length = 0;
            bool allowCreation = true;
            if (specularMap == null || glossMap == null)
            {
                allowCreation = false;
                requirementsMessageSB.AppendLine("Valid specular and gloss map need to be assigned");
            }
            else
            {
                if (specularMap.width != glossMap.width || specularMap.height != glossMap.height)
                {
                    allowCreation = false;
                    requirementsMessageSB.AppendLine("Assigned Specular and Gloss map do not have matching resolutions");
                }
            }
            if (string.IsNullOrWhiteSpace(outputTextureName))
            {
                allowCreation = false;
                requirementsMessageSB.AppendLine("Output Texture Name cannot be empty");
            }
            EditorGUILayout.Space();
            using (new EditorGUI.DisabledScope(!allowCreation))
            {
                if (GUILayout.Button("Combine Textures and Save"))
                {
                    CombineAndStoreTexture();
                }
            }
            if (!allowCreation)
            {
                if (requirementsErrorStyle == null)
                {
                    requirementsErrorStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
                    requirementsErrorStyle.normal.textColor = Color.red;
                }
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(requirementsMessageSB.ToString(), requirementsErrorStyle);
            }
        }

        /// <summary>
        /// Assumes that both textures have RGBA color channels in that order. Result texture will be of type png
        /// </summary>
        private void CombineAndStoreTexture()
        {
            string specularMapPath = AssetDatabase.GetAssetPath(specularMap);
            string folderMapPath = EditorHelpUtilities.GetAssetFolderPathFromAssetFilePath(specularMapPath);
            string targetMapPath = $"{folderMapPath}/{outputTextureName}.png"; 

            if (!EditorHelpUtilities.DisplayConfirmDialog("Generate and save combined texture?",
                $"Do you want to combine the specular map {specularMap.name} and gloss map {glossMap.name} and store it at the path {targetMapPath}"))
                return;

            if (EditorHelpUtilities.DoesAssetAtPathExist(targetMapPath))
            {
                if (!EditorHelpUtilities.DisplayConfirmDialog("Overwrite existing texture?",  
                    $"A texture at {targetMapPath} already exists, do you want to overwrite it?"))
                    return;
            }
            // With a simple AssetDatabase.CreateAsset call I get an unreadable texture asset, so I save it as png instead
            Texture2D resultTexture = new Texture2D(specularMap.width, specularMap.height, TextureFormat.RGBA32, specularMap.mipmapCount > 1);

            bool specularMapReadEnabledOriginal = specularMap.isReadable;
            bool glossMapReadEnabledOriginal = glossMap.isReadable;
            SetReadWriteEnabledFlag(specularMap, true);
            SetReadWriteEnabledFlag(glossMap, true);
            AssetDatabase.Refresh();
            // Check if isReadable flags have changed and if so, reload the textures:
            if (!specularMapReadEnabledOriginal)
                specularMap = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(specularMap), typeof(Texture2D));
            if (!glossMapReadEnabledOriginal)
                glossMap = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(glossMap), typeof(Texture2D));

            Color[] targetMapColors = specularMap.GetPixels();
            Color[] glossMapColors = glossMap.GetPixels();
            for (int i = 0; i < targetMapColors.Length; i++)
            {
                Color specularColor = targetMapColors[i];
                float glossGreyscale = glossMapColors[i].r;
                specularColor.a = glossGreyscale;
                targetMapColors[i] = specularColor;
            }
            resultTexture.SetPixels(targetMapColors);
            byte[] textureData = resultTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(targetMapPath, textureData);
            AssetDatabase.SaveAssets();

            SetReadWriteEnabledFlag(specularMap, specularMapReadEnabledOriginal);
            SetReadWriteEnabledFlag(glossMap, glossMapReadEnabledOriginal);

            AssetDatabase.Refresh();
        }

        private bool SetReadWriteEnabledFlag(Texture targetTexture, bool isReadable)
        {
            string assetPath = AssetDatabase.GetAssetPath(targetTexture);
            if (string.IsNullOrEmpty(assetPath))
                return false;
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
                return false;
            if (importer.isReadable != isReadable)
            {
                importer.isReadable = isReadable;
                importer.SaveAndReimport();
                AssetDatabase.SaveAssets();
            }
            return true;
        }
    }
}
