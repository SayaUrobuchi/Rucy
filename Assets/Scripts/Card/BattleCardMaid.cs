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

    private int Mana;
    private int Attack;
    private int Health;

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
        Mana = maid.Mana;
        Attack = maid.Attack;
        Health = maid.Health;
        Refresh();
    }

    private void Refresh()
    {
        DescText.text = Data.Description;
        ManaText.text = Mana.ToString();
        if (Data.Type == CardType.Monster)
        {
            AttackPanel.SetActive(true);
            HealthPanel.SetActive(true);
            AttackText.text = Attack.ToString();
            HealthText.text = Health.ToString();
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
        Mana = Data.Mana;
        Attack = Data.Attack;
        Health = Data.Health;
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
