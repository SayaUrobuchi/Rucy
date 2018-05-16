using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbility : TargetableAbility
{
    public int Damage = 1;

    public override void Cast(ICaster caster)
    {
        if (!inited)
        {
            if (TargetSelector.Selectable)
            {
                BattleMaid.Summon.SetSelector(TargetSelector, caster);
            }
            else
            {
                TargetSelector.Eval(caster);
                List<ITargetable> res = TargetSelector.Result;
                if (res.Count > 0)
                {
                    Apply(res[0]);
                }
                finished = true;
            }
            inited = true;
        }
        else
        {
            if (TargetSelector.IsSelected)
            {
                Apply(TargetSelector.SelectedTarget);
                finished = true;
            }
        }
    }

    protected void Apply(ITargetable target)
    {
        target.ApplyDamage(Damage, AttackBattleAction.DamageType.Ability);
        target.DeathCheck();
    }
}
