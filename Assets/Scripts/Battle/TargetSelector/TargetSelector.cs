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
        Spell = (1 << 5), 

        IgnoreTaunt = (1 << 6), 

        RandomOne = (1 << 7), 
        AOE = (1 << 8), 
        Unique = (1 << 9), 

        Hand = (1 << 10), 
        Tomb = (1 << 11), 
        Field = (1 << 12), 
        CardPool = (1 << 13), 

        Hero = (1 << 14), 

        /*
        AllCardPosition = (Hand | Tomb | Field | CardPool), 
        AllSide = (TopSide | BottomSide), 
        AllCardType = (Monster | Spell), 

        C_EnemyMonster = (Monster | Field | Opponent), 
        */
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
        return target.TargetableJudge(RangeMask);
    }

    public List<ITargetable> Eval(IBattler b)
    {
        Player p = b.Owner;
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
        List<ITargetable> tauntList = new List<ITargetable>();
        if ((t & Range.IgnoreTaunt) == 0)
        {
            for (int i = 0; i < res.Count; i++)
            {
                ITargetable tar = res[i];
                if (tar.IsTaunt)
                {
                    tauntList.Add(tar);
                }
            }
            if (tauntList.Count > 0)
            {
                res = tauntList;
            }
        }
        return result=res;
    }
}
