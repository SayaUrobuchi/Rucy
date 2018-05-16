using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMaid : MonoBehaviour
{
    private static BattleMaid self;
    public static BattleMaid Summon
    {
        get
        {
            return self;
        }
    }

    public enum State
    {
        None, 
        Prepare, 
        BattleStart, 
        TurnStart, 
        TurnAction, 
        TurnEnd, 
        BattleEnd, 
    }

    public enum Turn
    {
        Self = 1, 
        Opponent = 2, 
    }

    public enum CardState
    {
        None = 0, 
        CardPool = 1, 
        Hand = 2, 
        Field = 4, 
        Tomb = 8, 
        All = 15, 
    }

    public const int MaxCommand = 3;
    public const int MaxMana = 10;

    [Header("Reference")]
    public AudioSource BGMPlayer;
    public BattleCardMaid CardDetail;
    public RectTransform LeftPanel;
    public RectTransform LeftCommandPanel;
    public CommandMaid TurnEndCommand;
    public HelperMaid Helper;

    [Header("Field")]
    public AudioClip BGM;

    [Header("Template")]
    public BattleCardMaid CardTemplate;
    public CommandMaid CommandTemplate;
    public ManaBlockMaid ManaBlockTemplate;
    public DamagePopupMaid DamagePopupTemplate;

    [Header("Player")]
    public Player SelfPlayer;
    public Player OpponentPlayer;

    private State state;
    private Turn currentTurn;
    private BattleCardMaid selectedCard;
    private CommandMaid.State currentCmd;
    private TargetSelector currentSelector;
    private CommandMaid[] commands = new CommandMaid[MaxCommand];

    public BattleCardMaid CurrentSelectedCard
    {
        get
        {
            return selectedCard;
        }
    }

    public Turn CurrentTurn
    {
        get
        {
            return currentTurn;
        }
    }

	// Use this for initialization
	void Start ()
    {
		self = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (state)
        {
        case State.None:
            state = State.Prepare;
            break;
        case State.Prepare:
            Init();
            state = State.BattleStart;
            break;
        case State.BattleStart:
            state = State.TurnStart;
            break;
        case State.TurnStart:
            Player cp = (currentTurn == Turn.Self ? SelfPlayer : OpponentPlayer);
            cp.OnTurnStart();
            SetSelectedCard(selectedCard);
            state = State.TurnAction;
            break;
        case State.TurnAction:
            break;
        case State.TurnEnd:
            currentTurn = (currentTurn == Turn.Self ? Turn.Opponent : Turn.Self);
            state = State.TurnStart;
            break;
        case State.BattleEnd:
            break;
        }
	}

    private void Init()
    {
        currentTurn = Turn.Self;
        currentCmd = CommandMaid.State.None;
        selectedCard = null;

        BGMPlayer.clip = BGM;
        BGMPlayer.Play();
        SelfPlayer.HandGroup.ClearChildren();
        SelfPlayer.MonsterGroup.ClearChildren();
        OpponentPlayer.HandGroup.ClearChildren();
        OpponentPlayer.MonsterGroup.ClearChildren();
        LeftCommandPanel.Clear();

        SelfPlayer.Init();
        SelfPlayer.Side = Turn.Self;
        SelfPlayer.CardPool.Clear();
        for (int i = 0; i < 30; i++)
        {
            CardData card = CardPool.Cards[i % CardPool.Cards.Count].Value;
            BattleCardMaid maid = GenerateCard(card, SelfPlayer.CardPoolGroup.transform as RectTransform);
            maid.SetState(CardState.CardPool);
            maid.Owner = SelfPlayer;
            SelfPlayer.CardPool.Add(maid);
        }
        SelfPlayer.DrawCard(5);
        SelfPlayer.UpdateState();

        OpponentPlayer.Init();
        OpponentPlayer.Side = Turn.Opponent;
        OpponentPlayer.CardPool.Clear();
        for (int i = 0; i < 30; i++)
        {
            CardData card = CardPool.Cards[i % CardPool.Cards.Count].Value;
            BattleCardMaid maid = GenerateCard(card, OpponentPlayer.CardPoolGroup.transform as RectTransform);
            maid.SetState(CardState.CardPool);
            maid.Owner = OpponentPlayer;
            OpponentPlayer.CardPool.Add(maid);
        }
        OpponentPlayer.DrawCard(9);
        int c = 0;
        for (int i = 8; i >= 0 && c < 5; i--)
        {
            if (OpponentPlayer.Hand[i].Data.Type == CardType.Monster)
            {
                OpponentPlayer.SummonCard(OpponentPlayer.Hand[i]);
                ++c;
            }
        }
        OpponentPlayer.CurrentMana = 1;
        OpponentPlayer.UpdateState();

        for (int i = 0; i < MaxCommand; i++)
        {
            commands[i] = Instantiate(CommandTemplate);
            commands[i].transform.SetParent(LeftCommandPanel);
            commands[i].SetVisible(false);
        }
        TurnEndCommand.SetCommand(CommandMaid.State.TurnEnd, true);

        Helper.SetText("你的回合。");
    }

    public BattleCardMaid GenerateCard(CardData data, RectTransform Container)
    {
        BattleCardMaid maid = Instantiate(BattleMaid.Summon.CardTemplate);
        maid.SetCard(data);
        maid.transform.SetParent(Container);
        return maid;
    }

    public bool IsSummonPossible(Player p, BattleCardMaid c)
    {
        return p.CurrentMana >= c.CostMana;
    }

    public bool IsAttackPossible(Player p, BattleCardMaid c)
    {
        return !c.IsAttacked && !c.IsSleeping;
    }

    public bool IsCastPossible(Player p, BattleCardMaid c)
    {
        return p.CurrentMana >= c.CostMana;
    }

    public List<ITargetable> GetPlayerCards(Turn side, CardState type)
    {
        List<ITargetable> res = new List<ITargetable>();
        if ((side & Turn.Self) != 0)
        {
            res.AddRange(GetPlayerCards(SelfPlayer, type));
        }
        if ((side & Turn.Opponent) != 0)
        {
            res.AddRange(GetPlayerCards(OpponentPlayer, type));
        }
        return res;
    }

    public List<ITargetable> GetPlayerCards(Player p, CardState type)
    {
        List<ITargetable> res = new List<ITargetable>();
        if ((type & CardState.CardPool) != 0)
        {
            res.AddRange(p.CardPool.ToList<ITargetable, BattleCardMaid>());
        }
        if ((type & CardState.Hand) != 0)
        {
            res.AddRange(p.Hand.ToList<ITargetable, BattleCardMaid>());
        }
        if ((type & CardState.Field) != 0)
        {
            res.AddRange(p.Monsters.ToList<ITargetable, BattleCardMaid>());
        }
        return res;
    }

    public Player GetOpponentPlayer(Player p)
    {
        if (p == SelfPlayer)
        {
            return OpponentPlayer;
        }
        else if (p == OpponentPlayer)
        {
            return SelfPlayer;
        }
        return null;
    }

    public void ClearSetTargetable()
    {
        ClearSetTargetable(SelfPlayer);
        ClearSetTargetable(OpponentPlayer);
    }

    public void ClearSetTargetable(Player p)
    {
        List<ITargetable> all = GetPlayerCards(p, CardState.All);
        for (int i = 0; i < all.Count; i++)
        {
            all[i].SetTargetable(false);
        }
    }

    public void CommandExecute(BattleCardMaid maid, CommandMaid.State cmd)
    {
        if (maid == null)
        {
            maid = CurrentSelectedCard;
        }
        if (cmd == CommandMaid.State.TurnEnd)
        {
            EndTurn();
            return;
        }
        if (maid == null || cmd == CommandMaid.State.None)
        {
            return;
        }
        switch (cmd)
        {
        case CommandMaid.State.Cancel:
            {
                currentCmd = CommandMaid.State.None;
                ClearSetTargetable();
                Helper.SetText("攻擊取消。");
                break;
            }
        case CommandMaid.State.Attack:
            currentCmd = CommandMaid.State.Attacking;
            currentSelector = new MonsterAttackTargetSelector();
            List<ITargetable> targets = currentSelector.Eval(maid.Owner);
            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].SetTargetable();
            }
            Helper.SetText("請選擇攻擊目標：");
            break;
        case CommandMaid.State.Cast:
            break;
        case CommandMaid.State.Summon:
            maid.Owner.SummonCard(maid);
            MonsterCardData m = maid.Data as MonsterCardData;
            if (m.Warcry != null)
            {
                m.Warcry.Cast(maid.Owner, maid);
            }
            break;
        }
        maid.Owner.UpdateState();
        if (maid == CurrentSelectedCard)
        {
            SetCommand(maid);
        }
    }

    public void EndTurn()
    {
        state = State.TurnEnd;
    }

    public void OnAttack(IAttacker attacker, ITargetable defender)
    {
        AttackBattleAction action = attacker.Attack(defender);
        AttackBattleAction counterAction = null;
        if ((action.Type & AttackBattleAction.DamageType.CanBeCounter) != 0)
        {
            ITargetable a = attacker as ITargetable;
            if (a != null)
            {
                counterAction = defender.Counter(a);
            }
        }
        action.Execute();
        if (counterAction != null)
        {
            counterAction.Execute();
        }
        attacker.DeathCheck();
        defender.DeathCheck();
    }

    public void OnSelectCard(BattleCardMaid maid)
    {
        switch (currentCmd)
        {
        case CommandMaid.State.None:
            {
                if (CurrentSelectedCard != maid)
                {
                    SetSelectedCard(maid);
                }
                else
                {
                    SetSelectedCard(null);
                }
                break;
            }
        case CommandMaid.State.Attacking:
            {
                if (maid.Targetable)
                {
                    OnAttack(selectedCard, maid);
                    currentCmd = CommandMaid.State.None;
                    ClearSetTargetable();
                }
                break;
            }
        }
    }

    public void SetSelectedCard(BattleCardMaid maid)
    {
        selectedCard = maid;
        SetDetailCard(selectedCard);
    }

    public void SetDetailCard(BattleCardMaid maid)
    {
        if (maid == null)
        {
            if (selectedCard == null)
            {
                SetLeftPanelVisible(false);
                return;
            }
            maid = selectedCard;
        }
        CardDetail.SetCard(maid);
        SetCommand(maid);
        SetLeftPanelVisible(true);
    }

    public void SetCommand(BattleCardMaid maid)
    {
        int idx = 0;
        SelfPlayer.SetManaCostEstimate(0);
        if ((currentCmd & (CommandMaid.State.Attacking | CommandMaid.State.Casting | CommandMaid.State.Summoning)) != 0)
        {
            SetCommand(idx++, CommandMaid.State.Cancel, true);
        }
        else if (maid.Owner == SelfPlayer)
        {
            if (maid.Data.Type == CardType.Monster)
            {
                if (maid.State == CardState.Hand)
                {
                    SetCommand(idx++, CommandMaid.State.Summon, IsSummonPossible(SelfPlayer, maid));
                    if (IsSummonPossible(SelfPlayer, maid))
                    {
                        SelfPlayer.SetManaCostEstimate(maid.CostMana);
                    }
                }
                else if (maid.State == CardState.Field)
                {
                    SetCommand(idx++, CommandMaid.State.Attack, IsAttackPossible(SelfPlayer, maid));
                }
            }
            else if (maid.Data.Type == CardType.Spell)
            {
                SetCommand(idx++, CommandMaid.State.Cast, IsCastPossible(SelfPlayer, maid));
                if (IsCastPossible(SelfPlayer, maid))
                {
                    SelfPlayer.SetManaCostEstimate(maid.CostMana);
                }
            }
        }
        while (idx < MaxCommand)
        {
            commands[idx++].SetCommand(CommandMaid.State.None);
        }
    }

    public void SetCommand(int idx, CommandMaid.State cmd, bool clickable = true)
    {
        commands[idx].SetCommand(cmd, clickable);
    }

    public void SetLeftPanelVisible(bool value)
    {
        CardDetail.SetShow(value);
        LeftCommandPanel.gameObject.SetActive(value);
    }
}
