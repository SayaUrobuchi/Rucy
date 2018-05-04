using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    void ApplyDamage(int dmg, AttackBattleAction.DamageType type = 0);
    void ApplyHeal(int heal, int type = 0);
    void SetTargetable(bool value = true);
    AttackBattleAction Counter(ITargetable target);
}
