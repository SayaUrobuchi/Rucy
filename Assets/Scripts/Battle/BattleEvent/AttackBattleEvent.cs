using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBattleEvent : BattleEvent
{
    protected IAttacker attacker;
    protected TargetSelector selector = new MonsterAttackTargetSelector();

    public AttackBattleEvent(IAttacker a)
    {
        attacker = a;
    }

    public AttackBattleEvent(IAttacker a, TargetSelector sel) : this(a)
    {
        selector = sel;
    }

    public override void Execute()
    {
        if (!inited)
        {
            BattleMaid.Summon.SetSelector(selector, attacker);
            inited = true;
        }
        else
        {
            if (selector.IsSelected)
            {
                BattleMaid.Summon.OnAttack(attacker, selector.SelectedTarget);
                BattleMaid.Summon.ClearCurrentCommand();
                BattleMaid.Summon.ClearSetTargetable();
                finished = true;
            }
        }
        if (BattleMaid.Summon.CurrentCmd != CommandMaid.State.Attacking)
        {
            finished = true;
        }
    }
}
