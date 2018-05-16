﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    protected bool finished = false;

    public bool IsFinished
    {
        get
        {
            return finished;
        }
    }

    public abstract void Cast(ICaster caster);
}
