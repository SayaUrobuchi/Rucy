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

    [Header("Reference")]
    public AudioSource BGMPlayer;

    [Header("Field")]
    public AudioClip BGM;

    private State state;

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
            break;
        case State.TurnStart:
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
        BGMPlayer.clip = BGM;
        BGMPlayer.Play();
    }
}
