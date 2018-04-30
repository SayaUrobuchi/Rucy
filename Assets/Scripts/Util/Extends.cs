using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extends
{
    public static Transform Clear(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        return transform;
    }

    public static void ClearChildren(this MonoBehaviour mb)
    {
        if (mb != null)
        {
            mb.transform.Clear();
        }
    }

    public static void SetVisible(this MonoBehaviour mb, bool value)
    {
        mb.gameObject.SetActive(value);
    }
}
