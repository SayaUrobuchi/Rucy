using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBattleAction : BattleAction
{
    public enum DamageType
    {
        Monster = (1 << 0),
        Spell = (1 << 1),
        HeroWeapon = (1 << 2),
        Counter = (1 << 3),
        Preserved1 = (1 << 4),
        Preserved2 = (1 << 5),
        Preserved3 = (1 << 6),
        Preserved4 = (1 << 7),
        Preserved5 = (1 << 8),
        Preserved6 = (1 << 9),
    }

    public int Power;
    public int Type;

    public AttackBattleAction(int pow, int type)
    {
        Power = pow;
        Type = type;
    }
}
