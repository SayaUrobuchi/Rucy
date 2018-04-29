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

    [Header("Reference")]
    public AudioSource BGMPlayer;
    public BattleCardMaid CardDetail;
    public RectTransform LeftPanel;

    [Header("Field")]
    public AudioClip BGM;

    [Header("Template")]
    public BattleCardMaid CardTemplate;

    [Header("Player")]
    public Player SelfPlayer;
    public Player OpponentPlayer;

    private State state;
    private Turn currentTurn;
    private BattleCardMaid selectedCard;

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

        SelfPlayer.CardPool = new List<CardData>();
        for (int i = 0; i < 30; i++)
        {
            SelfPlayer.CardPool.Add(CardPool.Cards[Random.Range(0, CardPool.Cards.Count)].Value);
        }
        SelfPlayer.DrawCard(5);
    }

    public BattleCardMaid GenerateCard(CardData data, RectTransform Container)
    {
        BattleCardMaid maid = Instantiate(BattleMaid.Summon.CardTemplate);
        maid.SetCard(data);
        maid.transform.SetParent(Container);
        return maid;
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
        SetLeftPanelVisible(true);
    }

    public void SetLeftPanelVisible(bool value)
    {
        CardDetail.SetShow(value);
        LeftPanel.gameObject.SetActive(value);
    }
}
