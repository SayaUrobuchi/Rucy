using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Monster, 
    Spell, 
}

public enum CardAbility
{
    Taunt = (1 << 0), 
    Dash = (1 << 1), 
    WeakDash = (1 << 2), 
    MagicImmune = (1 << 3), 
}
