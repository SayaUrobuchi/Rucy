using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackTargetSelector : TargetSelector
{
    public override Range RangeMask
    {
        get
        {
            return (Range.Opponent | Range.Monster | Range.Hero | Range.Field);
        }
    }
}
