using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Properties")]
    public int HP;
    public int CurrentMana;
    public int MaxMana;
    public List<CardData> CardPool = new List<CardData>();
    public List<BattleCardMaid> Hand = new List<BattleCardMaid>();
    public List<BattleCardMaid> Monsters = new List<BattleCardMaid>();

    [Header("References")]
    public LayoutGroup HandGroup;
    public LayoutGroup MonsterGroup;

    public void DrawCard(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            int dice = Random.Range(0, CardPool.Count);
            CardData card = CardPool[dice];
            CardPool.RemoveAt(dice);
            BattleCardMaid maid = BattleMaid.Summon.GenerateCard(card, HandGroup.transform as RectTransform);
            Hand.Add(maid);
        }
    }
}
