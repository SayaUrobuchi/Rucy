using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelperMaid : MonoBehaviour
{
    public Text HelperText;

    public void SetText(string text)
    {
        HelperText.text = text;
    }

    public void SetText(string text, Color col)
    {
        SetText(text);
        HelperText.color = col;
    }
}
