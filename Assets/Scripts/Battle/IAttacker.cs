using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker : IBattler
{
    AttackBattleAction Attack(ITargetable target);
    void OnAttack(AttackBattleAction action);
    void DeathCheck();
}
