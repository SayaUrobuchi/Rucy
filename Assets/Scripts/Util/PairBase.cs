using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PairBase<T, U>
{
    public T Key;
    public U Value;

    public PairBase(T a, U b)
    {
        Key = a;
        Value = b;
    }
}
