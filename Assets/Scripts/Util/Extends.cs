using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extends
{
#region C#Generic
    public static List<T> ToList<T, U>(this List<U> list) where U : T
    {
        List<T> res = new List<T>();
        for (int i = 0; i < list.Count; i++)
        {
            res.Add((T)list[i]);
        }
        return res;
    }
#endregion

#region UNITY
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
#endregion
}
