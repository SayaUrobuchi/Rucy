﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(EnumMaskAttribute))]
public class EnumMaskAttrPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnumMaskAttribute attr = attribute as EnumMaskAttribute;

        Enum e = Enum.ToObject(attr.EnumType, property.intValue) as Enum;
        e = EditorGUI.EnumMaskField(position, label, e);
        property.intValue = (int)Enum.ToObject(attr.EnumType, e);
    }
}
