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
        Self, 
        Opponent, 
    }

    public enum CardState
    {
        None, 
        CardPool, 
        Hand, 
        Field, 
    }

    public const int MaxCommand = 3;
    public const int MaxMana = 10;

    [Header("Reference")]
    public AudioSource BGMPlayer;
    public BattleCardMaid CardDetail;
    public RectTransform LeftPanel;
    public RectTransform LeftCommandPanel;
    public CommandMaid TurnEndCommand;

    [Header("Field")]
    public AudioClip BGM;

    [Header("Template")]
    public BattleCardMaid CardTemplate;
    public CommandMaid CommandTemplate;
    public ManaBlockMaid ManaBlockTemplate;

    [Header("Player")]
    public Player SelfPlayer;
    public Player OpponentPlayer;

    private State state;
    private Turn currentTurn;
    private BattleCardMaid selectedCard;
    private CommandMaid[] commands = new CommandMaid[MaxCommand];

    public BattleCardMaid CurrentSelectedCard
    {
        get
        {
            return selectedCard;
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
            state = State.TurnAction;
            break;
        case State.TurnAction:
            break;
        case State.TurnEnd:
            break;
        case State.BattleEnd:
            break;
        }
	}

    private void Init()
    {
        currentTurn = Turn.Self;

        BGMPlayer.clip = BGM;
        BGMPlayer.Play();
        //SetLeftPanelVisible(false);
        SelfPlayer.HandGroup.ClearChildren();
        SelfPlayer.MonsterGroup.ClearChildren();
        LeftCommandPanel.Clear();

        SelfPlayer.Init();
        SelfPlayer.CardPool.Clear();
        for (int i = 0; i < 30; i++)
        {
            CardData card = CardPool.Cards[Random.Range(0, CardPool.Cards.Count)].Value;
            SelfPlayer.CardPool.Add(card);
        }
        SelfPlayer.DrawCard(5);
        SelfPlayer.UpdateState();
        OpponentPlayer.Init();
        OpponentPlayer.UpdateState();

        for (int i = 0; i < MaxCommand; i++)
        {
            commands[i] = Instantiate(CommandTemplate);
            commands[i].transform.SetParent(LeftCommandPanel);
            commands[i].SetVisible(false);
        }
        TurnEndCommand.SetCommand(CommandMaid.State.TurnEnd, true);
    }

    public BattleCardMaid GenerateCard(CardData data, RectTransform Container)
    {
        BattleCardMaid maid = Instantiate(BattleMaid.Summon.CardTemplate);
        maid.SetCard(data);
        maid.transform.SetParent(Container);
        return maid;
    }

    public void CommandExecute(BattleCardMaid maid, CommandMaid.State cmd)
    {
        if (maid == null)
        {
            maid = CurrentSelectedCard;
        }
        if (maid == null || cmd == CommandMaid.State.None)
        {
            return;
        }
        switch (cmd)
        {
        case CommandMaid.State.TurnEnd:
            break;
        case CommandMaid.State.Cancel:
            break;
        case CommandMaid.State.Attack:
        case CommandMaid.State.Summon:
        case CommandMaid.State.Cast:
            maid.Owner.UseCard(maid, cmd);
            break;
        }
        maid.Owner.UpdateState();
        if (maid == CurrentSelectedCard)
        {
            SetCommand(maid);
        }
    }

    public bool IsSummonPossible(Player p, BattleCardMaid c)
    {
        return p.CurrentMana >= c.CostMana;
    }

    public bool IsAttackPossible(Player p, BattleCardMaid c)
    {
        return false;
    }

    public bool IsCastPossible(Player p, BattleCardMaid c)
    {
        return p.CurrentMana >= c.CostMana;
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
