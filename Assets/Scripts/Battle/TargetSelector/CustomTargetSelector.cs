using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomTargetSelector : TargetSelector
{
    [EnumMask(typeof(Range))]
    public Range TargetRange;
    public bool Selectable;

    public override Range RangeMask
    {
        get
        {
            return TargetRange;
        }
    }
}
