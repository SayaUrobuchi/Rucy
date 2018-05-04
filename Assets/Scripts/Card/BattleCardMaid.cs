using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCardMaid : MonoBehaviour, ITargetable, IAttacker
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
    public Image Border;

    private int mana;
    private int atk;
    private int hp;

    private bool casting;
    private bool attacked;
    private bool targetable;
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
            return mana;
        }
    }

    public int ATK
    {
        get
        {
            return atk;
        }
    }

    public int HP
    {
        get
        {
            return hp;
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

    public bool Targetable
    {
        get
        {
            return targetable;
        }
    }

    public void SetShow(bool show)
    {
        gameObject.SetActive(show);
    }

    public void SetCard(CardData data)
    {
        Data = data;
        mana = Data.Mana;
        atk = Data.Attack;
        hp = Data.Health;
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
        attacked = maid.attacked;
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

    #region IAttacker
    public AttackBattleAction Attack(ITargetable target)
    {
        if (Data.Type == CardType.Monster)
        {
            return new AttackBattleAction(atk, AttackBattleAction.DamageType.Monster | AttackBattleAction.DamageType.CanBeCounter, this, target);
        }
        return new AttackBattleAction(atk, AttackBattleAction.DamageType.Spell, this, target);
    }
    #endregion

    #region ITargetable
    public void ApplyDamage(int dmg, AttackBattleAction.DamageType type = 0)
    {
        hp -= dmg;
        DamagePopupMaid popup = Instantiate(BattleMaid.Summon.DamagePopupTemplate);
        popup.SetDamageAmount(dmg);
        popup.SetPosition(transform);
        Refresh();
    }

    public void ApplyHeal(int heal, int type = 0)
    {
    }

    public void SetTargetable(bool value = true)
    {
        targetable = value;
        ShowAsTargetable(targetable);
    }

    public AttackBattleAction Counter(ITargetable target)
    {
        return new AttackBattleAction(atk, AttackBattleAction.DamageType.Counter, this, target);
    }
    #endregion

    public void ShowAsTargetable(bool value)
    {
        if (value)
        {
            Border.color = Color.magenta;
        }
        else
        {
            Border.color = Color.white;
        }
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
        BattleMaid.Summon.OnSelectCard(this);
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
