using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class MenuItemMaid
{
    [MenuItem("Rucy/Create/CardData", priority = 0)]
    public static void CreateSongData()
    {
        CardData data = ScriptableObject.CreateInstance<CardData>();
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Data/CardData/NewCardData.asset");
        AssetDatabase.CreateAsset(data, path);
        EditorGUIUtility.PingObject(data);
    }
}
