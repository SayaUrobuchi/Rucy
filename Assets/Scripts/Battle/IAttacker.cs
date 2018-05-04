using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    AttackBattleAction Attack(ITargetable target);
}
