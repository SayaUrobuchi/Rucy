﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable : IBattler
{
    void ApplyDamage(int dmg, AttackBattleAction.DamageType type = 0);
    void ApplyHeal(int heal, int type = 0);
    void SetTargetable(bool value = true);
    AttackBattleAction Counter(ITargetable target);
    void OnAttacked(AttackBattleAction action);
    void DeathCheck();
    bool IsTaunt { get; }
    bool TargetableJudge(TargetSelector.Range range);
}
