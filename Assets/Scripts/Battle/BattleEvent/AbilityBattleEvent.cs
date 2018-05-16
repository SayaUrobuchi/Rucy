using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBattleEvent : BattleEvent
{
    private Ability ability;
    private ICaster caster;

    public AbilityBattleEvent(Ability a, ICaster c)
    {
        ability = a;
        caster = c;
    }

    public override void Execute()
    {
        if (!ability.IsFinished)
        {
            ability.Cast(caster);
        }
        if (ability.IsFinished)
        {
            finished = true;
        }
    }
}
