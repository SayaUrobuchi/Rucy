using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardData : ScriptableObject
{
    [Header("CommonAttr")]
    public string Description = "";
    public int Mana = 1;
    [EnumMask(typeof(CardAbility))]
    public CardAbility Ability;

    [Header("CardLooks")]
    public Texture CardImage;
    public Rect UV;

    public abstract CardType Type
    {
        get;
    }

#region EDITOR_ONLY
#if UNITY_EDITOR
    [Header("Migrate")]
    public CardData Source;

    [ContextMenu("CopyFromSource")]
    public void CopyFromSource()
    {
        //Type = Source.Type;
        Description = Source.Description;
        Mana = Source.Mana;
        //Attack = Source.Attack;
        //Health = Source.Health;
        Ability = Source.Ability;
        CardImage = Source.CardImage;
        UV = Source.UV;
    }
#endif
#endregion
}
