using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPool : MonoBehaviour
{
    [System.Serializable]
    public class Pair : PairBase<string, CardData>
    {
        public Pair(string a, CardData b) : base(a, b)
        {
        }
    }

    private static CardPool self;
    public static CardPool Summon
    {
        get
        {
            return self;
        }
    }

    public static List<Pair> Cards
    {
        get
        {
            return self.CardList;
        }
    }

    [SerializeField]
    public List<Pair> CardList;

    private void Awake()
    {
        self = this;
    }

#if UNITY_EDITOR
    [ContextMenu("RefreshCard")]
    public void RefreshCard()
    {
        CardList = new List<Pair>();
        CardData[] assetList = Resources.LoadAll<CardData>("Data/CardData");
        Debug.Log(assetList.Length);
        for (int i = 0; i < assetList.Length; i++)
        {
            CardData asset = assetList[i];
            if (asset != null)
            {
                CardList.Add(new Pair(asset.name, asset));
            }
        }
    }
#endif
}
