using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBlockMaid : MonoBehaviour
{
    public enum State
    {
        None, 
        Normal, 
        CostEstimate, 
        Used, 
    }

    public Color NormalColor;
    public Color CostColor;
    public Color UsedColor;

    private Image image;

    private void Init()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
    }

    public void SetState(State s)
    {
        Init();
        if (s == State.None)
        {
            this.SetVisible(false);
        }
        else if (s == State.Normal)
        {
            this.SetVisible(true);
            image.color = NormalColor;
        }
        else if (s == State.CostEstimate)
        {
            this.SetVisible(true);
            image.color = CostColor;
        }
        else if (s == State.Used)
        {
            this.SetVisible(true);
            image.color = UsedColor;
        }
    }
}
