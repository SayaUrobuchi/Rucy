using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSpellBattleEvent : AbilityBattleEvent
{
    public CastSpellBattleEvent(Ability a, ICaster c) : base(a, c)
    {
    }

    public override void Execute()
    {
        base.Execute();
        if (ability.IsFinished)
        {
            BattleMaid.Summon.ClearCurrentCommand();
            caster.AfterCast();
        }
        if (BattleMaid.Summon.CurrentCmd != CommandMaid.State.Casting)
        {
            finished = true;
            BattleMaid.Summon.ClearSetTargetable();
        }
    }
}
