﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : TargetableAbility
{
    public override void Cast(ICaster caster)
    {
        finished = true;
    }
}
