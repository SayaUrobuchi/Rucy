using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbility : TargetableAbility
{
    public int Damage = 1;

    public override void Cast(Player owner, BattleCardMaid caster)
    {
        TargetSelector.Eval(owner);
        List<ITargetable> res = TargetSelector.Result;
        if (res.Count > 0)
        {
            res[0].ApplyDamage(Damage, AttackBattleAction.DamageType.Ability);
        }
    }
}
