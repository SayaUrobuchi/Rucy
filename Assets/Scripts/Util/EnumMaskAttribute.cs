using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class EnumMaskAttribute : PropertyAttribute
{
    public Type EnumType;

    public EnumMaskAttribute(Type T)
    {
        EnumType = T;
    }
}
