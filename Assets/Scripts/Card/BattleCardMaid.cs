using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCardMaid : MonoBehaviour
{
    public CardData Data;

    [Header("Reference")]
    public Text DescText;
    public Text ManaText;
    public Text AttackText;
    public Text HealthText;
    public GameObject AttackPanel;
    public GameObject HealthPanel;
    public UVMaid CardLook;

    private int mana;
    private int atk;
    private int hp;

    private bool casting;
    private BattleMaid.CardState state;
    private Player owner;

    public BattleMaid.CardState State
    {
        get
        {
            return state;
        }
    }

    public int CostMana
    {
        get
        {
            return Data.Mana;
        }
    }

    public Player Owner
    {
        get
        {
            return owner;
        }

        set
        {
            owner = value;
        }
    }

    public void SetShow(bool show)
    {
        gameObject.SetActive(show);
    }

    public void SetCard(CardData data)
    {
        Data = data;
        ShowCard();
    }

    public void SetCard(BattleCardMaid maid)
    {
        Data = maid.Data;
        mana = maid.mana;
        atk = maid.atk;
        hp = maid.hp;
        casting = maid.casting;
        state = maid.state;
        Refresh();
    }

    public void SetState(BattleMaid.CardState s)
    {
        state = s;
    }

    private void Refresh()
    {
        DescText.text = Data.Description;
        ManaText.text = mana.ToString();
        if (Data.Type == CardType.Monster)
        {
            AttackPanel.SetActive(true);
            HealthPanel.SetActive(true);
            AttackText.text = atk.ToString();
            HealthText.text = hp.ToString();
        }
        else
        {
            AttackPanel.SetActive(false);
            HealthPanel.SetActive(false);
        }
        CardLook.Texture = Data.CardImage;
        CardLook.UV = Data.UV;
    }

    public void OnMouseEnter()
    {
        BattleMaid.Summon.SetDetailCard(this);
    }

    public void OnMouseExit()
    {
        BattleMaid.Summon.SetDetailCard(null);
    }

    public void OnMouseClicked()
    {
        if (BattleMaid.Summon.CurrentSelectedCard != this)
        {
            BattleMaid.Summon.SetSelectedCard(this);
        }
        else
        {
            BattleMaid.Summon.SetSelectedCard(null);
        }
    }

    [ContextMenu("ShowCard")]
    public void ShowCard()
    {
        mana = Data.Mana;
        atk = Data.Attack;
        hp = Data.Health;
        Refresh();
    }

    [ContextMenu("RecordCardALL")]
    public void RecordCardAll()
    {
        RecordCardAttribute();
        RecordCardLooks();
    }

    [ContextMenu("RecordCardAttribute")]
    public void RecordCardAttribute()
    {
        Data.Description = DescText.text;
        Data.Mana = int.Parse(ManaText.text);
        Data.Attack = int.Parse(AttackText.text);
        Data.Health = int.Parse(HealthText.text);
    }

    [ContextMenu("RecordCardLooks")]
    public void RecordCardLooks()
    {
        Data.UV = CardLook.UV;
        Data.CardImage = CardLook.Texture;
    }
}
