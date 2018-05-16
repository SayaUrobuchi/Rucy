using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleEvent
{
    protected bool finished = false;

    public bool IsFinished
    {
        get
        {
            return finished;
        }
    }

    public abstract void Execute();
}
