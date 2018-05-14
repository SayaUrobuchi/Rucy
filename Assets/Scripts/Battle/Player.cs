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
    public int Armor;
    public List<BattleCardMaid> CardPool = new List<BattleCardMaid>();
    public List<BattleCardMaid> Hand = new List<BattleCardMaid>();
    public List<BattleCardMaid> Monsters = new List<BattleCardMaid>();
    public List<BattleCardMaid> Tomb = new List<BattleCardMaid>();
    public BattleMaid.Turn Side;

    [Header("References")]
    public LayoutGroup CardPoolGroup;
    public LayoutGroup HandGroup;
    public LayoutGroup MonsterGroup;
    public LayoutGroup TombGroup;
    public Text HPText;
    public Text ArmorText;
    public RectTransform ArmorPanel;
    public LayoutGroup ManaGroup;
    public Text CurrentManaText;
    public Text MaxManaText;

    private ManaBlockMaid[] manaBlocks = new ManaBlockMaid[BattleMaid.MaxMana];
    private int manaCostEstimate = 0;
    private int currentCardIdx = 0;
    private List<BattleCardMaid> movingToTomb = new List<BattleCardMaid>();

    public void Init()
    {
        ManaGroup.ClearChildren();
        for (int i = 0; i < BattleMaid.MaxMana; i++)
        {
            manaBlocks[i] = Instantiate(BattleMaid.Summon.ManaBlockTemplate);
            manaBlocks[i].transform.SetParent(ManaGroup.transform);
        }
        currentCardIdx = 0;
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

    public void SummonCard(BattleCardMaid maid)
    {
        CurrentMana -= maid.CostMana;
        Hand.Remove(maid);
        maid.SetState(BattleMaid.CardState.Field);
        Monsters.Add(maid);
        maid.transform.SetParent(MonsterGroup.transform);
        maid.OnSummon();
    }

    public void DrawCard(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            BattleCardMaid maid = CardPool[currentCardIdx];
            CardPool.Remove(maid);
            maid.SetState(BattleMaid.CardState.Hand);
            maid.transform.SetParent(HandGroup.transform);
            Hand.Add(maid);
        }
    }

    public void RemoveCard(BattleCardMaid c)
    {
        if (c.State == BattleMaid.CardState.CardPool)
        {
            CardPool.Remove(c);
        }
        else if (c.State == BattleMaid.CardState.Field)
        {
            Monsters.Remove(c);
        }
        else if (c.State == BattleMaid.CardState.Hand)
        {
            Hand.Remove(c);
        }
    }

    public void MoveCardToTomb(BattleCardMaid c)
    {
        RemoveCard(c);
        Tomb.Add(c);
        movingToTomb.Add(c);
    }

    public void OnTurnStart()
    {
        for (int i = 0; i < Monsters.Count; i++)
        {
            Monsters[i].Wakeup();
        }
        DrawCard();
        MaxMana = Mathf.Min(MaxMana+1, 10);
        CurrentMana = MaxMana;
        UpdateState();
    }

    private void Update()
    {
        if (movingToTomb.Count > 0)
        {
            for (int i = movingToTomb.Count-1; i >= 0; i--)
            {
                BattleCardMaid maid = movingToTomb[i];
                if (!maid.IsAnimating)
                {
                    maid.transform.SetParent(TombGroup.transform);
                    movingToTomb.RemoveAt(i);
                    if (BattleMaid.Summon.CurrentSelectedCard == maid)
                    {
                        BattleMaid.Summon.SetSelectedCard(null);
                    }
                }
            }
        }
        if (Side == BattleMaid.Turn.Opponent && BattleMaid.Summon.CurrentTurn == BattleMaid.Turn.Opponent)
        {
            BattleMaid.Summon.EndTurn();
        }
    }
}
