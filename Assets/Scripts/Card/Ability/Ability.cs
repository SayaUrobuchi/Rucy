using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    protected bool inited = false;
    protected bool finished = false;

    public bool IsFinished
    {
        get
        {
            return finished;
        }
    }

    public virtual void Init()
    {
        inited = false;
        finished = false;
    }

    public abstract void Cast(ICaster caster);
}
