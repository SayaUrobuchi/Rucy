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

    [ContextMenu("ShowCard")]
    public void ShowCard()
    {
        DescText.text = Data.Description;
        ManaText.text = Data.Mana.ToString();
        if (Data.Type == CardType.Monster)
        {
            AttackPanel.SetActive(true);
            HealthPanel.SetActive(true);
            AttackText.text = Data.Attack.ToString();
            HealthText.text = Data.Health.ToString();
        }
        else
        {
            AttackPanel.SetActive(false);
            HealthPanel.SetActive(false);
        }
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
