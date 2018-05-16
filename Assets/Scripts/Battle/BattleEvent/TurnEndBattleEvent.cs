using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndBattleEvent : BattleEvent
{
    public override void Execute()
    {
        BattleMaid.Summon.EndTurn();
        finished = true;
    }
}
