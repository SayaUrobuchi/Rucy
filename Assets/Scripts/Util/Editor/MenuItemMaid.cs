using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class MenuItemMaid
{
    [MenuItem("Rucy/Create/MonsterCard", priority = 0)]
    public static void CreateMonsterCard()
    {
        CreateData<MonsterCardData>("Assets/Resources/Data/CardData/NewMonsterCard.asset");
    }

    [MenuItem("Rucy/Create/SpellCard", priority = 0)]
    public static void CreateSpellCard()
    {
        CreateData<SpellCardData>("Assets/Resources/Data/CardData/NewSpellCard.asset");
    }
    
    public static void CreateData<T>(string path) where T : ScriptableObject
    {
        T data = ScriptableObject.CreateInstance<T>();
        string realPath = AssetDatabase.GenerateUniqueAssetPath(path);
        AssetDatabase.CreateAsset(data, realPath);
        EditorGUIUtility.PingObject(data);
    }
}
