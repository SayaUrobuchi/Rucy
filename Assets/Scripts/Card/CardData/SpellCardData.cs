using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCardData : CardData
{
    [Header("SpellAttr")]
    public Ability CastEffect;

    public override CardType Type
    {
        get
        {
            return CardType.Spell;
        }
    }
}
