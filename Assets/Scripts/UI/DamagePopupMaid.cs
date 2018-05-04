using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopupMaid : MonoBehaviour
{
    public string Format = "-{0}";
    public Text DamageText;
    public float TimeToDestroy = 3f;

    public void SetDamageAmount(int amount)
    {
        DamageText.text = string.Format(Format, amount);
        gameObject.SetActive(true);
        Destroy(gameObject, TimeToDestroy);
    }

    public void SetPosition(Transform parent)
    {
        transform.SetParent(parent, false);
    }
}
