﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetableAbility : Ability
{
    public CustomTargetSelector TargetSelector;

    public override void Init()
    {
        base.Init();
        TargetSelector.Clear();
    }
}
