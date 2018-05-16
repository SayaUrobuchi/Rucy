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

    [MenuItem("Rucy/Create/DrawCardAbility", priority = 16)]
    public static void CreateDrawCardAbility()
    {
        CreateData<DrawCardAbility>("Assets/Resources/Data/AbilityData/DrawCardAbility.asset");
    }

    [MenuItem("Rucy/Create/DamageAbility", priority = 16)]
    public static void CreateDamageAbility()
    {
        CreateData<DamageAbility>("Assets/Resources/Data/AbilityData/DamageAbility.asset");
    }

    [MenuItem("Rucy/Create/HealAbility", priority = 16)]
    public static void CreateHealAbility()
    {
        CreateData<HealAbility>("Assets/Resources/Data/AbilityData/HealAbility.asset");
    }

    public static void CreateData<T>(string path) where T : ScriptableObject
    {
        T data = ScriptableObject.CreateInstance<T>();
        string realPath = AssetDatabase.GenerateUniqueAssetPath(path);
        AssetDatabase.CreateAsset(data, realPath);
        EditorGUIUtility.PingObject(data);
    }
}
