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
    private bool sleeping;
    private bool targetable;
    private float animateTimer;
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

    public bool IsAttacked
    {
        get
        {
            return attacked;
        }
    }

    public bool IsSleeping
    {
        get
        {
            return sleeping && (Data.Ability & (CardAbility.Dash | CardAbility.WeakDash)) == 0;
        }
    }

    public bool IsAnimating
    {
        get
        {
            return animateTimer > 0f;
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
        if (Data is MonsterCardData)
        {
            MonsterCardData mc = Data as MonsterCardData;
            atk = mc.Attack;
            hp = mc.Health;
        }
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

    public void Wakeup()
    {
        sleeping = false;
        attacked = false;
    }

    public void OnSummon()
    {
        sleeping = true;
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

    public void OnAttack(AttackBattleAction action)
    {
        attacked = true;
    }

    public void DeathCheck()
    {
        if (hp <= 0)
        {
            animateTimer = 1.5f;
            owner.MoveCardToTomb(this);
        }
    }
    #endregion

    #region ITargetable
    public bool IsTaunt
    {
        get
        {
            return (Data.Ability & CardAbility.Taunt) != 0;
        }
    }

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

    public void OnAttacked(AttackBattleAction action)
    {
    }

    public bool TargetableJudge(TargetSelector.Range range)
    {
        bool res = true;
        switch (state)
        {
        case BattleMaid.CardState.CardPool:
            res = (range & TargetSelector.Range.CardPool) != 0;
            break;
        case BattleMaid.CardState.Hand:
            res = (range & TargetSelector.Range.Hand) != 0;
            break;
        case BattleMaid.CardState.Field:
            res = (range & TargetSelector.Range.Field) != 0;
            break;
        case BattleMaid.CardState.Tomb:
            res = (range & TargetSelector.Range.Tomb) != 0;
            break;
        }
        if (res)
        {
            if (Data.Type == CardType.Monster)
            {
                res = (range & TargetSelector.Range.Monster) != 0;
            }
            else if (Data.Type == CardType.Spell)
            {
                res = (range & TargetSelector.Range.Spell) != 0;
            }
        }
        return res;
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

    private void Update()
    {
        if (animateTimer >= 0f)
        {
            animateTimer -= Time.deltaTime;
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
        if (Data is MonsterCardData)
        {
            MonsterCardData mc = Data as MonsterCardData;
            atk = mc.Attack;
            hp = mc.Health;
        }
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
        if (Data is MonsterCardData)
        {
            MonsterCardData mc = Data as MonsterCardData;
            mc.Attack = int.Parse(AttackText.text);
            mc.Health = int.Parse(HealthText.text);
        }
    }

    [ContextMenu("RecordCardLooks")]
    public void RecordCardLooks()
    {
        Data.UV = CardLook.UV;
        Data.CardImage = CardLook.Texture;
    }
}
