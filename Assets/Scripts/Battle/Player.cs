﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Properties")]
    public int HP;
    public int CurrentMana;
    public int MaxMana;
    public int Armor;
    public List<CardData> CardPool = new List<CardData>();
    public List<BattleCardMaid> Hand = new List<BattleCardMaid>();
    public List<BattleCardMaid> Monsters = new List<BattleCardMaid>();

    [Header("References")]
    public LayoutGroup HandGroup;
    public LayoutGroup MonsterGroup;
    public Text HPText;
    public Text ArmorText;
    public RectTransform ArmorPanel;
    public LayoutGroup ManaGroup;
    public Text CurrentManaText;
    public Text MaxManaText;

    private ManaBlockMaid[] manaBlocks = new ManaBlockMaid[BattleMaid.MaxMana];
    private int manaCostEstimate = 0;

    public void Init()
    {
        ManaGroup.ClearChildren();
        for (int i = 0; i < BattleMaid.MaxMana; i++)
        {
            manaBlocks[i] = Instantiate(BattleMaid.Summon.ManaBlockTemplate);
            manaBlocks[i].transform.SetParent(ManaGroup.transform);
        }
    }

    public void SetManaCostEstimate(int e)
    {
        manaCostEstimate = e;
        UpdateState();
    }

    public void UpdateState()
    {
        HPText.text = HP.ToString();
        ArmorText.text = Armor.ToString();
        ArmorPanel.gameObject.SetActive(Armor > 0);
        CurrentManaText.text = CurrentMana.ToString();
        MaxManaText.text = MaxMana.ToString();
        for (int i = 0; i < CurrentMana; i++)
        {
            manaBlocks[i].SetState(i < (CurrentMana - manaCostEstimate) ? ManaBlockMaid.State.Normal : ManaBlockMaid.State.CostEstimate);
        }
        for (int i = CurrentMana; i < MaxMana; i++)
        {
            manaBlocks[i].SetState(ManaBlockMaid.State.Used);
        }
        for (int i = MaxMana; i < BattleMaid.MaxMana; i++)
        {
            manaBlocks[i].SetState(ManaBlockMaid.State.None);
        }
    }

    public void DrawCard(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            int dice = Random.Range(0, CardPool.Count);
            CardData card = CardPool[dice];
            CardPool.RemoveAt(dice);
            BattleCardMaid maid = BattleMaid.Summon.GenerateCard(card, HandGroup.transform as RectTransform);
            maid.SetState(BattleMaid.CardState.Hand);
            Hand.Add(maid);
        }
    }
}
