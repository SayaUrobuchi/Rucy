﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCardData : CardData
{
    [Header("MonsterAttr")]
    public int Attack = 0;
    public int Health = 0;

    [Header("Ability")]
    public Ability Warcry;
    public Ability Lastword;

    public override CardType Type
    {
        get
        {
            return CardType.Monster;
        }
    }
}
