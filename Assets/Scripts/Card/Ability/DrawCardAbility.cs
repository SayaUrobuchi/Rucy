using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardAbility : Ability
{
    [EnumMask(typeof(BattleMaid.Turn))]
    public BattleMaid.Turn Target;
    public int DrawCount = 1;

    public override void Cast(ICaster caster)
    {
        if ((Target & BattleMaid.Turn.Self) != 0)
        {
            caster.Owner.DrawCard(DrawCount);
        }
        if ((Target & BattleMaid.Turn.Opponent) != 0)
        {
            Player op = BattleMaid.Summon.GetOpponentPlayer(caster.Owner);
            op.DrawCard(DrawCount);
        }
        finished = true;
    }
}
