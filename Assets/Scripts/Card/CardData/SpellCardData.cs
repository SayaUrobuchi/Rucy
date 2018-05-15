using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCardData : CardData
{
    public override CardType Type
    {
        get
        {
            return CardType.Spell;
        }
    }
}
