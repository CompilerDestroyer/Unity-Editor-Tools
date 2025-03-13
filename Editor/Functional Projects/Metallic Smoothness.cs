#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using UnityEditor;

public class CreateMetallicSmoothness : EditorWindow
{
    [MenuItem("Tools/Create Metallic Smoothness")]
    public static EditorWindow EditorWindow()
    {
        return GetWindow<CreateMetallicSmoothness>();
    }

    private Texture2D metallicMap;
    private Texture2D roughnessMap;

    private string description = "Create: \nMetallic Smoothness Map from Metallic + Roughness Map\nor\nSmoothness Map from roughness Map!";
    public void OnGUI()
    {
        EditorGUILayout.Space(20f);

        GUILayout.Box(description, GUILayout.ExpandWidth(true));



        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Metallic Map");
        metallicMap = EditorGUILayout.ObjectField(metallicMap, typeof(Texture2D), true) as Texture2D;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Roughness Map");
        roughnessMap = EditorGUILayout.ObjectField(roughnessMap, typeof(Texture2D), true) as Texture2D;
        EditorGUILayout.EndHorizontal();


        if (GUILayout.Button("Create Metallic!"))
            {
                if (metallicMap == null && roughnessMap != null)
                {
                    CreateSmoothnessMap(roughnessMap);
                }
                else if (metallicMap != null && roughnessMap != null)
                {
                    CreateMetallicAndSmoothnessMap(metallicMap, roughnessMap);
                }
                else if (roughnessMap == null && metallicMap == null)
                {
                    Debug.LogError("You dont have roughness or metallic map!");
                }
            }
    }

    private void CreateSmoothnessMap(Texture2D roughnessMap)
    {
        Texture2D smoothnessMap = new Texture2D(roughnessMap.width, roughnessMap.height);
        Color[] roughnessPixels = roughnessMap.GetPixels();

        for (int i = 0; i < roughnessPixels.Length; i++)
        {
            roughnessPixels[i].r = 1f - roughnessPixels[i].r;

            roughnessPixels[i].a = 1f;
        }

        smoothnessMap.SetPixels(roughnessPixels);
        smoothnessMap.Apply();

        Texture2D createdMetallicMap = new Texture2D(roughnessMap.width, roughnessMap.height);
        Color[] smoothnessPixels = smoothnessMap.GetPixels();

        for (int i = 0; i < smoothnessPixels.Length; i++)
        {
            float smoothness = smoothnessPixels[i].r;

            smoothnessPixels[i] = new Color(0f, 0f, 0f, smoothness);
        }

        createdMetallicMap.SetPixels(smoothnessPixels);
        createdMetallicMap.Apply();

        string assetPath = AssetDatabase.GetAssetPath(roughnessMap);
        string directory = Path.GetDirectoryName(assetPath);
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(assetPath);
        string newFileName = $"{directory}/{fileNameWithoutExtension}_Combined.png";
        byte[] bytes = createdMetallicMap.EncodeToPNG();

        File.WriteAllBytes(newFileName, bytes);

        AssetDatabase.ImportAsset(newFileName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Smoothness map saved as {newFileName}");
    }

    private void CreateMetallicAndSmoothnessMap(Texture2D metallicMap, Texture2D roughnessMap)
    {
        Texture2D smoothnessMap = new Texture2D(roughnessMap.width, roughnessMap.height);
        Color[] roughnessPixels = roughnessMap.GetPixels();

        for (int i = 0; i < roughnessPixels.Length; i++)
        {
            roughnessPixels[i].r = 1f - roughnessPixels[i].r;
            roughnessPixels[i].a = 1f;
        }

        smoothnessMap.SetPixels(roughnessPixels);
        smoothnessMap.Apply();

        Texture2D createdMetallicMap = new Texture2D(metallicMap.width, metallicMap.height);
        Color[] metallicPixels = metallicMap.GetPixels();
        Color[] smoothnessPixels = smoothnessMap.GetPixels();

        for (int i = 0; i < metallicPixels.Length; i++)
        {
            float metallic = metallicPixels[i].r;

            float smoothness = smoothnessPixels[i].r;

            metallicPixels[i] = new Color(metallic, 0, 0, smoothness);
        }

        createdMetallicMap.SetPixels(metallicPixels);
        createdMetallicMap.Apply();

        string assetPath = AssetDatabase.GetAssetPath(metallicMap);
        string fileName = $"{Path.ChangeExtension(assetPath, null)}_Combined.png";
        byte[] bytes = createdMetallicMap.EncodeToPNG();

        File.WriteAllBytes(fileName, bytes);

        AssetDatabase.ImportAsset(fileName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
#endif