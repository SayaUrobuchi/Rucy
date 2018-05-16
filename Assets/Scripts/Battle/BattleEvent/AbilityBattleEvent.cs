using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBattleEvent : BattleEvent
{
    protected Ability ability;
    protected ICaster caster;

    public AbilityBattleEvent(Ability a, ICaster c)
    {
        ability = a;
        caster = c;
        ability.Init();
    }

    public override void Execute()
    {
        if (!finished)
        {
            ability.Cast(caster);
        }
        if (ability.IsFinished)
        {
            finished = true;
        }
    }
}
