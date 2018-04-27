using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : ScriptableObject
{
    [Header("CardAbility")]
    public CardType Type = CardType.Monster;
    public string Description = "";
    public int Mana = 1;
    public int Attack = 0;
    public int Health = 0;

    [Header("CardLooks")]
    public Texture CardImage;
    public Rect UV;
}
