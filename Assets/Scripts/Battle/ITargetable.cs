using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    void ApplyDamage(int dmg, int type = 0);
    void ApplyHeal(int heal, int type = 0);
    void ShowAsTarget(bool value = true);
}
