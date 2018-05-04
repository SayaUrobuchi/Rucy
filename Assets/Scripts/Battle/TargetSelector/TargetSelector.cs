using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetSelector
{
    public enum Range
    {
        Self = (1 << 0),
        Opponent = (1 << 1),
        TopSide = (1 << 2),
        BottomSide = (1 << 3),
        Monster = (1 << 4),
        Hero = (1 << 5),
        IgnoreTaunt = (1 << 6),
        RandomOne = (1 << 7),
        AOE = (1 << 8),
        Unique = (1 << 9),
        Preserved4 = (1 << 10),
    }

    private List<ITargetable> result;

    public virtual Range RangeMask
    {
        get
        {
            return 0;
        }
    }

    public List<ITargetable> Result
    {
        get
        {
            return result;
        }
    }

    public bool Judge(ITargetable target)
    {
        return true;
    }

    public List<ITargetable> Eval(Player p)
    {
        List<ITargetable> res = new List<ITargetable>();
        List<ITargetable> all = new List<ITargetable>();
        Range t = RangeMask;
        if ((t & Range.BottomSide) != 0
            || ((t & Range.Self) != 0 && (p.Side & BattleMaid.Turn.Self) != 0)
            || ((t & Range.Opponent) != 0 && (p.Side & BattleMaid.Turn.Opponent) != 0))
        {
            all.AddRange(BattleMaid.Summon.GetPlayerCards(BattleMaid.Turn.Self, BattleMaid.CardState.All));
        }
        if ((t & Range.TopSide) != 0
            || ((t & Range.Self) != 0 && (p.Side & BattleMaid.Turn.Opponent) != 0)
            || ((t & Range.Opponent) != 0 && (p.Side & BattleMaid.Turn.Self) != 0))
        {
            all.AddRange(BattleMaid.Summon.GetPlayerCards(BattleMaid.Turn.Opponent, BattleMaid.CardState.All));
        }
        for (int i = 0; i < all.Count; i++)
        {
            if (Judge(all[i]))
            {
                res.Add(all[i]);
            }
        }
        return result=res;
    }
}
