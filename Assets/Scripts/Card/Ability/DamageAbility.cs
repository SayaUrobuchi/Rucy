using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbility : TargetableAbility
{
    public int Damage = 1;

    public override void Cast(ICaster caster)
    {
        TargetSelector.Eval(caster);
        List<ITargetable> res = TargetSelector.Result;
        if (res.Count > 0)
        {
            res[0].ApplyDamage(Damage, AttackBattleAction.DamageType.Ability);
        }
        finished = true;
    }
}
